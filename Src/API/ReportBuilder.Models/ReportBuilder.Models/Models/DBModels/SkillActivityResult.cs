using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class SkillActivityResult 
    {
        public int SkillProcedureId { get; set; }
        public DateTime DateCalculated { get; set; }
        public int CompanyId { get; set; }
        public double TotalAnswered { get; set; }
        public double TotalCorrect { get; set; }
        public string QuestionText { get; set; }
        public string DomainName { get; set; }
    }

    class SARExcellFields
    {
        public SARExcellFields() { }

        public SARExcellFields(String questionText, string totalCorrect, string section)
        {
            Question = questionText;
            PercentCorrect = totalCorrect.ToString();
            Section = section;
        }

        public string Question { get; set; }
        public string PercentCorrect { get; set; }
        public string Section { get; set; }
    }
}