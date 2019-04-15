using ReportBuilder.Models.Models;
using System.Collections.Generic;


/*
 <copyright file="EmployeeRequest.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>04-12-2018</date>
 <summary>
    Model for QueryBuilder employee request object
 </summary>
*/
namespace ReportBuilder.Models.Request
{
    public class QueryBuilderRequest : AuthorizorRequest
    {
        public List<EmployeeModel> Fields { get; set; }

        public string[] ColumnList { get; set; }
        public string QueryName { get; set; }
        public string EntityName { get; set; }

        public string AppType { get; set; }

        public string QueryId { get; set; }

      
        public QueryBuilderRequest Payload { get; set; }
    }
}
