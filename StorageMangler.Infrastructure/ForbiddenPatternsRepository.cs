using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using StorageMangler.Domain.Infrastructure;
using StorageMangler.Domain.Model;

namespace StorageMangler.Infrastructure
{
    public class ForbiddenPatternsRepository: IForbiddenPatternsRepository
    {
        private readonly ILogger<ForbiddenPatternsRepository> _logger;
        private readonly TableClient _tableClient;
        private const string DataPartitionKey = "pattern";

        public ForbiddenPatternsRepository(ITableClientFactory factory, ILogger<ForbiddenPatternsRepository> logger)
        {
            _logger = logger;
            _tableClient = factory.Construct("patterns");
        }
        
        public async Task<ForbiddenPattern> InsertPattern(string pattern)
        {
            var now = DateTimeOffset.Now;
            var fp = new ForbiddenPattern()
            {
                Created = now,
                Modified = now,
                PartitionKey = DataPartitionKey,
                RowKey = Guid.NewGuid().ToString(),
                Pattern = pattern
            };
            var res = await _tableClient.AddEntityAsync<ForbiddenPattern>(fp);
            
            return await GetByRowKey(fp.RowKey);
        }

        public async Task<ForbiddenPattern> GetByRowKey(string rowKey)
        {
            return await _tableClient.GetEntityAsync<ForbiddenPattern>(partitionKey: DataPartitionKey, rowKey: rowKey);
        }

        public async Task<List<ForbiddenPattern>> FetchAll()
        {
            var queryEnum = _tableClient.QueryAsync<ForbiddenPattern>(f =>
                    f.Deleted.CompareTo(DateTimeOffset.MinValue) == 0)
                .GetAsyncEnumerator();

            var res = new List<ForbiddenPattern>();
            try
            {
                while (await queryEnum.MoveNextAsync())
                {
                    res.Add(queryEnum.Current);
                }
            } 
            finally { if (queryEnum != null) await queryEnum.DisposeAsync(); }

            return res;
        }


    }
}