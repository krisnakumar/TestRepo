namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("Template")]
    public partial class Template
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public short Type { get; set; }

        public int Options { get; set; }

        [Required]
        public string HTML { get; set; }

        public bool? IsEnabled { get; set; }

        public int? CompanyId { get; set; }

        public int? SeriesId { get; set; }

        public int? DaysToAggregate { get; set; }
    }
}
