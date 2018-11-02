using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuilder.Models.Models
{
    public class QueryStringModel
    {
        public int CompletedWorkBooks {get;set;}

        public int WorkBookInDue { get; set; }

        public int PastDueWorkBook { get; set; }

        public string Param { get; set; }

        public string CompletedWorkBook { get; set; }

        public string WorkBooksInDue { get; set; }

        public string PastDueWorkBooks { get; set; }

    }
}
