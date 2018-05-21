using Newtonsoft.Json.Linq;
using SocialNetwork.BLL.BusinessLogic.ContentManagement;
using SocialNetwork.BLL.BusinessLogic.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Modules.UserModule;
using SocialNetwork.PresentationLayer.Infastructure;
using SocialNetwork.PresentationLayer.Infastructure.EntityConverters;
using SocialNetwork.PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.PresentationLayer.Controllers
{  
    [Authorize]
    public class UserController : Controller
    {
        [Authorize]
        public ActionResult Home()
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if(cookie == null)
                return RedirectToAction("Logout", "Account");

            var module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
            var userView = EntityConverter.GetUserView(module.GetMyInfo);

            return View(userView);
        }

        [HttpGet]
        [Authorize]
        public FileContentResult GetAvatar(int ownerID)
        {
            byte[] file = null;
            string contentType = string.Empty;

            try
            {
                var content = new UserModule(MockResolver.GetUnitOfWorkFactory(), ownerID).GetMyInfo.Content;

                if (content != null)
                {
                    file = System.IO.File.ReadAllBytes(content.Path);
                    contentType = ApplicationHelper.GetMimeType(content.Extension);                    
                }
                else
                    throw new BusinessEntityNullException();
            }
            catch (ArgumentException)
            {
                file = System.IO.File.ReadAllBytes(ApplicationConstants.ImageIsNotFountDefaultImagePath);
                contentType = ApplicationConstants.ImageIsNotFountDefaultImageMimeType;
            }
            catch (BusinessEntityNullException)
            {
                file = System.IO.File.ReadAllBytes(ApplicationConstants.ImageIsNotFountDefaultImagePath);                
                contentType = ApplicationConstants.ImageIsNotFountDefaultImageMimeType;
            }   

            return File(file, contentType);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Search(int page = 0, string order = "asc", string filter = "FirstName")
        {           
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return RedirectToAction("Logout", "Account");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            var users = GetFilteredUsers(module.GetAllUsers.Where(x => x.ID != Convert.ToInt32(cookie.Value)), page, order, filter);
            ViewBag.IsLastPage = users.Count() < ApplicationConstants.QuantityOfUsersPerPage ? true : false;
            ViewBag.Page = page;

            return View(users);

        }

        [HttpGet]
        [Authorize]
        public ActionResult MyFollowers(int page = 0, string order = "asc", string filter = "FirstName")
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return RedirectToAction("Logout", "Account");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            var users = GetFilteredUsers(module.GetMyFollewers, page, order, filter);
            ViewBag.IsLastPage = users.Count() < ApplicationConstants.QuantityOfUsersPerPage ? true : false;
            ViewBag.Page = page;

            return View(users);
        }

        [HttpGet]
        [Authorize]
        public ActionResult FollowedUsers(int page = 0, string order = "asc", string filter = "FirstName")
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return RedirectToAction("Logout", "Account");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            var users = GetFilteredUsers(module.GetUsersIFollowing, page, order, filter);
            ViewBag.IsLastPage = users.Count() < ApplicationConstants.QuantityOfUsersPerPage ? true : false;
            ViewBag.Page = page;

            return View(users);
        }

        [HttpGet]
        [Authorize]
        public ActionResult BlockedUsers(int page = 0, string order = "asc", string filter = "FirstName")
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return RedirectToAction("Logout", "Account");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            var users = GetFilteredUsers(module.GetUsersAddedToBlackList, page, order, filter);
            ViewBag.IsLastPage = users.Count() < ApplicationConstants.QuantityOfUsersPerPage ? true : false;
            ViewBag.Page = page;

            return View(users);
        }

        private IEnumerable<UserView> GetFilteredUsers(IEnumerable<UserInfoBLL> seakingUsers, int page, string order, string filter)
        {
            IEnumerable<UserView> users = null;    
              
            users = seakingUsers
                .Skip(ApplicationConstants.QuantityOfUsersPerPage * page)
                .Take(ApplicationConstants.QuantityOfUsersPerPage)
                .Select(x => EntityConverter.GetUserView(x));

            users = SortUsers(users, filter, order);

            return users;
        }

        private IEnumerable<UserView> SortUsers(IEnumerable<UserView> users, string filter, string order)
        {
            var _users = users;

            Func<UserView, object> predicate;
            switch (filter)
            {
                case "FirstName": { predicate = x => x.FirstName; break; }
                case "SurName": { predicate = x => x.SurName; break; }
                case "Age": { predicate = x => x.Age; break; }
                case "Location": { predicate = x => x.Locality; break; }
                case "Country": { predicate = x => x.Country; break; }
                default: { predicate = x => x.FirstName; break; }
            }

            if (order == "desc")
                _users = users.OrderByDescending(predicate);
            else
                _users = users.OrderBy(predicate);

            return _users;
        }

        [HttpGet]
        [Authorize]
        public ActionResult UserPage(int id)
        {            
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return RedirectToAction("Logout", "Account");

            int userObserverID = Convert.ToInt32(cookie.Value);

            if(id == userObserverID)
                return RedirectToAction("Home", "User");

            UserView userView = null;
            try
            {
                var module = new UserModule(MockResolver.GetUnitOfWorkFactory(), id);

                userView = EntityConverter.GetUserView(module.GetMyInfo);

                if (module.GetUsersAddedToBlackList.FirstOrDefault(x => x.ID == userObserverID) != null)
                {
                    ViewBag.IsAnotherUserBlockedMe = true;
                    return RedirectToAction("Search");
                }

                if (module.GetMyFollewers.FirstOrDefault(x => x.ID == userObserverID) != null)
                    ViewBag.IsAnotherUserIsFollowedByMe = true;
                else
                    ViewBag.IsAnotherUserIsFollowedByMe = false;

                if (module.GetUsersAddedMeToBlackList.FirstOrDefault(x => x.ID == userObserverID) != null)
                    ViewBag.IsIBlockedThisUser = true;
                else
                    ViewBag.IsIBlockedThisUser = false;
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Search", "User");
            }
            catch (BusinessEntityNullException)
            {
                return RedirectToAction("Logout", "Account");
            }                                                                

            return View(userView);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Follow(int id)
        {
            string result = string.Empty;

            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return RedirectToAction("Logout", "Account");

            try
            {
                IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
                module.Follow(id);

                result = "Followed";
            }
            catch (BusinessEntityNullException ex)
            {
                result = ex.Message;
            }
            catch (BusinessLogicException ex)
            {
                result = ex.Message;
            }

            var anonym = new { Result = result };
            return Json(anonym, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Unfollow(int id)
        {
            string result = string.Empty;

            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return RedirectToAction("Logout", "Account");

            try
            {
                IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
                module.Unfollow(id);

                result = "Unfollowed";
            }
            catch (BusinessEntityNullException ex)
            {
                result = ex.Message;
            }
            catch (BusinessLogicException ex)
            {
                result = ex.Message;
            }

            var anonym = new { Result = result };
            return Json(anonym, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Block(int id)
        {
            string result = string.Empty;

            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return RedirectToAction("Logout", "Account");

            try
            {
                IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
                module.AddToBlackList(id);

                result = "Blocked";
            }
            catch (BusinessEntityNullException ex)
            {
                result = ex.Message;
            }
            catch (BusinessLogicException ex)
            {
                result = ex.Message;
            }

            var anonym = new { Result = result };
            return Json(anonym, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Unblock(int id)
        {
            string result = string.Empty;

            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return RedirectToAction("Logout", "Account");

            try
            {
                IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
                module.RemoveFromBlackList(id);

                result = "Unblocked";
            }
            catch (BusinessEntityNullException ex)
            {
                result = ex.Message;
            }
            catch (BusinessLogicException ex)
            {
                result = ex.Message;
            }

            var anonym = new { Result = result };
            return Json(anonym, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateDialog(int? companion)
        {
            if (companion != null)
                ViewBag.Companion = companion.Value;

            return View();
        }

        [HttpPost]
        [Authorize]
        public RedirectResult CreateDialog(string name, bool isReadOnly, int? companion)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
            module.StartDialog(name, isReadOnly, companion);
            return Redirect("/User/DialogPreviews");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DialogPreviews()
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return RedirectToAction("Logout", "Account");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            IEnumerable<DialogPreview> dialogPreview = new List<DialogPreview>();
            var dialogBLL = module.GetDialogs;
            if (dialogBLL != null)
                dialogPreview = dialogBLL.Select(x => EntityConverter.GetDialogPreview(x, module));

            var cookieLastDialogID = HttpContext.Request.Cookies["lastDialogId"];
            ViewBag.LastDialogID = cookieLastDialogID != null ? cookieLastDialogID.Value : "none";

            return View(dialogPreview);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Dialog(int dialogID)
        {
            var cookieUserID = HttpContext.Request.Cookies["id"];
            if (cookieUserID == null)
                return RedirectToAction("Logout", "Account");

            var cookieLastDialogID = HttpContext.Request.Cookies["lastDialogId"];
            if (cookieLastDialogID == null)
                HttpContext.Response.Cookies.Add(new HttpCookie("lastDialogId", "none"));
            else
                HttpContext.Response.Cookies["lastDialogId"].Value = dialogID.ToString();

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookieUserID.Value));

            var dialog = EntityConverter.GetDialog(dialogID, module);

            bool isMaster = dialog.MasterID == Convert.ToInt32(cookieUserID.Value);
            ViewBag.IsCanSendMessage = !(!isMaster && dialog.IsReadOnly);
            ViewBag.IsMaster = isMaster;

            return PartialView(dialog);
        }

        [HttpGet]
        [Authorize]
        public FileContentResult GetDialogContent(int contentID)
        {
            byte[] file = null;
            string contentType = string.Empty;

            try
            {
                var cookie = HttpContext.Request.Cookies["id"];
                if (cookie == null)
                    RedirectToAction("Logout", "Account");

                var content = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value)).GetContent(contentID);

                if (content != null)
                {
                    file = System.IO.File.ReadAllBytes(content.Path);
                    contentType = ApplicationHelper.GetMimeType(content.Extension);
                }
                else
                    throw new BusinessEntityNullException();
            }
            catch (BusinessEntityNullException)
            {
                file = System.IO.File.ReadAllBytes(ApplicationConstants.ImageIsNotFountDefaultImagePath);
                contentType = ApplicationConstants.ImageIsNotFountDefaultImageMimeType;
            }

            return File(file, contentType);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddMessageToDialog(int dialogID, string message, IEnumerable<HttpPostedFileBase> upload)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            if (upload != null)
            {
                var streams = upload
                    .Where(x => x != null && x.InputStream != null)
                    .Select(x => x.InputStream).ToList();
                module.SendMessage(dialogID, message, streams);
            }
            else
            {
                module.SendMessage(dialogID, message, null);
            }               

            return Redirect($"/User/Dialog?dialogID={dialogID}");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DialogInfo(int dialogID)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
            DialogBLL dialog = module.GetDialog(dialogID);

            var dialogInfo = EntityConverter.GetDialogInfo(dialogID, module);

            if (Convert.ToInt32(cookie.Value) == dialogInfo.MasterID)
                return View("DialogInfoMasterView", dialogInfo);

            return View("DialogInfoView", dialogInfo);
        }

        [HttpPost]
        [Authorize]
        public ActionResult DialogInfoMasterView(int dialogID, string dialogName, bool isReadOnly)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            module.ChangeDialogSetting(dialogID, dialogName, isReadOnly);
            var dialogInfo = EntityConverter.GetDialogInfo(dialogID, module);

            if (Convert.ToInt32(cookie.Value) == dialogInfo.MasterID)
                return View("DialogInfoMasterView", dialogInfo);

            return View("DialogInfoView", dialogInfo);
        }

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
        public ActionResult ChangePersonalInfo(SettingViewModel model)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

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
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            string password = module.GetMyInfo.Password;

            return PartialView("ChangePassword", password);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePassword(string newPass, string repeatedPass)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));            

            if(newPass != repeatedPass)
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
            catch (ArgumentNullException ex)
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
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            var user = module.GetMyInfo;
            return PartialView("ChangePrivacy", GetPrivacySettingAsStrings(user.IsOthersCanComment, user.IsOthersCanStartDialog, user.IsShowInfoForAnonymousUsers));
        }

        [HttpPost]
        [Authorize]
        public ActionResult ChangePrivacy(bool IsOthersCanComment, bool IsOthersCanStartDialog, bool IsShowInfoForAnonymousUsers)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

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
        
        [HttpPost]
        [Authorize]
        public ActionResult AppointNewMaster(int dialogID, int userID)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            module.AppointNewMaster(userID, dialogID);

            return RedirectToAction("DialogInfo", "User", new { @dialogID = dialogID });
        }

        [HttpPost]
        [Authorize]
        public ActionResult RemoveFromDialog(int dialogID, int userID)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));

            module.RemoveFromDialog(userID, dialogID);

            return RedirectToAction("DialogInfo", "User", new { @dialogID = dialogID });
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddToDialog(int userID)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            var module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
            var dialogs = module.GetDialogs.Where(x => x.MasterID == Convert.ToInt32(cookie.Value)).Select(y => EntityConverter.GetDialogInfo(y.ID, module));

            ViewBag.Pretendent = userID;
            return View(dialogs);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddToDialog(int dialogID, int userID)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = null;

            try
            {
                module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
                module.AddUserToDialog(userID, dialogID);                                               
            }
            catch (BusinessAdmissionException ex)
            {
                return RedirectToAction("ErrorMessage", "User", new { @errorMessage = ex.Message });
            }

            HttpContext.Response.Cookies["lastDialogId"].Value = dialogID.ToString();
            return RedirectToAction("DialogPreviews", "User", null);
        }

        [HttpGet]
        [Authorize]
        public ActionResult LeaveFromDialog(int dialogID)
        {

            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            IUserModule module = null;

            try
            {
                module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
                module.LeaveFromDialog(dialogID);
            }
            catch (BusinessAdmissionException ex)
            {
                return RedirectToAction("ErrorMessage", "User", new { @errorMessage = ex.Message });
            }

            HttpContext.Response.Cookies["lastDialogId"].Value = "none";
            return RedirectToAction("DialogPreviews", "User", null);
        }

        [HttpGet]
        [Authorize]
        public ActionResult ErrorMessage(string errorMessage)
        {
            return View(errorMessage);
        }
    }
}