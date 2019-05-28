namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public partial class TaskRevisionDetail
    {
        [Key]
        public int TaskRevisionId { get; set; }

        public int CreatedByCompanyId { get; set; }

        public int UserId { get; set; }

        public int CompanyId { get; set; }

        public int CreatedByUserId { get; set; }

        public int? ReviewerId { get; set; }

        public int? CurrentAdminId { get; set; }

        public string CompanyLogo { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime DateCreated { get; set; }

        public bool IsEnabled { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }

        public bool Disqualified { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime ExpirationDate { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Message { get; set; }

        public string Status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? LastModified { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string CreatedByFullName { get; set; }

        public long CompanyPreference { get; set; }
    }
}