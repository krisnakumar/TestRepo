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
    /// Used for OnBoard NOC (notice of change) attachments
    /// </summary>
    [Table("TaskRevisionAttachment")]
    public partial class TaskRevisionAttachment
    {
        //[Key]
        [JsonProperty(PropertyName = "taskRevisionId")]
        public int TaskRevisionID { get; set; }

        [JsonProperty(PropertyName = "fileDescription")]
        public string FileDescription { get; set; }

        [JsonProperty(PropertyName = "filePath")]
        public string FilePath { get; set; }

        [JsonProperty(PropertyName = "userFileName")]
        public string UserFileName { get; set; }
    }
}