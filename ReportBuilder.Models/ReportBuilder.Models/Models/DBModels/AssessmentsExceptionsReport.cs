using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class AssessmentsExceptionsReport
    {
        public AssessmentsExceptionsReport()
        {
        }

        public int UserId { get; set; }

        public string UserFullName { get; set; }

        public string UserName { get; set; }

        public string UserDisplayName { get; set; }

        public int FormVersionId { get; set; }

        public string FormVersionTitle { get; set; }

        public int AuditorId { get; set; }

        public string AuditorFullName { get; set; }

        public string AuditorUserName { get; set; }
        [Key]
        public int AssessmentExceptionId { get; set; }

        [StringLength(255)]
        public string ExceptionTypeTitle { get; set; }

        public string ExceptionText { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime DateTaken { get; set; }

        public string PDF { get; set; }

        public bool IsResolved { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? DateResolved { get; set; }
        public string DateResolved_DateOnly { get; set; }

        public string Status { get; set; }

        
    }
}