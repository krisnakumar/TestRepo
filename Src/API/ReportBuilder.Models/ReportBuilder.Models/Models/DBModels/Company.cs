namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("Company")]
    public partial class Company
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Desc { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime DateCreated { get; set; }

        public int LicenseType { get; set; }

        public int? ApprovalLevel { get; set; }

        public int? PassScore { get; set; }

        public int? RequalificationInterval { get; set; }

        public int AuditLevel { get; set; }

        public long Preference { get; set; }

        public byte Language { get; set; }

        public byte? Discount { get; set; }

        public int? VoucherExpirationDays { get; set; }

        [StringLength(128)]
        public string CustomScript { get; set; }

        [StringLength(64)]
        public string Logo { get; set; }

        public DateTime? PreviousContractDate { get; set; }

        public DateTime? ContractDate { get; set; }

        public int? ContractLength { get; set; }

        public DateTime? DatePaid { get; set; }

        public Guid? PartnerId { get; set; }

        public int? Type { get; set; }

        public int? ConnectSessionTimeout { get; set; }

        [StringLength(64)]
        public string NCMSID { get; set; }

        [StringLength(64)]
        public string NCMSOperatorID { get; set; }

        [NotMapped]
        public string Settings { get; set; }

        public string EncryptionKeyARN { get; set; }
    }
}
