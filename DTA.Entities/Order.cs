using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DTA.Entities
{
    public class Order : IModificationHistory
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public int Tax { get; set; }
        public decimal Subtotal { get; set; }
        public int Shipping { get; set; }
        public string PayPalReference { get; set; }

        [Display(Name = "Date Created")]
        [Column(TypeName = "datetime2")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Modified")]
        [Column(TypeName = "datetime2")]
        public DateTime DateModified { get; set; }

        [NotMapped]
        public bool IsDirty { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }
    }
}
