using System.Threading.Tasks;
using System.Web.Mvc;
using Scheduler.MVC5.Users;
using TaskManager.MVC5.Models;

namespace TaskManager.MVC5.Controllers
{
    public class AccountController : Controller
    {

        #region sign in helper
        //get the SignInHelper object during a mvc controller execution 
        private SignInHelper _helper;

        private SignInHelper SignInHelper
        {
            get { return _helper ?? (_helper = new SignInHelper(AppUserManagerProvider.GetAppUserManager, AppUserManagerProvider.AuthenticationManager)); }
        }
        #endregion
        
        public AccountController(){}

        public AccountController(AppUserManager userManager)
        {
            AppUserManagerProvider.GetAppUserManager = userManager;
        }
        
        [HttpGet]
        public ActionResult LogOn()
        {
            return View();
        }
    
        [HttpPost]
        public async Task<ActionResult> LogOn(LogOnModel model, string returnUrl)
        {
            if (!ModelState.IsValid) return View(model);
            //Sign in if a model is valid
            var result = await SignInHelper.PasswordSignIn(model.UserName, model.Password, model.RememberMe);

            switch (result)
            {
                case "Success":
                {
                    return RedirectToAction("Index", "System");
                }
                default:
                {
                    ModelState.AddModelError("", "The user name or password is incorrect.");
                        
                }break;
                        
            }
            //if something failed, redisplay form
            return View(model);
        }

        public ActionResult LogOff()
        {
            AppUserManagerProvider.AuthenticationManager.SignOut();
            return RedirectToAction("LogOn", "Account", new { Area = "" });
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);
            //if a model is valid,create a user with credentials
            var user = new ApplicationUser {UserName = model.UserName, Email = model.Email};
            var result = await AppUserManagerProvider.GetAppUserManager.CreateAsync(user, model.Password);
                
            if (result.Succeeded)
            {
                //add default role for a new user
                await AppUserManagerProvider.GetAppUserManager.AddToRoleAsync(user.Id, "Employee");
                //sign in
                await SignInHelper.SignInAsync(user, isPersistent: true, rememderBrowser: false);
                return RedirectToAction("Index", "System");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
            //if something failed, redisplay form
            return View(model);
        }
    }
}