namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("EvaluatorApprovedTaskByCourse")]
    public partial class EvaluatorApprovedTaskByCourse
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Evaluator_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Skill_Id { get; set; }

        [StringLength(1024)]
        public string Task_Name { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(100)]
        public string User_Name { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(303)]
        public string Full_Name_Format1 { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(302)]
        public string Full_Name { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(255)]
        public string Company_Name { get; set; }

        [Key]
        [Column(Order = 6)]
        public Guid Course_Id { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(255)]
        public string Course_Name { get; set; }

        [Key]
        [Column(Order = 8)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CourseIntId { get; set; }
    }
}
