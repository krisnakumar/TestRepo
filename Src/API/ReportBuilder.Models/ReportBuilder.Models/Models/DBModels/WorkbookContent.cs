using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing a workbook's content
    /// </summary>
    [Table("WorkbookContent")]
    public class WorkbookContent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "entityId")]
        public int EntityId { get; set; }

        [JsonProperty(PropertyName = "entityType")]
        public int EntityType { get; set; }

        [JsonProperty(PropertyName = "repetitions")]
        public int Repetitions { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "workbookId")]
        public int WorkbookId { get; set; }
    }
}