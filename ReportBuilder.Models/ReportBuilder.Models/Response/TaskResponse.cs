using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuilder.Models.Response
{
    public class TaskResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TaskId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? UserId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? WorkbookId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TaskCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string TaskName { get; set; }
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
        public string DateTaken{ get; set; }

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


    }
}
