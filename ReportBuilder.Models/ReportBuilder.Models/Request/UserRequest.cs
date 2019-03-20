using Newtonsoft.Json;


/*
 <copyright file="UserRequest.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>10-10-2018</date>
 <summary>
    Model for login/refresh token request object
 </summary>
*/
namespace ReportBuilder.Models.Request
{
    public class UserRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string UserName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Password { get; set; }


        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string RefreshToken { get; set; }

        public int CompanyId { get; set; }

        public string CognitoPoolId { get; set; }

        public string CognitoClientId { get; set; }
    }
}
