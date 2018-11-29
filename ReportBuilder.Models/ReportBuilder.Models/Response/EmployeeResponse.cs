using Newtonsoft.Json;
using ReportBuilder.Models.Models;


// <copyright file="EmployeeResponse.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
// <date>10-10-2018</date>
// <summary>Response Model for the employee</summary>
namespace ReportBuilder.Models.Response
{
    /// <summary>
    /// Resposne model to get the list of employees
    /// </summary>
    public class EmployeeResponse
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
