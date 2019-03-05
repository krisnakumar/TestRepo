using Newtonsoft.Json;


namespace ReportBuilder.Models.Response
{
    public class RoleModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Role { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? RoleId { get; set; }
    }
}
