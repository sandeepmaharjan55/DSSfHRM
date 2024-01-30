using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskManager.MVC5.Models;

namespace TaskManager.MVC5.Areas.Admin.Controllers
{
    public class ReportsController : Controller
    {
        private DSSDbContext db = new DSSDbContext();

        // GET: Admin/Reports
        public ActionResult Index()
        {
            var reports = db.Reports.Include(r => r.Employee).Include(r => r.Taskss);
            return View(reports.ToList());
        }

        // GET: Admin/Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
