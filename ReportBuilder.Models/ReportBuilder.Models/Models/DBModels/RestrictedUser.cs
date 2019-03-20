using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Used for managing users who have been banned from using certain series
    /// </summary>
    [Table("RestrictedUser")]
    public partial class RestrictedUser
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [JsonProperty(PropertyName = "seriesId")]
        public int SeriesId { get; set; }

        [JsonProperty(PropertyName = "restrictedBy")]
        public int RestrictedBy { get; set; }

        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }

        [JsonProperty(PropertyName = "restrictionDate")]
        public DateTime RestrictionDate { get; set; }

        [JsonProperty(PropertyName = "attachment")]
        public string Attachment { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "restrictionLiftedDate")]
        public DateTime? RestrictionLiftedDate { get; set; }

        [JsonProperty(PropertyName = "restrictionLiftedBy")]
        public int? RestrictionLiftedBy { get; set; }
    }
}