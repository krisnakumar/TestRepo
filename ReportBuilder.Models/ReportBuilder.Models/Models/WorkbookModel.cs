﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuilder.Models.Models
{
    public class WorkbookModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? UserId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WorkbookName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WorkBookName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompletedTasks { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PercentageCompleted { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DueDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? WorkBookId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? RepsRequired { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? RepsCompleted { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompletionDate { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public int? AssignedWorkBook { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PastDueWorkBook { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? InDueWorkBook { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? InCompleteWorkBook { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalWorkbook { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalTasks { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CompletedWorkbook { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WorkbookCreated { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedBy { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DaysToComplete { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TaskCount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? UserCount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AlternateName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DateCreated { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string MiddleName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string City { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string State { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Zip { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TotalEmployees { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? EntityCount { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? NumberCompleted { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LastAttemptDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FirstAttemptDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Repetitions { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WorkbookAssignedDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WorkbookRemoved { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LastSignoffBy { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? WorkbookEnabled { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TaskCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TaskName { get; set; }
         [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TaskId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NumberofAttempts { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LastAttemptDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Comment { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EvaluatorName { get; set; }
       
        
    }
}
