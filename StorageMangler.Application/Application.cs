using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StorageMangler.Domain.Service;

namespace StorageMangler.Application
{
    public class Application
    {
        private readonly IStorageService _service;
        private readonly ILogger<Application> _logger;

        public Application(IStorageService service, ILogger<Application> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<bool> Run()
        {
            var files = await _service.ListNonForbiddenFiles();
            foreach (var fileInfo in files)
            {
               _logger.LogInformation($"{fileInfo.Name}"); 
            }
            return true;
        }
    }
}