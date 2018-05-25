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

namespace ChurchWebsite.Areas.Admin.Data
{
    public class CRecurringEventFilter
    {
        public List<CEvent> AddReurringEventsBetweenDates(DateTime StartDate, DateTime EndDate, int currentMonth, List<CEvent> events, List<CEvent> recurringEvents)
        {
            //recurrance options: 1=Weekly 2=MonthlyOnThisDate 3=ThisDayOnThisWeekOfMonth 4=LastOcurranceOfThisDay
            foreach (CEvent recEvt in recurringEvents)
            {
                DateTime RecurringStartMinusTime = DateTime.Parse(recEvt.start.Value.ToShortDateString());
                switch (recEvt.recurranceType)
                {
                    case 1:
                        DateTime WeeklyEventDate = StartDate;
                        //get the first repetion of the Day of this recurring event
                        while (WeeklyEventDate.DayOfWeek != DateTime.Parse(recEvt.start.ToString()).DayOfWeek)
                        {
                            WeeklyEventDate = WeeklyEventDate.AddDays(1);
                        }
                        //now clone the event every 7 days
                        while (WeeklyEventDate < (DateTime)EndDate)
                        {

                            if (WeeklyEventDate.Month == currentMonth && WeeklyEventDate >= RecurringStartMinusTime)
                            {
                                CEvent myRecurranceClone = recEvt.DeepCopy();
                                myRecurranceClone.start = DateTime.Parse(WeeklyEventDate.ToShortDateString() + " " + recEvt.start.Value.ToShortTimeString());
                                myRecurranceClone.end = DateTime.Parse(WeeklyEventDate.ToShortDateString() + " " + recEvt.end.Value.ToShortTimeString());
                                myRecurranceClone.id = recEvt.id;
                                events = checkForSingleInstanceClone(events, myRecurranceClone);
                            }
                            WeeklyEventDate = WeeklyEventDate.AddDays(7);
                        }
                        break;
                    case 2:
                        DateTime MonthlyEventDate = StartDate;
                        DateTime RecurringMonthlyEvent = DateTime.Parse(recEvt.start.ToString());
                        while (MonthlyEventDate < (DateTime)EndDate)
                        {
                            if (MonthlyEventDate.Day == RecurringMonthlyEvent.Day)
                            {
                                if (MonthlyEventDate.Month == currentMonth && MonthlyEventDate >= RecurringStartMinusTime)
                                {
                                    CEvent myRecurranceClone = recEvt.DeepCopy();
                                    myRecurranceClone.start = DateTime.Parse(MonthlyEventDate.ToShortDateString() + " " + recEvt.start.Value.ToShortTimeString());
                                    myRecurranceClone.end = DateTime.Parse(MonthlyEventDate.ToShortDateString() + " " + recEvt.end.Value.ToShortTimeString());
                                    myRecurranceClone.id = recEvt.id;
                                    events = checkForSingleInstanceClone(events, myRecurranceClone);
                                }
                                MonthlyEventDate = MonthlyEventDate.AddMonths(1);
                            }
                            else
                            {
                                MonthlyEventDate = MonthlyEventDate.AddDays(1);
                            }
                        }
                        break;
                    case 3:
                        var currentYear = currentMonth != 1 ? StartDate.Year.ToString() : EndDate.Year.ToString();
                        DateTime FirstDayOfMonth = DateTime.Parse("01-" + currentMonth + "-" + currentYear + " " + recEvt.start.Value.ToShortTimeString());
                        //the changing date
                        DateTime MonthlyThisDayEventDate = FirstDayOfMonth;
                        DateTime RecurringMonthlyDayEvent = DateTime.Parse(recEvt.start.ToString());
                        int myTargetWeekInMonth = (RecurringMonthlyDayEvent.Day / 7);
                        //Which week of the month is it!!!!!!!!!!!!!!!!!!!!!!!!
                        int myCurrentWeekInMonth = 0;
                        //set the day for recurrance zero of day of the week
                        while (MonthlyThisDayEventDate.DayOfWeek != RecurringMonthlyDayEvent.DayOfWeek)
                        {
                            MonthlyThisDayEventDate = MonthlyThisDayEventDate.AddDays(1);
                        }

                        while (MonthlyThisDayEventDate < (DateTime)EndDate)
                        {
                            if (myTargetWeekInMonth == myCurrentWeekInMonth)
                            {
                                if (MonthlyThisDayEventDate.Month == currentMonth && MonthlyThisDayEventDate >= RecurringStartMinusTime)
                                {
                                    CEvent myRecurranceClone = recEvt.DeepCopy();
                                    myRecurranceClone.start = DateTime.Parse(MonthlyThisDayEventDate.ToShortDateString() + " " + recEvt.start.Value.ToShortTimeString());
                                    myRecurranceClone.end = DateTime.Parse(MonthlyThisDayEventDate.ToShortDateString() + " " + recEvt.end.Value.ToShortTimeString());
                                    myRecurranceClone.id = recEvt.id;
                                    events = checkForSingleInstanceClone(events, myRecurranceClone);
                                }
                                MonthlyThisDayEventDate = MonthlyThisDayEventDate.AddMonths(1);
                            }
                            else
                            {
                                MonthlyThisDayEventDate = MonthlyThisDayEventDate.AddDays(7);
                                myCurrentWeekInMonth++;
                            }
                        }
                        break;
                    case 4:
                        DateTime LastOccuranceEventDate = StartDate;
                        if (currentMonth != LastOccuranceEventDate.Month)
                        {
                            LastOccuranceEventDate = LastOccuranceEventDate.AddMonths(1);
                        }
                        int daysToEnd = DateTime.DaysInMonth(LastOccuranceEventDate.Year, LastOccuranceEventDate.Month) - LastOccuranceEventDate.Day;
                        LastOccuranceEventDate = LastOccuranceEventDate.AddDays(daysToEnd);
                        while (LastOccuranceEventDate.DayOfWeek != Convert.ToDateTime(recEvt.start).DayOfWeek)
                        {
                            LastOccuranceEventDate = LastOccuranceEventDate.AddDays(-1);
                        }
                        CEvent LastOccuranceClone = recEvt.DeepCopy();
                        LastOccuranceClone.start = DateTime.Parse(LastOccuranceEventDate.ToShortDateString() + " " + recEvt.start.Value.ToShortTimeString());
                        LastOccuranceClone.end = DateTime.Parse(LastOccuranceEventDate.ToShortDateString() + " " + recEvt.end.Value.ToShortTimeString());
                        LastOccuranceClone.id = recEvt.id;
                        events = checkForSingleInstanceClone(events, LastOccuranceClone);
                        break;
                }
            }
            //remove cancelled events from list now. These are recurring events that have been deleted
            //they are left in to hide the recurring event
            events.RemoveAll(evt => evt.cancelled == true);
            return events;
        }

        private List<CEvent> checkForSingleInstanceClone(List<CEvent> eventList, CEvent rEventToCheck)
        {
            var query = from evt in eventList
                        where evt.start.Value.Date == rEventToCheck.start.Value.Date && evt.recurranceRef == rEventToCheck.id
                        select evt;
            CEvent myEvent = query.FirstOrDefault();
            if (myEvent == null) eventList.Add(rEventToCheck);
            return eventList;
        }
    }
}