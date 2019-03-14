using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using System.Collections.Generic;




/*
 <copyright file="TaskResponse.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>22-10-2018</date>
 <summary>
    Response model for task [ReportBuilder]
 </summary>
*/

namespace ReportBuilder.Models.Response
{
    public class TaskResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public List<TaskModel> Tasks { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public ErrorResponse Error { get; set; }
    }
}
