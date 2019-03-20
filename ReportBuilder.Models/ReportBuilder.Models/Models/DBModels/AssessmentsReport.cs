using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class AssessmentsReport
    {
        public AssessmentsReport()
        {
        }

        public int Id { get; set; }

        public string Employee { get; set; }

        public string FullName { get; set; }
        
        public string UserName { get; set; }

        public string CompanyName { get; set; }

        [StringLength(255)]
        public string Title { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime DateTaken { get; set; }

        public string Performed_By { get; set; }

        public int Exceptions { get; set; }

        public string Status { get; set; }

        public string PDF { get; set; }
    }
}