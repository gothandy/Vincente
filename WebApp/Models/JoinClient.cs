﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Vincente.Azure;
using Vincente.Azure.Entities;
using Vincente.Azure.Tables;
using Vincente.Data.Entities;
using Vincente.Data.Tables;

namespace WebApp.Models
{
    public class JoinClient
    {
        private CardTable cardTable;
        private ITimeEntryTable timeEntryTable;

        public JoinClient(CardTable cardTable, ITimeEntryTable timeEntryTable)
        {
            this.cardTable = cardTable;
            this.timeEntryTable = timeEntryTable;
        }

        public IEnumerable<JoinModel> GetJoinedData()
        {

            return from timeEntry in timeEntryTable.Query()
                   join card in cardTable.Query()
                   on timeEntry.DomId equals card.DomId
                   where card.Invoice == null
                   select new JoinModel()
                   {
                       ListIndex = card.ListIndex,
                       ListName = card.ListName,
                       Epic = card.Epic,
                       CardId = card.RowKey,
                       DomId = card.DomId,
                       Name = card.Name,
                       Month = timeEntry.Month,
                       UserName = timeEntry.UserName,
                       Billable = timeEntry.Billable
                   };
        }

        public IEnumerable<TimeEntry> GetTimeEntriesByMonth()
        {
            var data = timeEntryTable.Query();

            return GroupByMonth(data);
        }

        public IEnumerable<CardEntity> GetCards()
        {
            return cardTable.Query();
        }

        public IEnumerable<JoinModel> GetHousekeeping()
        {
            return
                from timeEntry in GroupByMonth(timeEntryTable.Query())
                where timeEntry.Housekeeping != null && timeEntry.Month > new System.DateTime(2015, 6, 30)
                select new JoinModel()
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

        public IEnumerable<JoinModel> GetStories()
        {
            return
                from timeEntry in GroupByMonth(timeEntryTable.Query())
                join card in cardTable.Query()
                on timeEntry.DomId equals card.DomId
                orderby timeEntry.Month, card.Epic, card.ListIndex, card.Name, timeEntry.UserName
                select new JoinModel()
                {
                    Month = timeEntry.Month,
                    Epic = card.Epic,
                    ListIndex = card.ListIndex,
                    ListName = card.ListName,
                    CardId = card.RowKey,
                    DomId = timeEntry.DomId,
                    Name = card.Name,
                    UserName = timeEntry.UserName,
                    Billable = timeEntry.Billable,
                    Invoice = card.Invoice
                };
        }

        public IEnumerable<TimeEntry> GetOrphans()
        {
            return
                from timeEntry in timeEntryTable.Query()
                where
                    timeEntry.Housekeeping == null &&
                    timeEntry.DomId == null &&
                    timeEntry.Month > new System.DateTime(2015, 6, 30)
                orderby timeEntry.Start
                select new TimeEntry()
                { 
                    Start = timeEntry.Start,
                    Housekeeping = timeEntry.Housekeeping,
                    UserName = timeEntry.UserName,
                    Billable = timeEntry.Billable,
                    TaskId = timeEntry.TaskId,
                };
        }

        private static IEnumerable<TimeEntry> GroupByMonth(IEnumerable<TimeEntry> query)
        {
            var result =
                from e in query
                group e by new
                {
                    e.Month,
                    e.UserName,
                    e.DomId,
                    e.Housekeeping

                } into g
                select new TimeEntry()
                {
                    Month = g.Key.Month,
                    UserName = g.Key.UserName,
                    DomId = g.Key.DomId,
                    Housekeeping = g.Key.Housekeeping,
                    Billable = g.Sum(e => e.Billable)
                };

            return result;
        }
    }
}