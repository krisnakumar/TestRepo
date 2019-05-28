namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DataAuditReport")]
    public partial class DataAuditReport
    {
        public int id { get; set; }

        public int parentAudit { get; set; }

        [Column(TypeName = "date")]
        public DateTime date { get; set; }

        public long exceptionCount { get; set; }

        public long sourceTotal { get; set; }

        public long targetTotal { get; set; }

        [Required]
        [StringLength(1028)]
        public string fileLocation { get; set; }

        public bool isEnabled { get; set; }
    }
}
