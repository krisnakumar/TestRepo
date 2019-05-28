using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class EventDetail
    {
        public int Id { get; set; }

        [StringLength(2048)]
        public string Title { get; set; }

        [StringLength(8000)]
        public string Description { get; set; }

        public int? Capacity { get; set; }

        [StringLength(8000)]
        public string Location { get; set; }

        public int? OrganizerId { get; set; }

        public int? EvaluatorId { get; set; }

        public int? FacilitatorId { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? StartDateTime { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? StopDateTime { get; set; }

        public int? CompanyId { get; set; }

        public short? Type { get; set; }

        public short? Status { get; set; }

        [StringLength(3)]
        public string TimeZone { get; set; }

        public string FacilitatorName { get; set; }
        public string OrganizerName { get; set; }
        public string EvaluatorName { get; set; }
        public string CourseName { get; set; }
        public string CourseGUID { get; set; }
        public int CourseIntId { get; set; }
        public string PreReqName { get; set; }
        public int CourseId2 { get; set; }


    }
}