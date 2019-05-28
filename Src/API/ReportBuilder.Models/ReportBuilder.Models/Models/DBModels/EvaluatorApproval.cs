using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing requests from evaluators to have new tasks added to their Evaluator Approved Task List
    /// </summary>
    [Table("EvaluatorApproval")]
    public class EvaluatorApproval
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "evaluatorId")]
        public int EvaluatorId { get; set; }

        [JsonProperty(PropertyName = "skillId")]
        public int SkillId { get; set; }

        [JsonProperty(PropertyName = "reviewedBy")]
        public int? ReviewedBy { get; set; }

        [JsonProperty(PropertyName = "reviewDate")]
        public DateTime? ReviewDate { get; set; }

        [JsonProperty(PropertyName = "status")]
        public Int16 Status { get; set; }

        [JsonProperty(PropertyName = "requestDate")]
        [Column(TypeName = "smalldatetime")]
        public DateTime RequestDate { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }
    }
}