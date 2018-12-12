using Amazon.Lambda.APIGatewayEvents;
using ReportBuilder.Models.Models;


/*
  <copyright file = "IEmployee.cs" >
        Copyright(c) 2018 All Rights Reserved
  </copyright>
  <author> Shoba Eswar </author>
  <date>10-10-2018</date>
  <summary>
        Interface for Employees
  </summary>
 */
namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    ///     Interface that handles Employees crud operations
    /// </summary>
    public interface IEmployee
    {
        APIGatewayProxyResponse GetEmployeeList(int userId, QueryStringModel queryStringModel);
    }
}
