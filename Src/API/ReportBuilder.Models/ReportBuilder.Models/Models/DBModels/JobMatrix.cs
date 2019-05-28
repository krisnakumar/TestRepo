using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Used for the OnBoard Job feature which shows which tasks are required for a user to be able
    /// to perform a specific job.  Ex:  TaskA and TaskB are required for a user to be qualified to perform JobC
    /// </summary>
    [Table("JobMatrix")]
    public partial class JobMatrix
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Column(Order = 1)]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [Column(Order = 2)]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [Column(Order = 3)]
        [JsonProperty(PropertyName = "companyId")]
        public int CompanyId { get; set; }

        [Column(Order = 4)]
        [JsonProperty(PropertyName = "dateCreated")]
        public DateTime? DateCreated { get; set; }

        [Column(Order = 5)]
        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [Column(Order = 6)]
        [JsonProperty(PropertyName = "options")]
        public int? Options { get; set; }

        [Column(Order = 7)]
        [JsonProperty(PropertyName = "isShared")]
        public bool IsShared { get; set; }
    }
}