using System;

namespace Order.MicroService.Models
{
    public class OrderEntity
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? ProductId { get; set; }
        public int? Count { get; set; }
        public decimal Money { get; set; }
        public int? Status { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
