using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTA.Web.Models.Ecommerce
{
    public class CartItem
    {
        public int BookId { get; set; }
        public string Cover { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}