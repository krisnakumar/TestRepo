using Newtonsoft.Json;
using ReportBuilder.Models.Response;
using System.Collections.Generic;

namespace ReportBuilder.Models.Models
{
    public class QueryModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TaskModel> Tasks { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<WorkbookModel> Workbooks { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<EmployeeQueryModel> Employee { get; set; }


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
        public QueryModel QueryResult { get; set; }

    }
}
