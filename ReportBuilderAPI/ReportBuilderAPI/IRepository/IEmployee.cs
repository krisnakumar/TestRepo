using Amazon.Lambda.APIGatewayEvents;



// <copyright file="IEmployee.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
// <date>10-10-2018</date>
// <summary>Interfaces for the Employees</summary>
namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    /// Interfaces that handles the Employees crud operations
    /// </summary>
    public interface IEmployee
    {
        APIGatewayProxyResponse GetEmployeeList(int userId);
    }
}
