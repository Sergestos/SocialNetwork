using SocialNetwork.BLL.BusinessLogic.ContentManagement;
using SocialNetwork.BLL.BusinessLogic.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Modules.UserModule;
using SocialNetwork.PresentationLayer.Infastructure;
using System;
using System.Collections.Generic;
using System.Linq;
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
            UserInfoBLL userInfo = null;

            try
            {
                int id = Convert.ToInt32(HttpContext.Request.Cookies["id"].Value);
                userInfo = new UserModule(MockResolver.GetUnitOfWorkFactory(), id).GetItselfInfo;
                ViewBag.AvatarPath = string.Concat(userInfo.Content.Path, userInfo.Content.Extension);
            }
            catch (Exception)
            {
                return RedirectToAction("Logout", "Account");
            }

            return View(userInfo);
        }

        [Authorize]
        public FileContentResult GetAvatar(int ownerID)
        {
            try
            {
                var content = new UserModule(MockResolver.GetUnitOfWorkFactory(), ownerID).GetItselfInfo.Content;

                if (content != null)
                {
                    byte[] file = System.IO.File.ReadAllBytes(content.Path);
                    return File(file, GetMimeType(content.Extension));
                }
            }
            catch (ArgumentException ex)
            {
                throw;
            }
            catch (BusinessEntityNullException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            byte[] defaultfile = System.IO.File.ReadAllBytes(ApplicationConstants.ImageIsNotFountDefaultImagePath);
            return File(defaultfile, ApplicationConstants.ImageIsNotFountDefaultImageMimeType);
        }

        private string GetMimeType(string extension)
        {
            string mime = "image/";

            if (extension == ".png")
                return mime + "png";

            if (extension == ".jpg")
                return mime + "jpeg";

            if (extension == "png" || extension == "jpeg")
                return "image/" + extension;

            if (extension == mime + "png" || extension == mime + "jpeg")
                return extension;

            throw new ArgumentException($"Extension \'{extension}\' is not avaibale for this operation");
        }
    }
}