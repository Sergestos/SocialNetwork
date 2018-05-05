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
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.result = "Ваш логин: " + User.Identity.Name;
            }
            return View();
        }
    }
}