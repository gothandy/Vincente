﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vincente.Azure.Entities;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Vincente.Data.Tables;
using Vincente.Formula;
using Vincente.Toggl.DataObjects;

namespace TogglConsoleApp
{
    public class TogglToData
    {
        public static void Execute(ITimeEntryTable timeEntryTable, List<ReportTimeEntry> togglTimeEntries)
        {
            foreach (ReportTimeEntry togglTimeEntry in togglTimeEntries)
            {
                TimeEntry timeEntry = GetTimeEntry(togglTimeEntry);

                timeEntryTable.BatchInsertOrReplace(timeEntry);
            }

            timeEntryTable.ExecuteBatch();
        }

        public static TimeEntry GetTimeEntry(ReportTimeEntry togglTimeEntry)
        {
            if (togglTimeEntry.Start == null) throw new ArgumentNullException("Start");
            if (togglTimeEntry.Id == null) throw new ArgumentException("Id");
            if (togglTimeEntry.TaskId == null) throw new ArgumentException("TaskId");
            if (togglTimeEntry.Billable == null) throw new ArgumentException("Billable");

            return new TimeEntry()
            {
                Id = togglTimeEntry.Id.GetValueOrDefault(),
                Start = togglTimeEntry.Start.GetValueOrDefault(),
                TaskId = togglTimeEntry.TaskId.GetValueOrDefault(),
                UserName = togglTimeEntry.UserName,
                Billable = togglTimeEntry.Billable.GetValueOrDefault(),
                DomId = FromName.GetDomID(togglTimeEntry.TaskName),
                Housekeeping = FromProject.IfHouseKeepingReturnTaskName(togglTimeEntry.ProjectId, togglTimeEntry.TaskName)
            };
        }

        private static DateTime MonthFromStart(DateTime? start)
        {
            return new DateTime(start.GetValueOrDefault().Year, start.GetValueOrDefault().Month, 1);
        }
    }
}