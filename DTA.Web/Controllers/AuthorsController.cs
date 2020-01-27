using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DTA.Database;
using DTA.Entities;
using DTA.Services;
using PagedList;

namespace DTA.Web.Controllers
{
    public class AuthorsController : Controller
    {
        private GenericRepository<Author> db = new GenericRepository<Author>();

        // GET: Authors        
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            var authors = from a in db.GetAll()
                          select a;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.FirstNameSortParm = String.IsNullOrEmpty(sortOrder) ? "fname_desc" : "";
            ViewBag.LastNameSortParm = sortOrder == "lname_asc" ? "lname_desc" : "lname_asc";


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
                authors = authors.Where(a => a.FirstName.ToUpper().Contains(searchString.ToUpper())
                                       || a.LastName.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "fname_desc":
                    authors = authors.OrderByDescending(a => a.FirstName);
                    break;
                case "lname_desc":
                    authors = authors.OrderByDescending(a => a.LastName);
                    break;
                case "lname_asc":
                    authors = authors.OrderBy(a => a.LastName);
                    break;
                default:
                    authors = authors.OrderBy(a => a.FirstName);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(authors.ToPagedList(pageNumber, pageSize));          

        }

        // GET: Authors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.GetById(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // GET: Authors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AuthorId,FirstName,LastName")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Insert(author);
                db.Save();
                return RedirectToAction("Index");
            }

            return View(author);
        }

        // GET: Authors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.GetById(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AuthorId,FirstName,LastName")] Author author)
        {
            if (ModelState.IsValid)
            {
                db.Update(author);
                db.Save();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        // GET: Authors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Author author = db.GetById(id);
            if (author == null)
            {
                return HttpNotFound();
            }
            return View(author);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Delete(id);
            db.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose(disposing);
            }
            base.Dispose(disposing);
        }
    }
}
