using System;
using System.Collections.Generic;

namespace OnBoardLMS.WebAPI.Models
{
    public class CoachingReport
    {
        public CoachingReport()
        {
            Results = new List<CoachingReportDomainAnalysis>();
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
        /// <summary>
        /// Name of the skill or evaluation
        /// </summary>
        public string EvaluationTitle { get; set; }
        /// <summary>
        /// Code for the skill or evaluation
        /// </summary>
        public string EvaluationCode { get; set; }
        /// <summary>
        /// Name of the task
        /// </summary>
        public string TaskTitle { get; set; }
        public string TaskCode { get; set; }
        /// <summary>
        /// Text status (Pass/Fail)
        /// </summary>
        public string EvaluationStatus { get; set; }
        /// <summary>
        /// Evaluator or Proctor User Name
        /// </summary>
        public string EvaluatorUserName { get; set; }
        /// <summary>
        /// Evaluator or Proctor Full Name
        /// </summary>
        public string EvaluatorFullName { get; set; }
        /// <summary>
        /// Evaluation or Skill Type 
        /// </summary>
        
        public int SkillId { get; set; }
        public int TotalEvaluationNumber { get; set; }
        public int TotalQuestions { get; set; }
        /// <summary>
        /// Integer Score that defines the master (passing score)
        /// </summary>
        public int Mastery { get; set; }
        public double UserScore { get; set; }
        public double AverageScore { get; set; }

        public DateTime EvaluationDateTaken { get; set; }
        public int EvaluationAttempt { get; set; }
        public int UserId { get; set; }
        public List<CoachingReportDomainAnalysis> Results { get; set; }

    }

    public class CoachingReportIDandEmailHolder
    {
        public List<string> EmailList { get; set; }
        public List<CoachingReportIdHolder> IDList { get; set; }
    }
     public class CoachingReportIdHolder
    {
        public int CompanyId { get; set; }
        public int SkillActivityId { get; set; }
        public int UserId { get; set; }
    }

}