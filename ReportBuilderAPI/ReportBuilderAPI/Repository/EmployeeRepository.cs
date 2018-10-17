using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.DatabaseManager;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;



// <copyright file="EmployeeRepository.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
// <date>10-10-2018</date>
// <summary>Repository that helps to read the data from the Table</summary>
namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Repository that helps to read the data from the table
    /// </summary>
    public class EmployeeRepository : IEmployee
    {
        /// <summary>
        /// Get list of employee who currently working under the specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetEmployeeList(int userId, QueryStringModel queryStringModel)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<EmployeeResponse> employeeList = new List<EmployeeResponse>();
            string query = string.Empty;
            try
            {
                if (queryStringModel != null)
                {
                    query = EmployeeQueries.GetWorkBookDetails(userId, queryStringModel.CompletedWorkBooks, queryStringModel.WorkBookInDue, queryStringModel.PastDueWorkBook);
                }
                else
                {
                    query = EmployeeQueries.ReadEmployeeDetails(userId);
                }

                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        EmployeeResponse employeeResponse = new EmployeeResponse
                        {
                            FirstName = Convert.ToString(sqlDataReader["FirstName"]),
                            LastName = Convert.ToString(sqlDataReader["LastName"]),
                            Role = Convert.ToString(sqlDataReader["Role"]),
                            WorkbookName = Convert.ToString(sqlDataReader["WorkbookName"]),
                            AssignedWorkBooks = Convert.ToInt32(sqlDataReader["AssignedWorkbooks"]),
                            InDueWorkBooks = Convert.ToInt32(sqlDataReader["WorkbooksinDue"]),
                            PastDueWorkBooks = Convert.ToInt32(sqlDataReader["PastDueWorkbooks"]),
                            CompletedWorkBooks = Convert.ToInt32(sqlDataReader["CompletedWorkbooks"]),
                            EmployeeCount = Convert.ToInt32(sqlDataReader["TotalEmployees"])
                        };
                        employeeList.Add(employeeResponse);
                    }
                }
                return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(employeeList), 0);
            }
            catch (Exception getEmployeeException)
            {
                LambdaLogger.Log(getEmployeeException.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }
    }
}
