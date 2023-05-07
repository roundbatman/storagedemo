using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using StorageMangler.Domain.Infrastructure;
using StorageMangler.Domain.Model;
using StorageMangler.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace StorageMangler.Domain.Test
{
    [TestFixture]
    public class TestStorageService
    {
        private IStorageService _storageService;
        private Mock<IFileMetaDataRepository> _fileMetaDataRepository;
        private Mock<IFileStorage> _fileStorage;
        private ForbiddenNamesService _forbiddenNamesService;
        private Mock<IForbiddenPatternsRepository> _forbiddenPatternsRepository;
        private Mock<ILogger<ForbiddenNamesService>> _mocklogger;
        private Mock<ILoggerFactory> _mockLogger;
        /// <summary>
        /// TestStorageService constructor defined
        /// </summary>
        public TestStorageService()
        {

            _fileMetaDataRepository = new Mock<IFileMetaDataRepository>();
            _fileStorage = new Mock<IFileStorage>();
            _forbiddenPatternsRepository = new Mock<IForbiddenPatternsRepository>();
            _mocklogger = new Mock<ILogger<ForbiddenNamesService>>();
            _mockLogger=new Mock<ILoggerFactory>();
            _forbiddenNamesService = new ForbiddenNamesService(_forbiddenPatternsRepository.Object, _mocklogger.Object);
            _storageService = new StorageService(_fileMetaDataRepository.Object, _fileStorage.Object, _forbiddenNamesService, (ILoggerFactory)_mockLogger.Object);
        }

        /// <summary>
        /// Check if file exist in the storage
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ListNonForbiddenFiles_ExistFiles()
        {
            //declare files objects
            var fileslist = new List<FileInfo>
            {
                new FileInfo
                {
                    Name = "document.pdf",
                    CreatedOn = DateTime.Now
                },
                new FileInfo
                {
                    Name = "presentation.ppt",
                    CreatedOn = DateTime.Now
                },
                new FileInfo
                {
                    Name = "textfile.txt",
                    CreatedOn = DateTime.Now
                },
                new FileInfo
                {
                    Name = "thefile.xlsx",
                    CreatedOn = DateTime.Now
                }
            }.AsQueryable();
            //declare pattern
            var patternlist = new List<ForbiddenPattern>
            {
                new ForbiddenPattern
                {
                     Pattern = "xlsx$",
                     Created= DateTime.Now
                },
               new ForbiddenPattern
                {
                     Pattern = "xls$",
                     Created= DateTime.Now
                },
                new ForbiddenPattern
                {
                     Pattern = "docx$",
                     Created= DateTime.Now
                },
                 new ForbiddenPattern
                {
                     Pattern = "doc$",
                     Created= DateTime.Now
                },
                 new ForbiddenPattern
                {
                     Pattern = "pptx$",
                     Created= DateTime.Now
                },
                 new ForbiddenPattern
                {
                     Pattern = "ppt$",
                     Created= DateTime.Now
                },
            }.AsQueryable();
            //setup PreStaging files
            var files = _fileStorage.Setup(mr => mr.ListPreStagingFiles()).Returns(Task.FromResult(fileslist.ToList()));
            //setup Fetch all pattern
            _forbiddenPatternsRepository.Setup(m => m.FetchAll()).Returns(Task.FromResult(patternlist.ToList()));
            //Getting patterns
            var patterns = await _forbiddenNamesService.GetForbiddenPatternsAsync();
            //comparing files
            var result = fileslist.Where(f => !IsAMatch(patterns, f.Name)).ToList();

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);

        }
        /// <summary>
        /// Check if file not exist in the storage
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ListNonForbiddenFiles_If_NotExistFiles()
        {
            //declare files objects
            var fileslist = new List<FileInfo>
            {
                new FileInfo
                {
                    Name = "document.pdf",
                    CreatedOn = DateTime.Now
                },
                new FileInfo
                {
                    Name = "presentation.ppt",
                    CreatedOn = DateTime.Now
                },
                new FileInfo
                {
                    Name = "textfile.txt",
                    CreatedOn = DateTime.Now
                },
                new FileInfo
                {
                    Name = "thefile.xlsx",
                    CreatedOn = DateTime.Now
                }
            }.AsQueryable();
            //declare pattern
            var patternlist = new List<ForbiddenPattern>
            {
                new ForbiddenPattern
                {
                     Pattern = "csv",
                     Created= DateTime.Now
                },
               new ForbiddenPattern
                {
                     Pattern = "aspx",
                     Created= DateTime.Now
                },
                new ForbiddenPattern
                {
                     Pattern = "mrt$",
                     Created= DateTime.Now
                }
            }.AsQueryable();
            //setup PreStaging files
            var files = _fileStorage.Setup(mr => mr.ListPreStagingFiles()).Returns(Task.FromResult(fileslist.ToList()));
            //setup Fetch all pattern
            _forbiddenPatternsRepository.Setup(m => m.FetchAll()).Returns(Task.FromResult(patternlist.ToList()));
            //Getting patterns
            var patterns = await _forbiddenNamesService.GetForbiddenPatternsAsync();
            //comparing files
            var result = fileslist.Where(f => !IsAMatch(patterns, f.Name)).ToList();

            Assert.AreEqual(0,result.Count);

        }
        /// <summary>
        /// Pattern match function
        /// </summary>
        /// <param name="patterns"></param>
        /// <param name="name"></param>
        /// <returns></returns>
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

