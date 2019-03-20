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
    [Table("TaskRevisionCompany")]
    public partial class TaskRevisionCompany
    {
        [Key]
        [JsonProperty(PropertyName = "taskRevisionId")]
        public int TaskRevisionID { get; set; }

        [Key]
        [JsonProperty(PropertyName = "companyId")]
        public int CompanyID { get; set; }

        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "lastModified")]
        [Column(TypeName = "date")]
        public DateTime? LastModified { get; set; }
    }
}