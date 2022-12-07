using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StorageMangler.Domain.Infrastructure;
using StorageMangler.Domain.Model;

namespace StorageMangler.Domain.Service
{
    public class StorageService: IStorageService
    {
        private readonly IFileMetaDataRepository _metaDataRepository;
        private readonly IFileStorage _fileStorage;
        private readonly ForbiddenNamesService _forbiddenService;
        private readonly ILogger _logger;

        public StorageService(
            IFileMetaDataRepository metaDataRepository,
            IFileStorage fileStorage,
            ForbiddenNamesService forbiddenService,
            ILoggerFactory loggerFactory
            )
        {
            _metaDataRepository = metaDataRepository;
            _fileStorage = fileStorage;
            _forbiddenService = forbiddenService;
            _logger = loggerFactory.CreateLogger<StorageService>();
        }
        
        public async Task<List<FileInfo>> ListNonForbiddenFiles()
        {
            var files = await _fileStorage.ListPreStagingFiles();
            _logger.LogInformation($"Found {files.Count} total");

            var patterns = await _forbiddenService.GetForbiddenPatternsAsync();
            files = files.Where(f => !IsAMatch(patterns, f.Name)).ToList();
            _logger.LogInformation($"{files.Count} files remaining after name matching");

            return files;
        }

        private bool IsAMatch(HashSet<string> patterns, string name)
        {
            foreach (var pattern in patterns)
            {
                if (Regex.IsMatch(name, pattern))
                {
                    return false;
                }
            }
            return true;
        }
    }
}