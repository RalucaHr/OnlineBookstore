using System;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DTA.Entities;
using DTA.Services;
using DTA.Services.Repository;
using DTA.Web.Filters;
using DTA.Web.ViewModels;
using PagedList;

namespace DTA.Web.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class BooksController : Controller
    {
        private GenericRepository<Book> db = new GenericRepository<Book>();

        // GET: Books
        [Log]
        public ActionResult Index(string sortingParameter = "Default Order", string groupingParameter = "All", int minPrice = 1, int maxPrice = 100, int? page = null, string searchString = "")
        {
            SortContextClass sortContext;
            BooksViewModel booksViewModel = new BooksViewModel();
            if (minPrice > maxPrice)
            {
                var temp = minPrice;
                minPrice = maxPrice;
                maxPrice = temp;
            }
            
            var books = db.GetAll(b => b.Author, b => b.Category).ToList();
            if (sortingParameter != "Default Order")
            {
                switch (sortingParameter)
                {
                    case "Price(ASC)":
                        sortContext = new SortContextClass(new SortByPriceAscending());
                        books = sortContext.SortTheBooks(books);
                        break;
                    case "Price(DESC)":
                        sortContext = new SortContextClass(new SortByPriceDescending());
                        books = sortContext.SortTheBooks(books);
                        break;
                    case "Title(A-Z)":
                        sortContext = new SortContextClass(new SortByTitleAscending());
                        books = sortContext.SortTheBooks(books);
                        break;
                    case "Title(Z-A)":
                        sortContext = new SortContextClass(new SortByTitleDescending());
                        books = sortContext.SortTheBooks(books);
                        break;
                }
            }
            Session["currentSearchString"] = searchString;
            Session["currentmaxPrice"] = maxPrice;
            Session["currentminPrice"] = minPrice;
            Session["currentSorting"] = sortingParameter;
            Session["currentGrouping"] = groupingParameter;
            if (Session["currentGrouping"] != null && Session["currentGrouping"].ToString() != "All")
            {
                books = books.Where(x => x.Category.Title == groupingParameter).ToList();
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.ToUpper().Contains(searchString.ToUpper())).ToList();

                //|| b.Author.FirstName.Contains(searchString)              
            }
            books = books.Where(x => x.Price >= minPrice && x.Price <= maxPrice).ToList();
            
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            booksViewModel.Books = books.ToPagedList(pageNumber, pageSize);
            var categories = new GenericRepository<Category>().GetAll();
            foreach (var item in categories)
            {
                int count = books.Count(x => x.Category.Title == item.Title);
                booksViewModel.Categories.Add(item.Title, count);
            }
            return View(booksViewModel);
        }

        // GET: Books
        public ActionResult AdminIndex(string sortOrder, string searchString, string currentFilter, int? page)
        {
            var books = db.GetAll(b => b.Author, b => b.Category);

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                books = books.Where(b => b.Title.ToUpper().Contains(searchString.ToUpper())
                                       || b.Author.FirstName.ToUpper().Contains(searchString.ToUpper())
                                       || b.Author.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || b.Category.Title.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    books = books.OrderByDescending(b => b.Title);
                    break;
                default:
                    books = books.OrderBy(b => b.Title);
                    break;
            }

            int pageSize = 12;
            int pageNumber = (page ?? 1);
            return View(books.ToPagedList(pageNumber, pageSize));

        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.GetById(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/AdminDetails/5
        public ActionResult AdminDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.GetById(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            populateAuthorsDrobDownList();
            populateCategoriesDrobDownList();
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BookId,Cover,Title,Price,ISBN,YearPublished,Language,Description,AuthorId,CategoryId")] Book book, HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                try
                {
                    var cover = postedFile;
                    cover.SaveAs(Server.MapPath("~/Covers/" + cover.FileName));
                    book.Cover = "~/Covers/" + cover.FileName;
                    if (ModelState.IsValid)
                    {
                        db.Insert(book);
                        db.Save();
                        return RedirectToAction("Index");
                    }
                }
                catch (RetryLimitExceededException)
                {
                    
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            else
            {
                book.Cover = "~/Covers/ComingSoon.png";
                if (ModelState.IsValid)
                {
                    db.Insert(book);
                    db.Save();
                    return RedirectToAction("AdminIndex");
                }
            }
            populateAuthorsDrobDownList(book.AuthorId);
            populateCategoriesDrobDownList(book.CategoryId);
            return View(book);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.GetById(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            TempData["oldCoverDir"] = book.Cover;
            ViewBag.AuthorId = new SelectList(new GenericRepository<Author>().GetAll(), "AuthorId", "FirstName", book.AuthorId);
            ViewBag.CategoryId = new SelectList(new GenericRepository<Category>().GetAll(), "CategoryId", "Title", book.CategoryId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BookId,Cover,Title,Price,ISBN,YearPublished,Language,Description,AuthorId,CategoryId")] Book book, HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
            {
                var cover = postedFile;
                cover.SaveAs(Server.MapPath("~/Covers/" + cover.FileName));
                book.Cover = "~/Covers/" + cover.FileName;
            }
            else
            {
                book.Cover = TempData["oldCoverDir"].ToString();
            }
            if (ModelState.IsValid)
            {
                db.Update(book);
                db.Save();
                return RedirectToAction("AdminIndex");
            }
            ViewBag.AuthorId = new SelectList(new GenericRepository<Author>().GetAll(), "AuthorId", "FirstName", book.AuthorId);
            ViewBag.CategoryId = new SelectList(new GenericRepository<Category>().GetAll(), "CategoryId", "Title", book.CategoryId);
            return View(book);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.GetById(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Delete(id);
            db.Save();
            return RedirectToAction("AdminIndex");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose(disposing);
            }
            base.Dispose(disposing);
        }
        private void populateAuthorsDrobDownList(object authorSelected = null)
        {
            var authorsAtDropDown = new GenericRepository<Author>().GetAll().OrderBy(x => x.FirstName);
            ViewBag.AuthorID = new SelectList(authorsAtDropDown, "AuthorId", "Lastname", authorSelected);
        }
        private void populateCategoriesDrobDownList(object categorySelected = null)
        {
            var categoriesAtDropDown = new GenericRepository<Category>().GetAll().OrderBy(x => x.Title);
            ViewBag.CategoryID = new SelectList(categoriesAtDropDown, "CategoryId", "Title", categorySelected);
        }
    }
}

