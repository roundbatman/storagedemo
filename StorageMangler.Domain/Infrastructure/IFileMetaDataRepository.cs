using System.Collections.Generic;
using System.Threading.Tasks;
using StorageMangler.Domain.Model;

namespace StorageMangler.Domain.Infrastructure
{
    public interface IFileMetaDataRepository
    {
        Task<FileMetaData> InsertMetaData(FileMetaData data);
        Task<FileMetaData> GetByRowKey(string rowKey);
        Task<List<FileMetaData>> FetchAll();
    }
}