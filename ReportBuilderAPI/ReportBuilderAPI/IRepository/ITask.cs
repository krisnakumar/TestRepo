using Amazon.Lambda.APIGatewayEvents;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using System.Collections.Generic;


/*
 <copyright file = "ITask.cs" >
    Copyright(c) 2018 All Rights Reserved
 </copyright>
 <author> Shoba Eswar </author>
 <date>01-11-2018</date>
 <summary>
    Interface for tasks
 </summary>
*/
namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    ///     Interface that handles Tasks crud operations
    /// </summary>
    public interface ITask
    {
        APIGatewayProxyResponse GetTaskDetails(int userId, int workbookId);

        APIGatewayProxyResponse GetTaskAttemptsDetails(int userId, int workbookId, int taskId);

        string CreateTaskQuery(QueryBuilderRequest queryRequest, int companyId);

        APIGatewayProxyResponse GetQueryTaskDetails(string requestBody, int companyId);


        Dictionary<string, string> Getparameters(QueryBuilderRequest queryRequest, int companyId);

        List<TaskResponse> ReadTaskDetails(string query, Dictionary<string, string> parameters);
    }
}
