namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("SkillActivity")]
    public partial class SkillActivity
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int SkillId { get; set; }

        public int? Evaluator { get; set; }

        public int Status { get; set; }

        public DateTime DateTaken { get; set; }

        public DateTime? DateExpired { get; set; }

        public int? Method { get; set; }

        public DateTime DateCreated { get; set; }

        [StringLength(128)]
        public string AffidavitLoc { get; set; }

        public int CompanyId { get; set; }

        public bool ThirdPartyVer { get; set; }

        public bool IsCurrent { get; set; }

        public int? CreatedBy { get; set; }

        public byte Options { get; set; }

        public int? AssignmentWorkflowId { get; set; }

        public int? Attempt { get; set; }

        public int? RecommendationId { get; set; }

        public DateTime? NextAvailableDate { get; set; }

        public bool IsEnabled { get; set; }

        [StringLength(8)]
        public string DraftTime { get; set; }

        [StringLength(8)]
        public string TotalTime { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeleteReason { get; set; }
    }
}
