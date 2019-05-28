using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Model of UserCompanyTransfer records with additional details
    /// </summary>
    public class UserCompanyTransferDetail
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "newCompanyId")]
        public int NewCompanyId { get; set; }

        [JsonProperty(PropertyName = "oldCompanyId")]
        public int OldCompanyId { get; set; }

        [JsonProperty(PropertyName = "studentId")]
        public int StudentId { get; set; }

        [JsonProperty(PropertyName = "seriesId")]
        public int SeriesId { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "dateRequested")]
        public DateTime DateRequested { get; set; }

        [JsonProperty(PropertyName = "requestedBy")]
        public int RequestedBy { get; set; }

        [JsonProperty(PropertyName = "dateReviewed")]
        public DateTime? DateReviewed { get; set; }

        [JsonProperty(PropertyName = "reviewedBy")]
        public int? ReviewedBy { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "newCompanyName")]
        public string NewCompanyName { get; set; }

        [JsonProperty(PropertyName = "oldCompanyName")]
        public string OldCompanyName { get; set; }

        [JsonProperty(PropertyName = "seriesName")]
        public string SeriesName { get; set; }

        [JsonProperty(PropertyName = "studentFullName")]
        public string StudentFullName { get; set; }

        [JsonProperty(PropertyName = "studentUsername")]
        public string StudentUsername { get; set; }

        [JsonProperty(PropertyName = "studentUsername2")]
        public string StudentUsername2 { get; set; }

        [JsonProperty(PropertyName = "reviewedByName")]
        public string ReviewedByName { get; set; }

        [JsonProperty(PropertyName = "requestedByName")]
        public string RequestedByName { get; set; }

        [JsonProperty(PropertyName = "licenseType")]
        public byte? LicenseType { get; set; }

        //Pipe delimited string of parent company names
        [JsonProperty(PropertyName = "parentCompanies")]
        public string ParentCompanies { get; set; }
    }
}