using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

/*
 <copyright file="TaskModel.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>22-10-2018</date>
 <summary>
    Model for task attempt comment 
 </summary>
*/

namespace ReportBuilder.Models.Models
{
    public class TaskModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TaskId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CompanyId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? UserId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? WorkbookId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TaskCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TaskName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Comments { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CompletedTasksCount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? IncompletedTasksCount { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalTasks { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CompletionPrecentage { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IP { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Duration { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Score { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EvaluatorName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DateTaken { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Status { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompletionDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LastAttemptDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedBy { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string DeletedBy { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ParentTaskName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ChildTaskName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AssignedTo { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NumberofAttempts { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string ExpirationDate { get; set; }



        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AssignedDate { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CompanyName { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeName { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? AssignedQualification { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CompletedQualification { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CompletedUserQualification { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? IncompleteUserQualification { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? InDueQualification { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PastDueQualification { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? IncompleteQualification { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalEmployees { get; set; }



        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CompletedRoleQualification { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? SuspendedQualification { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? DisqualifiedQualification { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? InCompletedRoleQualification { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? RoleId { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CompletedCompanyQualification { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? InCompletedCompanyQualification { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalCompanyQualification { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Company { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Comment { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string LockoutReason { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LockoutCount { get; set; }

        [JsonIgnore]
        public string RoleStatus { get; set; }

        [JsonIgnore]
        public string TaskStatus { get; set; }
    }
}
