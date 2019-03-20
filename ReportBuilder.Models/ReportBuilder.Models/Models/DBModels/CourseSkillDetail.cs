using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Manages details related to a Course and its associated skill record
    /// </summary>
    public class CourseSkillDetail
    {
        [Key]
        [Column(Order = 0)]
        [JsonProperty(PropertyName = "courseId")]
        public Guid CourseId { get; set; }

        [Required]
        [StringLength(255)]
        [JsonProperty(PropertyName = "courseName")]
        public string CourseName { get; set; }

        [JsonProperty(PropertyName = "courseType")]
        public byte CourseType { get; set; }

        [JsonProperty(PropertyName = "courseSubType")]
        public int CourseSubType { get; set; }

        [JsonProperty(PropertyName = "seriesId")]
        public int? SeriesId { get; set; }

        [JsonProperty(PropertyName = "taskId")]
        public int? TaskId { get; set; }

        [Key]
        [Column(Order = 1)]
        [JsonProperty(PropertyName = "skillId")]
        public int SkillId { get; set; }

        [StringLength(1024)]
        [JsonProperty(PropertyName = "skillName")]
        public string SkillName { get; set; }

        [StringLength(48)]
        [JsonProperty(PropertyName = "skillCode")]
        public string SkillCode { get; set; }

        [JsonProperty(PropertyName = "skillType")]
        public int? SkillType { get; set; }
    }
}