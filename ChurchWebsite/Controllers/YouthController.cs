using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChurchWebsite.Controllers
{
    public class YouthController : Controller
    {
        //
        // GET: /Youth/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult YouthActivities()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "Youth", action = "Index" }) + "#YouthActivities");
        }

        public ActionResult KingsDaughters()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "Youth", action = "Index" }) + "#KingsDaughters");
        }

        public ActionResult YouthMinistries()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "Youth", action = "Index" }) + "#YouthMinistries");
        }

        public ActionResult YouthDays()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "Youth", action = "Index" }) + "#YouthDays");
        }

        public ActionResult SundaySchool()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "Youth", action = "Index" }) + "#SundaySchool");
        }
    }
}
