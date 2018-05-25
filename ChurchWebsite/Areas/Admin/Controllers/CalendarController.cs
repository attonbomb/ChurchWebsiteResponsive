using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using ChurchWebsite.Models;
using System.Globalization;
using Newtonsoft.Json;
using System.Data.Entity.Validation;
using ChurchWebsite.Areas.Admin.Utilities.Data;
using ChurchWebsite.Areas.Admin.Data;

namespace ChurchWebsite.Areas.Admin.Controllers
{
    public class CalendarController : Controller
    {
        private ISiteAdminContext context;

        public CalendarController()
        {
            context = new SiteAdminContext();
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        [HttpGet]
        public ActionResult _EventDialogue(string evtDate)
        {
            //may have to pass view and have a switch statement as in day view time will be selectable
            CEvent myEvent = new CEvent();
            myEvent.EventDate = DateTime.Parse(evtDate);
            //myEvent.end = DateTime.Parse(evtDate);
            return PartialView("_EventDialogue", myEvent);
        }

        [HttpGet]
        public ActionResult _EventEdit(int evtId, int ignoreRecChk = 0)
        {
            //may have to pass view and have a switch statement as in day view time will be selectable
            var query = from evt in context.CEvents
                        where evt.id == evtId
                        select evt;

            CEvent myEvent = query.First();
            //set not Mapped properties Event Date / StartTime / EndTime
            myEvent.EventDate = DateTime.Parse(myEvent.start.ToString());
            myEvent.StartTime = DateTime.Parse(myEvent.start.ToString()).TimeOfDay;
            myEvent.EndTime = DateTime.Parse(myEvent.end.ToString()).TimeOfDay;
            //clear the time of the event
            myEvent.EventDate = myEvent.EventDate.Subtract(myEvent.EventDate.TimeOfDay);
            if (myEvent.recurranceType != 0 && ignoreRecChk != 1)
            {
                return PartialView("_RecurrancePartial", myEvent);
            }
            return PartialView("_EventDialogue", myEvent);
        }

        [HttpGet]
        public ActionResult _EventCreateInstance(int evtId, string evtDate)
        {
            var query = from evt in context.CEvents
                        where evt.id == evtId
                        select evt;
            //clone the event
            CEvent myEvent = query.First();
            CEvent myClonedEvent = myEvent.DeepCopy();
            //set the recurring to false
            myClonedEvent.Recurring = false;
            myClonedEvent.recurranceType = (int)Recurrance.None;
            //the existence of this ref will also hide recurrance options from the form
            myClonedEvent.recurranceRef = myEvent.id;

            myClonedEvent.EventDate = DateTime.Parse(evtDate);
            //reset the start and end datetime values

            string myNewStartDate = myClonedEvent.EventDate.ToShortDateString() + " " + DateTime.Parse(myEvent.start.ToString()).ToShortTimeString();
            myClonedEvent.start = DateTime.Parse(myNewStartDate);

            string myNewEndDate = myClonedEvent.EventDate.ToShortDateString() + " " + DateTime.Parse(myEvent.end.ToString()).ToShortTimeString();
            myClonedEvent.end = DateTime.Parse(myNewEndDate);

            myClonedEvent.id = context.CEvents.OrderByDescending(evt => evt.id).FirstOrDefault().id + 1;

            int myNewEventId;
            try
            {
                context.Add<CEvent>(myClonedEvent);
                myNewEventId = context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        ModelState.AddModelError(ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }

            return PartialView("_EventDialogue", myClonedEvent);
        }

        [HttpGet]
        public object MonthEvents(string start, string end, string currentDate) // Not List<Item> 
        {
            List<CEvent> events;
            List<CEvent> recurringEvents;
            CRecurringEventFilter myRecurringEventFilter;
            
            DateTime myStartParam = DateTime.ParseExact(start, "yyyy-MM-dd", CultureInfo.CurrentCulture);
            DateTime myEndParam = DateTime.ParseExact(end, "yyyy-MM-dd", CultureInfo.CurrentCulture);

            int currentMonth;

            //DateTime myStartParam = new DateTime(1970, 1, 1).AddSeconds(int.Parse(start));
            //DateTime myEndParam = new DateTime(1970, 1, 1).AddSeconds(int.Parse(end));

            //Sort out the current month
            if (myStartParam.Month + 2 == myEndParam.Month)
            {
                currentMonth = myStartParam.Month + 1;
            }
            else
            {
                currentMonth = myStartParam.Month;
            }

            if (myStartParam.Month > myEndParam.Month)
            {
                currentMonth = 12;
            }

            if (myStartParam.Month == 12 && myEndParam.Month < 12)
            {
                currentMonth = 1;
            }

            events = (from evt in context.CEvents
                      where evt.start.Value >= myStartParam && evt.end.Value <= myEndParam && evt.recurranceType == 0
                      select evt).ToList();

            recurringEvents = (from evt in context.CEvents
                               where evt.recurranceType > 0 && myEndParam >= evt.start.Value
                               select evt).ToList();

            myRecurringEventFilter = new CRecurringEventFilter();
            events = myRecurringEventFilter.AddReurringEventsBetweenDates(myStartParam, myEndParam, currentMonth, events, recurringEvents);

            return Json(JsonConvert.SerializeObject(events), JsonRequestBehavior.AllowGet).Data;
        }

        
        [HttpPost]
        public ActionResult Create(CEvent myEvent)
        {
            //add the times of the the event to the date
            myEvent.start = myEvent.EventDate + myEvent.StartTime;
            myEvent.end = myEvent.EventDate + myEvent.EndTime;

            //add other Event propeties createdBy etc...

            //Do we try and add the image here then do check on model valid state?
            try
            {
                context.Add<CEvent>(myEvent);
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    //Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        //Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                        //ve.PropertyName, ve.ErrorMessage);
                        ModelState.AddModelError(ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_EventDialogue", myEvent);
            }
            else
            {
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Home");
                return Json(new { Url = redirectUrl });
            }
        }

        [HttpPost]
        public ActionResult Update(CEvent myEvent, HttpPostedFileBase image)
        {
            //have IEvent then determine whether or not event is recurring
            //add the times of the the event to the date
            myEvent.start = myEvent.EventDate + myEvent.StartTime;
            myEvent.end = myEvent.EventDate + myEvent.EndTime;

            if (!ModelState.IsValid)
            {
                return PartialView("_EventDialogue", myEvent);
            }
            else
            {
                //if (image != null)
                //{
                //photo.ImageMimeType = image.ContentType;
                //photo.PhotoFile = new byte[image.ContentLength];
                //image.InputStream.Read(photo.PhotoFile, 0, image.ContentLength);
                //}
                //
                CEvent original = context.FindEventById(myEvent.id);
                if (original != null)
                {
                    //context.Entry(original).CurrentValues.SetValues(myEvent);
                    context.Update(original, myEvent);
                    context.SaveChanges();
                }
                var redirectUrl = new UrlHelper(Request.RequestContext).Action("ManageEvents", "Home");
                return Json(new { Url = redirectUrl });
            }
        }

        [HttpGet]
        public ActionResult _DeleteEventRequested(int evtId)
        {
            CEvent myEvent = context.FindEventById(evtId);
            if (myEvent == null)
            {
                return HttpNotFound();
            }
            return PartialView("_EventDeleteConfirm", myEvent);
        }

        [HttpPost]
        public ActionResult _DeleteEvent(int id)
        {
            CEvent myEvent = context.FindEventById(id);

            //check for recurrane ref
            if (myEvent.recurranceRef != null)
            {
                myEvent.cancelled = true;
                context.SaveChanges();
            }
            else
            {
                context.Delete<CEvent>(myEvent);
                context.SaveChanges();
            }
            var redirectUrl = new UrlHelper(Request.RequestContext).Action("Index", "Home");
            return Json(new { Url = redirectUrl });
        }
    }
}
