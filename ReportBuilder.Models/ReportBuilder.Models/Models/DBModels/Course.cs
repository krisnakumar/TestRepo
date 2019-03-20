using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    /// <summary>
    /// Manages OnBoard course records
    /// </summary>
    [Table("Course")]
    public class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Desc { get; set; }

        public int CreatedByUser { get; set; }

        public DateTime DateCreated { get; set; }

        public byte Type { get; set; }

        public int SubType { get; set; }

        public int SeriesId { get; set; }

        public int Credits { get; set; }

        public int RequalificationInterval { get; set; }

        public int PassScore { get; set; }

        public int TaskId { get; set; }

        public bool IsEnabled { get; set; }

        public bool CanCertify { get; set; }

        public bool ShowTitle { get; set; }

        public byte DisplayTypeAllowed { get; set; }

        public long? Options { get; set; }

        public Int16? CustomMessage { get; set; }

        public bool? IsCustom { get; set; }

        public Guid? ParentCourseId { get; set; }

        public int Id2 { get; set; }

        public int? AssignmentWorkFlow { get; set; }

        [StringLength(1024)]
        public string CompletionEvent { get; set; }

        public int? RemainingCurrentInterval { get; set; }

        public string ProctorVerificationTemplate { get; set; }

        public int? OrderNumber { get; set; }

        public int? EventTypeAllowed { get; set; }
    }
}