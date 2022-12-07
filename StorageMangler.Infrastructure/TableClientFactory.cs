using System.Threading.Tasks;
using Azure.Data.Tables;

namespace StorageMangler.Infrastructure
{
    public interface ITableClientFactory
    {
        TableClient Construct(string tableName);
        Task<TableClient> ConstructAsync(string tableName);
    }

    public class TableClientFactory : ITableClientFactory
    {
        private readonly string _conString;
        private readonly TableServiceClient _client;

        public TableClientFactory(string conString)
        {
            _conString = conString;
            _client = new TableServiceClient(_conString);

        }

        public TableClient Construct(string tableName)
        {
            var tableClient = _client.GetTableClient(tableName: tableName);
//            tableClient.CreateIfNotExists();
            return tableClient;
        }
        
        public async Task<TableClient> ConstructAsync(string tableName)
        {
            var tableClient = _client.GetTableClient(tableName: tableName);
//            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }
    }
}