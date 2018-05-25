using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChurchWebsite.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /About/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Beliefs()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "About", action = "Index" }) + "#Beliefs");
        }

        public ActionResult Vision()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "About", action = "Index" }) + "#Vision");
        }

        public ActionResult MinistryTeam()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "About", action = "Index" }) + "#MinistryTeam");
        }

        public ActionResult History()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "About", action = "Index" }) + "#History");
        }

        public ActionResult Testimonies()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "About", action = "Index" }) + "#Testimonies");
        }

        public ActionResult DeclarationOfFaith()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "About", action = "Index" }) + "#DeclarationOfFaith");
        }
    }
}
