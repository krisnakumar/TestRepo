using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;


namespace ReportBuilder.Models.Models
{
    /// <summary>
    /// Model class to handle the query request for employee
    /// </summary>
    public class EmployeeModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Bitwise { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Value { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Operator { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public string Group { get; set; }
    }
}
