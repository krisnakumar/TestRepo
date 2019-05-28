using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing approvals
    /// </summary>
    [Table("Approval")]
    public class Approval
    {
        /// <summary>
        /// Maps to whichever record's uniqueId. Ex:  Skill Certificate approvals would have an EntityId mapping to a FailedSkillActivityId
        /// </summary>
        [Key]
        [JsonProperty(PropertyName = "entityId")]
        public int EntityId { get; set; }

        /// <summary>
        /// Maps to ApprovalType enum
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public Int16 Type { get; set; }

        [JsonProperty(PropertyName = "reviewedBy")]
        public int? ReviewedBy { get; set; }

        [JsonProperty(PropertyName = "reviewDate")]
        public DateTime? ReviewDate { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

        [JsonProperty(PropertyName = "status")]
        public Int16 Status { get; set; }

        /// <summary>
        /// Maps to ApprovalDenialReason id
        /// </summary>
        [JsonProperty(PropertyName = "reasonId")]
        public int? ReasonId { get; set; }
    }
}