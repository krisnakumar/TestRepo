using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBoardLMS.WebAPI.Models
{
    public class SkillActivityDraftReport
    {
        public int Id { get; set; }

        public string Student { get; set; }

        public string Evaluation { get; set; }

        public string Evaluator { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateTaken { get; set; }
    }
}