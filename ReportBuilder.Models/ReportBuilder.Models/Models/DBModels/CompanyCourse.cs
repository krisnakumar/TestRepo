namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("CompanyCourse")]
    public partial class CompanyCourse
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EntityId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EntityType { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid CourseId { get; set; }

        public int? Price { get; set; }

        public int? RequalificationInterval { get; set; }

        public int? PassScore { get; set; }

        public byte? Options { get; set; }

        public int? TestingWaitPeriod { get; set; }

        public int? AssignmentWorkFlow { get; set; }
    }
}
