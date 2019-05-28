using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing batches of evaluator approval requests (grouped by evaluatorId and dateRequested)
    /// </summary>
    public class EvaluatorApprovalRequestBatch
    {
        [Key]
        [Column(Order = 0)]
        [JsonProperty(PropertyName = "evaluatorId")]
        public int EvaluatorId { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "smalldatetime")]
        [JsonProperty(PropertyName = "requestDate")]
        public DateTime RequestDate { get; set; }

        [NotMapped]
        [JsonProperty(PropertyName = "requestDateTicks")]
        public long RequestDateTicks { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "evaluatorFullName")]
        public string EvaluatorFullName { get; set; }

        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "status")]
        public short status { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }
    }
}