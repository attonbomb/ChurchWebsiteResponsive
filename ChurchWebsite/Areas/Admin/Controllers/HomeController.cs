using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChurchWebsite.Models;

namespace ChurchWebsite.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Admin/

        private ISiteAdminContext context;

        public HomeController()
        {
            context = new SiteAdminContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageEvents()
        {
            return View();
        }

        public ActionResult SermonManagement()
        {
            return View();
        }

        public ActionResult ManageBannerImages()
        {
            return View();
        }
    }
}
