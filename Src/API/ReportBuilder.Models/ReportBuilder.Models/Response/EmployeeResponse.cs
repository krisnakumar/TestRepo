using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using System.Collections.Generic;


/*
 <copyright file="EmployeeResponse.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>10-10-2018</date>
 <summary>
    Response model for employee [ReportBuilder]
 </summary>
*/

namespace ReportBuilder.Models.Response
{
    public class EmployeeResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ErrorResponse Error { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<EmployeeQueryModel> Employees { get; set; }
    }
}
