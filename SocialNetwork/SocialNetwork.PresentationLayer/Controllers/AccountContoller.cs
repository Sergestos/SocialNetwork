using SocialNetwork.PresentationLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.PresentationLayer.Controllers
{
    [Authorize]
    public class AccountContoller : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModelView user, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                
            }

            return null;
        }

        public RedirectResult Logout()
        {
            return null;
        }
    }
}