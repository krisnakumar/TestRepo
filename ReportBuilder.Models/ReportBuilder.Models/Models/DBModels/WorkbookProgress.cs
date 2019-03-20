using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing which a user's progress in a workbook
    /// </summary>
    [Table("WorkbookProgress")]
    public class WorkbookProgress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "workbookId")]
        public int WorkbookId { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public int UserId { get; set; }

        [JsonProperty(PropertyName = "entityId")]
        public int EntityId { get; set; }

        [JsonProperty(PropertyName = "numberCompleted")]
        public int NumberCompleted { get; set; }

        [JsonProperty(PropertyName = "lastAttemptDate")]
        public DateTime? LastAttemptDate { get; set; }

        [JsonProperty(PropertyName = "lastSignOffBy")]
        public int? LastSignOffBy { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "dateInserted")]
        public DateTime DateInserted { get; set; }

        [JsonProperty(PropertyName = "firstAttemptDate")]
        public DateTime? FirstAttemptDate { get; set; }
    }
}