namespace OnBoardLMS.WebAPI.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    

    [Table("CompanyCodeCourseMapping")]
    public partial class CompanyCodeCourseMapping
    {
        public int Id { get; set; }

        public String CompanyCode { get; set; }
        
        public Guid CourseId { get; set; }

        public bool IsEnabled { get; set; }
    }
}
