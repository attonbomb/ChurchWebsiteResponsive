using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChurchWebsite.Models;
using ChurchWebsite.Areas.Admin.Data;

namespace ChurchWebsite.Controllers
{
    public class HomeController : Controller
    {
        private ISiteAdminContext context;

        public HomeController()
        {
            context = new SiteAdminContext();
        }
        
        public ActionResult Index()
        {
            ViewBag.Message = "New Testament Church of God (Leeds) ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Visting()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Youth()
        {
            ViewBag.Message = "Youth Page";

            return View();
        }

        public ActionResult WorkingChurch()
        {
            ViewBag.Message = "Working Church Page";

            return View();
        }

        public ActionResult Media()
        {
            ViewBag.Message = "Sermons and Media Page";

            return View();
        }

        public ActionResult _UpcomingEvents(int mobile = 0)
        {
            List<CEvent> myUpcomingEvents;
            List<CEvent> events;
            List<CEvent> recurringEvents;
            CRecurringEventFilter myRecurringEventFilter;

            ViewBag.Mobile = mobile;

            DateTime myStartParam = DateTime.Now;
            DateTime myEndParam = DateTime.Parse(myStartParam.ToShortDateString()).AddMonths(2);

            events = (from evt in context.CEvents
                      where evt.start.Value >= myStartParam && evt.end.Value <= myEndParam && evt.recurranceType == 0
                      select evt).ToList();

            recurringEvents = (from evt in context.CEvents
                               where evt.recurranceType > 0 && myEndParam >= evt.start.Value
                               select evt).ToList();

            myRecurringEventFilter = new CRecurringEventFilter();
            //Add Start month events
            events = myRecurringEventFilter.AddReurringEventsBetweenDates(myStartParam, myEndParam, myStartParam.Month, events, recurringEvents);
            //Add End month events
            events = myRecurringEventFilter.AddReurringEventsBetweenDates(myStartParam, myEndParam, myEndParam.Month, events, recurringEvents);
            events.RemoveAll(e => Convert.ToDateTime(e.start) < myStartParam);
            events = events.OrderBy(evt => Convert.ToDateTime(evt.start)).ToList();

            return PartialView("_UpcomingEvents", events);
        }

        public FileContentResult GetTodaysEventImage()
        {
            List<CEvent> events;
            List<CEvent> recurringEvents;
            CRecurringEventFilter myRecurringEventFilter;
            
            DateTime myStartParam = DateTime.Now;
            DateTime myEndParam = DateTime.Parse(myStartParam.ToShortDateString()).AddDays(1);

            events = (from evt in context.CEvents
                      where evt.start.Value >= myStartParam && evt.end.Value <= myEndParam && evt.recurranceType == 0
                      select evt).ToList();

            recurringEvents = (from evt in context.CEvents
                               where evt.recurranceType > 0 && myEndParam >= evt.start.Value
                               select evt).ToList();

            if(events == null){
                events = new List<CEvent>();
            }

            myRecurringEventFilter = new CRecurringEventFilter();
            events = myRecurringEventFilter.AddReurringEventsBetweenDates(myStartParam, myEndParam, myStartParam.Month, events, recurringEvents);
            events = events.FindAll(evt => Convert.ToDateTime(evt.start).Date == myStartParam.Date);


            int myEventImageId = 10;//this is the placeholder image id

            if (events.Count > 0)
            {
                foreach (CEvent evt in events)
                {
                    if (evt.imgId != null)
                    {
                        myEventImageId = Convert.ToInt32(evt.imgId);
                        break;
                    }
                }
            }
            CImage myImage = context.FindPhotoById(myEventImageId);
            return File(myImage.imageFile, myImage.imageMimeType);
        }
    }
}
