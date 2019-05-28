using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class PerformanceEvaluationSummary
    {
        public int SkillId { get; set; }
        public string EvaluationTitle { get; set; }
        public int CompanyId { get; set; }

        public List<List<SkillActivityResult>> SkillProcedures { get; set; }

        public int NumberOfQuestions { get; set; }
        public double MasteryScore { get; set; }
        public int TotalEvaluations { get; set; }
        public int TotalMastered { get; set; }
        public double Average { get; set; }
        public double Median { get; set; }
        public double StandardDeviation { get; set; }
    }
}