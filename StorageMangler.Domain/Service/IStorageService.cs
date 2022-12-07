using System.Collections.Generic;
using System.Threading.Tasks;
using StorageMangler.Domain.Model;

namespace StorageMangler.Domain.Service
{
    public interface IStorageService
    {
        Task<List<FileInfo>> ListNonForbiddenFiles();
    }
}