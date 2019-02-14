using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuilder.Models.Response
{
    public class QueryResponse
    {
        public string QueryId { get; set; }

        public string QueryName { get; set; }

        public string QueryJson { get; set; }


        public string CreatedDate { get; set; }

        public string LastModified { get; set; }

        public string CreatedBy { get; set; }

    }
}
