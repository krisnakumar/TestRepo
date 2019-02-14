using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
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
<copyright file="WorkbookRepository.cs">
    Copyright (c) 2018 All Rights Reserved
</copyright>
<author>Shoba Eswar</author>
<date>23-10-2018</date>
<summary> 
    Repository that helps to read workbook(s) details for specific user from database.
</summary>
*/
namespace ReportBuilderAPI.Repository
{
    /// <summary>
    ///     Class that handles to read workbook(s) details for specific user from database
    /// </summary>
    public class WorkbookRepository : IWorkbook
    {
        /// <summary>
        ///      Get assigned workbook(s) details for a user [ReportBuilder]
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetWorkbookDetails(int userId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<WorkbookResponse> workbookList = new List<WorkbookResponse>();
            string query = string.Empty;
            try
            {
                if (userId != 0)
                {
                    query = Workbook.ReadWorkbookDetails(userId);
                    DataSet dataSet = databaseWrapper.ExecuteAdapter(query);
                    foreach (DataRow rows in dataSet.Tables[0].Rows)
                    {
                        WorkbookResponse workbookResponse = new WorkbookResponse
                        {
                            EmployeeName = Convert.ToString(rows["FirstName"]) + " " + Convert.ToString(rows["LastName"]),
                            WorkbookName = Convert.ToString(rows["WorkbookName"]),
                            Role = Convert.ToString(rows["Role"]),
                            CompletedTasks = Convert.ToString(rows["CompletedWorkbooks"]) + "/" + Convert.ToString(rows["totalTasks"]),
                            PercentageCompleted = (int)Math.Round((double)(100 * (Convert.ToInt32(rows["CompletedWorkbooks"]))) / (Convert.ToInt32(rows["TotalTasks"]))),
                            DueDate = Convert.ToDateTime(rows["DueDate"]).ToString("MM/dd/yyyy"),
                            UserId = Convert.ToInt32(rows["UserId"]),
                            WorkBookId = Convert.ToInt32(rows["workbookId"])
                        };
                        workbookList.Add(workbookResponse);
                    }
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(workbookList), 0);
                }
                else
                {
                    return ResponseBuilder.BadRequest(string.Empty);
                }
            }
            catch (Exception getWorkbookDetailsException)
            {
                LambdaLogger.Log(getWorkbookDetailsException.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        ///     Get past due workbook(s) details for a user [ReportBuilder]
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetPastDueWorkbooks(int userId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<WorkbookResponse> workbookList = new List<WorkbookResponse>();
            string query = string.Empty;
            try
            {
                if (userId != 0)
                {
                    query = Workbook.ReadPastDueWorkbookDetails(userId);
                    DataSet dataSet = databaseWrapper.ExecuteAdapter(query);
                    foreach (DataRow rows in dataSet.Tables[0].Rows)
                    {
                        WorkbookResponse workbookResponse = new WorkbookResponse
                        {
                            EmployeeName = Convert.ToString(rows["FirstName"]) + " " + Convert.ToString(rows["LastName"]),
                            WorkbookName = Convert.ToString(rows["WorkbookName"]),
                            Role = Convert.ToString(rows["Role"]),
                            CompletedTasks = Convert.ToString(rows["CompletedWorkbooks"]) + "/" + Convert.ToString(rows["totalTasks"]),
                            PercentageCompleted = (int)Math.Round((double)(100 * (Convert.ToInt32(rows["CompletedWorkbooks"]))) / (Convert.ToInt32(rows["TotalTasks"]))),
                            DueDate = Convert.ToDateTime(rows["DueDate"]).ToString("MM/dd/yyyy"),
                            UserId = Convert.ToInt32(rows["UserId"]),
                            WorkBookId = Convert.ToInt32(rows["workbookId"])
                        };
                        workbookList.Add(workbookResponse);
                    }
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(workbookList), 0);
                }
                else
                {
                    return ResponseBuilder.BadRequest(string.Empty);
                }
            }
            catch (Exception getPastDueWorkbooksException)
            {
                LambdaLogger.Log(getPastDueWorkbooksException.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        ///     Get coming due workbook(s) details for a user [ReportBuilder]
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetInDueWorkbooks(int userId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<WorkbookResponse> workbookList = new List<WorkbookResponse>();
            string query = string.Empty;
            try
            {
                if (userId != 0)
                {
                    query = Workbook.ReadInDueWorkbookDetails(userId);
                    DataSet dataSet = databaseWrapper.ExecuteAdapter(query);
                    foreach (DataRow rows in dataSet.Tables[0].Rows)
                    {
                        WorkbookResponse workbookResponse = new WorkbookResponse
                        {
                            EmployeeName = Convert.ToString(rows["FirstName"]) + " " + Convert.ToString(rows["LastName"]),
                            WorkbookName = Convert.ToString(rows["WorkbookName"]),
                            Role = Convert.ToString(rows["Role"]),
                            CompletedTasks = Convert.ToString(rows["CompletedWorkbooks"]) + "/" + Convert.ToString(rows["totalTasks"]),
                            PercentageCompleted = (int)Math.Round((double)(100 * (Convert.ToInt32(rows["CompletedWorkbooks"]))) / (Convert.ToInt32(rows["TotalTasks"]))),
                            DueDate = Convert.ToDateTime(rows["DueDate"]).ToString("MM/dd/yyyy"),
                            UserId = Convert.ToInt32(rows["UserId"]),
                            WorkBookId = Convert.ToInt32(rows["workbookId"])
                        };
                        workbookList.Add(workbookResponse);
                    }
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(workbookList), 0);
                }
                else
                {
                    return ResponseBuilder.BadRequest(string.Empty);
                }
            }
            catch (Exception getInDueWorkbooksException)
            {
                LambdaLogger.Log(getInDueWorkbooksException.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        ///     Get completed workbook(s) details for a user [ReportBuilder]
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetCompletedWorkbooks(int userId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<WorkbookResponse> workbookList = new List<WorkbookResponse>();
            string query = string.Empty;
            try
            {
                if (userId != 0)
                {
                    query = Workbook.CompletedWorkbookDetails(userId);
                    DataSet dataSet = databaseWrapper.ExecuteAdapter(query);
                    foreach (DataRow rows in dataSet.Tables[0].Rows)
                    {
                        WorkbookResponse workbookResponse = new WorkbookResponse
                        {
                            EmployeeName = Convert.ToString(rows["FirstName"]) + " " + Convert.ToString(rows["LastName"]),
                            WorkbookName = Convert.ToString(rows["WorkbookName"]),
                            Role = Convert.ToString(rows["Role"]),
                            CompletionDate = Convert.ToDateTime(rows["LastAttemptDate"]).ToString("MM/dd/yyyy"),
                            UserId = Convert.ToInt32(rows["UserId"]),
                            WorkBookId = Convert.ToInt32(rows["workbookId"])
                        };
                        workbookList.Add(workbookResponse);
                    }

                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(workbookList), 0);
                }
                else
                {
                    return ResponseBuilder.BadRequest(string.Empty);
                }
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        ///     Dictionary having Column list for the workbook(s) details.
        ///     Based on column name, query is being formed/updated
        /// </summary>
        private readonly Dictionary<string, string> workbookColumnList = new Dictionary<string, string>()
        {
                { Constants.EMPLOYEE_NAME, ", (ISNULL(NULLIF(u.LName, '') + ', ', '') + u.Fname)  as employeeName" },
                { Constants.WORKBOOK_NAME, ",  wb.name as workbookName" },
                { Constants.DESCRIPTION, ", wb.Description"},
                { Constants.WORKBOOK_CREATED, ", wb.datecreated"},
                { Constants.WORKBOOK_ISENABLED, ", wb.isEnabled"},
                { Constants.WORKBOOK_ID, ", wb.Id"},
                { Constants.WORKBOOK_CREATED_BY, ", (Select us.UserName from dbo.[User] us WHERE us.Id=wb.Createdby) as Createdby"},
                { Constants.DAYS_TO_COMPLETE, ", wb.daystocomplete"},
                { Constants.DUE_DATE, ", DATEADD(DAY, wb.DaysToComplete, uwb.DateAdded) AS DueDate"},
                 { Constants.USERNAME, ", u.UserName"},
                { Constants.USERNAME2, ", u.UserName2"},

                { Constants.USER_CREATED_DATE, ", u.DateCreated"},
                { Constants.FIRSTNAME, ", u.FName"},
                { Constants.MIDDLENAME, ", u.MName"},
                { Constants.LASTNAME, ", u.LName"},
                { Constants.EMAIL, ", u.Email"},
                { Constants.CITY, ", u.City"},
                { Constants.STATE, ", u.State"},
                { Constants.ZIP, ", u.Zip"},
                { Constants.PHONE, ", u.phone"},
                { Constants.USERID, ", u.Id as userId"},
                { Constants.ENTITY_COUNT, ", (SELECT COUNT(DISTINCT EntityId) FROM WorkbookProgress WHERE WorkbookId=wb.Id) as entityCount"},
                { Constants.USER_COUNT, ", (SELECT COUNT(DISTINCT UserId) FROM UserWorkbook WHERE WorkbookId=wb.Id) as userCount"},
                { Constants.ASSIGNED_WORKBOOK, ", (SELECT COUNT(DISTINCT uwt.WorkBookId)  FROM dbo.UserWorkBook uwt  LEFT JOIN UserCompany uc on uc.UserId= uwt.UserId  WHERE uc.companyId=@companyId AND uwt.UserId IN ((SELECT u.Id UNION SELECT * FROM getChildUsers (u.Id))) AND uwt.IsEnabled=1) AS AssignedWorkbooks"},

                { Constants.PAST_DUE_WORKBOOK, ", (SELECT ISNULL((SELECT COUNT(DISTINCT uwb.WorkBookId) FROM  WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId     LEFT JOIN UserCompany uc on uc.UserId= uwb.UserId JOIN WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId JOIN dbo.WorkBook wb ON wb.Id=wbt.WorkBookId WHERE uc.companyId=@companyId AND  wbs.UserId IN ((SELECT u.Id UNION SELECT * FROM getChildUsers (u.Id))) AND uwb.IsEnabled=1 AND    ABS(DATEDIFF(DAY,GETDATE(),CONVERT(DATE,DATEADD(DAY, wb.DaysToComplete, uwb.DateAdded),101))) < @duedays AND wb.DaysToComplete <= ABS(DATEDIFF(DAY,  GETDATE(), DateAdded)) AND (SELECT SUM(www.NumberCompleted) FROM WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) < (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbt.WorkBookId)),0))  AS PastDueWorkbooks"},

                { Constants.WORKBOOK_DUE, ", (SELECT ISNULL((SELECT COUNT(DISTINCT uwb.WorkBookId) FROM  WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId     LEFT JOIN UserCompany uc on uc.UserId= uwb.UserId  JOIN WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId JOIN dbo.WorkBook wb ON wb.Id=wbt.WorkBookId WHERE  uc.companyId=@companyId AND   ABS(DATEDIFF(DAY,GETDATE(),CONVERT(DATE,DATEADD(DAY, wb.DaysToComplete, uwb.DateAdded),101))) < @duedays AND  wbs.UserId IN ((SELECT u.Id UNION SELECT * FROM getChildUsers (u.Id))) AND uwb.IsEnabled=1 AND wb.DaysToComplete >= ABS(DATEDIFF(DAY,  GETDATE(), DateAdded)) AND (SELECT SUM(www.NumberCompleted) FROM WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) < (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbt.WorkBookId)),0)) AS InDueWorkbooks"},

                { Constants.INCOMPLETE_WORKBOOK, ", (SELECT ISNULL((SELECT  COUNT(DISTINCT wbt.EntityId) FROM  WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId     LEFT JOIN UserCompany uc on uc.UserId= uwb.UserId JOIN WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId WHERE uc.companyId=@companyId AND  wbs.UserId IN ((SELECT u.Id UNION SELECT * FROM getChildUsers (u.Id))) AND wbs.WorkBookId=wb.id GROUP BY wbt.WorkBookId HAVING (SELECT SUM(www.NumberCompleted) FROM WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) < (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbt.WorkBookId )),0)) AS InCompletedWorkbooks"},

                { Constants.TOTAL_WORKBOOK, ", (SELECT ISNULL((SELECT  COUNT(DISTINCT uwb.WorkBookId) FROM  WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId     LEFT JOIN UserCompany uc on uc.UserId= uwb.UserId JOIN WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId WHERE uc.companyId=@companyId AND  wbs.UserId IN ((SELECT u.Id UNION SELECT * FROM getChildUsers (u.Id))) AND wbs.WorkBookId=wb.id ),0)) AS TotalWorkbooks"},


                { Constants.TOTAL_TASK, ", (SELECT ISNULL((SELECT  COUNT(DISTINCT wbt.EntityId) FROM  WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId     LEFT JOIN UserCompany uc on uc.UserId= uwb.UserId JOIN WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId WHERE uc.companyId=@companyId AND  wbs.UserId IN ((u.Id)) AND wbs.WorkBookId=wb.id ),0)) AS TotalTasks"},

                { Constants.TOTAL_EMPLOYEES, ", (SELECT COUNT(*) FROM dbo.Supervisor ss LEFT JOIN UserCompany uc on uc.UserId=ss.UserId   WHERE ss.SupervisorId = u.Id and uc.companyId=@companyId) AS TotalEmployees"},


                { Constants.COMPLETED_WORKBOOK, ",  (SELECT ISNULL((SELECT  COUNT(DISTINCT uwb.WorkBookId) FROM  WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId  LEFT JOIN UserCompany uc on uc.UserId= uwb.UserId JOIN WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId WHERE uc.companyId=@companyId AND uwb.IsEnabled=1 AND  wbs.UserId IN ((SELECT u.Id UNION SELECT * FROM getChildUsers (u.Id))) AND (SELECT SUM(www.NumberCompleted) FROM WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) >= (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbt.WorkBookId)),0)) AS CompletedWorkbooks"},

                { Constants.COMPLETED_TASK, ",  (SELECT ISNULL((SELECT  COUNT(DISTINCT wbt.EntityId) FROM  WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId  LEFT JOIN UserCompany uc on uc.UserId= uwb.UserId JOIN WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId WHERE uc.companyId=@companyId AND uwb.IsEnabled=1 AND  wbs.UserId IN ((u.Id)) AND (SELECT SUM(www.NumberCompleted) FROM WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) >= (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbt.WorkBookId)),0)) AS CompletedTasks"},

                { Constants.ROLE, ", r.Name AS Role "},

                { Constants.NUMBER_COMPLETED, ", wbp.NumberCompleted"},

                { Constants.LAST_ATTEMPT_DATE, ", (SELECT  MAX(LastAttemptDate) FROM dbo.WorkBookProgress WHERE WorkBookId=wbp.WorkBookId) AS LastAttemptDate"},

                { Constants.FIRST_ATTEMPT_DATE, ", wbp.FirstAttemptDate"},

                { Constants.REPETITIONS, ", wbc.Repetitions"},

                { Constants.WORKBOOK_ASSIGNED_DATE, ", uwb.DateAdded"},

                { Constants.LAST_SIGNOFF_BY, ", (Select us.UserName from dbo.[User] us WHERE us.Id=wbp.LastSignOffBy) as LastSignOffBy"}
        };

        /// <summary>
        ///     Dictionary having fields that requried for the workbook entity.
        ///     Based on fields, query is being formed/updated
        /// </summary>
        private readonly Dictionary<string, string> workbookFields = new Dictionary<string, string>()
        {
            {Constants.WORKBOOK_ID, "wb.ID " },
            {Constants.SUPERVISOR_ID, " u.Id IN (SELECT * FROM getChildUsers (@userId))  " },
            {Constants.NOT_SUPERVISORID, " u.Id NOT IN (SELECT * FROM getChildUsers (@userId))  " },
            {Constants.SUPERVISOR_USER, " u.Id IN (SELECT  @userId UNION SELECT * FROM getChildUsers (@userId))  " },
            {Constants.SUPERVISOR_SUB, " s.supervisorId " },
            {Constants.USERID, " u.Id " },
            {Constants.WORKBOOK_NAME, "wb.Name " },
            {Constants.DESCRIPTION, "wb.Description" },
            {Constants.WORKBOOK_CREATED, "CONVERT(DATE,wb.DateCreated,101)" },
            {Constants.WORKBOOK_ISENABLED, "wb.isenabled" },
            {Constants.DAYS_TO_COMPLETE, "wb.Daystocomplete" },
            {Constants.DUE_DATE, "CONVERT(DATE,DATEADD(DAY, wb.DaysToComplete, uwb.DateAdded),101)" },
            {Constants.DATE_ADDED, "uwb.DateAdded" },
            { Constants.WORKBOOK_CREATED_BY, "(Select us.UserName from dbo.[User] us WHERE us.Id=wb.createdby) " },
            {Constants.ASSIGNED_TO, "uwb.UserId" },
            {Constants.ASSIGNED, " u.Id IN (SELECT * FROM getChildUsers (@userId)) " },

            {Constants.WORKBOOK_IN_DUE, " uwb.IsEnabled=1 AND CONVERT(date,(DATEADD(DAY, wb.DaysToComplete, uwb.DateAdded)))  Between CONVERT(date,GETDATE())  and CONVERT(date,DATEADD(DAY,CONVERT(INT, @duedays), GETDATE())) AND (SELECT SUM(www.NumberCompleted) FROM WorkBookProgress www WHERE www.WorkBookId=wbc.WorkBookId) < (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbc.WorkBookId)" },

            {Constants.PAST_DUE, " uwb.IsEnabled=1 AND CONVERT(date,(DATEADD(DAY, wb.DaysToComplete, uwb.DateAdded)))  Between CONVERT(date,DATEADD(DAY, (CONVERT(INT, @duedays) * -1), GETDATE())) and CONVERT(date,GETDATE())  and  (SELECT SUM(www.NumberCompleted) FROM WorkBookProgress www WHERE www.WorkBookId=wbc.WorkBookId) < (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbc.WorkBookId)" },

            {Constants.COMPLETED, " uwb.IsEnabled=1 AND (SELECT SUM(www.NumberCompleted) FROM WorkBookProgress www WHERE www.WorkBookId=wbc.WorkBookId) >= (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbc.WorkBookId) " }
        };


        /// <summary>
        ///     Dictionary having list of tables that requried to get the columns for workbook(s) result
        /// </summary>
        private readonly Dictionary<string, List<string>> tableJoins = new Dictionary<string, List<string>>()
        {

            { " FULL OUTER JOIN dbo.[user] u on u.Id=uc.UserId ", new List<string> { Constants.USERNAME, Constants.USERNAME2, Constants.USER_CREATED_DATE, Constants.FIRSTNAME, Constants.MIDDLENAME, Constants.LASTNAME, Constants.EMAIL, Constants.CITY, Constants.STATE, Constants.ZIP, Constants.PHONE, Constants.ASSIGNED_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.ASSIGNED_TO, Constants.ROLEID, Constants.ROLE, Constants.USERID, Constants.EMPLOYEE_NAME, Constants.SUPERVISOR_ID, Constants.CREATED_BY } },

            { " FULL OUTER JOIN Supervisor s ON s.UserId=u.Id ", new List<string> {Constants.SUPERVISOR_ID} },

            { " FULL OUTER JOIN WorkbookProgress wbp on wbp.WorkbookId=wb.Id ", new List<string> {Constants.NUMBER_COMPLETED, Constants.LAST_ATTEMPT_DATE, Constants.FIRST_ATTEMPT_DATE, Constants.LAST_SIGNOFF_BY, Constants.DATE_ADDED} },

            { " FULL OUTER JOIN WorkbookContent wbc on wbc.WorkbookId=wb.Id ", new List<string> {Constants.REPETITIONS, Constants.USERNAME, Constants.USERNAME2, Constants.USER_CREATED_DATE, Constants.FIRSTNAME, Constants.MIDDLENAME, Constants.LASTNAME, Constants.EMAIL, Constants.CITY, Constants.STATE, Constants.ZIP, Constants.PHONE, Constants.ASSIGNED_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.ASSIGNED_TO, Constants.ROLEID, Constants.ROLE, Constants.USERID, Constants.EMPLOYEE_NAME, Constants.SUPERVISOR_ID } },

            { "FULL OUTER JOIN UserRole ur on ur.UserId=u.Id LEFT JOIN Role r on r.Id=ur.roleId " , new List<string> {Constants.ROLEID, Constants.ROLE} },

        };

        /// <summary>
        /// Create SQL Query for the workbook based on the request
        /// </summary>
        /// <param name="queryRequest"></param>
        /// <param name="companyId"></param>
        /// <returns>string</returns>
        public string CreateWorkbookQuery(QueryBuilderRequest queryRequest, int companyId)
        {
            string query = string.Empty, tableJoin = string.Empty, selectQuery = string.Empty, whereQuery = string.Empty, companyQuery = string.Empty, supervisorId = string.Empty;
            EmployeeRepository employeeRepository = new EmployeeRepository();
            List<string> fieldList = new List<string>();
            try
            {
                selectQuery = "SELECT  DISTINCT ";

                //getting column List
                query = string.Join("", (from column in workbookColumnList
                                         where queryRequest.ColumnList.Any(x => x == column.Key)
                                         select column.Value));
                query = query.TrimStart(',');
                query = selectQuery + query;
                query += "  FROM Workbook wb FULL OUTER JOIN UserWorkBook uwb ON uwb.workbookId=wb.Id  FULL OUTER JOIN  UserCompany uc ON uc.UserId=uwb.UserId";

                //get table joins
                fieldList = queryRequest.ColumnList.ToList();

                fieldList.AddRange(queryRequest.Fields.Select(x => x.Name.ToUpper()).ToArray());

                tableJoin = string.Join("", from joins in tableJoins
                                            where fieldList.Any(x => joins.Value.Any(y => y == x))
                                            select joins.Key);

                query += tableJoin;

                //Read the supervisorId based on the request
                supervisorId = Convert.ToString(queryRequest.Fields.Where(x => x.Name.ToUpper() == Constants.SUPERVISOR_ID).Select(x => x.Value).FirstOrDefault());
            
                if (queryRequest.ColumnList.Contains(Constants.TOTAL_EMPLOYEES) && !string.IsNullOrEmpty(supervisorId))
                {
                    queryRequest.Fields.Select(x => x.Name == Constants.SUPERVISOR_ID ? x.Name = Constants.SUPERVISOR_SUB : x.Name).ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(supervisorId))
                    {
                        queryRequest.Fields.Select(x => x.Name == Constants.SUPERVISOR_ID && x.Operator == "!=" ? x.Name = Constants.NOT_SUPERVISORID : x.Name).ToList();
                    }
                }


                if (queryRequest.Fields.Where(x => x.Name == Constants.SUPERVISOR_ID).ToList().Count > 0 && queryRequest.Fields.Where(x => x.Name == Constants.USERID).ToList().Count > 0)
                {
                    ReportBuilder.Models.Models.EmployeeModel userDetails = queryRequest.Fields.Where(x => x.Name == Constants.USERID).FirstOrDefault();
                    queryRequest.Fields.Remove(userDetails);
                    queryRequest.Fields.Select(x => x.Name == Constants.SUPERVISOR_ID ? x.Name = Constants.SUPERVISOR_USER : x.Name).ToList();
                }


                //getting where conditions
                whereQuery = string.Join("", from employee in queryRequest.Fields
                                             select (!string.IsNullOrEmpty(employee.Bitwise) ? (" " + employee.Bitwise + " ") : string.Empty) + (!string.IsNullOrEmpty(workbookFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault()) ? (workbookFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault() + employeeRepository.CheckOperator(employee.Operator, employee.Value.Trim(), employee.Name)) : string.Empty));


                companyQuery = (" WHERE  uc.CompanyId=" + companyId);

                if (queryRequest.ColumnList.Contains(Constants.WORKBOOK_NAME))
                {
                    whereQuery += "AND uwb.isEnabled=1";
                }
                query += (!string.IsNullOrEmpty(whereQuery)) ? (companyQuery + " and (" + whereQuery) + ")" : string.Empty;
                return query;
            }
            catch (Exception createWorkbookQueryException)
            {
                LambdaLogger.Log(createWorkbookQueryException.ToString());
                return string.Empty;
            }
        }

        /// <summary>
        ///     Get list of workbook(s) based on input field and column(s) for specific company [QueryBuilder]
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="companyId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetWorkbookDetails(string requestBody, int companyId)
        {
            string query = string.Empty, supervisorId = string.Empty, dueDays = string.Empty;
            List<WorkbookResponse> workbookDetails;  
            Dictionary<string, string> parameterList;
            try
            {

                QueryBuilderRequest queryRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(requestBody);
                supervisorId = Convert.ToString(queryRequest.Fields.Where(x => x.Name.ToUpper() == Constants.SUPERVISOR_ID).Select(x => x.Value).FirstOrDefault());
                dueDays = Convert.ToString(queryRequest.Fields.Where(x => x.Name.ToUpper() == (Constants.WORKBOOK_IN_DUE) || x.Name.ToUpper() == (Constants.PAST_DUE)).Select(x => x.Value).FirstOrDefault());

                query = CreateWorkbookQuery(queryRequest, companyId);
                //Read the SQL Parameters value
            

                if (string.IsNullOrEmpty(dueDays))
                {
                    dueDays = "30";
                }

                //Create the dictionary to pass the parameter value
                parameterList = new Dictionary<string, string>() { { "userId", Convert.ToString(supervisorId) }, { "companyId", Convert.ToString(companyId) }, { "duedays", Convert.ToString(dueDays) } };

                workbookDetails = ReadWorkBookDetails(query, parameterList);

                if (workbookDetails != null)
                {
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(workbookDetails), 0);
                }
                else
                {
                    return ResponseBuilder.InternalError();
                }
            }
            catch (Exception getEmployeeDetails)
            {
                LambdaLogger.Log(getEmployeeDetails.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     Creating response object after reading workbook(s) details from database
        /// </summary>
        /// <param name="query"></param>
        /// <returns>WorkbookResponse</returns>
        public List<WorkbookResponse> ReadWorkBookDetails(string query, Dictionary<string, string> parameters)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<WorkbookResponse> workbookList = new List<WorkbookResponse>();
            try
            {
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query, parameters);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        WorkbookResponse workbookResponse = new WorkbookResponse
                        {
                            EmployeeName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'employeeName'").Count() == 1) ? Convert.ToString(sqlDataReader["employeeName"]) : null,
                            WorkBookName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'workbookName'").Count() == 1) ? Convert.ToString(sqlDataReader["workbookName"]) : null,
                            Description = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Description'").Count() == 1) ? Convert.ToString(sqlDataReader["Description"]) : null,
                            WorkbookCreated = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'datecreated'").Count() == 1) ? Convert.ToString(sqlDataReader["datecreated"]) : null,
                            WorkbookEnabled = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'isEnabled'").Count() == 1) ? (bool?)(sqlDataReader["isEnabled"]) : null,
                            WorkBookId = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Id'").Count() == 1) ? (int?)(sqlDataReader["Id"]) : null,
                            LastSignoffBy = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'LastSignOffBy'").Count() == 1) ? Convert.ToString((sqlDataReader["LastSignOffBy"])) : null,

                            WorkbookAssignedDate = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'DateAdded'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(sqlDataReader["DateAdded"])) ? Convert.ToDateTime(sqlDataReader["DateAdded"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,

                            Repetitions = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Repetitions'").Count() == 1) ? Convert.ToString((sqlDataReader["Repetitions"])) : null,

                            FirstAttemptDate = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'FirstAttemptDate'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(sqlDataReader["FirstAttemptDate"])) ? Convert.ToDateTime(sqlDataReader["FirstAttemptDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,

                            LastAttemptDate = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'LastAttemptDate'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(sqlDataReader["LastAttemptDate"])) ? Convert.ToDateTime(sqlDataReader["LastAttemptDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,

                            NumberCompleted = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'NumberCompleted'").Count() == 1) ? (int?)(sqlDataReader["NumberCompleted"]) : null,
                            Role = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Role'").Count() == 1) ? Convert.ToString((sqlDataReader["Role"])) : null,
                            CompletedWorkbook = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'CompletedWorkbooks'").Count() == 1) ? (int?)(sqlDataReader["CompletedWorkbooks"]) : null,
                            TotalTasks = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'TotalTasks'").Count() == 1) ? (int?)(sqlDataReader["TotalTasks"]) : null,
                            TotalWorkbook = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'TotalWorkbooks'").Count() == 1) ? (int?)(sqlDataReader["TotalWorkbooks"]) : null,
                            PastDueWorkBook = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'PastDueWorkbooks'").Count() == 1) ? (int?)(sqlDataReader["PastDueWorkbooks"]) : null,
                            InDueWorkBook = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'InDueWorkbooks'").Count() == 1) ? (int?)(sqlDataReader["InDueWorkbooks"]) : null,
                            AssignedWorkBook = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'AssignedWorkbooks'").Count() == 1) ? (int?)(sqlDataReader["AssignedWorkbooks"]) : null,
                            UserCount = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'userCount'").Count() == 1) ? (int?)(sqlDataReader["userCount"]) : null,
                            EntityCount = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'entityCount'").Count() == 1) ? (int?)(sqlDataReader["entityCount"]) : null,
                            UserName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'UserName'").Count() == 1) ? Convert.ToString(sqlDataReader["UserName"]) : null,
                            DaysToComplete = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'daystocomplete'").Count() == 1) ? Convert.ToString(sqlDataReader["daystocomplete"]) : null,
                            AlternateName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'UserName2'").Count() == 1) ? Convert.ToString(sqlDataReader["UserName2"]) : null,
                            Email = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'email'").Count() == 1) ? Convert.ToString(sqlDataReader["email"]) : null,
                            CreatedBy = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'CreatedBy'").Count() == 1) ? Convert.ToString(sqlDataReader["CreatedBy"]) : null,
                            Address = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Address'").Count() == 1) ? Convert.ToString(sqlDataReader["Address"]) : null,
                            Phone = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Phone'").Count() == 1) ? Convert.ToString(sqlDataReader["Phone"]) : null,
                            TotalEmployees = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'TotalEmployees'").Count() == 1) ? Convert.ToString(sqlDataReader["TotalEmployees"]) : null,
                            InCompleteWorkbook = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'InCompletedWorkbooks'").Count() == 1) ? (int?)(sqlDataReader["InCompletedWorkbooks"]) : null,
                            UserId = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'UserId'").Count() == 1) ? (int?)(sqlDataReader["UserId"]) : null,

                            DueDate = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'DueDate'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(sqlDataReader["DueDate"])) ? Convert.ToDateTime(sqlDataReader["DueDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,

                            CompletedTasks = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'CompletedTasks'").Count() == 1) ? Convert.ToString((sqlDataReader["CompletedTasks"])) : null



                        };
                        // Adding each workbook details in array list
                        workbookList.Add(workbookResponse);
                    }
                }
                return workbookList;
            }
            catch (Exception readWorkbookDetailsException)
            {
                LambdaLogger.Log(readWorkbookDetailsException.ToString());
                return null;
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }
    }
}
