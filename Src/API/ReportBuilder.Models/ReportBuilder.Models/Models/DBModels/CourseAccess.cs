namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("CourseAccess")]
    public partial class CourseAccess
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

        [StringLength(40)]
        public string ConfCode { get; set; }

        [StringLength(16)]
        public string Passkey { get; set; }

        public int? ProctorId { get; set; }

        public bool? PasskeyConsumed { get; set; }

        public DateTime? DatePasskeyConsumed { get; set; }

        public int Status { get; set; }

        public int? ProgressComp { get; set; }

        public int? ProgressTot { get; set; }

        public DateTime? FirstAccessed { get; set; }

        public DateTime? LastAccessed { get; set; }

        public DateTime? DateCertified { get; set; }

        public DateTime? DateCertExpired { get; set; }

        public bool IsReleased { get; set; }

        public int Options { get; set; }

        public bool ThirdPartyVer { get; set; }

        public bool? IsCurrent { get; set; }

        public byte CertStatus { get; set; }

        public byte? DecertReason { get; set; }

        public int? DecertBy { get; set; }

        public DateTime? DecertDate { get; set; }

        public byte MediaType { get; set; }

        public int? AssignmentWorkflowId { get; set; }

        public byte? Attempt { get; set; }

        public byte LicenseType { get; set; }

        public int? TaskVersionId { get; set; }

        [StringLength(512)]
        public string Note { get; set; }

        public DateTime? DateEdited { get; set; }

        [StringLength(8000)]
        public string DecertNotes { get; set; }

        [StringLength(256)]
        public string AffidavitLoc { get; set; }

        public DateTime? DateCurrentExpired { get; set; }

        public int? EventId { get; set; }
        
        public int? DeletedBy { get; set; }
        
        public DateTime? DateDeleted { get; set; }
        
        public int? DeleteReason { get; set; }
    }
}
