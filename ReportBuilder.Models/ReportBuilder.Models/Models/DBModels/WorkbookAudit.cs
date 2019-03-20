using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing audit entries related to workbooks
    /// </summary>
    [Table("WorkbookAudit")]
    public class WorkbookAudit
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "workbookId")]
        public int WorkbookId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        [StringLength(255)]
        [JsonProperty(PropertyName = "oldValue")]
        public string OldValue { get; set; }

        [StringLength(255)]
        [JsonProperty(PropertyName = "newValue")]
        public string NewValue { get; set; }

        [JsonProperty(PropertyName = "editedDate")]
        public DateTime EditedDate { get; set; }

        [JsonProperty(PropertyName = "editedBy")]
        public int EditedBy { get; set; }
    }
}