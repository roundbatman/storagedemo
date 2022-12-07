using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Microsoft.Extensions.Logging;
using StorageMangler.Domain.Infrastructure;
using StorageMangler.Domain.Model;

namespace StorageMangler.Infrastructure
{
    public class FileMetadataRepository: IFileMetaDataRepository
    {
        private const string DataPartitionKey = "metadata";
        private readonly ILogger<FileMetadataRepository> _logger;
        private readonly TableClient _tableClient;
        // DefaultEndpointsProtocol=https;AccountName=tabledemo;AccountKey=CfNZWRlsYinsMBb2eGvfQpXLCGYYdBWprYe8llY7Qjy91lW8UxkLwpCsiBVS29ij8BbiyMxzJ91mACDbP0B65g==;TableEndpoint=https://tabledemo.table.cosmos.azure.com:443/;

        public FileMetadataRepository(ITableClientFactory factory, ILogger<FileMetadataRepository> logger)
        {
            _logger = logger;
            _tableClient = factory.Construct("filemetadata");
        }

        public async Task<FileMetaData> InsertMetaData(FileMetaData data)
        {
            var now = DateTimeOffset.Now;
            data.Created = now;
            data.Modified = now;
            data.PartitionKey = DataPartitionKey;
            data.RowKey = Guid.NewGuid().ToString();
            var res = await _tableClient.AddEntityAsync<FileMetaData>(data);

            return await GetByRowKey(data.RowKey);
        }

        public async Task<FileMetaData> GetByRowKey(string rowKey)
        {
            return await _tableClient.GetEntityAsync<FileMetaData>(partitionKey: DataPartitionKey, rowKey: rowKey);
        }
        
        public async Task<List<FileMetaData>> FetchAll()
        {
            var queryEnum = _tableClient.QueryAsync<FileMetaData>(f =>
                f.Deleted.CompareTo(DateTimeOffset.MinValue) == 0)
                .GetAsyncEnumerator();

            var res = new List<FileMetaData>();
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