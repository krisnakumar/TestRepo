using Newtonsoft.Json;
using ReportBuilder.Models.Response;
using System.Collections.Generic;

namespace ReportBuilder.Models.Models
{
    public class QueryModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<TaskResponse> Tasks { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<WorkbookResponse> Workbooks { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<EmployeeResponse> Employee { get; set; }
    }
}
