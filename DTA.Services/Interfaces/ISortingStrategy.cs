using DTA.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DTA.Services.Interfaces
{
    public abstract class ISortingStrategy
    {
        public abstract List<Book> SortStrategy(List<Book> books);
    }
}


