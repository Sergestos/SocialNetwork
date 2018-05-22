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
    }
}