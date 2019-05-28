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
    [Table("TaskRevision")]
    public partial class TaskRevision
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "createdByUserId")]
        public int CreatedByUserID { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "disqualified")]
        public bool Disqualified { get; set; }

        [JsonProperty(PropertyName = "createdByCompanyId")]
        public int CreatedByCompanyID { get; set; }

        [Required(AllowEmptyStrings = false)]
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [Column(TypeName = "smalldatetime")]
        [JsonProperty(PropertyName = "dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "reviewerRoleId")]
        public int ReviewerRoleID { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [Column(TypeName = "smalldatetime")]
        [JsonProperty(PropertyName = "expirationDate")]
        public DateTime ExpirationDate { get; set; }

        [JsonProperty(PropertyName = "type")]
        public Int16 Type { get; set; }
    }
}