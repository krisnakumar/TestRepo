using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using System.Collections.Generic;

namespace ReportBuilder.Models.Response
{
    public class QueryResponse : SuccessResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<QueryModel> Queries { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ErrorResponse Error { get; set; }
    }
}
