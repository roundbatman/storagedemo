using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Castle.Components.DictionaryAdapter;
using Microsoft.Extensions.Logging;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;
using StorageMangler.Domain.Infrastructure;
using StorageMangler.Domain.Model;
using StorageMangler.Domain.Service;


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
        [NUnit.Framework.Ignore("no reason")]
        public async Task ListNonForbiddenFiles_NoFiles()
        {
            List<FileInfo> fileslist = new List<FileInfo>();
            var files = _mockFileStorage.Setup(mr => mr.ListPreStagingFiles()).Returns(fileslist);
            var service = new ForbiddenNamesService(_mockrepository.Object, _mocklogger.Object);
            var patterns = await service.GetForbiddenPatternsAsync();
            Assert.Null(files);
        }

        [Test]
        [Ignore("no reason")]
        public async Task ListNonForbiddenFiles()
        {
            var fileslist = new List<FileInfo>
            {
                new FileInfo
                {
                    Name = "test",
                    CreatedOn = DateTime.Now
                },
            }.AsQueryable();

            var files = _mockFileStorage.Setup(mr => mr.ListPreStagingFiles()).Returns(fileslist);
            var service = new ForbiddenNamesService(_mockrepository.Object, _mocklogger.Object);
            var patterns = await service.GetForbiddenPatternsAsync();

            Assert.IsNotNull(files);
            Assert.AreEqual("test", files[0].Name);
        }
    }
}



