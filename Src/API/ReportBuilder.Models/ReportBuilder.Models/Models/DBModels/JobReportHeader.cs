using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class JobReportHeader
    {
        public JobReportHeader()
        {
            Tasks = new List<JobReportTask>();
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string CompanyName { get; set; }
        public string JobTitle { get; set; }
        public string JobStatus { get; set; }
        public int JobMatrixId { get; set; }
        public int JobCompanyId { get; set; }
        public string JobCompanyName { get; set; }
        public List<JobReportTask> Tasks { get; set; }
    }
}