using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using System.Collections.Generic;

namespace ReportBuilder.Models.Response
{
    public class CompanyResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<CompanyModels> Companies { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ErrorResponse Error { get; set; }
    }
}
