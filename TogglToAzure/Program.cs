﻿using System;
using Toggl.DataObjects;
using Toggl;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace TC.easyJet.Reporting
{
    class Program
    {
        static void Main(string[] args)
        {
            var accountKey = args[0];
            var apiKey = args[1];
            var connectionString = String.Format(
                "DefaultEndpointsProtocol=https;AccountName=tceasyjetreporting;AccountKey={0}",
                accountKey);

            var workspaceId = 605632;
            var clientId = 15242883;

            var since = new DateTime(2015, 11, 1);
            var until = new DateTime(2015, 12, 31);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("TimeEntries");

            //table.Delete();
            table.CreateIfNotExists();

            var workspace = new Workspace(apiKey, workspaceId);

            List<ReportTimeEntry> reportTimeEntries = workspace.GetReportTimeEntries(clientId, since, until);

            foreach (ReportTimeEntry timeEntry in reportTimeEntries)
            {

                TimeEntryEntity entity = new TimeEntryEntity(timeEntry.TaskId, timeEntry.Id);

                entity.ProjectId = timeEntry.ProjectId;
                entity.TaskName = timeEntry.TaskName;
                entity.Start = timeEntry.Start;
                entity.UserName = timeEntry.UserName;
                entity.Billable = timeEntry.Billable;

                TableOperation operation = TableOperation.InsertOrReplace(entity);

                table.Execute(operation);
            }
        }
    }
}
