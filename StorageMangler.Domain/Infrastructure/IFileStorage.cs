using System.Collections.Generic;
using System.Threading.Tasks;
using StorageMangler.Domain.Model;

namespace StorageMangler.Domain.Infrastructure
{
    public interface IFileStorage
    {
        Task<List<FileInfo>> ListPreStagingFiles();
    }
}