﻿using Gothandy.Tables.Interfaces;
using System;

namespace Vincente.Data.Entities
{
    public class TimeEntry : ICompare<TimeEntry>
    {
        public long Id { get; set; }
        public long? TaskId { get; set; }
        public string DomId { get; set; }
        public DateTime Start { get; set; }
        public DateTime Month { get; set; }
        public DateTime Week { get; set; }
        public long? UserId { get; set; }
        public string UserName { get; set; }
        public string TeamName { get; set; }
        public string Description { get; set; }
        public decimal Billable { get; set; }
        public string Housekeeping { get; set; }
        public DateTime Timestamp { get; set; }

        public bool ValueEquals(TimeEntry other)
        {
            if (this.Id != other.Id) return false;
            if (this.TaskId != other.TaskId) return false;
            if (this.DomId != other.DomId) return false;
            if (this.Start != other.Start) return false;
            if (this.UserId != other.UserId) return false;
            if (this.UserName != other.UserName) return false;
            if (this.TeamName != other.TeamName) return false;
            if (this.Description != other.Description) return false;
            if (this.Billable != other.Billable) return false;
            if (this.Housekeeping != other.Housekeeping) return false;

            //Don't test for Month or Week as they are derived from Start.

            return true;
        }

        public bool KeyEquals(TimeEntry other)
        {
            if (this.Id != other.Id) return false;

            return true;
        }
    }
}
