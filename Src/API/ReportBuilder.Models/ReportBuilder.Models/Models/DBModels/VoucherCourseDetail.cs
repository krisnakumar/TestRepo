using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace OnBoardLMS.WebAPI.Models
{
    public class VoucherCourseDetail
    {
        [Key]
        [JsonProperty(PropertyName = "courseId")]
        public Guid CourseId { get; set; }

        [StringLength(255)]
        [JsonProperty(PropertyName = "courseName")]
        public string CourseName { get; set; }

        [StringLength(128)]
        [JsonProperty(PropertyName = "taskCode")]
        public string TaskCode { get; set; }

        [JsonProperty(PropertyName = "prereqCompleted")]
        public bool PrereqCompleted { get; set; }
    }
}