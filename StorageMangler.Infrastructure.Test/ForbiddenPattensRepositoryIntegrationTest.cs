using System;
using System.Threading.Tasks;
using NUnit.Framework;
using StorageMangler.Domain.Model;

namespace StorageMangler.Infrastructure.Test
{
    [TestFixture]
    public class ForbiddenPattensRepositoryIntegrationTest
    {
        private const string ConString = "DefaultEndpointsProtocol=https;AccountName=tabledemo;AccountKey=PTPhkkNPtO0Dd5SfeJLdp77jszpVAoPXv81cDnrkMXBVXaRVcHwN2Jnpxeca8MPwMlsaZN87hQUPACDbP2D6tw==;TableEndpoint=https://tabledemo.table.cosmos.azure.com:443/;";

        [Test]
        [Ignore("disabled")]
        public async Task TestInsertEntityExpectSomething()
        {
            var factory = new TableClientFactory(ConString);
            var repo = new ForbiddenPatternsRepository(factory, null);
            await repo.InsertPattern("xlsx$");
            await repo.InsertPattern("xls$");
            await repo.InsertPattern("docx");
            await repo.InsertPattern("doc$");
            await repo.InsertPattern("pptx$");
            await repo.InsertPattern("ppt$");
        }

        [Test]
        [Ignore("disabled")]
        public async Task TestFetchAllExpectSomething()
        {
            var factory = new TableClientFactory(ConString);
            var repo = new ForbiddenPatternsRepository(factory, null);
            var pepe = await repo.FetchAll();
            foreach (var forbiddenPattern in pepe)
            {
                Console.WriteLine(forbiddenPattern.Pattern);
                
            }
        }

    }
}