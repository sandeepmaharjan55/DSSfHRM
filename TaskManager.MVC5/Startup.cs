using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Scheduler.MVC5.Users;

namespace TaskManager.MVC5
{
    public class Startup
    {
        public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }

        public void Configuration(IAppBuilder app)
        {
            //var dbContext = new UserIdentityDbContext();

            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888
            app.CreatePerOwinContext((new UserIdentityDbContext()).Create);
            app.CreatePerOwinContext<AppUserManager>((new AppUserManager(new UserStore<ApplicationUser>(new UserIdentityDbContext()))).Create);


            OAuthBearerOptions = new OAuthBearerAuthenticationOptions();

            //This will used the HTTP header: "Authorization" Value: "Bearer 1234123412341234asdfasdfasdfasdf"
            app.UseOAuthBearerAuthentication(OAuthBearerOptions);
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/LogOn")
            });
        }
    }
}