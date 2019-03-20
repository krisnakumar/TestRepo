using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    [Table("JobMatrixTask")]
    public class JobMatrixTask
    {
        [Key]
        [Column(Order = 0)]
        [JsonProperty(PropertyName = "jobMatrixId")]
        public int JobMatrixId { get; set; }

        [Key]
        [Column(Order = 1)]
        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "taskId")]
        public int TaskId { get; set; }

        [Column(Order = 2)]
        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [Column(Order = 3)]
        [JsonProperty(PropertyName = "dateCreated")]
        public DateTime? DateCreated { get; set; }
    }
}