using System;


namespace ReportBuilder.Models.Request
{
    public class QueryRequest : AuthorizorRequest
    {
        public string Name { get; set; }        
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
    }
}
