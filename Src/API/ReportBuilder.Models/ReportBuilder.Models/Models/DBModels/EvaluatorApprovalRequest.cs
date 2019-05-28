using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing evaluator task list approval requests and the provided references
    /// Used in POSTs so one endpoint can be used for saving all related database records including:
    /// EvaluatorApproval, References, ApprovalReferences
    /// </summary>
    public class EvaluatorApprovalRequest
    {
        [Key]
        [JsonProperty(PropertyName = "evaluatorId")]
        public int EvaluatorId { get; set; }

        [JsonProperty(PropertyName = "skillIds")]
        public List<int> SkillIds { get; set; }

        [JsonProperty(PropertyName = "references")]
        public List<Reference> References { get; set; }
    }
}