using DTA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTA.Web.Models.Ecommerce
{
    public class IndexViewModel
    {
        public List<Book> Books { get; set; }

        public IndexViewModel()
        {
            Books = new List<Book>();
        }
    }
}