using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChurchWebsite.Areas.Admin.Utilities.Data
{
    public class CCalendarUtils
    {
    }

    public enum Recurrance
    {
        None,
        Weekly,
        MonthlySpecificDate,
        MonthlySpecificDay,
        MonthlyLastDayPosition
    }

    public enum RecurrenceEnum
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4,
        Fifth = 5
    }
}