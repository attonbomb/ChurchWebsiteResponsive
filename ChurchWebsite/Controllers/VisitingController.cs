using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ChurchWebsite.Controllers
{
    public class VisitingController : Controller
    {
        //
        // GET: /Visiting/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ServiceTimes()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "Visiting", action = "Index" }) + "#ServiceTimes");
        }

        public ActionResult WhatToExpect()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "Visiting", action = "Index" }) + "#WhatToExpect");
        }

        public ActionResult ProvForChildren()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "Visiting", action = "Index" }) + "#ProvisionForChildren");
        }

        public ActionResult WeeklyProg()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "Visiting", action = "Index" }) + "#WeeklyProgramme");
        }

        public ActionResult Contact()
        {
            //return View();
            return Redirect(Url.RouteUrl(new { controller = "Visiting", action = "Index" }) + "#Contact");
        }
    }
}
