using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskManager.MVC5.Models;
using System.Net.Mail;

namespace TaskManager.MVC5.Areas.Admin.Controllers
{
    public class EmployeesController : Controller
    {
        private DSSDbContext db = new DSSDbContext();

        // GET: Admin/Employees
        public ActionResult Index()
        {
            var employees = db.Employees.Include(t => t.Skill);
            return View(employees.ToList());
        }
        

        public ActionResult Rank()
        {
            return View(db.Employees.ToList());
        }

        

        // GET: Admin/Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.SkillId = new SelectList(db.Skills, "Id", "SkillName", employee.SkillId);
            return View(employee);
        }

        // GET: Admin/Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(new Employee() {
                    FirstName=employee.FirstName,
                    LastName=employee.LastName,
                    Address=employee.Address,
                    ContactNo=employee.ContactNo,
                    designation=employee.designation,
                    Email=employee.Email,
                    enrollDate=employee.enrollDate,
                    Password=employee.Password,
                    SkillId=employee.SkillId,
                    Status=employee.Status
                });
                
                db.SaveChanges();




                return RedirectToAction("Index");
            }

            return View(employee);
        }

        // GET: Admin/Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.SkillId = new SelectList(db.Skills, "Id", "SkillName",employee.SkillId);
            return View(employee);
        }

        // POST: Admin/Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,designation,Address,Email,Password,ContactNo,enrollDate,ModifiedDate,SkillId,Status")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(employee);
        }

        // GET: Admin/Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Admin/Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = db.Employees.Find(id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Index");
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
