using DTA.Entities;
using DTA.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTA.Services.Repository
{
    public class SortByPriceAscending : ISortingStrategy
    {
        public override List<Book> SortStrategy(List<Book> books)
        {
            books = books.OrderBy(x => x.Price).ToList();
            return books;
        }
    }
}


