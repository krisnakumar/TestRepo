using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OnBoardLMS.WebAPI.Models
{
    public class LoginDetail
    {
        [Key]
        [JsonProperty(PropertyName = "userid")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "fullname")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "companyid")]
        public int CompanyId { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "testsessionid")]
        public int TestSessionId { get; set; }

        [JsonProperty(PropertyName = "reqchange")]
        public bool ReqChange { get; set; }

    }
}