using SocialNetwork.BLL.DataProvider;
using SocialNetwork.BLL.Modules.AnonymousModule;
using SocialNetwork.PresentationLayer.Infastructure;
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
            module = new AnonymousModule(MockResolver.GetUnitOfWorkFactory());
        }

        [HttpGet]
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
                if (module.GetAllUsers.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password) != null) 
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    return RedirectToAction("Home", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password are incorrect");
                }              
            }

            ViewBag.IsAnyError = true;
            return View(model);
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registration(RegistrationUserModel model)
        {
            if (ModelState.IsValid)
            {
                var users = module.GetAllUsers.ToList();
                if (module.GetAllUsers.FirstOrDefault(x => x.Email == model.Email) == null)
                {
                    module.Registrate(new BLL.Models.UserInfoBLL()
                    {
                        Email = model.Email,
                        FirstName = model.Name,
                        SurName = model.Surname,
                        Gender = model.Gender,
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber,
                        Country = model.Country,
                        Locality = model.Location,
                        //BirthDate = model.BirthDayDate,
                    });

                    return RedirectToAction("Login", "Account");
                }
                else
                {                    
                    ModelState.AddModelError("EmailIsExist", "User with this email is already exist");
                }                
            }
            else
            {
                ModelState.AddModelError("ValidationError", "Validation Error");
            }

            return View(model);
        }

        public RedirectResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Account/Login");
        }
    }
}