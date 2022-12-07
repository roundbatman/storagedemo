using System;

namespace StorageMangler.Domain.Model
{
    public class FileMetaData: Auditable
    {
        public DateTimeOffset Staged { get; set; } = DateTimeOffset.MinValue;
        public DateTimeOffset Verified { get; set; } = DateTimeOffset.MinValue;
        public DateTimeOffset Exprired { get; set; } = DateTimeOffset.MinValue;
        public string FileName { get; set; }
        public string Path { get; set; }

        public string GetFullPath()
        {
            return $"{Path}/{FileName}";
        }

    }
}