using Amazon.Lambda.APIGatewayEvents;


// <copyright file="IEmployee.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
// <date>01-11-2018</date>
// <summary>Interfaces for the tasks</summary>
namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    /// Interfaces for the tasks api
    /// </summary>
    public interface ITask
    {
        APIGatewayProxyResponse GetTaskDetails(int userId, int workbookId);

        APIGatewayProxyResponse GetTaskAttemptsDetails(int userId, int workbookId, int taskId);
    }
}
