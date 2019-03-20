using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    public class EvaluatorApprovalRequestBatchDetail
    {
        [Key]
        [Column(Order= 0)]
        [JsonProperty(PropertyName = "evaluatorId")]
        public int EvaluatorId { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "smalldatetime")]
        [JsonProperty(PropertyName = "requestDate")]
        public DateTime RequestDate { get; set; }

        [JsonProperty(PropertyName = "evaluatorFullName")]
        public string EvaluatorFullName { get; set; }

        [JsonProperty(PropertyName = "evaluatorUsername")]
        public string EvaluatorUsername { get; set; }

        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; }

        [JsonProperty(PropertyName = "evaluatorApprovalDetails")]
        public List<EvaluatorApprovalDetail> EvaluatorApprovalDetails { get; set; }

        [JsonProperty(PropertyName = "references")]
        public List<Reference> References { get; set; }
    }
}