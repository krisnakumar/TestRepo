using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuilder.Models.Request
{
    public class QueryRequest
    {
        public string Name { get; set; }        
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
    }
}
