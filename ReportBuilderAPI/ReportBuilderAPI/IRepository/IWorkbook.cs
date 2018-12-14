using Amazon.Lambda.APIGatewayEvents;


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
        APIGatewayProxyResponse GetWorkbookDetails(int userId);
    }
}
