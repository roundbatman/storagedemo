using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using StorageMangler.Domain.Infrastructure;
using StorageMangler.Domain.Model;

namespace StorageMangler.Infrastructure
{
    public class FileBlobStorage: IFileStorage
    {
        private const string PreStagingName = "prestaging";
        private const string ArchiveName = "archive";
        private BlobContainerClient _preStagingContainer;
        private readonly BlobContainerClient _archiveContainer;

        public FileBlobStorage(string conString)
        {
            _preStagingContainer = new BlobContainerClient(conString, PreStagingName);
            _archiveContainer = new BlobContainerClient(conString, ArchiveName);
//            _preStagingContainer.CreateIfNotExists();
//            _archiveContainer.CreateIfNotExists();
        }

        public async Task<List<FileInfo>> ListPreStagingFiles()
        {
            var listEnum = _preStagingContainer.GetBlobsAsync().GetAsyncEnumerator();
            var res = new List<BlobItem>();
            try
            {
                while (await listEnum.MoveNextAsync())
                {
                    res.Add(listEnum.Current);
                }
            } 
            finally { if (listEnum != null) await listEnum.DisposeAsync(); }
            return res
                .Select(b => new FileInfo()
                {
                    Name = b.Name,
                    CreatedOn = b.Properties.CreatedOn
                })
                .ToList();
        }

    }
}