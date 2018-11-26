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
        public string EmployeeName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string FirstName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LastName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int UserId { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string WorkbookName { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        public int AssignedWorkBooks { get; set; }
        public int InDueWorkBooks { get; set; }
        public int PastDueWorkBooks { get; set; }
        public int CompletedWorkBooks { get; set; }
        public int EmployeeCount { get; set; }
        public int Code { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }


        

    }
}
