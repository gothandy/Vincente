﻿using System.Collections.Generic;
using Gothandy.Toggl.DataObjects;
using Gothandy.Toggl.Tables;

namespace Vincente.WebJobs.TogglToTask.Operations
{
    internal static class GetAllTogglTasks
    {
        internal static List<Task> Execute(TaskTable taskTable, List<Project> projects)
        {
            var tasks = new List<Task>();

            foreach (Project project in projects)
            {
                var projectTasks = taskTable.GetTasks((int)project.Id);

                if (projectTasks != null) tasks.AddRange(projectTasks);

                System.Threading.Thread.Sleep(1000);
            }

            return tasks;
        }
    }
}
