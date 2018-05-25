using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChurchWebsite.Controllers
{
    public class WorkingChurchController : Controller
    {
        //
        // GET: /WorkingChurch/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult International()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "WorkingChurch", action = "Index" }) + "#International");
        }

        public ActionResult InTheCommunity()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "WorkingChurch", action = "Index" }) + "#InTheCommunity");
        }

        public ActionResult InChurch()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "WorkingChurch", action = "Index" }) + "#InChurch");
        }

        public ActionResult AdultMinistries()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "WorkingChurch", action = "Index" }) + "#AdultMinistries");
        }

        public ActionResult AdultEducation()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "WorkingChurch", action = "Index" }) + "#AdultEducation");
        }

        public ActionResult GetInvolved()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "WorkingChurch", action = "Index" }) + "#GetInvolved");
        }
    }
}
