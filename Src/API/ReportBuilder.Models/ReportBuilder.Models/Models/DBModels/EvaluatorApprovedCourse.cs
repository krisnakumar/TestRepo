namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("EvaluatorApprovedCourse")]
    public partial class EvaluatorApprovedCourse
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EvaluatorId { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid CourseId { get; set; }

        public bool IsEnabled { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime DateCreated { get; set; }
    }
}
