using DTA.Entities;
using PagedList;
using System.Collections.Generic;

namespace DTA.Web.ViewModels
{
    public class BooksViewModel
    {
        public IPagedList<Book> Books { get; set; }
        public Dictionary<string, int> Categories { get; set; } = new Dictionary<string, int>();
    }
}