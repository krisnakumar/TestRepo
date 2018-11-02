using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuilder.Models.Response
{
    public class AttemptsResponse
    {
        public string Status { get; set; }
        public DateTime DateTime { get; set; }
        public string Location { get; set; }

        public string Evaluator { get; set; }

        public string Comments { get; set; }
    }
}
