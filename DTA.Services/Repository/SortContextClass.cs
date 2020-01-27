using DTA.Entities;
using DTA.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DTA.Services.Repository
{
    public class SortContextClass
    {
        private ISortingStrategy Strategy { get; set; }
        public SortContextClass(ISortingStrategy strategy)
        {
            this.Strategy = strategy;
        }
        public List<Book> SortTheBooks(List<Book> books)
        {
            var booksSorted = Strategy.SortStrategy(books);
            return booksSorted;
        }
    }
}