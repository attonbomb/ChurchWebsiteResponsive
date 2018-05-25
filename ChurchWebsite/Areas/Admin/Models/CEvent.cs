using System;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ChurchWebsite.Areas.Admin.Utilities.Data;
using ChurchWebsite.Areas.Admin.Validation;
using System.Web.Helpers;

namespace ChurchWebsite.Models
{   
    public class CEventMetaData
    {
        [DisplayName("Event Title")]
        [Required]
        public string title { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(300)]
        [RequiredIf("showInCal", true, ErrorMessage = "If you are displaying this event in upcoming events you require a description")]
        public string description { get; set; }

        [DisplayName("All Day Event")]
        public bool allDay { get; set; }

        [RequiredIf("showOnHome", true, ErrorMessage = "An image is required to display this event on the home screen")]
        public Nullable<int> imgId { get; set; }

        [DisplayName("Show In Upcoming Events")]
        public bool showInCal { get; set; }
    }

    [MetadataType(typeof(CEventMetaData))]
    public partial class CEvent
    {
        private List<SelectListItem> DefaultRecurranceOptions = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "None" }, new SelectListItem { Value = "1", Text = "Weekly" }, new SelectListItem { Value = "2", Text = "Monthly On this Date" } };
        private Boolean isRecurring;

        [NotMapped]
        public Recurrance? Recurrance { get; set; }

        [Required]
        [NotMapped]
        public TimeSpan StartTime { get; set; }

        [Required]
        [NotMapped]
        public TimeSpan EndTime { get; set; }

        [NotMapped]
        public DateTime EventDate { get; set; }

        [NotMapped]
        public Boolean Recurring
        {
            get
            {
                return recurranceType > 0;
            }
            set
            {
                isRecurring = value;
            }
        }

        [NotMapped]
        [DisplayName("Event home screen image")]
        public string imageTitle { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> CurrentRecurranceOptions
        {
            get
            {
                return GetRecurranceOptsFromDate();
            }
        }

        public CEvent()
        {

        }

        public CEvent(CEvent cloneSource = null)
        {

        }

        public CEvent DeepCopy()
        {
            CEvent evtClone = (CEvent)CDataUtilities.ShallowCopyEntity(this);
            return evtClone;
        }

        private IEnumerable<SelectListItem> GetRecurranceOptsFromDate()
        {
            DayOfWeek myRecurringDay = EventDate.DayOfWeek;
            bool isLast = false;

            RecurrenceEnum myRecurrance = GetRecurrence(EventDate.Year, EventDate.Month, EventDate.Day, out isLast);

            if (myRecurrance < RecurrenceEnum.Fifth)
            {
                string dynamicOptionString = "The " + myRecurrance + " " + EventDate.DayOfWeek + " of every Month.";
                DefaultRecurranceOptions.Add(new SelectListItem { Value = "3", Text = dynamicOptionString });
            }

            if (isLast)
            {
                string dynamicOptionString = "The last " + EventDate.DayOfWeek + " of every Month.";
                DefaultRecurranceOptions.Add(new SelectListItem { Value = "4", Text = dynamicOptionString });
            }

            return new SelectList(DefaultRecurranceOptions, "Value", "Text");
        }

        private RecurrenceEnum GetRecurrence(int year, int month, int day, out bool isLast)
        {
            isLast = false; //set the out param, defaulted to false            
            int numberOfDaysInMonth = DateTime.DaysInMonth(year, month);
            int firstDayOfMonth = (int)new DateTime(year, month, 1).DayOfWeek;
            int checkDayOfWeek = (int)new DateTime(year, month, day).DayOfWeek;
            int lastWeekInMonth = (numberOfDaysInMonth / 7);
            int sevenDaysFromEnd = (numberOfDaysInMonth - 7);
            int checkWeekInMonth = (day / 7);

            //not directly divisible (i.e. not Feb (28 days))
            if (numberOfDaysInMonth % 7 != 0)
            {
                lastWeekInMonth += 1;
            }
            //day (date) not directly divisible by 7 (i.e. not 7th, 14th etc), so add 1
            if (day % 7 != 0)
            {
                checkWeekInMonth += 1;
            }
            //is this the last week in the month?
            if (day > sevenDaysFromEnd)
            {
                isLast = true;
            }
            //if the month started with a weekend day or a day which comes before the check date then
            //we just return the week (i.e. second week == second recurrence)
            /*if (firstDayOfMonth == 0 || firstDayOfMonth == 6 || firstDayOfMonth <= checkDayOfWeek)
            {*/
            return (RecurrenceEnum)checkWeekInMonth;
            /*}
            //else it means we have an extra recurrence before the day requested, so add one recurrence
            else
            {
                return (RecurrenceEnum)checkWeekInMonth + 1;
            }*/
        }
    }
}