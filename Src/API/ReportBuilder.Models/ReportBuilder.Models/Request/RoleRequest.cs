using Newtonsoft.Json;

namespace ReportBuilder.Models.Request
{
    public class RoleRequest : AuthorizorRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string AppType { get; set; }

        public RoleRequest Payload { get; set; }
    }
}
