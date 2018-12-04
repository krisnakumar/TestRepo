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

        public string DueDate { get; set; }

        public int WorkBookId { get; set; }

        public string CompletionDate { get; set; }


        public string WorkBookName { get; set; }
        public string AssignedWorkBook { get; set; }

        public string PastDuedWorkBook { get; set; }

        public string InCompleteWorkbook { get; set; }

        public string TotalWorkbook { get; set; }

        public string CompletedWorkbook { get; set; }


        public string WorkbookCreated { get; set; }

        public string CreatedBy { get; set; }

        public string DaysToComplete { get; set; }

        public string TaskCount { get; set; }

        public string UserCount { get; set; }


        public string UserName { get; set; }

        public string UserName2 { get; set; }


        
    }
}
