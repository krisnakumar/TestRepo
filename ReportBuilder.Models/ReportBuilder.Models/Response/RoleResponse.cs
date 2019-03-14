

using Newtonsoft.Json;
using System.Collections.Generic;


namespace ReportBuilder.Models.Response
{
    public class RoleResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<RoleModel> Roles { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ErrorResponse Error { get; set; }
    }
}
