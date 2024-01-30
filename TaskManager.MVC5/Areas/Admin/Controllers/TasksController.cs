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
    public class TasksController : Controller
    {
        private DSSDbContext db = new DSSDbContext();

        // GET: Admin/Tasks
        public ActionResult Index()
        {
            var tasks = db.Tasks.Include(t => t.Employee);
            return View(tasks.ToList());
        }

        // GET: Admin/Tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taskss task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: Admin/Tasks/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName");
            ViewBag.SkillId = new SelectList(db.Skills, "Id", "SkillName");
            return View();
        }

        // POST: Admin/Tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TaskTitle,Description,AddedDate,ModifiedDate,CompletionDate,SkillId,EmployeeId")] Taskss task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", task.EmployeeId);
            ViewBag.SkillId = new SelectList(db.Skills, "Id", "SkillName", task.SkillId);
            return View(task);
        }

        // GET: Admin/Tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taskss task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", task.EmployeeId);
            ViewBag.SkillId = new SelectList(db.Skills, "Id", "SkillName", task.SkillId);
            return View(task);
        }

        // POST: Admin/Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TaskTitle,Description,AddedDate,ModifiedDate,CompletionDate,SkillId,EmployeeId")] Taskss task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", task.EmployeeId);
            ViewBag.SkillId = new SelectList(db.Skills, "Id", "SkillName", task.SkillId);
            return View(task);
        }

        // GET: Admin/Tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Taskss task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: Admin/Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Taskss task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

       


        // GET: Admin/Tasks/Edit/5
        public ActionResult Assign(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
           
            // ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", task.EmployeeId);
            // ViewBag.EmployeeId = new SelectList(db.Skills, "Id", "Skill", task.SkillId);
            TaskViewModel model = new TaskViewModel();
            var task = db.Tasks.Find(id);
            model.Id = task.Id;
            model.TaskTitle = task.TaskTitle;
            var emp = db.Employees.Where(x => x.SkillId==task.SkillId);
            List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
            foreach (var item in emp)
            {
                employees.Add(new EmployeeViewModel { Id = item.Id, SkillId = item.SkillId, FirstName = item.FirstName, LastName = item.LastName });
            }
            model.employees = employees;
            //  model.employees =db.Employees.Select(x=>new EmployeeViewModel {Id=x.Id,FirstName=x.FirstName,LastName=x.LastName, SkillId=x.SkillId }).ToList();
            //foreach (EmployeeViewModel employee in model.employees)
            //{
            //    var skills = db.Skills.Where(x => x.Id.Equals(employee.SkillId)).ToList();

            //    List<SkillViewModel> employeeSkills = new List<SkillViewModel>();
            //    foreach(var item in skills)
            //    {
            //        employeeSkills.Add(new SkillViewModel { Id = item.Id, SkillName = item.SkillName });
            //    }
            //    employee.skills = employeeSkills;

            //}
            return View(model);
        }

        // POST: Admin/Tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult Assign(int empId,int taskId)
        {
            //if (ModelState.IsValid)
            //{
            bool success = false;
            try
            {
                var task = db.Tasks.FirstOrDefault(x => x.Id == taskId);
                if (task != null)
                {
                    task.EmployeeId = empId;
                    db.SaveChanges();
                    success = true;
                }
            }
            catch (Exception ex)
            {
                success = false;                
            }


            //    return RedirectToAction("Index");
            //}
            //ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", task.EmployeeId);
            //ViewBag.EmployeeId = new SelectList(db.Skills, "Id", "Skill", task.SkillId);
            return Json(success, JsonRequestBehavior.AllowGet);
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
