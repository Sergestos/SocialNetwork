using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialNetwork.PresentationLayer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.result = "Ваш логин: " + User.Identity.Name;
            }
            return View();
        }
    }
}