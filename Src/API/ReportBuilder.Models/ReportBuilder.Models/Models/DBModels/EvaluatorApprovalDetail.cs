using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for giving additional details on evaluator approval records
    /// </summary>
    public class EvaluatorApprovalDetail
    {
        [Key]
        [JsonProperty(PropertyName = "approvalId")]
        public int ApprovalId { get; set; }

        [JsonProperty(PropertyName = "evaluatorId")]
        public int EvaluatorId { get; set; }

        [JsonProperty(PropertyName = "evaluatorFullName")]
        public string EvaluatorFullName { get; set; }

        [JsonProperty(PropertyName = "evaluatorUsername")]
        public string EvaluatorUsername { get; set; }

        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "skillId")]
        public int SkillId { get; set; }

        [JsonProperty(PropertyName = "skillCode")]
        public string SkillCode { get; set; }

        [JsonProperty(PropertyName = "skillName")]
        public string SkillName { get; set; }

        [JsonProperty(PropertyName = "reviewedBy")]
        public int? ReviewedBy { get; set; }

        [JsonProperty(PropertyName = "reviewedByFullName")]
        public string ReviewedByFullName { get; set; }

        [JsonProperty(PropertyName = "reviewedByUsername")]
        public string ReviewedByUsername { get; set; }

        [JsonProperty(PropertyName = "reviewDate")]
        public DateTime? ReviewDate { get; set; }

        [JsonProperty(PropertyName = "status")]
        public Int16 Status { get; set; }

        [JsonProperty(PropertyName = "requestDate")]
        [Column(TypeName = "smalldatetime")]
        public DateTime RequestDate { get; set; }

        [JsonProperty(PropertyName = "courseId")]
        public Guid? CourseId { get; set; }

        [JsonProperty(PropertyName = "seriesId")]
        public int SeriesId { get; set; }

        [JsonProperty(PropertyName = "seriesName")]
        public string SeriesName { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "references")]
        [NotMapped]
        public List<Reference> References { get; set; }
    }
}