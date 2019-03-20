using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class EvaluationPause
    {
        /// <summary>
        /// Start time from UTC
        /// </summary>
        [JsonProperty(PropertyName = "start")]
        public DateTime Start { get; set; }

        /// <summary>
        /// Stop time from UTC
        /// </summary>
        [JsonProperty(PropertyName = "stop")]
        public DateTime Stop { get; set; }

        /// <summary>
        /// Type (need an enum for this? what are other values?)
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }
    }
}