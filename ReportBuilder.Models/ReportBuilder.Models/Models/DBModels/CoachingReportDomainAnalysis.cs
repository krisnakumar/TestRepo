using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class CoachingReportDomainAnalysis
    {
        public CoachingReportDomainAnalysis()
        {
        }
        public String DomainName { get; set; }
        public String RecommendationText { get; set; }
        public int DomainId { get; set; }
        public int TotalQuestions { get; set; }
        public int MissedQuestions {get; set;}
        public int UnansweredQuestions { get; set; }
    }
}