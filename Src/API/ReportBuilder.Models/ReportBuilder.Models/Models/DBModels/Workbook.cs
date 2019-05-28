using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing workbooks (on the job training)
    /// </summary>
    [Table("Workbook")]
    public class Workbook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(255)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(1024)]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "companyId")]
        public int CompanyId { get; set; }

        [JsonProperty(PropertyName = "dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }

        [JsonProperty(PropertyName = "options")]
        public int? Options { get; set; }

        [JsonProperty(PropertyName = "daysToComplete")]
        public int? DaysToComplete { get; set; }

        [JsonProperty(PropertyName = "isFinal")]
        public bool IsFinal { get; set; }
    }
}