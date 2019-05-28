using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing Approval denial reasons
    /// </summary>
    [Table("ApprovalDenialReason")]
    public class ApprovalDenialReason
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "companyId")]
        public int? CompanyId { get; set; }

        [JsonProperty(PropertyName = "reasonText")]
        public string ReasonText { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "type")]
        public Int16 Type { get; set; }
    }
}