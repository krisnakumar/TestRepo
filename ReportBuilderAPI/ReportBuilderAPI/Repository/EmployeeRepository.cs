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
using System.Linq;
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

        private readonly Dictionary<string, string> columnList = new Dictionary<string, string>()
        {
                { Constants.EMPLOYEE_NAME, ", u.FName AS FirstName, u.LName AS LastName  " },
                { Constants.ROLE, ", r.Name AS Role "},
                { Constants.USERNAME, ", u.UserName" },
                { Constants.ALTERNATE_USERNAME, ", u.UserName2" },
                { Constants.TOTAL_EMPLOYEES, ", (SELECT COUNT(*) FROM dbo.Supervisor WHERE SupervisorId=u.id)" },
                { Constants.EMAIL, ", u.Email" },
                { Constants.ADDRESS, ", CONCAT(CASE WHEN u.Street1 IS NOT NULL THEN (u.Street1 + ',') ELSE '' END, CASE WHEN u.Street2 IS NOT NULL THEN (u.Street2 + ',') ELSE '' END, CASE WHEN u.City IS NOT NULL THEN (u.city+ ',') ELSE '' END, CASE WHEN u.State IS NOT NULL THEN (u.State+ ',') ELSE '' END, CASE WHEN u.Zip IS NOT NULL THEN (u.Zip+ ',') ELSE '' END) as address " },
                { Constants.PHONE, ", u.Phone" },
                { Constants.SUPERVISOR_NAME, ", (SELECT CONCAT(usr.LName, ',', usr.Fname ) FROM dbo.[User] usr LEFT JOIN Supervisor s on s.SupervisorId=usr.Id WHERE s.userId=u.Id) as supervisorName" },
                { Constants.USER_CREATED_DATE, ", u. DateCreated" },
                { Constants.USER_PERMS, ", u.UserPerms" },
                { Constants.SETTINGS_PERMS, ", u.settingsperms" },
                { Constants.COURSE_PERMS, ", u.courseperms" },
                { Constants.TRANSCRIPT_PERMS, ", u.Transcriptperms" },
                { Constants.COMPANY_PERMS, ", u.companyperms" },
                { Constants.REPORTS_PERMS, ", u.reportsperms" },
                { Constants.ANNOUNCEMENT_PERMS, ", u.announcementperms" },
                { Constants.SYSTEM_PERMS, ",u.systemperms" },
                { Constants.USER_ID, ", u. Id as userId" },
                { Constants.SUPERVISOR_ID, ",(SELECT supervisorId FROM Supervisor s WHERE userId=7) as supervisorId" },

        };

        private readonly Dictionary<string, string> conditions = new Dictionary<string, string>()
        {
            {Constants.ID, "u.ID" },
            {Constants.USERNAME, "u.UserName" },
            {Constants.USERNAME2, "u.UserName2" },
            {Constants.USER_CREATED_DATE, "u.DateCreated" },
            {Constants.FIRSTNAME, "u.FName" },
            {Constants.LASTNAME, "u.LName" },
            {Constants.MIDDLENAME, "u.MName" },
            {Constants.EMAIL, "u.Email" },
            {Constants.CITY, "u.City" },
            {Constants.STATE, "u.State" },
            {Constants.ZIP, "u.Zip" },
            {Constants.PHONE, "u.phone" },
            {Constants.ROLE, "r.Name" }
        };


        private readonly Dictionary<string, List<string>> tableJoins = new Dictionary<string, List<string>>()
        {
            { "LEFT JOIN UserRole ur on ur.UserId=u.Id LEFT JOIN Role r on r.Id=ur.roleId", new List<string> {Constants.ROLEID, Constants.ROLE} }
        };

        /// <summary>
        /// 
        /// </summary>

        public APIGatewayProxyResponse GetEmployeeDetails(string requestBody, int companyId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            string query = string.Empty, tableJoin = string.Empty, selectQuery = string.Empty, whereQuery = string.Empty;
            EmployeeResponse employeeResponse = new EmployeeResponse();
            List<string> fieldList = new List<string>();
            try
            {
                selectQuery = "SELECT  ";
                EmployeeRequest employeeRequest = JsonConvert.DeserializeObject<EmployeeRequest>(requestBody);

                //getting column List
                query = string.Join("", (from column in columnList
                                         where employeeRequest.ColumnList.Any(x => x == column.Key)
                                         select column.Value));
                query = query.TrimStart(',');
                query = selectQuery + query;
                query += " FROM dbo.[User] u  ";
                
                //get table joins
                fieldList = employeeRequest.ColumnList.ToList();
                fieldList.AddRange(employeeRequest.Fields.Select(x => x.Name.ToUpper()).ToArray());
                tableJoin = string.Join("", from joins in tableJoins
                                            where fieldList.Any(x => joins.Value.Any(y => y == x))
                                            select joins.Key);

                query += tableJoin;

                //getting where conditions
                whereQuery = string.Join("", from emplyoee in employeeRequest.Fields
                                             select conditions.Where(x => x.Key == emplyoee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault() + emplyoee.Operator + ("'" + emplyoee.Value + "'") + (!string.IsNullOrEmpty(emplyoee.Bitwise) ? (" " + emplyoee.Bitwise + " ") : string.Empty));
                whereQuery = (!string.IsNullOrEmpty(whereQuery)) ? (" WHERE " + whereQuery) : string.Empty;

                query += whereQuery;

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
