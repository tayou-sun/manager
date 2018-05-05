using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TaskManager.Models;

namespace TaskManager.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (TaskContext db = new TaskContext())
                {
                    user = db.User.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);

                }
                if (user != null)
                {

                    FormsAuthentication.SetAuthCookie(model.Login, true);

                    return RedirectToAction("Index","Home");
                }
                else {
                    return RedirectToAction("Login","Account");
                }
            }

             return RedirectToAction("Login","Account");
        }

    }
}
