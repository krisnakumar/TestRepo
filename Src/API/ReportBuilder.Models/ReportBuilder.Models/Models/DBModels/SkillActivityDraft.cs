namespace OnBoardLMS.WebAPI.Models
{
    using Newtonsoft.Json;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("SkillActivityDraft")]
    public partial class SkillActivityDraft
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "userid")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "skillid")]
        public int SkillId { get; set; }

        [JsonProperty(PropertyName = "evaluator")]
        public int? Evaluator { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "datetaken")]
        public DateTime DateTaken { get; set; }

        [JsonProperty(PropertyName = "dateexpired")]
        public DateTime? DateExpired { get; set; }

        [JsonProperty(PropertyName = "method")]
        public int? Method { get; set; }

        [JsonProperty(PropertyName = "datecreated")]
        public DateTime DateCreated { get; set; }

        [StringLength(128)]
        [JsonProperty(PropertyName = "affidavitloc")]
        public string AffidavitLoc { get; set; }

        [JsonProperty(PropertyName = "companyid")]
        public int CompanyId { get; set; }

        [JsonProperty(PropertyName = "thirdpartyver")]
        public bool ThirdPartyVer { get; set; }

        [JsonProperty(PropertyName = "iscurrent")]
        public bool IsCurrent { get; set; }

        [JsonProperty(PropertyName = "createdby")]
        public int? CreatedBy { get; set; }

        [JsonProperty(PropertyName = "options")]
        public byte Options { get; set; }

        [JsonProperty(PropertyName = "assignmentworkflowid")]
        public int? AssignmentWorkflowId { get; set; }

        [JsonProperty(PropertyName = "attempt")]
        public int? Attempt { get; set; }

        [JsonProperty(PropertyName = "recommendationid")]
        public int? RecommendationId { get; set; }

        [JsonProperty(PropertyName = "nextavailabledate")]
        public DateTime? NextAvailableDate { get; set; }

        [JsonProperty(PropertyName = "isenabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "deletedby")]
        public int? DeletedBy { get; set; }

        [JsonProperty(PropertyName = "datedeleted")]
        public DateTime? DateDeleted { get; set; }

        [JsonProperty(PropertyName = "deletereason")]
        public int? DeleteReason { get; set; }

        [JsonProperty(PropertyName = "payload")]
        public string Payload { get; set; }
    }
}
