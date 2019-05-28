using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class EvaluationFeedback
    {
        /// <summary>
        /// Id of evaluation draft record
        /// </summary>
        [JsonProperty(PropertyName = "draftId")]
        public int DraftId { get; set; }
    }
}