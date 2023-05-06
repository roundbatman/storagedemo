using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using StorageMangler.Domain.Infrastructure;
using StorageMangler.Domain.Service;
using StorageMangler.Domain.Model;
using System.Linq;

namespace StorageMangler.Domain.Test
{
    public class TestStorageService
    {
        private Mock<IFileMetaDataRepository> _mockFileMetaDataRepository;
        private Mock<IFileStorage> _mockFileStorage;
        private ForbiddenNamesService _forbiddenService;
        private Mock<ILogger> _mockLogger;
        private IStorageService _service;
        private Mock<IForbiddenPatternsRepository> _mockrepository;
        private Mock<ILogger<ForbiddenNamesService>> _mocklogger;

        public void Initialize()
        {
            _mockFileMetaDataRepository = new Mock<IFileMetaDataRepository>();
            _mockFileStorage = new Mock<IFileStorage>();
            _service = new StorageService(_mockFileMetaDataRepository.Object, _mockFileStorage.Object, _forbiddenService, (ILoggerFactory)_mockLogger.Object);

        }

        [Test]
        [Ignore("no reason")]
        public async Task ListNonForbiddenFiles()
        {
            var fileslist = new List<FileInfo>
            {
                new FileInfo
                {
                    Name = "testFile",
                    CreatedOn = DateTime.Now
                },
            }.AsQueryable();

            var files = _mockFileStorage.Setup(mr => mr.ListPreStagingFiles()).Returns((Task<List<FileInfo>>)fileslist);
            var service = new ForbiddenNamesService(_mockrepository.Object, _mocklogger.Object);
            var patterns = await service.GetForbiddenPatternsAsync();

            Assert.IsNotNull(files);
        }
    }
}

