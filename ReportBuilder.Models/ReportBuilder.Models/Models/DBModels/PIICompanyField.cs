using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Model containing data on PII Fields used by a company
    /// </summary>
    [Table("PII_CompanyField")]
    public class PIICompanyField
    {
        /// <summary>
        /// Field Id
        /// </summary>
        [Key]
        [Column(Order = 1)]
        [JsonProperty(PropertyName = "fieldId")]
        public int Field_Id { get; set; }

        /// <summary>
        /// Company Id
        /// </summary>
        [Key]
        [Column(Order = 2)]
        [JsonProperty(PropertyName = "companyId")]
        public int Company_Id { get; set; }

        /// <summary>
        /// Series Id
        /// </summary>
        [Key]
        [Column(Order = 3)]
        [JsonProperty(PropertyName = "seriesId")]
        public int Series_Id { get; set; }

        /// <summary>
        /// IsEnabled (does company still use this field?)
        /// </summary>
        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// UserId of person who created this CompanyField record
        /// </summary>
        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Date this company field record was created
        /// </summary>
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// UserId of person who deleted this CompanyField record
        /// </summary>
        [JsonProperty(PropertyName = "deletedBy")]
        public int? DeletedBy { get; set; }

        /// <summary>
        /// Date this company field record was deleted
        /// </summary>
        [JsonProperty(PropertyName = "deletedDate")]
        public DateTime? DeletedDate { get; set; }
    }
}