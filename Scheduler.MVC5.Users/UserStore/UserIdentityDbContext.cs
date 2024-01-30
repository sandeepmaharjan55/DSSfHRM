using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Scheduler.MVC5.Users
{
    /// <summary>
    /// An implementation of IDatabaseInitializer that will DELETE,
    /// recreate, and optionally re-seed the database only if the 
    /// model has changed since the database was created
    /// </summary>
    public class UserIdentityDbContextInit : DropCreateDatabaseIfModelChanges<UserIdentityDbContext>
    {
        protected override void Seed(UserIdentityDbContext context)
        {
            InitTables(context);
            base.Seed(context);
        }

        private void InitTables(UserIdentityDbContext context)
        {
            var userManager = new AppUserManager(new UserStore<ApplicationUser>(context));
            var rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var names = new[]{"Manager","Saujan","Srijana","Sandeep","Sanjeev"};
            var roles = new[]{"Manager", "Employee"};

            //Create role(s) if not exists

            foreach (var role in roles)
            {
                if (!rolemanager.RoleExists(role))
                {
                    rolemanager.Create(new IdentityRole(role));
                } 
            }

            //Create user(s)

            foreach (var name in names)
            {
                var user = new ApplicationUser()
                {
                    UserName = name
                };
                var result = userManager.Create(user, "password");
                
                //and adding user role(s)

                if (result.Succeeded)
                {
                    string role = name == "Manager" ? "Manager" : "Employee";
                    userManager.AddToRole(user.Id, role);
                }
            }
        }
    }
}
