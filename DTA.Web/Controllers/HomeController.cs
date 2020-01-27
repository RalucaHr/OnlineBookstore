using DTA.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTA.Entities;
using System.Data.Entity;
using DTA.Services;

namespace DTA.Web.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private GenericRepository<Book> db = new GenericRepository<Book>();
        public ActionResult Index()
        {
            var books = db.GetAll(y => y.Category).ToList();
            return View(books);
        }

        public ActionResult About()
        {
            ViewBag.Message = "About us.";

            return View();
        }

        public ActionResult Contact()
        {

            return View();
        }

        public ActionResult Chat()
        {
            return View();
        }

        public ActionResult GetApi()
        {
            return View();
        }
    }
}