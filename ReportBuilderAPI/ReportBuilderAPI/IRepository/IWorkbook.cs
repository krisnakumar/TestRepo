using Amazon.Lambda.APIGatewayEvents;

// <copyright file="IWorkbook.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
// <date>01-11-2018</date>
// <summary>Interfaces for the workbooks</summary>
namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    /// Interfaces for the workbooks
    /// </summary>
    public interface IWorkbook
    {
        APIGatewayProxyResponse GetWorkbookDetails(int userId);
    }
}
