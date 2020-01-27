using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTA.Entities
{
    public class OrderItem : IModificationHistory
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public string Cover { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime DateModified { get; set; }
        [NotMapped]
        public bool IsDirty { get; set; }
    }
}
