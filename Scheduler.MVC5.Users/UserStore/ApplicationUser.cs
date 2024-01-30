using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Scheduler.MVC5.Users
{
    /// <summary>
    ///  represents default EntityFramework IUser implementation
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(AppUserManager manager)
        {
            return await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
        }
    }
    /// <summary>
    /// represents a class that uses the default entity types for ASP.NET Identity Users, Roles, Claims, Logins
    /// </summary>
    public class UserIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserIdentityDbContext()
            : base("TaskManagerString")
        {
        }

        public UserIdentityDbContext Create()
        {
            return new UserIdentityDbContext();
        }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserInRoles");
           
        }
    }
   
}
