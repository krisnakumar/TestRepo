using Amazon.Lambda.APIGatewayEvents;


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
    }
}
