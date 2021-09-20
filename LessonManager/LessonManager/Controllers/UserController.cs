using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LessonManager.Models;
using LessonManager.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace LessonManager.Controllers
{
    public class UserController : Controller
    {
        protected readonly UserManager<IdentityUser> userManager;
        protected readonly UserStore<IdentityUser> userStore;

        /**
         * Constructor for UserController
         * @param userStore Microsoft.AspNet.Identity.EntityFramework.UserStore
         * @param userManager  Microsoft.AspNet.Identity.UserManager
         */
        public UserController()
        {
            userStore = new UserStore<IdentityUser>();
            userManager = new UserManager<IdentityUser>(userStore);
        }

        // Loads Login View
        public ActionResult Login()
        {
            return View();
        }

        // Loads Register View
        public ActionResult Register()
        {
            return View();
        }

        //  Εγγραφή των χρηστών στην πλατφόρμα
        // <param name="username"></param>
        // <param name="password"></param>
        // <returns>Json</returns>
        public JsonResult RegisterUser(string username, string password,string fullname )
        {
            var user = new IdentityUser {UserName = username};
            var result = userManager.Create(user, password);
            // Κατασκευή και του Application User
            if (!result.Succeeded) return Json(new {status = result.Succeeded, message = result.Errors});
            using (var db = new AppContext())
            {
                db.ApplicationUsers.Add(new ApplicationUser {Id = user.Id, Role = "simpleUser", FullName=fullname});
                db.SaveChanges();
            }

            return Json(new {status = result.Succeeded, message = result.Errors});
            ;
        }

        public JsonResult LoginUser(string username, string password)
        {
            var user = userManager.Find(username, password);
            if (user == null) return Json(new {status = "fail", message = "Wrong username or password"});
            var authenticationManager = System.Web.HttpContext.Current.GetOwinContext().Authentication;
            var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
            authenticationManager.SignIn(new AuthenticationProperties(), userIdentity);
            return Json(new {status = "success", message = "user logged in"});
        }

        public JsonResult GetUsers(string role, string type = null)
        {
            using (var db = new AppContext())
            {
                var users = db.ApplicationUsers.Where(sh => sh.Role == role).ToList();
                // Σε περίπτωση που δεν θέλουμε κάποια συγκεκριμένμη μορφή αρχείου κάνουμε return
                // αλλιώς  επιστρέφουμε στην μορφή που χρειαζόμαστε την πληροφορία
                if (type == "select2")
                {
                    var formattedUser = new List<Select2>();
                    foreach (var user in users) formattedUser.Add(new Select2 {id = user.Id, text = user.FullName});
                    return Json(new {results = formattedUser}, JsonRequestBehavior.AllowGet);
                }

                return Json(new { data = users } , JsonRequestBehavior.AllowGet);
            }
        }

        public RedirectResult Logout()
        {
            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
            AuthenticationManager.SignOut();
            return Redirect("/user/login");
        }
    }
}