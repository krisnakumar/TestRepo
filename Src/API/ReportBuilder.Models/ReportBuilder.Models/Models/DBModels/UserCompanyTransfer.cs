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
    /// Model used for managing users imported into other companies and their import status (approved/denied/pending)
    /// </summary>
    [Table("UserCompanyTransfer")]
    public class UserCompanyTransfer
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Column(Order = 1)]
        [JsonProperty(PropertyName = "newCompanyId")]
        public int NewCompanyId { get; set; }

        [Column(Order = 2)]
        [JsonProperty(PropertyName = "oldCompanyId")]
        public int OldCompanyId { get; set; }

        [Column(Order = 3)]
        [JsonProperty(PropertyName = "studentId")]
        public int StudentId { get; set; }

        [Column(Order = 4)]
        [JsonProperty(PropertyName = "seriesId")]
        public int SeriesId { get; set; }

        [Column(Order = 5)]
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [Column(Order = 6)]
        [JsonProperty(PropertyName = "dateRequested")]
        public DateTime DateRequested { get; set; }

        [Column(Order = 7)]
        [JsonProperty(PropertyName = "requestedBy")]
        public int RequestedBy { get; set; }

        [Column(Order = 8)]
        [JsonProperty(PropertyName = "dateReviewed")]
        public DateTime? DateReviewed { get; set; }

        [Column(Order = 9)]
        [JsonProperty(PropertyName = "reviewedBy")]
        public int? ReviewedBy { get; set; }

        [Column(Order = 10)]
        [JsonProperty(PropertyName = "isEnabled")]
        public bool IsEnabled { get; set; }

        [Column(Order = 11)]
        [JsonProperty(PropertyName = "licenseType")]
        public byte LicenseType { get; set; }

        [Column(Order = 12)]
        [JsonProperty(PropertyName = "roleId")]
        public int RoleId { get; set; }
    }
}