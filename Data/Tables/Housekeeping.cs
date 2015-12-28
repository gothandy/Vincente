﻿using System.Collections.Generic;
using System.Linq;
using Vincente.Data.Entities;
using Vincente.Data.Interfaces;

namespace Vincente.Data.Tables
{
    public class Housekeeping : ITableRead<CardWithTime>
    {
        private TimeEntriesByMonth timeEntriesByMonth;

        public Housekeeping(TimeEntriesByMonth timeEntriesByMonth)
        {
            this.timeEntriesByMonth = timeEntriesByMonth;
        }

        public IEnumerable<CardWithTime> Query()
        {
            return
                from timeEntry in timeEntriesByMonth.Query()
                where timeEntry.Housekeeping != null && timeEntry.Month > new System.DateTime(2015, 6, 30)
                select new CardWithTime()
                {
                    Month = timeEntry.Month,
                    Epic = "Housekeeping",
                    ListIndex = null,
                    ListName = null,
                    DomId = null,
                    Name = timeEntry.Housekeeping,
                    UserName = timeEntry.UserName,
                    Billable = timeEntry.Billable,
                    Invoice = timeEntry.Month
                };
        }
    }
}
