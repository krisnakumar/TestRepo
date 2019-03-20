namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public partial class SkillActivityMetric
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SkillActivityId { get; set; }

        [StringLength(255)]
        public string Street { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        [StringLength(255)]
        public string State { get; set; }

        [StringLength(15)]
        public string Zip { get; set; }

        [StringLength(255)]
        public string Country { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public int? Duration { get; set; }

        public string Payload { get; set; }

        [StringLength(255)]
        public string IP { get; set; }

        [StringLength(1000)]
        public string UserAgent { get; set; }

        public int? Attempts { get; set; }

        [StringLength(255)]
        public string DeviceId { get; set; }

        public double? Score { get; set; }
    }
}
