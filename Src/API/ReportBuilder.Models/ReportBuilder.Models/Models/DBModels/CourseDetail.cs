namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    public partial class CourseDetail
    {
        [Key]
        [Column(Order = 0)]
        public Guid Course_Id { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(255)]
        public string Course_Name { get; set; }

        [Key]
        [Column(Order = 2)]
        public byte Course_Type { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Course_Sub_Type { get; set; }

        [Key]
        [Column(Order = 4)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Course_Requalification_Interval { get; set; }

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Course_Pass_Score { get; set; }

        public int? Task_Id { get; set; }

        [Key]
        [Column(Order = 6)]
        public bool Is_Enabled { get; set; }

        public int? Course_Series { get; set; }

        [Key]
        [Column(Order = 7)]
        public DateTime Course_Date_Created { get; set; }

        [Key]
        [Column(Order = 8)]
        public bool Show_Title { get; set; }

        [Key]
        [Column(Order = 9)]
        public byte Display_Type_Allowed { get; set; }

        public long? Course_Options { get; set; }

        public int? Course_Version_Id { get; set; }

        [StringLength(20)]
        public string Course_Version { get; set; }

        [StringLength(255)]
        public string Course_Version_Location { get; set; }

        [StringLength(256)]
        public string Course_Version_Skill { get; set; }

        public bool? Is_Current_Version { get; set; }

        public int? Lesson_Id { get; set; }

        [StringLength(500)]
        public string Lesson_Name { get; set; }

        public int? Lesson_Index { get; set; }

        [StringLength(100)]
        public string Lesson_Location { get; set; }

        public bool? Lesson_Proctored { get; set; }

        public bool? Lesson_Credited { get; set; }

        [StringLength(1024)]
        public string Lesson_Prerequisites { get; set; }

        [StringLength(128)]
        public string Lesson_Dimensions { get; set; }

        public byte? Lesson_SCORM_Version { get; set; }

        public short? Price { get; set; }

        [Key]
        [Column(Order = 10)]
        public bool Can_Certify { get; set; }

        public int Course_Int_Id { get; set; }


        public int? Event_Type_Allowed { get; set; }
    }
}
