using System;

namespace StorageMangler.Domain.Model
{
    public class FileInfo
    {
        public string Name { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
    }
}