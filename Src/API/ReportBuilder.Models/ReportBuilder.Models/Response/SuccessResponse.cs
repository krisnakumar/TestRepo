using Newtonsoft.Json;

namespace ReportBuilder.Models.Response
{
    public class SuccessResponse
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }
    }
}
