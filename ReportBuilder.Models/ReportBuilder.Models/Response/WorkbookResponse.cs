using System;

namespace ReportBuilder.Models.Response
{
    public class WorkbookResponse
    {
        public int UserId { get; set; }

        public string EmployeeName { get; set; }

        public string Role { get; set; }

        public string WorkbookName { get; set; }

        public string CompletedTasks { get; set; }

        public int PercentageCompleted { get; set; }

        public DateTime DueDate { get; set; }
    }
}
