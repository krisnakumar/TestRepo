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
    [Table("TaskRevisionUser")]
    public partial class TaskRevisionUser
    {
        //[Key]
        [JsonProperty(PropertyName = "taskRevisionId")]
        public int TaskRevisionID { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public int UserID { get; set; }

        [JsonProperty(PropertyName = "reviewerId")]
        public int ReviewerID { get; set; }

        [JsonProperty(PropertyName = "currentAdminId")]
        public int CurrentAdminID { get; set; }
    }
}