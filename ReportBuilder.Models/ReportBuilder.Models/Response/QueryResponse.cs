using Newtonsoft.Json;
using ReportBuilder.Models.Models;

namespace ReportBuilder.Models.Response
{
    public class QueryResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string QueryId { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string QueryName { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string QueryJson { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedDate { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string LastModified { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string CreatedBy { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string QuerySQL { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public QueryModel QueryModel { get; set; }
    }
}
