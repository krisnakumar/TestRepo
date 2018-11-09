using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuilder.Models.Response
{
    public class TaskResponse
    {
        public int TaskId { get; set; }

        public int UserId { get; set; }

        public int WorkbookId { get; set; }

        public string TaskCode { get; set; }

        public string TaskName { get; set; }

        public int CompletedTasksCount { get; set; }

        public int IncompletedTasksCount { get; set; }

        public int TotalTasks { get; set; }

        public int CompletionPrecentage { get; set; }
    }
}
