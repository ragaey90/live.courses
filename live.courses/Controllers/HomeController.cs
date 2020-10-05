using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace live.courses.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult chat()
        {
            return View();
        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult profile()
        {
            
            var user = User.Identity;
            return View();
        }
    }
}
