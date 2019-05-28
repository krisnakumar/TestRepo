using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OnBoardLMS.WebAPI.Models
{
    public partial class UserCompanySeries
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? CompanyId { get; set; }
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? SeriesId { get; set; }

        public DateTime? DateActivated { get; set; }

        public DateTime? DateDeActivated { get; set; }

        public int? ActivatedBy { get; set; }

        public int? DeactivatedBy { get; set; }

        public byte? Status { get; set; }

        public byte? LicenseType { get; set; }

        public int Options { get; set; }
    }
}
