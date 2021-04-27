using System;

namespace Storage.MicroService.Models
{
    public partial class StorageEntity
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public int? Total { get; set; }
        public int? Used { get; set; }
        public int? Residue { get; set; }
    }
}
