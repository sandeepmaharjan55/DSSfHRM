using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Mvc;
using DHTMLX.Common;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Authentication;
using DHTMLX.Scheduler.Controls;
using DHTMLX.Scheduler.Data;

using Scheduler.MVC5.Users;
using TaskManager.MVC5.Model;
using TaskManager.MVC5.Models;
using System.Runtime.Serialization.Json;

namespace TaskManager.MVC5.Controllers
{
    public class SystemController : Controller
    {
        private IRepository _repository;
        private AppUserManagerProvider _appUserManagerProvider;
        public IRepository Repository
        {
            get { return _repository ?? (_repository = new Repository()); }
        }

        public AppUserManagerProvider AppUserManagerProvider
        {
            get { return _appUserManagerProvider ?? (_appUserManagerProvider = new AppUserManagerProvider()); }
        }



        // GET: System
        public ActionResult Index()
        {
            //redirect to login page unauthorized user
            return !Request.IsAuthenticated ? RedirectToAction("LogOn", "Account") : RedirectToAction(User.IsInRole("Manager") ? "Manager" : "Employee", "System");
        }

        public ActionResult Manager()
        {
            var scheduler = new DHXScheduler(this);
            
            #region check rights
            if (!RoleIs("Manager"))// checks the role
                return RedirectToAction("Index", "System");//in case the role is not manager, redirects to the login page

            #endregion

            #region configuration

            scheduler.Config.first_hour = 8;//sets the minimum value for the hour scale (Y-Axis)
            scheduler.Config.hour_size_px = 88;
            scheduler.Config.last_hour = 17;//sets the maximum value for the hour scale (Y-Axis)
            scheduler.Config.time_step = 30;//sets the scale interval for the time selector in the lightbox. 
            scheduler.Config.full_day = true;// blocks entry fields in the 'Time period' section of the lightbox and sets time period to a full day from 00.00 the current cell date untill 00.00 next day. 

            scheduler.Skin = DHXScheduler.Skins.Flat;
            scheduler.Config.separate_short_events = true;

            scheduler.Extensions.Add(SchedulerExtensions.Extension.ActiveLinks);

            #endregion

            #region views configuration
            scheduler.Views.Clear();//removes all views from the scheduler
            scheduler.Views.Add(new WeekView());// adds a tab with the week view
            var units = new UnitsView("staff", "owner_id") { Label = "Staff" };// initializes the units view

            var users = AppUserManagerProvider.Users;
            var staff = new List<object>();
            
            foreach (var user in users)
            {
                if (AppUserManagerProvider.GetUserRolesName(user.Id).Contains("Employee"))
                {
                    staff.Add(new { key = user.Id, label = user.UserName });
                }
            }

            units.AddOptions(staff);// sets X-Axis items to names of employees  
            scheduler.Views.Add(units);//adds a tab with the units view
            scheduler.Views.Add(new MonthView()); // adds a tab with the Month view
            scheduler.InitialView = units.Name;// makes the units view selected initially

            scheduler.Config.active_link_view = units.Name;
            #endregion

            #region lightbox configuration
            var text = new LightboxText("text", "Task") { Height = 20, Focus = true };// initializes a text input with the label 'Task'
            scheduler.Lightbox.Add(text);// adds the control to the lightbox
            var description = new LightboxText("details", "Details") { Height = 80 };// initializes a text input with the label 'Task'
            scheduler.Lightbox.Add(description);
            var status = new LightboxSelect("status_id", "Status");// initializes a dropdown list with the label 'Status'
            status.AddOptions(Repository.GetAll<Status>().Select(s => new
            {
                key = s.id,
                label = s.title
            }));// populates the list with values from the 'Statuses' table
            scheduler.Lightbox.Add(status);
            //add users list 
            var sUser = new LightboxSelect("owner_id", "Employee");
            sUser.AddOptions(staff);
            //--
            scheduler.Lightbox.Add(sUser);

            scheduler.Lightbox.Add(new LightboxTime("time"));// initializes and adds a control area for setting start and end times of a task
            #endregion

            #region data
            scheduler.EnableDataprocessor = true;// enables dataprocessor
            scheduler.LoadData = true;//'says' to send data request after scheduler initialization 
            scheduler.Data.DataProcessor.UpdateFieldsAfterSave = true;// Tracks after server responses for modified event fields 
            #endregion

            var employees = users.Select(o => new Employees
            {
                key = o.Id,
                userName = o.UserName
            }).ToArray();

            var statuses = Repository.GetAll<Status>().ToList();

            var js = new DataContractJsonSerializer(typeof(Employees[]));
            var ms = new MemoryStream();
            js.WriteObject(ms, employees);
            ms.Position = 0;
            var sr = new StreamReader(ms);
            var json = sr.ReadToEnd();
            sr.Close();
            ms.Close();
            var model = new SystemModel(scheduler, json,statuses);
            return View(model);
        }

        private bool RoleIs(string role)
        {
            return Request.IsAuthenticated && User.IsInRole(role);
        }

        public ActionResult Employee()
        {
            var scheduler = new DHXScheduler(this);

            #region check rights
            if (!RoleIs("Employee"))
            {
                return RedirectToAction("Index", "System");
            }
            #endregion

            #region configuration

            scheduler.Config.separate_short_events = true;
            scheduler.Config.hour_size_px = 88;

            scheduler.Extensions.Add(SchedulerExtensions.Extension.Cookie);// activates the extension to provide cookie
            scheduler.Extensions.Add(SchedulerExtensions.Extension.Tooltip);// activates the extension to provide tooltips
            var template = "<b>Task:</b> {text}<br/><b>Start date:</b>";
            template += "<%= scheduler.templates.tooltip_date_format(start) %><br/><b>End date:</b>";
            template += "<%= scheduler.templates.tooltip_date_format(end) %>";
            scheduler.Templates.tooltip_text = template; // sets template for the tooltip text

            scheduler.Skin = DHXScheduler.Skins.Flat;
            #endregion

            #region views
            scheduler.Views.Clear();//removes all views from the scheduler 
            scheduler.Views.Add(new WeekAgendaView());// adds a tab with the weekAgenda view
            scheduler.Views.Add(new MonthView()); // adds a tab with the Month view
            scheduler.InitialView = scheduler.Views[0].Name;// makes the weekAgenda view selected initially
            #endregion

            #region data
            scheduler.SetEditMode(EditModes.Forbid);// forbids editing of tasks  
            scheduler.LoadData = true;//'says' to send data request after scheduler initialization 
            scheduler.DataAction = "Tasks";//sets a controller action which will be called for data requests 
            scheduler.Data.Loader.PreventCache();// adds the current ticks value to url to prevent caching of the request 
            #endregion

            return View(scheduler);
        }


        public ActionResult Data()
        {
            //if the user is not authorized or not in the Manager Role, returns the empty dataset
            if (!RoleIs("Manager")) return new SchedulerAjaxData();

            var tasks = Repository.GetAll<Task>()
                .Join(Repository.GetAll<Status>(), task => task.status_id, status => status.id, (task, status) => new { Task = task, Status = status })
                .Select(o => new
                {
                    o.Task.id,
                    o.Task.owner_id,
                    o.Task.details,
                    o.Task.end_date,
                    o.Task.start_date,
                    o.Task.text,
                    o.Task.status_id
                });

            var resp = new SchedulerAjaxData(tasks);
            return resp;

        }
        public ActionResult Save(Task task)
        {
            // an action against particular task (updated/deleted/created) 
            var action = new DataAction(Request.Form);
            #region check rights
            if (!RoleIs("Manager"))
            {
                action.Type = DataActionTypes.Error;
                return new AjaxSaveResponse(action);
            }
            #endregion

            task.creator_id = Guid.Parse(AppUserManagerProvider.UserId);
            try
            {
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        Repository.Insert(task);
                        break;
                    case DataActionTypes.Delete:
                        Repository.Delete(task);
                        break;
                    case DataActionTypes.Update:
                        Repository.Update(task);
                        break;
                }
                action.TargetId = task.id;
            }
            catch (Exception)
            {
                action.Type = DataActionTypes.Error;
            }

            var color = Repository.GetAll<Status>().SingleOrDefault(s => s.id == task.status_id);

            var result = new AjaxSaveResponse(action);
            result.UpdateField("color", color.color);
            return result;
        }

        public ActionResult Tasks()
        {
            #region check rights
            if (!RoleIs("Employee"))
            {
                return new SchedulerAjaxData();//returns the empty dataset
            }
            #endregion


            var result = Repository.GetAll<Task>()
                .Join(Repository.GetAll<Status>(), task => task.status_id, status => status.id, (task, status) => new { Task = task, Status = status })
                .Select(o => new
                {
                    o.Status.color,
                    o.Task.id,
                    o.Task.owner_id,
                    o.Task.details,
                    o.Task.end_date,
                    o.Task.start_date,
                    o.Task.text,
                    o.Task.status_id
                });

            var tasks = new List<object>();

            foreach (var r in result.ToList())
            {
                if (r.owner_id == Guid.Parse(AppUserManagerProvider.UserId))
                {
                    tasks.Add(new
                    {
                        r.color,
                        r.id,
                        r.owner_id,
                        r.details,
                        r.end_date,
                        r.start_date,
                        r.text,
                        r.status_id
                    });
                }
            }

            var resp = new SchedulerAjaxData(tasks);
            return resp;
        }

        public ActionResult TaskDetails(int? id)
        {
            #region check rights

            if (!RoleIs("Employee"))
            {
                return RedirectToAction("Index", "System");
            }
            #endregion

            var task = default(Task);
            if (id != null)
            {
                task = Repository.GetAll<Task>().FirstOrDefault(o => o.id == id);

                if (task.owner_id != Guid.Parse(AppUserManagerProvider.UserId))
                    task = default(Task);
            }

            var statuses = Repository.GetAll<Status>().ToArray();

            ViewData["status"] = task != default(Task) ? task.status_id : statuses[0].id;
            ViewData["user"] = User.Identity.Name;
            return View(new TaskDetails(task, statuses));
        }

        public ActionResult UpdateStatus(int? id)
        {
            if (!RoleIs("Employee") || Request.Form["result"] != "Update" || id == null)
                return RedirectToAction("Index", "System");


            var task = Repository.GetAll<Task>().SingleOrDefault(ev => ev.id == id);

            if (task == default(Task) && task.owner_id != Guid.Parse(AppUserManagerProvider.UserId))
                return RedirectToAction("Index", "System");

            task.status_id = int.Parse(Request.Form["status_id"]);
            UpdateModel(task);
            Repository.Update(task);

            return RedirectToAction("Index", "System");
        }

        public class SystemModel
        {
            public DHXScheduler Scheduler { get; set; }
            public string Users { get; set; }
            public List<Status> Statuses { get; set; } 
            public SystemModel(DHXScheduler sched, string users,List<Status> statuses )
            {
                Scheduler = sched;
                Users = users;
                Statuses = statuses;
            }
        }


        //class for json string
        [DataContract]
        public class Employees
        {
            [DataMember]
            public string key { get; set; }
            [DataMember]
            public string userName { get; set; }
        }
    }
}