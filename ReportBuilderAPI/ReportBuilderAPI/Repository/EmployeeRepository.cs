using Amazon.Lambda.Core;
using DataInterface.Database;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Helpers;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;


namespace ReportBuilderAPI.Repository
{
    /// <summary>
    ///     Class that handles to read employee(s) details under specific user(s) from database
    /// </summary>
    public class EmployeeRepository : IEmployee
    {
        /// <summary>
        ///     Dictionary having Column list for the employee details.
        ///     Based on column name, query is being formed/updated
        /// </summary>
        private readonly Dictionary<string, string> columnList = new Dictionary<string, string>()
        {
                { Constants.EMPLOYEE_NAME, ", (ISNULL(NULLIF(u.LName, '') + ', ', '') + u.Fname)  AS employeeName " },
                { Constants.ROLE, ", r.Name AS Role "},
                { Constants.USERNAME, ", u.UserName" },
                { Constants.ALTERNATE_USERNAME, ", u.UserName2" },
                { Constants.TOTAL_EMPLOYEES, ", (SELECT COUNT(*) FROM dbo.Supervisor WHERE SupervisorId=u.id) as employeeCount" },
                { Constants.EMAIL, ", u.Email" },
                { Constants.ADDRESS, ", CONCAT(CASE WHEN u.Street1 IS NOT NULL THEN (u.Street1 + ',') ELSE '' END, CASE WHEN u.Street2 IS NOT NULL THEN (u.Street2 + ',') ELSE '' END, CASE WHEN u.City IS NOT NULL THEN (u.city+ ',') ELSE '' END, CASE WHEN u.State IS NOT NULL THEN (u.State+ ',') ELSE '' END, CASE WHEN u.Zip IS NOT NULL THEN (u.Zip+ ',') ELSE '' END) as address " },
                { Constants.PHONE, ", u.Phone" },
                { Constants.SUPERVISOR_NAME, ", (SELECT (ISNULL(NULLIF(usr.LName, '') + ', ', '') + usr.Fname) FROM dbo.[User] usr LEFT JOIN dbo.Supervisor s on s.SupervisorId=usr.Id WHERE s.userId=u.Id) as supervisorName" },
                { Constants.USER_CREATED_DATE, ", u.DateCreated" },
                { Constants.USERID, ", u. Id as userId" },
                { Constants.SUPERVISOR_ID, ",(SELECT supervisorId FROM dbo.Supervisor s WHERE userId=u.Id) as supervisorId" }

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
            {Constants.QR_CODE, "u.BarcodeHash " },
            {Constants.DEPARTMENT, "d.Name " },
            {Constants.SUPERVISOR_ID, "s.SupervisorId " },
            {Constants.ME, "u.Id in (@currentuserId)" },
            {Constants.ME_AND_DIRECT_SUBORDINATES, "u.Id in(select @currentuserId Union select userId from dbo.supervisor where supervisorId=@currentuserId) " },
            {Constants.ME_AND_ALL_SUBORDINATES, " u.Id in (SELECT @currentuserId UNION SELECT * FROM getchildUsers(@currentuserId)) " },
            {Constants.DIRECT_SUBORDINATES, "u.Id in(SELECT userId FROM dbo.supervisor WHERE supervisorId=@currentuserId)  " },
            {Constants.ALL_SUBORDINATES, " u.Id in (SELECT * FROM getchildUsers(@currentuserId))  " },
            {Constants.NOT_ME, "u.Id not in (@currentuserId)" },
            {Constants.NOT_ME_AND_DIRECT_SUBORDINATES, "u.Id not in (select @currentuserId Union select userId from dbo.supervisor where supervisorId=@currentuserId) " },
            {Constants.NOT_ME_AND_ALL_SUBORDINATES, " u.Id not in (SELECT @currentuserId UNION SELECT * FROM getchildUsers(@currentuserId)) " },
            {Constants.NOT_DIRECT_SUBORDINATES, "u.Id not in (SELECT userId FROM dbo.supervisor WHERE supervisorId=@currentuserId)  " },
            {Constants.NOT_ALL_SUBORDINATES, " u.Id not in (SELECT * FROM getchildUsers(@currentuserId))  " }
        };


        /// <summary>
        ///     Dictionary having list of tables that requried to get the columns for employee(s) result
        /// </summary>
        private readonly Dictionary<string, List<string>> tableJoins = new Dictionary<string, List<string>>()
        {
            { " LEFT JOIN UserRole ur on ur.UserId=u.Id LEFT JOIN Role r on r.Id=ur.roleId", new List<string> {Constants.ROLEID, Constants.ROLE} },
            { " LEFT JOIN Supervisor s on s.userId=u.Id", new List<string> {Constants.SUPERVISOR_ID, Constants.REPORTING} },
            { " LEFT JOIN UserDepartment ud on ud.userId=u.Id LEFT JOIN Department d on d.Id=ud.DepartmentId ", new List<string> {Constants.DEPARTMENT} }
        };

        /// <summary>
        /// Generate the employee query based on the query fields
        /// </summary>
        /// <param name="employeeRequest"></param>
        /// <returns>string</returns>
        public string CreateEmployeeQuery(QueryBuilderRequest employeeRequest)
        {
            //Parameters that used to read the data
            string query = string.Empty, tableJoin = string.Empty, selectQuery = string.Empty, whereQuery = string.Empty, currentUserId = string.Empty;
            List<string> fieldList = new List<string>();

            try
            {
                //Select statement for the query
                selectQuery = "SELECT  ";

                //getting column List
                query = string.Join("", (from column in columnList
                                         where employeeRequest.ColumnList.Any(x => x == column.Key)
                                         select column.Value));
                query = query.TrimStart(',');
                query = selectQuery + query;

                //Append the user query with select statement
                query += " FROM dbo.[User] u  LEFT JOIN UserCompany uc on uc.UserId=u.Id ";

                //get table joins
                fieldList = employeeRequest.ColumnList.ToList();
                fieldList.AddRange(employeeRequest.Fields.Select(x => x.Name.ToUpper()).ToArray());
                tableJoin = string.Join("", from joins in tableJoins
                                            where fieldList.Any(x => joins.Value.Any(y => y == x))
                                            select joins.Key);
                query += tableJoin;

                //Handles the smart parameter for the userName
                currentUserId = employeeRequest.Fields.Where(x => x.Name.ToUpper() == Constants.CURRENT_USER).Select(x => x.Value).FirstOrDefault();

                if (employeeRequest.Fields.Where(x => x.Name == Constants.USERNAME).ToList().Count > 0 && employeeRequest.Fields.Where(x => x.Name == Constants.CURRENT_USER).ToList().Count > 0)
                {
                    EmployeeModel userDetails = employeeRequest.Fields.Where(x => x.Name == Constants.CURRENT_USER).FirstOrDefault();
                    employeeRequest.Fields.Remove(userDetails);

                    employeeRequest.Fields.Select(x => x.Name == Constants.USERNAME && UsernameSmartParameters.Contains(x.Value) ? x.Name = x.Value : x.Name).ToList();

                    employeeRequest.Fields.Select(x => (UsernameSmartParameters.Contains(x.Name) && x.Operator == "!=") ? x.Name = "NOT_" + x.Name : x.Name).ToList();
                }

                //getting where conditions
                whereQuery = string.Join("", from employee in employeeRequest.Fields
                                             select (!string.IsNullOrEmpty(employee.Bitwise) ? (" " + employee.Bitwise + " ") : string.Empty) + (!string.IsNullOrEmpty(employeeFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault()) ? (employeeFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault() +
                        OperatorHelper.CheckOperator(employee.Operator, employee.Value.Trim(), employee.Name) + CheckValues(employee.Value, employee.Operator)) : string.Empty));

                //Append the company query
                whereQuery = (!string.IsNullOrEmpty(whereQuery)) ? (" WHERE uc.CompanyId=" + employeeRequest.CompanyId + " and  (" + whereQuery) : string.Empty;

                whereQuery = whereQuery.Replace("@currentuserId", currentUserId);
                //Create the final query that helps to retrieve the data
                query += whereQuery + " )";
                return query;
            }
            catch (Exception createEmployeeQueryException)
            {
                LambdaLogger.Log(createEmployeeQueryException.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        /// Get employee(s) details based on provided companyId [QueryBuilder]
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <returns>EmployeeResponse</returns>
        public EmployeeResponse GetEmployeeDetails(QueryBuilderRequest queryBuilderRequest)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            string query = string.Empty, userId = string.Empty;
            
            EmployeeResponse employeeResponse = new EmployeeResponse();
            Dictionary<string, string> parameterList;
            int companyId = 0;
            try
            {
                //Assign the request details to corresponding objects
                companyId = Convert.ToInt32(queryBuilderRequest.CompanyId);
                queryBuilderRequest = queryBuilderRequest.Payload;           
                queryBuilderRequest.CompanyId = companyId;

                //Read sql parameters from the DB
                userId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.USERID).Select(x => x.Value).FirstOrDefault();


                //Create the query based on the input fields
                query = CreateEmployeeQuery(queryBuilderRequest);

                //SQL Parameter list
                parameterList = new Dictionary<string, string>() { { "userId", Convert.ToString(userId) }, { "companyId", Convert.ToString(companyId) } };

                //Read the response from the DB
                employeeResponse.Employees = ReadEmployeeDetails(query, parameterList);
                

                //Create response based on the employee response
                if (employeeResponse.Employees != null)
                {
                    return employeeResponse;
                }
                else
                {
                    employeeResponse.Error = ResponseBuilder.InternalError();
                    return employeeResponse;
                }
            }
            catch (Exception getEmployeeDetailsException)
            {
                LambdaLogger.Log(getEmployeeDetailsException.ToString());
                employeeResponse.Error = ResponseBuilder.InternalError();
                return employeeResponse;
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        /// Smart parameters for the Username
        /// </summary>
        private readonly List<string> UsernameSmartParameters = new List<string>
        {
            Constants.ME,
            Constants.ME_AND_ALL_SUBORDINATES,
            Constants.ME_AND_DIRECT_SUBORDINATES,
            Constants.ALL_SUBORDINATES,
            Constants.DIRECT_SUBORDINATES
        };


        /// <summary>
        /// Get the list of values which having specific values
        /// </summary>
        private readonly List<string> valueList = new List<string>
        {
            Constants.NULL,
            Constants.YES,
            Constants.NO
        };

        /// <summary>
        /// Handles Yes and No values for some fields
        /// </summary>
        /// <param name="value"></param>
        /// <param name="fieldOperator"></param>
        /// <returns>string</returns>
        public string CheckValues(string value, string fieldOperator)
        {
            string customizedValue = string.Empty;
            try
            {
                if (valueList.Contains(value.ToUpper()))
                {
                    switch (value.ToUpper())
                    {
                        case Constants.YES:
                            customizedValue = " IS NULL";
                            break;
                        case Constants.NO:
                            customizedValue = " IS NOT NULL";
                            break;
                        default:
                            break;
                    }
                }
                return customizedValue;
            }
            catch (Exception checkValuesException)
            {
                LambdaLogger.Log(checkValuesException.ToString());
                return customizedValue;
            }
        }

        /// <summary>
        /// Creating response object after reading Employee(s) details from database
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns>List<EmployeeQueryModel></returns>
        public List<EmployeeQueryModel> ReadEmployeeDetails(string query, Dictionary<string, string> parameters)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();             
            List<EmployeeQueryModel> employeeList = new List<EmployeeQueryModel>();

            try
            {
                //Read the employee details from the DB
                using (SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query, parameters))
                {
                    if (sqlDataReader != null && sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            EmployeeQueryModel employeeResponse = new EmployeeQueryModel
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
