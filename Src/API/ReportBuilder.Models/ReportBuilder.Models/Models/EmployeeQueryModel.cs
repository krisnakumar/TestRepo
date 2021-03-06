﻿using Newtonsoft.Json;


namespace ReportBuilder.Models.Models
{
    /// <summary>
    /// Model class that helps to query request for employee
    /// </summary>
    public class EmployeeQueryModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WorkbookName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? AssignedWorkBooks { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? InDueWorkBooks { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? PastDueWorkBooks { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? CompletedWorkBooks { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? EmployeeCount { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string EmployeeName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? UserId { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? SupervisorId { get; set; }



        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AlternateName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TotalEmployees { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Email { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Address { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Phone { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string SupervisorName { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string UserCreatedDate { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? Userpermission { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? SettingsPermission { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? CoursePermission { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? TranscriptPermission { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? CompanyPermission { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? ForumPermission { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? ComPermission { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? ReportsPermission { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? AnnouncementPermission { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? SystemPermission { get; set; }
    }
}
