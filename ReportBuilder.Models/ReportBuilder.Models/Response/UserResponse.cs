using Newtonsoft.Json;


/*
 <copyright file="UserResponse.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>10-10-2018</date>
 <summary>
    Response model for user login / refresh token
 </summary>
*/

namespace ReportBuilder.Models.Response
{
    public class UserResponse
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Code { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string IdentityToken { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public string UserName { get; set; }

    }
}
