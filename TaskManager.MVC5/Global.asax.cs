using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Scheduler.MVC5.Users;
using TaskManager.MVC5.Model;
using System.Web.Optimization;

namespace TaskManager.MVC5
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //to create users table with/without data if not exists
            Database.SetInitializer(new TaskManagerDbContextInit());
            var db = new TaskManagerDbContext();
            db.Database.Initialize(true);
            //to create others tables with/without data if not exists
            Database.SetInitializer(new UserIdentityDbContextInit());
            var users = new UserIdentityDbContext();
            users.Database.Initialize(true);
        }
    }
}
