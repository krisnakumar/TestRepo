namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("CompanyIntegration")]
    public partial class CompanyIntegration
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CompanyId { get; set; }

        [Key]
        [Column(Order = 1)]
        public byte IntegrationType { get; set; }

        public byte AccessLevel { get; set; }

        [Required]
        [StringLength(128)]
        public string ApprovedDomain { get; set; }

        public int? UserId { get; set; }

        public byte Options { get; set; }

        public bool IsEnabled { get; set; }

        [StringLength(128)]
        public string ApprovedNotes { get; set; }
    }
}
