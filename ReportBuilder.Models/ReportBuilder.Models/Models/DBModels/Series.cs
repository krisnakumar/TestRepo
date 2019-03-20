namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public partial class Series
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Series()
        {
        }
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(1024)]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public bool IsTransferrable { get; set; }

        public bool? ShowAllAttachments { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? ProctorTrainingExpiration { get; set; }

        public string EvalInstruction { get; set; }

        public int? Options { get; set; }

        public string ProctorTemplate { get; set; }

        public int? ParentSeriesId { get; set; }

        public string CertificateTemplate { get; set; }

        public string ProctorVerificationTemplate { get; set; }

        public string EvaluatorTemplate { get; set; }

        public long? DefaultOptions { get; set; }
    }
}
