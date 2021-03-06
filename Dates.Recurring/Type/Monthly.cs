﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

namespace Dates.Recurring.Type
{
    public class Monthly : RecurrenceType
    {
        public int DayOfMonth { get; set; }

        public Monthly(int skipMonths, int dayOfMonth, DateTime starting, DateTime? ending) : base(skipMonths, starting, ending)
        {
            DayOfMonth = dayOfMonth;
        }

        public override DateTime? Next(DateTime after)
        {
            var next = Starting;

            if (after.Date < Starting.Date)
            {
                after = Starting - 1.Days();
            }

            while (next.Date <= after.Date || !DayOfMonthMatched(next))
            {
                int dayOfMonth = Math.Min(DayOfMonth, DateTime.DaysInMonth(next.Year, next.Month));

                if (next.Day < dayOfMonth)
                {
                    next = next + 1.Days();
                }
                else
                {
                    // Rewind to the first of the month.
                    next = next + ((-1 * next.Day) + 1).Days();

                    // Skip ahead by the required number of months.
                    next = next.AddMonths(X);
                }
            }

            if (Ending.HasValue && next.Date >= Ending.Value.Date)
            {
                return null;
            }

            return next;
        }

        private bool DayOfMonthMatched(DateTime date)
        {
            int dayOfMonth = Math.Min(DayOfMonth, DateTime.DaysInMonth(date.Year, date.Month));
            return (date.Day == dayOfMonth);
        }

    }
}
