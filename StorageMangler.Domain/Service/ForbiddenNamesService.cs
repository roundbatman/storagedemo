using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StorageMangler.Domain.Infrastructure;

namespace StorageMangler.Domain.Service
{
    public class ForbiddenNamesService
    {
        private readonly IForbiddenPatternsRepository _repository;
        private readonly ILogger<ForbiddenNamesService> _logger;

        public ForbiddenNamesService(IForbiddenPatternsRepository repository, ILogger<ForbiddenNamesService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<HashSet<string>> GetForbiddenPatternsAsync()
        {
            var forbiddenPatterns = await _repository.FetchAll();
            return forbiddenPatterns
                .Select(f => f.Pattern)
                .ToHashSet();
        }        
        
    }
}