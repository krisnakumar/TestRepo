namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("DataAudit")]
    public partial class DataAudit
    {
        public int id { get; set; }

        [Required]
        [StringLength(1024)]
        public string name { get; set; }

        [Required]
        [StringLength(1024)]
        public string description { get; set; }

        [Required]
        [StringLength(1028)]
        public string schedule { get; set; }

        public bool isEnabled { get; set; }

        [Column(TypeName = "date")]
        public DateTime created { get; set; }

        [Required]
        [StringLength(1024)]
        public string location { get; set; }

        public int companyId { get; set; }

        [Required]
        [StringLength(1024)]
        public string fileNameConvention { get; set; }

        public int options { get; set; }

        [Column(TypeName = "date")]
        public DateTime lastRun { get; set; }

        public int lastRunStatus { get; set; }
    }
}
