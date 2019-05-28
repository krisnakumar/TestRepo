using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Model containing data on a PII Field
    /// </summary>
    [Table("PII_Field")]
    public class PIIField
    {
        /// <summary>
        /// Field Id
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Field Type (maps to PIIFieldType enum)
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        /// <summary>
        /// Field Caption
        /// </summary>
        [JsonProperty(PropertyName = "caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Regex string for how this field should be validated
        /// </summary>
        [JsonProperty(PropertyName = "validation")]
        public string Validation { get; set; }

        /// <summary>
        /// Max Length allowed for field
        /// </summary>
        [JsonProperty(PropertyName = "maxLength")]
        public int MaxLength { get; set; }

        /// <summary>
        /// Min Length allowed for field
        /// </summary>
        [JsonProperty(PropertyName = "minLength")]
        public int MinLength { get; set; }

        /// <summary>
        /// UserId of person who created the field
        /// </summary>
        [JsonProperty(PropertyName = "createdBy")]
        public int CreatedBy { get; set; }

        /// <summary>
        /// Date the field was created
        /// </summary>
        [JsonProperty(PropertyName = "createdDate")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Flag showing if field is enabled
        /// </summary>
        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Data Type (maps to PIIDataType enum)
        /// </summary>
        [JsonProperty(PropertyName = "dataType")]
        public int DataType { get; set; }
    }
}