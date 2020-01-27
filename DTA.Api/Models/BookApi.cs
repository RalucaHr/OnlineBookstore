using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DTA.Api.Models
{
    public class BookApi
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int ISBN { get; set; }
        public int YearPublished { get; set; }
        public string Language { get; set; }
        public string Description { get; set; }
        public string Author_firstname { get; set; }
        public string Author_lastname { get; set; }
        public string Category { get; set; }

        public BookApi(int bookId, string title, decimal price, int iSBN, int yearPublished, 
            string language, string description, string author_firstname,string author_lastname, string category)
        {
            BookId = bookId;
            Title = title;
            Price = price;
            ISBN = iSBN;
            YearPublished = yearPublished;
            Language = language;
            Description = description;
            Author_firstname = author_firstname;
            Author_lastname = author_lastname;
            Category = category;
        }
    }
}