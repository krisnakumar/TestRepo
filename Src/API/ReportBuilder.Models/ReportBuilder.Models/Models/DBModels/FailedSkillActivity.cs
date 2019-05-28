using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    [Table("FailedSkillActivity")]
    public class FailedSkillActivity
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "skillId")]
        public int SkillId { get; set; }

        [JsonProperty(PropertyName = "evaluator")]
        public int? Evaluator { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "dateTaken")]
        public DateTime DateTaken { get; set; }

        [JsonProperty(PropertyName = "dateExpired")]
        public DateTime? DateExpired { get; set; }

        [JsonProperty(PropertyName = "method")]
        public int? Method { get; set; }

        [JsonProperty(PropertyName = "dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "affidavitLoc")]
        [StringLength(128)]
        public string AffidavitLoc { get; set; }

        [JsonProperty(PropertyName = "companyId")]
        public int CompanyId { get; set; }

        [JsonProperty(PropertyName = "thirdPartyVer")]
        public bool ThirdPartyVer { get; set; }

        [JsonProperty(PropertyName = "isCurrent")]
        public bool IsCurrent { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public int? CreatedBy { get; set; }

        [JsonProperty(PropertyName = "options")]
        public byte Options { get; set; }

        [JsonProperty(PropertyName = "recommendationId")]
        public int? RecommendationId { get; set; }

        [JsonProperty(PropertyName = "lockoutStatus")]
        public int? LockoutStatus { get; set; }

        [JsonProperty(PropertyName = "isDraft")]
        public bool? IsDraft { get; set; }
    }
}