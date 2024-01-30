using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Scheduler.MVC5.Users
{
    public class AppUserManagerProvider
    {
       UserIdentityDbContext _dbUsers = new UserIdentityDbContext();

        UserIdentityDbContext Db
        {
            get
            {
                if (_dbUsers == null)
                    _dbUsers=new UserIdentityDbContext();
                return _dbUsers;
            }
        }
        
        private static AppUserManager _userManager;
        public static AppUserManager GetAppUserManager
        {
            get
            {
                return _userManager ??
                       (_userManager = HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>());
            }
            set
            {
                if (value != null)
                {
                    _userManager = value;
                }
            }
        }

        public List<ApplicationUser> Users
        {
            get { return Db.Users.ToList(); }
        }

        public string UserId { get { return HttpContext.Current.User.Identity.GetUserId(); } }

        public List<string> GetUserRolesName(string id)
        {
            return Db.Roles.Where(o=>o.Users.Any(item=>item.UserId==id)).Select(item=>item.Name).ToList();
        }
        
        public static IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.Current.GetOwinContext().Authentication;
            }
        }
    }
}
