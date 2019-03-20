using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class JobReportTask
    {
        public string TaskName { get; set; }
        public int TaskId { get; set; }
        public string QualificationStatus { get; set; }
        public string DateQualified { get; set; }
        public string DateExpires { get; set; }
        //public List<Skill> Skills { get; set; }
    }
}