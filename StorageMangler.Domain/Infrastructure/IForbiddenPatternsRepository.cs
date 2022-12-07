using System.Collections.Generic;
using System.Threading.Tasks;
using StorageMangler.Domain.Model;

namespace StorageMangler.Domain.Infrastructure
{
    public interface IForbiddenPatternsRepository
    {
        Task<ForbiddenPattern> InsertPattern(string pattern);
        Task<ForbiddenPattern> GetByRowKey(string rowKey);
        Task<List<ForbiddenPattern>> FetchAll();
    }
}