using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Scheduler.MVC5.Users
{
    public class SignInHelper
    {
        public AppUserManager AppUserManager { get; private set; }
        public IAuthenticationManager AuthenticationManager { get; set; }

        public SignInHelper(AppUserManager userManager, IAuthenticationManager authManager)
        {
            this.AppUserManager = userManager;
            this.AuthenticationManager = authManager;
        }

        public async Task SignInAsync(ApplicationUser user, bool isPersistent, bool rememderBrowser)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie,
                DefaultAuthenticationTypes.TwoFactorCookie);
            var userIdentity = await user.GenerateUserIdentityAsync(AppUserManager);
            if (rememderBrowser)
            {
                var rememberBrowserIdentity = AuthenticationManager.CreateTwoFactorRememberBrowserIdentity(user.Id);
                AuthenticationManager.SignIn(new AuthenticationProperties() {IsPersistent = isPersistent}, userIdentity,
                    rememberBrowserIdentity);
            }
            else
            {
                AuthenticationManager.SignIn(new AuthenticationProperties(){IsPersistent = isPersistent},userIdentity);
            }
        }

        public async Task<string> PasswordSignIn(string userName, string password, bool isPersistent)
        {
            var user = await AppUserManager.FindByNameAsync(userName);
            if (user == null)
            {
                return SignInStatus.Failure.ToString();
            }
            if (await AppUserManager.CheckPasswordAsync(user, password))
            {
                await SignInAsync(user, isPersistent, false);
                return SignInStatus.Success.ToString();
            }
            if (await AppUserManager.IsLockedOutAsync(user.Id))
            {
                return SignInStatus.LockedOut.ToString();
            }
            return SignInStatus.Failure.ToString();
        }

    }
}
