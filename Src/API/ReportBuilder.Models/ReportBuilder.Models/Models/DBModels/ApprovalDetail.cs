using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing Approvals with additional details
    /// </summary>
    public class ApprovalDetail
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

        [JsonProperty(PropertyName = "requestDate")]
        public DateTime RequestDate { get; set; }

        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }

        [JsonProperty(PropertyName = "status")]
        public Int16 Status { get; set; }

        /// <summary>
        /// Maps to ApprovalDenialReason id
        /// </summary>
        [JsonProperty(PropertyName = "reasonId")]
        public int? ReasonId { get; set; }

        [JsonProperty(PropertyName = "studentId")]
        public int StudentId { get; set; }

        [JsonProperty(PropertyName = "studentFullName")]
        public string StudentFullName { get; set; }

        [JsonProperty(PropertyName = "studentUsername")]
        public string StudentUsername { get; set; }

        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "skillId")]
        public int SkillId { get; set; }

        [JsonProperty(PropertyName = "skillCode")]
        public string SkillCode { get; set; }

        [JsonProperty(PropertyName = "skillName")]
        public string SkillName { get; set; }

        [JsonProperty(PropertyName = "reviewedByFullName")]
        public string ReviewedByFullName { get; set; }

        [JsonProperty(PropertyName = "reviewedByUsername")]
        public string ReviewedByUsername { get; set; }

        [JsonProperty(PropertyName = "reasonText")]
        public string ReasonText { get; set; }

        [JsonProperty(PropertyName = "affidavitLoc")]
        public string AffidavitLoc { get; set; }
    }
}