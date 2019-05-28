namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("Task")]
    public partial class Task
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(512)]
        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        [Required]
        [StringLength(128)]
        public string Code { get; set; }

        public DateTime DateCreated { get; set; }

        public int SeriesId { get; set; }

        public bool? IsCustom { get; set; }

        public int? ParentTaskId { get; set; }

        public int? Options { get; set; }

        public int? AllowRequalificationPeriod { get; set; }

    }
}
