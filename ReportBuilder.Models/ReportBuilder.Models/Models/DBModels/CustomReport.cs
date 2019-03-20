using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing custom reports.  Fields map to GetMyReports stored proc return data
    /// </summary>
    public class CustomReport
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public Int16 Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "desc")]
        public string Desc { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty(PropertyName = "url")]
        public string URL { get; set; }

        [JsonProperty(PropertyName = "isScheduled")]
        public bool IsScheduled { get; set; }

        [JsonProperty(PropertyName = "type")]
        public Int16 Type { get; set; }

        [JsonProperty(PropertyName = "isShared")]
        public bool IsShared { get; set; }

        [JsonProperty(PropertyName = "scheduledReportId")]
        public int? ScheduledReportId { get; set; }
    }
}