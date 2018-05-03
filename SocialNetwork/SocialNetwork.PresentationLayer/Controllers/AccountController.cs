using SocialNetwork.BLL.DataProvider;
using SocialNetwork.BLL.Modules.AnonymousModule;
using SocialNetwork.PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SocialNetwork.PresentationLayer.Controllers
{
    public class AccountController : Controller
    {
        private IAnonymoysModule module;

        public AccountController()
        {
            module = new AnonymousModule(new EFUnitOfWorkFactory("string1", "string2"));
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserModel model)
        {
            if (ModelState.IsValid)
            {               
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        public ActionResult Registration()
        {
            return View();
        }

        public ActionResult Registration(RegistrationUserModel user)
        {
            return View();
        }        

        public RedirectResult Logout()
        {
            return Redirect("/Account/Login");
        }
    }
}