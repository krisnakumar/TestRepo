using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Class for managing CourseAttachment records (My Downloads) with additional details such as the File's S3 URL
    /// </summary>
    public class CourseAttachmentDetail
    {
        /// <summary>
        /// AttachmentId is left as a nullable field in the model to prevent an issue with Entity thinking the model state
        /// is invalid when a POST is made and we set the AttachmentId to a new GUID within the controller.  It still sees
        /// that the request that came in didn't have an AttachmentId specified and throws the invalid model state error.
        /// </summary>
        [Key]
        [JsonProperty(PropertyName = "attachmentId")]
        public Guid? AttachmentId { get; set; }

        [JsonProperty(PropertyName = "courseId")]
        public Guid? CourseId { get; set; }

        [JsonProperty(PropertyName = "companyId")]
        public int CompanyId { get; set; }

        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }

        [JsonProperty(PropertyName = "path")]
        [StringLength(128)]
        [Required(AllowEmptyStrings = false)]
        public string Path { get; set; }

        [JsonProperty(PropertyName = "dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty(PropertyName = "isCurrent")]
        public bool IsCurrent { get; set; }

        [JsonProperty(PropertyName = "isShared")]
        public bool IsShared { get; set; }

        [JsonProperty(PropertyName = "desc")]
        [StringLength(2000)]
        public string Desc { get; set; }

        [JsonProperty(PropertyName = "fileUrl")]
        public string FileUrl { get; set; }

        [JsonProperty(PropertyName = "taskId")]
        public int? TaskId { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public int? CreatedBy { get; set; }
    }
}