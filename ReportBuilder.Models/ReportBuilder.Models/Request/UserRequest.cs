using Newtonsoft.Json;

// <copyright file="UserRequest.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
// <date>10-10-2018</date>
// <summary>Class that handle the request user CRUD operations</summary>
namespace ReportBuilder.Models.Request
{
    /// <summary>
    /// Class that handle the request user CRUD operations
    /// </summary>
    public class UserRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }
    }
}
