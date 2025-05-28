using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class AccountController : Controller
    {
        private StudentManagementContext db = new StudentManagementContext();

        // GET: Account/Login
        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Statistics", "Home");
            }
            Session
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                var foundUser = db.Users.Include("Role").FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

                if (foundUser != null)
                {
                    FormsAuthentication.SetAuthCookie(foundUser.Username, false);
                    Session["UserId"] = foundUser.UserId;
                    Session["NameUser"] = foundUser.FullName;
                    Session["Role"] = foundUser.Role.RoleName;

                    return RedirectToAction("Statistics", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(user);
        }

        // GET: Account/Logout
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}