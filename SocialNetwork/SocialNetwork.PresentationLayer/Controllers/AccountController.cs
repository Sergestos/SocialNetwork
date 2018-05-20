using SocialNetwork.BLL.DataProvider;
using SocialNetwork.BLL.Modules.AnonymousModule;
using SocialNetwork.BLL.Modules.UserModule;
using SocialNetwork.PresentationLayer.Infastructure;
using SocialNetwork.PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Home", "User");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginUserModel model)
        {            
            if (ModelState.IsValid)
            {
                var users = module.GetAllUsers;

                if (users.FirstOrDefault(x => x.Email == model.Email && x.Password == model.Password) != null) 
                {
                    FormsAuthentication.SetAuthCookie(model.Email, true);
                    HttpContext.Response.Cookies.Add(new HttpCookie("id", users.First(x => x.Email == model.Email).ID.ToString()));
                    HttpContext.Response.Cookies.Add(new HttpCookie("lastDialogId", "none"));

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
                if (module.GetAllUsers.FirstOrDefault(x => x.Email == model.Email) == null)
                {
                    var newUser = new BLL.Models.UserInfoBLL()
                    {
                        Email = model.Email,
                        FirstName = model.Name,
                        SurName = model.Surname,
                        Gender = model.Gender ?? "Unknown",
                        Password = model.Password,
                        PhoneNumber = model.PhoneNumber ?? "-",
                        Country = model.Country ?? "-",
                        Locality = model.Location ?? "-",
                        BirthDate = model.BirthDayDate,
                        IsOthersCanComment = true,
                        IsOthersCanStartDialog = true,
                        IsShowInfoForAnonymousUsers = true
                    };

                    module.Registrate(newUser, model.Avatar.InputStream, Path.GetExtension(model.Avatar.FileName));
                    
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

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            if (Request.Cookies["id"] != null)
            {
                var c = new HttpCookie("id");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            if (Request.Cookies["lastDialogId"] != null)
            {
                var c = new HttpCookie("lastDialogId");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            return RedirectToAction("Login", "Account");
        }
    }
}