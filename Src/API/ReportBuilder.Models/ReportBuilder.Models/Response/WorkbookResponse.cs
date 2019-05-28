using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using System.Collections.Generic;



namespace ReportBuilder.Models.Response
{
    public class WorkbookResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<WorkbookModel> Workbooks { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ErrorResponse Error { get; set; }
    }   
}
