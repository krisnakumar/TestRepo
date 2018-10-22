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

    }
}
