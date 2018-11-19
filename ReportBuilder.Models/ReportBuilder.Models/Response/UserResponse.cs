using Newtonsoft.Json;

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
        

    }
}
