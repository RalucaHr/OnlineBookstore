using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DTA.Entities;
using DTA.Services;
using PagedList;

namespace DTA.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private GenericRepository<Category> db = new GenericRepository<Category>();

        // GET: Categories
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            var categories = from c in db.GetAll()
                          select c;

            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";

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
                categories = categories.Where(c => c.Title.ToUpper().Contains(searchString.ToUpper()));
            }
            switch (sortOrder)
            {
                case "title_desc":
                    categories = categories.OrderByDescending(c => c.Title);
                    break;               
                default:
                    categories = categories.OrderBy(c => c.Title);
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(categories.ToPagedList(pageNumber, pageSize));

        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.GetById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,Title")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Insert(category);
                db.Save();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.GetById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,Title")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Update(category);
                db.Save();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.GetById(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
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
