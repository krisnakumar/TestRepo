using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class CoachingReportAggregated
    {
        public CoachingReportAggregated()
        {
            CoachingReports = new List<CoachingReport>();
        }
        public string CompanyLogo { get; set; }
        public string CompanyName { get; set; }
        public int CompanyId { get; set; }
        public long CompPreference { get; set; }
        public string UserName { get; set; }
        public string UserFullName { get; set; }
        public string CostCenter { get; set; }
        public string WorkLocation { get; set; }
        public string Department { get; set; }
        public string UserSupervisorName { get; set; }
        public int SeriesId { get; set; }
        public string UserSupervisorUserName { get; set; }
        public int TotalEvaluationNumber { get; set; }
        public int TotalQuestions { get; set; }
        /// <summary>
        /// Integer Score that defines the master (passing score)
        /// </summary>
        public int Mastery { get; set; }
        public double UserScore { get; set; }
        public double AverageScore { get; set; }
        public int UserId { get; set; }
        public List<CoachingReport> CoachingReports { get; set; }
    }
}