using System;
using Azure;
using Azure.Data.Tables;

namespace StorageMangler.Domain.Model
{
    public abstract class Auditable: ITableEntity
    {
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Modified { get; set; }

        public DateTimeOffset Deleted { get; set; }

        public string PartitionKey { get; set; } = default!;

        public string RowKey { get; set; } = default!;

        public DateTimeOffset? Timestamp { get; set; } = default!;

        public ETag ETag { get; set; } = default!;
    }
}