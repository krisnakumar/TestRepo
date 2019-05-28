using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing references, such as for an evaluator's request to add tasks to their approved task list
    /// </summary>
    [Table("Reference")]
    public class Reference
    {
        [Key]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [StringLength(100)]
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        [StringLength(100)]
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        [StringLength(20)]
        [JsonProperty(PropertyName = "contactPhone")]
        public string ContactPhone { get; set; }

        [StringLength(100)]
        [JsonProperty(PropertyName = "contactEmail")]
        public string ContactEmail { get; set; }

        [JsonProperty(PropertyName = "numberOfYears")]
        public int? NumberOfYears { get; set; }

        [StringLength(100)]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [StringLength(255)]
        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; }
    }
}