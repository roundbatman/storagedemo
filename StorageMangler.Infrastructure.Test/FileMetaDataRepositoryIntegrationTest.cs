using System;
using System.Threading.Tasks;
using NUnit.Framework;
using StorageMangler.Domain.Model;

namespace StorageMangler.Infrastructure.Test
{
    [TestFixture]
    public class FileMetaDataRepositoryIntegrationTest
    {
        private const string ConString = "";

        [Test]
        [Ignore("disabled")]
        public async Task TestInsertEntityExpectSomething()
        {
            var factory = new TableClientFactory(ConString);
            var repo = new FileMetadataRepository(factory, null);
            await repo.InsertMetaData(new FileMetaData()
            {
                Path = "femdfmfea",
                FileName = "fefefe"
            });
        }
        
        [Test]
        [Ignore("disabled")]
        public async Task TestFetchAllExpectSomething()
        {
            var factory = new TableClientFactory(ConString);
            var repo = new FileMetadataRepository(factory, null);
            var res = await repo.FetchAll();
            Console.WriteLine("done");
        }
    }
}