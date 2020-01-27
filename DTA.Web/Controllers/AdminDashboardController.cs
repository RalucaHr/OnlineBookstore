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
    public class AdminDashboardController : Controller
    {
        private GenericRepository<OrderItem> db = new GenericRepository<OrderItem>();
        private GenericRepository<Category> cat = new GenericRepository<Category>();
        // GET: AdminDashboard
        public ActionResult Index()
        {
            ViewBag.Message = "Admin Dashboard";
            var categories = cat.GetAll(c => c.Books).ToList();

            return View(categories);
        }

        public ActionResult Orders()
        {
            var orders = db.GetAll().ToList();
            return View(orders);
        }
    }
}