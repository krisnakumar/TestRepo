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
    /// Used for OnBoard NOC (notice of change)
    /// </summary>
    [Table("TaskRevisionTask")]
    public partial class TaskRevisionTask
    {
        [Key]
        [JsonProperty(PropertyName = "taskRevisionId")]
        public int TaskRevisionID { get; set; }

        [Key]
        [JsonProperty(PropertyName = "taskId")]
        public int TaskID { get; set; }
    }
}