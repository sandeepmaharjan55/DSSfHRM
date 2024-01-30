using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Scheduler.MVC5.Users
{
    /// <summary>
    /// Exposes user related APIs which will automatically save changes to the UserStore
    /// </summary>
    public class AppUserManager : UserManager<ApplicationUser>
    {
        public AppUserManager(IUserStore<ApplicationUser> store) : base(store)
        {
        }
        public AppUserManager Create(IdentityFactoryOptions<AppUserManager> options, IOwinContext context)
        {
            var manager =
                new AppUserManager(new UserStore<ApplicationUser>(context.Get<UserIdentityDbContext>()));

            return manager;
        }
    }
}
