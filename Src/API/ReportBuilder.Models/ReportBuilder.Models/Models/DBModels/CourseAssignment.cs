namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("CourseAssignment")]
    public partial class CourseAssignment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public Guid CourseId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int AccessGrantedBy { get; set; }

        public bool IsEnabled { get; set; }

        public int OrderNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string ConfCode { get; set; }

        public int? ProctorId { get; set; }

        public int Status { get; set; }

        public int? ProgressComp { get; set; }

        public int? ProgressTot { get; set; }

        public DateTime? FirstAccessed { get; set; }

        public DateTime? LastAccessed { get; set; }

        public bool IsReleased { get; set; }

        public short Options { get; set; }

        public bool IsCurrent { get; set; }

        public bool MediaType { get; set; }

        public int? AssignmentWorkflowId { get; set; }

        public byte? Attempt { get; set; }

        public byte LicenseType { get; set; }

        public int? TaskVersionId { get; set; }

        [StringLength(512)]
        public string Note { get; set; }

        public byte? source { get; set; }

        public DateTime? NextAvailableDate { get; set; }

        public int? EventId { get; set; }

        public int? LockoutReason { get; set; }

        public int? TestSessionId { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DateDeleted { get; set; }

        public int? DeleteReason { get; set; }
    }
}
