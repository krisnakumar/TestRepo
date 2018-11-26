using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.DatabaseManager;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;



// <copyright file="EmployeeRepository.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Shoba Eswar</author>
// <date>10-10-2018</date>
// <summary>Repository that helps to read the data from the Table</summary>
namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Repository that helps to read the data from the table
    /// </summary>
    public class EmployeeRepository : IEmployee, IDisposable
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
                    query = Employee.GetWorkBookDetails(userId, queryStringModel.CompletedWorkBooks, queryStringModel.WorkBookInDue, queryStringModel.PastDueWorkBook);
                }
                else
                {
                    query = Employee.ReadEmployeeDetails(userId);
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
                            EmployeeCount = Convert.ToInt32(sqlDataReader["TotalEmployees"]),
                            UserId = Convert.ToInt32(sqlDataReader["UserId"])
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


        /// <summary>
        /// 
        /// </summary>

        public APIGatewayProxyResponse GetEmployeeDetails(string requestBody, int companyId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<EmployeeModel> fieldList = new List<EmployeeModel>();
            EmployeeRequest employeeRequest = new EmployeeRequest();
            string query = string.Empty, tableJoin = string.Empty, selectQuery = string.Empty;
            EmployeeResponse employeeResponse = new EmployeeResponse();
            string[] columnList;

            try
            {

                selectQuery = "SELECT DISTINCT ";
                employeeRequest = JsonConvert.DeserializeObject<EmployeeRequest>(requestBody);
                columnList = employeeRequest.ColumnList.Split('|');
                foreach (string column in columnList)
                {
                    switch (column.ToUpper())
                    {
                        case Constants.EMPLOYEE_NAME:
                            query += ", u.FName AS FirstName, u.LName AS LastName  ";
                            break;
                        case Constants.ROLE:
                            query += ", r.Name AS Role ";
                            tableJoin += "LEFT JOIN UserRole ur ON ur.userId=u.Id ";
                            break;
                        case Constants.WORKBOOK_NAME:
                            query += ", wb.Name AS WorkbookName ";
                            tableJoin += "	LEFT JOIN UserWorkBook uwb ON uwb.UserId=u.Id    LEFT JOIN WorkBook wb ON wb.Id = uwb.WorkBookId";
                            break;
                        case Constants.ASSIGNED_WORKBOOK:
                            query += ", (SELECT COUNT(DISTINCT uwt.WorkBookId)  FROM dbo.UserWorkBook uwt WHERE uwt.UserId IN((SELECT u.Id UNION SELECT* FROM getChildUsers (u.Id))) AND uwt.IsEnabled = 1) AS AssignedWorkbooks";
                            break;
                        case Constants.INCOMPLETE_WORKBOOK:
                            break;
                        case Constants.WORKBOOK_DUE:
                            break;
                        case Constants.PAST_DUE_WORKBOOK:
                            break;
                        case Constants.COMPLETED_WORKBOOK:
                            break;
                        case Constants.ASSIGNED_QUALIFICATIONS:

                            break;
                        case Constants.COMPLETED_QUALIFICATIONS:

                            break;
                        case Constants.IN_COMPLETE_QUALIFICATIONS:

                            break;
                        case Constants.PAST_DUE_QUALIFICATIONS:

                            break;
                        case Constants.QUALIFICATIONS_IN_DUE:

                            break;
                        case Constants.TOTAL_EMPLOYEES:

                            break;
                    }
                }
                query = query.TrimStart(',');
                query = selectQuery + query;
                query += " FROM dbo.[User] u WHERE ";
                query += tableJoin;
                foreach (EmployeeModel field in employeeRequest.Fields)
                {
                    switch (field.Field.ToUpper())
                    {
                        case Constants.ID:
                            query += "ID=" + field.Value;
                            break;
                        case Constants.USERNAME:
                            query += "UserName='" + field.Value + "'";
                            break;
                        case Constants.USERNAME2:
                            query += "UserName2='" + field.Value + "'";
                            break;
                    }
                }
                ReadEmployeeDetails(query);
                return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(employeeResponse), 0);
            }
            catch (Exception getEmployeeDetails)
            {
                LambdaLogger.Log(getEmployeeDetails.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }

        /// <summary>
        /// Read Employee Details
        /// </summary>
        /// <param name="query"></param>

        private EmployeeResponse ReadEmployeeDetails(string query)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            EmployeeResponse employeeResponse = new EmployeeResponse();
            try
            {
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        employeeResponse = new EmployeeResponse
                        {

                        };
                    }
                }
                return employeeResponse;
            }
            catch (Exception readEmployeeDetailsException)
            {
                LambdaLogger.Log(readEmployeeDetailsException.ToString());
                return employeeResponse;
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }

        /// <summary>
        /// Disposing object once the process is completed
        /// </summary>
        public void Dispose()
        {

            try
            {

            }
            catch (Exception disposeException)
            {
                LambdaLogger.Log(disposeException.ToString());
            }
            finally
            {

            }
        }
    }
}
