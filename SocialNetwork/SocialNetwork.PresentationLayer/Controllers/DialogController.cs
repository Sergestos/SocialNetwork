using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.PresentationLayer.Controllers
{
    using SocialNetwork.BLL.BusinessLogic.Exceptions;
    using SocialNetwork.BLL.Models;
    using SocialNetwork.BLL.Modules.UserModule;
    using SocialNetwork.PresentationLayer.Infastructure;
    using SocialNetwork.PresentationLayer.Infastructure.EntityConverters;
    using SocialNetwork.PresentationLayer.Models;

    [Authorize]
    public class DialogController : Controller
    {
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
        [ValidateAntiForgeryToken]
        public RedirectResult CreateDialog(string name, bool isReadOnly, int? companion)
        {
            IUserModule module = ApplicationHelper.GetUserModule(HttpContext.Request.Cookies["id"]);
            if (module == null)
                return Redirect("/Account/Logout");

            module.StartDialog(name, isReadOnly, companion);

            return Redirect("/Dialog/DialogPreviews");
        }

        [HttpGet]
        [Authorize]
        public ActionResult DialogPreviews()
        {
            IUserModule module = ApplicationHelper.GetUserModule(HttpContext.Request.Cookies["id"]);
            if (module == null)
                return RedirectToAction("Logout", "Account");

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
            ViewBag.IsCanSendMessage = (!(!isMaster && dialog.IsReadOnly)).ToString().ToLower();
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
            IUserModule module = ApplicationHelper.GetUserModule(HttpContext.Request.Cookies["id"]);
            if (module == null)
                return RedirectToAction("Logout", "Account");

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

            return Redirect($"/Dialog/Dialog?dialogID={dialogID}");
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

        [HttpPost]
        [Authorize]
        public ActionResult AppointNewMaster(int dialogID, int userID)
        {
            IUserModule module = ApplicationHelper.GetUserModule(HttpContext.Request.Cookies["id"]);
            if (module == null)
                return RedirectToAction("Logout", "Account");

            module.AppointNewMaster(userID, dialogID);

            return RedirectToAction("DialogInfo", "Dialog", new { @dialogID = dialogID });
        }

        [HttpPost]
        [Authorize]
        public ActionResult RemoveFromDialog(int dialogID, int userID)
        {
            IUserModule module = ApplicationHelper.GetUserModule(HttpContext.Request.Cookies["id"]);
            if (module == null)
                return RedirectToAction("Logout", "Account");

            module.RemoveFromDialog(userID, dialogID);

            return RedirectToAction("DialogInfo", "Dialog", new { @dialogID = dialogID });
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddToDialog(int userID)
        {
            var cookie = HttpContext.Request.Cookies["id"];
            if (cookie == null)
                return Redirect("/Account/Logout");

            var module = new UserModule(MockResolver.GetUnitOfWorkFactory(), Convert.ToInt32(cookie.Value));
            var dialogs = module.GetDialogs
                .Where(x => x.MasterID == Convert.ToInt32(cookie.Value))
                .Select(z => EntityConverter.GetDialogInfo(z.ID, module));            

            ViewBag.Pretendent = userID;
            return View(dialogs);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddToDialog(int dialogID, int userID)
        {
            IUserModule module = ApplicationHelper.GetUserModule(HttpContext.Request.Cookies["id"]);
            if (module == null)
                return RedirectToAction("Logout", "Account");

            if (module.GetDialog(dialogID).Members.All(x => x.ID != userID)) 
                module.AddUserToDialog(userID, dialogID);

            HttpContext.Response.Cookies["lastDialogId"].Value = dialogID.ToString();
            return RedirectToAction("DialogPreviews", "Dialog", null);
        }

        [HttpGet]
        [Authorize]
        public ActionResult LeaveFromDialog(int dialogID)
        {
            IUserModule module = ApplicationHelper.GetUserModule(HttpContext.Request.Cookies["id"]);
            if (module == null)
                return RedirectToAction("Logout", "Account");

            module.LeaveFromDialog(dialogID);

            HttpContext.Response.Cookies["lastDialogId"].Value = "none";
            return RedirectToAction("DialogPreviews", "Dialog", null);
        }
    }
}