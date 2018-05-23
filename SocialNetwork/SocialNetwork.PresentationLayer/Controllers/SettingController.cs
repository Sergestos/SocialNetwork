using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.PresentationLayer.Controllers
{
    using SocialNetwork.BLL.Models;
    using SocialNetwork.BLL.Modules.UserModule;
    using SocialNetwork.PresentationLayer.Infastructure;
    using SocialNetwork.PresentationLayer.Infastructure.EntityConverters;
    using SocialNetwork.PresentationLayer.Models;

    public class SettingController : Controller
    {
        [HttpGet]
        [Authorize]
        public ActionResult Setting()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePersonalInfo()
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            return PartialView(EntityConverter.GetSettingInfoModel(Convert.ToInt32(cookie.Value), module));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePersonalInfo(SettingViewModel model)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("ValidationError", "Validation Error");
                return PartialView(EntityConverter.GetSettingInfoModel(Convert.ToInt32(cookie.Value), module));
            }          

            if (module.GetAllUsers.Where(x => x.ID != Convert.ToInt32(cookie.Value)).FirstOrDefault(x => x.Email == model.Email) != null)
            {
                ModelState.AddModelError("", "User with this email is already exist");
                return PartialView(EntityConverter.GetSettingInfoModel(Convert.ToInt32(cookie.Value), module));
            }

            module.ChangeItselfInfo(new UserInfoBLL()
            {
                ID = Convert.ToInt32(cookie.Value),
                SurName = model.Surname,
                FirstName = model.Name,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Country = model.Country,
                Locality = model.Location,
                Gender = model.Gender
            });

            ViewBag.Result = "Success";
            return PartialView(EntityConverter.GetSettingInfoModel(Convert.ToInt32(cookie.Value), module));
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePassword()
        {
            IUserModule module = ApplicationHelper.GetUserModule(HttpContext.Request.Cookies["id"]);
            if (module == null)
                return RedirectToAction("Logout", "Account");

            string password = module.GetMyInfo.Password;

            return PartialView("ChangePassword", password);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string newPass, string repeatedPass)
        {
            IUserModule module = ApplicationHelper.GetUserModule(HttpContext.Request.Cookies["id"]);
            if (module == null)
                return RedirectToAction("Logout", "Account");

            if (newPass != repeatedPass)
            {
                var oldPassword = module.GetMyInfo.Password;
                ViewBag.Result = "Wrong repeated password";
                return PartialView("ChangePassword", oldPassword);
            }

            module.ChangePassword(newPass);
            ViewBag.Result = "Success";

            return PartialView("ChangePassword", newPass);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangeAvatar()
        {
            return PartialView("ChangeAvatar");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(HttpPostedFileBase upload)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = null;

            try
            {
                if (upload == null || upload.InputStream == null)
                    throw new ArgumentNullException("Uploaded stream is null");

                module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
                module.ChangeAvatar(upload.InputStream);
            }
            catch (ArgumentNullException)
            {
                return RedirectToAction("Home");
            }

            ViewBag.Result = "Success";
            return PartialView("ChangeAvatar");
        }

        [HttpGet]
        [Authorize]
        public ActionResult ChangePrivacy()
        {
            IUserModule module = ApplicationHelper.GetUserModule(HttpContext.Request.Cookies["id"]);
            if (module == null)
                return RedirectToAction("Logout", "Account");

            var user = module.GetMyInfo;
            return PartialView("ChangePrivacy", GetPrivacySettingAsStrings(user.IsOthersCanComment, user.IsOthersCanStartDialog, user.IsShowInfoForAnonymousUsers));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePrivacy(bool IsOthersCanComment, bool IsOthersCanStartDialog, bool IsShowInfoForAnonymousUsers)
        {
            IUserModule module = ApplicationHelper.GetUserModule(HttpContext.Request.Cookies["id"]);
            if (module == null)
                return RedirectToAction("Logout", "Account");

            module.ChangePrivacy(IsOthersCanComment, IsOthersCanStartDialog, IsShowInfoForAnonymousUsers);
            ViewBag.Result = "Success";

            return PartialView("ChangePrivacy", GetPrivacySettingAsStrings(IsOthersCanComment, IsOthersCanStartDialog, IsShowInfoForAnonymousUsers));
        }

        private List<string> GetPrivacySettingAsStrings(bool IsOthersCanComment, bool IsOthersCanStartDialog, bool IsShowInfoForAnonymousUsers)
        {
            return new List<string>()
            {
                IsOthersCanComment == true ? "true" : "false",
                IsOthersCanStartDialog == true ? "true" : "false",
                IsShowInfoForAnonymousUsers == true ? "true" : "false"
            };
        }

    }
}