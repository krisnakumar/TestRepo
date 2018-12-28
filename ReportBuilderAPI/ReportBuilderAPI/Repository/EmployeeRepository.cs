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
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;


/* 
<copyright file="EmployeeRepository.cs">
    Copyright (c) 2018 All Rights Reserved
</copyright>
<author>Shoba Eswar</author>
<date>10-10-2018</date>
<summary> 
    Repository that helps to read employee(s) details for specific user(s) from database.
</summary>
*/
namespace ReportBuilderAPI.Repository
{
    /// <summary>
    ///     Class that handles to read employee(s) details under specific user(s) from database
    /// </summary>
    public class EmployeeRepository : IEmployee
    {

        /// <summary>
        ///     Get list of employee(s) who currently working under the specific user [ReportBuilder]
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="queryStringModel"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetEmployeeList(int userId, QueryStringModel queryStringModel)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<EmployeeResponse> employeeList = new List<EmployeeResponse>();
            string query = string.Empty;
            try
            {
                if (userId != 0)
                {
                    if (queryStringModel != null)
                    {
                        query = Employee.GetWorkBookDetails(userId, queryStringModel.CompletedWorkBooks, queryStringModel.WorkBookInDue, queryStringModel.PastDueWorkBook);
                    }
                    else
                    {
                        query = Employee.ReadEmployeeDetails(userId);
                    }

                    DataSet dataSet = databaseWrapper.ExecuteAdapter(query);
                    foreach (DataRow rows in dataSet.Tables[0].Rows)
                    {
                        EmployeeResponse employeeResponse = new EmployeeResponse
                        {
                            FirstName = Convert.ToString(rows["FirstName"]),
                            LastName = Convert.ToString(rows["LastName"]),
                            Role = Convert.ToString(rows["Role"]),
                            WorkbookName = Convert.ToString(rows["WorkbookName"]),
                            AssignedWorkBooks = Convert.ToInt32(rows["AssignedWorkbooks"]),
                            InDueWorkBooks = Convert.ToInt32(rows["WorkbooksinDue"]),
                            PastDueWorkBooks = Convert.ToInt32(rows["PastDueWorkbooks"]),
                            CompletedWorkBooks = Convert.ToInt32(rows["CompletedWorkbooks"]),
                            EmployeeCount = Convert.ToInt32(rows["TotalEmployees"]),
                            UserId = Convert.ToInt32(rows["UserId"])
                        };
                        employeeList.Add(employeeResponse);
                    }
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(employeeList), 0);
                }
                else
                {
                    return ResponseBuilder.BadRequest("userId");
                }
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
        ///     Dictionary having Column list for the employee details.
        ///     Based on column name, query is being formed/updated
        /// </summary>
        private readonly Dictionary<string, string> columnList = new Dictionary<string, string>()
        {
                { Constants.EMPLOYEE_NAME, ", (ISNULL(NULLIF(u.LName, '') + ', ', '') + u.Fname)  as employeeName " },
                { Constants.ROLE, ", r.Name AS Role "},
                { Constants.USERNAME, ", u.UserName" },
                { Constants.ALTERNATE_USERNAME, ", u.UserName2" },
                { Constants.TOTAL_EMPLOYEES, ", (SELECT COUNT(*) FROM dbo.Supervisor WHERE SupervisorId=u.id) as employeeCount" },
                { Constants.EMAIL, ", u.Email" },
                { Constants.ADDRESS, ", CONCAT(CASE WHEN u.Street1 IS NOT NULL THEN (u.Street1 + ',') ELSE '' END, CASE WHEN u.Street2 IS NOT NULL THEN (u.Street2 + ',') ELSE '' END, CASE WHEN u.City IS NOT NULL THEN (u.city+ ',') ELSE '' END, CASE WHEN u.State IS NOT NULL THEN (u.State+ ',') ELSE '' END, CASE WHEN u.Zip IS NOT NULL THEN (u.Zip+ ',') ELSE '' END) as address " },
                { Constants.PHONE, ", u.Phone" },
                { Constants.SUPERVISOR_NAME, ", (SELECT (ISNULL(NULLIF(usr.LName, '') + ', ', '') + usr.Fname) FROM dbo.[User] usr LEFT JOIN Supervisor s on s.SupervisorId=usr.Id WHERE s.userId=u.Id) as supervisorName" },
                { Constants.USER_CREATED_DATE, ", u.DateCreated" },
                { Constants.USER_PERMS, ", u.UserPerms" },
                { Constants.SETTINGS_PERMS, ", u.settingsperms" },
                { Constants.COURSE_PERMS, ", u.courseperms" },
                { Constants.TRANSCRIPT_PERMS, ", u.Transcriptperms" },
                { Constants.COMPANY_PERMS, ", u.companyperms" },
                { Constants.FORUM_PERMS, ", u.forumperms" },
                { Constants.COM_PERMS, ", u.comperms" },
                { Constants.REPORTS_PERMS, ", u.reportsperms" },
                { Constants.ANNOUNCEMENT_PERMS, ", u.announcementperms" },
                { Constants.SYSTEM_PERMS, ",u.systemperms" },
                { Constants.USERID, ", u. Id as userId" },
                { Constants.SUPERVISOR_ID, ",(SELECT supervisorId FROM Supervisor s WHERE userId=u.Id) as supervisorId" }

        };

        /// <summary>
        ///     Dictionary having fields that requried for the employee entity.
        ///     Based on fields, query is being formed/updated
        /// </summary>
        private readonly Dictionary<string, string> employeeFields = new Dictionary<string, string>()
        {
            {Constants.USERID, "u.ID" },
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
            {Constants.ROLE, "r.Name " },
            {Constants.PHONE, "u.phone" },
            {Constants.STATUS, "u.IsEnabled " },
            {Constants.REPORTING, "s.IsDirectReport " },
            {Constants.PHOTO, "u.Photo " },
            {Constants.SUPERVISORID, "s.SupervisorId " }
        };


        /// <summary>
        ///     Dictionary having list of tables that requried to get the columns for employee(s) result
        /// </summary>
        private readonly Dictionary<string, List<string>> tableJoins = new Dictionary<string, List<string>>()
        {
            { " LEFT JOIN UserRole ur on ur.UserId=u.Id LEFT JOIN Role r on r.Id=ur.roleId", new List<string> {Constants.ROLEID, Constants.ROLE} },
            { " LEFT JOIN Supervisor s on s.userId=u.Id", new List<string> {Constants.SUPERVISORID, Constants.REPORTING} }
        };

        /// <summary>
        ///     Get employee(s) details based on provided companyId [QueryBuilder]
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="companyId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetEmployeeDetails(string requestBody, int companyId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            string query = string.Empty, tableJoin = string.Empty, selectQuery = string.Empty, whereQuery = string.Empty;
            List<EmployeeResponse> employeeResponse;
            List<string> fieldList = new List<string>();
            try
            {
                selectQuery = "SELECT  ";
                QueryBuilderRequest employeeRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(requestBody);

                //getting column List
                query = string.Join("", (from column in columnList
                                         where employeeRequest.ColumnList.Any(x => x == column.Key)
                                         select column.Value));
                query = query.TrimStart(',');
                query = selectQuery + query;
                query += " FROM dbo.[User] u  LEFT JOIN UserCompany uc on uc.UserId=u.Id ";

                //get table joins
                fieldList = employeeRequest.ColumnList.ToList();
                fieldList.AddRange(employeeRequest.Fields.Select(x => x.Name.ToUpper()).ToArray());
                tableJoin = string.Join("", from joins in tableJoins
                                            where fieldList.Any(x => joins.Value.Any(y => y == x))
                                            select joins.Key);

                query += tableJoin;


                bool isExist = employeeRequest.Fields.Any(x => workbookFieldList.Contains(x.Name.ToUpper()));

                if (!isExist)
                {
                    //getting where conditions
                    whereQuery = string.Join("", from employee in employeeRequest.Fields
                                                 select (!string.IsNullOrEmpty(employee.Bitwise) ? (" " + employee.Bitwise + " ") : string.Empty) + (!string.IsNullOrEmpty(employeeFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault()) ? (employeeFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault() +
                            CheckOperator(employee.Operator, employee.Value.Trim(), employee.Name)) : string.Empty));
                }

                whereQuery = (!string.IsNullOrEmpty(whereQuery)) ? (" WHERE uc.CompanyId=" + companyId + " and  (" + whereQuery) : string.Empty;
                query += whereQuery + " )";
                employeeResponse = ReadEmployeeDetails(query);
                if (employeeResponse != null)
                {
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(employeeResponse), 0);
                }
                else
                {
                    return ResponseBuilder.InternalError();
                }
            }
            catch (Exception getEmployeeDetailsException)
            {
                LambdaLogger.Log(getEmployeeDetailsException.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        private readonly List<string> workbookFieldList = new List<string>
        {
            Constants.ASSIGNED,
            Constants.COMPLETED,
            Constants.WORKBOOK_IN_DUE,
            Constants.PAST_DUE,
            Constants.WORKBOOK_ID,
            Constants.SUPERVISOR_ID
        };

        /// <summary>
        /// form he query based on the operator Name
        /// </summary>
        /// <param name="operatorName"></param>
        /// <returns></returns>
        public string CheckOperator(string operatorName, string value, string field)
        {
            string queryString = string.Empty;
            try
            {
                if (!workbookFieldList.Contains(field.ToUpper()))
                {
                    switch (operatorName.ToUpper())
                    {
                        case Constants.CONTAINS:
                            queryString = " like '%" + value + "%'";
                            break;
                        case Constants.DOES_NOT_CONTAINS:
                            queryString = " not like '%" + value + "%'";
                            break;
                        case Constants.START_WITH:
                            queryString = " like '" + value + "%'";
                            break;
                        case Constants.END_WITH:
                            queryString = " like '%" + value + "'";
                            break;
                        default:
                            queryString = operatorName + ("'" + value + "'");
                            break;
                    }
                }
                return queryString;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return queryString;
            }
        }

        /// <summary>
        ///     Creating response object after reading Employee(s) details from database
        /// </summary>
        /// <param name="query"></param>
        /// <returns>EmployeeResponse</returns>
        private List<EmployeeResponse> ReadEmployeeDetails(string query)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            EmployeeResponse employeeResponse;
            List<EmployeeResponse> employeeList = new List<EmployeeResponse>();
            try
            {
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query, new Dictionary<string, string>() { });
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        employeeResponse = new EmployeeResponse
                        {
                            UserId = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'userId'").Count() == 1) ? (int?)sqlDataReader["userId"] : null,
                            SupervisorId = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'supervisorId'").Count() == 1) ? (int?)sqlDataReader["supervisorId"] : null,
                            TotalEmployees = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'employeeCount'").Count() == 1) ? (int?)sqlDataReader["employeeCount"] : null,

                            EmployeeName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'employeeName'").Count() == 1) ? Convert.ToString(sqlDataReader["employeeName"]) : null,

                            Role = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Role'").Count() == 1) ? Convert.ToString(sqlDataReader["Role"]) : null,

                            UserName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'UserName'").Count() == 1) ? Convert.ToString(sqlDataReader["UserName"]) : null,

                            AlternateName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'UserName2'").Count() == 1) ? Convert.ToString(sqlDataReader["UserName2"]) : null,
                            Email = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'email'").Count() == 1) ? Convert.ToString(sqlDataReader["email"]) : null,

                            Address = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Address'").Count() == 1) ? Convert.ToString(sqlDataReader["Address"]) : null,
                            Phone = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Phone'").Count() == 1) ? Convert.ToString(sqlDataReader["Phone"]) : null,
                            SupervisorName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'SupervisorName'").Count() == 1) ? Convert.ToString(sqlDataReader["SupervisorName"]) : null,
                            UserCreatedDate = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'DateCreated'").Count() == 1) ? Convert.ToString(sqlDataReader["DateCreated"]) : null,

                            Userpermission = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'UserPerms'").Count() == 1) ? (bool?)(sqlDataReader["UserPerms"]) : null,
                            SettingsPermission = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'settingsperms'").Count() == 1) ? (bool?)sqlDataReader["settingsperms"] : null,
                            CoursePermission = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'courseperms'").Count() == 1) ? (bool?)sqlDataReader["courseperms"] : null,
                            TranscriptPermission = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Transcriptperms'").Count() == 1) ? (bool?)sqlDataReader["Transcriptperms"] : null,
                            CompanyPermission = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'companyperms'").Count() == 1) ? (bool?)sqlDataReader["companyperms"] : null,
                            ForumPermission = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'forumperms'").Count() == 1) ? (bool?)sqlDataReader["forumperms"] : null,
                            ComPermission = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'comperms'").Count() == 1) ? (bool?)sqlDataReader["comperms"] : null,
                            ReportsPermission = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'reportsperms'").Count() == 1) ? (bool?)sqlDataReader["reportsperms"] : null,
                            AnnouncementPermission = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'announcementperms'").Count() == 1) ? (bool?)sqlDataReader["announcementperms"] : null,
                            SystemPermission = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'systemperms'").Count() == 1) ? (bool?)sqlDataReader["systemperms"] : null
                        };
                        // Adding each employee details in array list
                        employeeList.Add(employeeResponse);
                    }
                }
                return employeeList;
            }
            catch (Exception readEmployeeDetailsException)
            {
                LambdaLogger.Log(readEmployeeDetailsException.ToString());
                return employeeList;
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }
    }
}
