using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Maps to CourseSkill database table, links enhanced course to its associated skill record
    /// </summary>
    [Table("CourseSkill")]
    public class CourseSkill
    {
        [Key]
        [Column(Order = 0)]
        [JsonProperty(PropertyName = "courseId")]
        public Guid CourseId { get; set; }

        [Key]
        [Column(Order = 1)]
        [JsonProperty(PropertyName = "skillId")]
        public int SkillId { get; set; }
    }
}