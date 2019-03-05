using Amazon.Lambda.APIGatewayEvents;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using System.Collections.Generic;


/*
 <copyright file = "IWorkbook.cs" >
    Copyright(c) 2018 All Rights Reserved
 </copyright>
 <author> Shoba Eswar </author>
 <date>01-11-2018</date>
 <summary>
    Interface for workbooks
 </summary>
*/
namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    ///     Interface that handles Workbooks crud operations
    /// </summary>
    public interface IWorkbook
    {
        APIGatewayProxyResponse GetWorkbookDetails(string requestBody, int userId);
        string CreateWorkbookQuery(QueryBuilderRequest queryRequest, int companyId);
        List<WorkbookResponse> ReadWorkBookDetails(string query, Dictionary<string, string> parameters);
    }
}
