using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for use in the restricted users report
    /// </summary>
    public class RestrictedUsersReport
    {
        public RestrictedUsersReport()
        {
        }

        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "seriesId")]
        public int SeriesId { get; set; }

        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "seriesName")]
        public string SeriesName { get; set; }

        [Column(TypeName = "smalldatetime")]
        [JsonProperty(PropertyName = "restrictionDate")]
        public DateTime RestrictionDate { get; set; }

        [JsonProperty(PropertyName = "restrictedBy")]
        public string RestrictedBy { get; set; }

        [StringLength(255)]
        [JsonProperty(PropertyName = "attachment")]
        public string Attachment { get; set; }
    }
}