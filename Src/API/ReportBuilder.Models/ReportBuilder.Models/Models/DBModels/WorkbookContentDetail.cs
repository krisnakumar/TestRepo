using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing details on workbook content
    /// </summary>
    public class WorkbookContentDetail
    {
        [Key]
        [JsonProperty(PropertyName = "workbookContentId")]
        public int WorkbookContentId { get; set; }

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

        [JsonProperty(PropertyName = "entityName")]
        public string EntityName { get; set; }

        [JsonProperty(PropertyName = "entityCode")]
        public string EntityCode { get; set; }
    }
}