namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CompanySeries")]
    public partial class CompanySeries
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CompanySeries()
        {
        }

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CompanyId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SeriesId { get; set; }

        public int SkillSubmitPeriod { get; set; }

        public bool IsCustom { get; set; }

        public long? Options { get; set; }

        public byte? LicenseType { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDefault { get; set; }

        public int SkillSubmitPeriodBack { get; set; }

        public int? AssignmentWorkflowId { get; set; }

        public int? ContractLength { get; set; }

        public DateTime? PreviousContractDate { get; set; }

        public DateTime? ContractDate { get; set; }

        public DateTime? DatePaid { get; set; }

        public DateTime? DateReset { get; set; }

        public int? TestingWaitPeriod { get; set; }

        public int? EvaluationDataRetentionPeriod { get; set; }

        [StringLength(2000)]
        public string NotAvailableText { get; set; }
    }
}
