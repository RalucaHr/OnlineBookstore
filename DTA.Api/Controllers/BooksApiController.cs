using DTA.Api.Models;
using DTA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;

namespace DTA.Api.Controllers
{
    public class BooksApiController : ApiController
    {
        LibraryDbContext db = new LibraryDbContext();

        [HttpGet]
        public IHttpActionResult GetBooksJson()
        {
            var books = db.Books.Include(x => x.Author).Include(x => x.Category).ToList();
            List<BookApi> booksJson = new List<BookApi>();
            foreach (var item in books)
            {
                booksJson.Add(new BookApi(item.BookId, item.Title, item.Price, item.ISBN, item.YearPublished, item.Language, item.Description, item.Author.FirstName,item.Author.LastName, item.Category.Title));
            }
            return Json(booksJson);

        }
    }
}
