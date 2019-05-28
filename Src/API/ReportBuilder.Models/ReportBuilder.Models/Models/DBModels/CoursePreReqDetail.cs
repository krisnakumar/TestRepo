namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public partial class CoursePreReqDetail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(255)]
        public string Course_Name { get; set; }

        [Key]
        [Column(Order = 1)]
        public Guid Course_Id { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(255)]
        public string Course_PreReq_Name { get; set; }

        [Key]
        [Column(Order = 3)]
        public Guid Course_PreReq_Id { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Company_Id { get; set; }

        public byte? PreReq_Type { get; set; }

        public int? PreReq_Options { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Course_IntId { get; set; }

        [Key]
        [Column(Order = 6)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CoursePrereq_IntId { get; set; }
    }
}
