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
using System.Data.SqlClient;
using System.Linq;
using System.Net;



// <copyright file="WorkbookRepository.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Shoba Eswar</author>
// <date>23-10-2018</date>
// <summary>Repository that helps to read the Workbook data from the Table</summary>
namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Class that helps to read the Workbook
    /// </summary>
    public class WorkbookRepository : IWorkbook
    {
        /// <summary>
        /// APIGateway Response
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
                query = Workbook.ReadWorkbookDetails(userId);
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        WorkbookResponse workbookResponse = new WorkbookResponse
                        {
                            EmployeeName = Convert.ToString(sqlDataReader["FirstName"]) + " " + Convert.ToString(sqlDataReader["LastName"]),
                            WorkbookName = Convert.ToString(sqlDataReader["WorkbookName"]),
                            Role = Convert.ToString(sqlDataReader["Role"]),
                            CompletedTasks = Convert.ToString(sqlDataReader["CompletedWorkbooks"]) + "/" + Convert.ToString(sqlDataReader["totalTasks"]),
                            PercentageCompleted = (int)Math.Round((double)(100 * (Convert.ToInt32(sqlDataReader["CompletedWorkbooks"]))) / (Convert.ToInt32(sqlDataReader["TotalTasks"]))),
                            DueDate = Convert.ToDateTime(sqlDataReader["DueDate"]).ToString("MM/dd/yyyy"),
                            UserId = Convert.ToInt32(sqlDataReader["UserId"]),
                            WorkBookId = Convert.ToInt32(sqlDataReader["workbookId"])
                        };
                        workbookList.Add(workbookResponse);
                    }
                }
                return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(workbookList), 0);
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
        /// Get Past due workbook based on the userId
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
                query = Workbook.ReadPastDueWorkbookDetails(userId);
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        WorkbookResponse workbookResponse = new WorkbookResponse
                        {
                            EmployeeName = Convert.ToString(sqlDataReader["FirstName"]) + " " + Convert.ToString(sqlDataReader["LastName"]),
                            WorkbookName = Convert.ToString(sqlDataReader["WorkbookName"]),
                            Role = Convert.ToString(sqlDataReader["Role"]),
                            CompletedTasks = Convert.ToString(sqlDataReader["CompletedWorkbooks"]) + "/" + Convert.ToString(sqlDataReader["totalTasks"]),
                            PercentageCompleted = (int)Math.Round((double)(100 * (Convert.ToInt32(sqlDataReader["CompletedWorkbooks"]))) / (Convert.ToInt32(sqlDataReader["TotalTasks"]))),
                            DueDate = Convert.ToDateTime(sqlDataReader["DueDate"]).ToString("MM/dd/yyyy"),
                            UserId = Convert.ToInt32(sqlDataReader["UserId"]),
                            WorkBookId = Convert.ToInt32(sqlDataReader["workbookId"])
                        };
                        workbookList.Add(workbookResponse);
                    }
                }
                return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(workbookList), 0);
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
        /// Get in due workbook Details based on the userId
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
                query = Workbook.ReadInDueWorkbookDetails(userId);
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        WorkbookResponse workbookResponse = new WorkbookResponse
                        {
                            EmployeeName = Convert.ToString(sqlDataReader["FirstName"]) + " " + Convert.ToString(sqlDataReader["LastName"]),
                            WorkbookName = Convert.ToString(sqlDataReader["WorkbookName"]),
                            Role = Convert.ToString(sqlDataReader["Role"]),
                            CompletedTasks = Convert.ToString(sqlDataReader["CompletedWorkbooks"]) + "/" + Convert.ToString(sqlDataReader["totalTasks"]),
                            PercentageCompleted = (int)Math.Round((double)(100 * (Convert.ToInt32(sqlDataReader["CompletedWorkbooks"]))) / (Convert.ToInt32(sqlDataReader["TotalTasks"]))),
                            DueDate = Convert.ToDateTime(sqlDataReader["DueDate"]).ToString("MM/dd/yyyy"),
                            UserId = Convert.ToInt32(sqlDataReader["UserId"]),
                            WorkBookId = Convert.ToInt32(sqlDataReader["workbookId"])

                        };
                        workbookList.Add(workbookResponse);
                    }
                }
                return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(workbookList), 0);
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
        /// Get in due workbook Details based on the userId
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
                query = Workbook.CompletedWorkbookDetails(userId);
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        WorkbookResponse workbookResponse = new WorkbookResponse
                        {
                            EmployeeName = Convert.ToString(sqlDataReader["FirstName"]) + " " + Convert.ToString(sqlDataReader["LastName"]),
                            WorkbookName = Convert.ToString(sqlDataReader["WorkbookName"]),
                            Role = Convert.ToString(sqlDataReader["Role"]),
                            CompletionDate = Convert.ToDateTime(sqlDataReader["LastAttemptDate"]).ToString("MM/dd/yyyy"),
                            UserId = Convert.ToInt32(sqlDataReader["UserId"]),
                            WorkBookId = Convert.ToInt32(sqlDataReader["workbookId"])
                        };
                        workbookList.Add(workbookResponse);
                    }
                }
                return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(workbookList), 0);
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
        /// 
        /// </summary>
        private readonly Dictionary<string, string> workbookColumnList = new Dictionary<string, string>()
        {
                { Constants.WORKBOOK_NAME, ",  w.name" },
                { Constants.DESCRIPTION, ", w.Description"},
                { Constants.WORKBOOK_CREATED, ", w.datecreated"},
                { Constants.WORKBOOK_ISENABLED, ", w.isEnabled"},
                { Constants.WORKBOOK_ID, ", w.Id"},
                { Constants.WORKBOOK_CREATED_BY, ", w.Createdby"},
                { Constants.DAYS_TO_COMPLETE, ", w.daystocomplete"},
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
                { Constants.ENTITY_COUNT, ", (SELECT COUNT(DISTINCT EntityId) FROM WorkbookProgress WHERE WorkbookId=wb.Id) as entityCount"},
                { Constants.USER_COUNT, ", (SELECT COUNT(DISTINCT UserId) FROM UserWorkbook WHERE WorkbookId=wb.Id) as userCount"},
                { Constants.ASSIGNED_WORKBOOK, ", (SELECT COUNT(DISTINCT uwt.WorkBookId)  FROM dbo.UserWorkBook uwt WHERE uwt.UserId IN ((SELECT u.Id UNION SELECT * FROM getChildUsers (u.Id))) AND uwt.IsEnabled=1) AS AssignedWorkbooks"},
                { Constants.PAST_DUE_WORKBOOK, ", (SELECT ISNULL((SELECT COUNT(DISTINCT uwb.WorkBookId) FROM  WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId JOIN WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId JOIN dbo.WorkBook wb ON wb.Id=wbt.WorkBookId WHERE wbs.UserId IN ((SELECT u.Id UNION SELECT * FROM getChildUsers (u.Id))) AND uwb.IsEnabled=1 AND wb.DaysToComplete <= DATEDIFF(DAY, DateAdded, GETDATE()) AND (SELECT SUM(www.NumberCompleted) FROM WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) < (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre  WHERE tre.WorkBookId=wbt.WorkBookId)),0)) AS PastDueWorkbooks"},
                { Constants.INCOMPLETE_WORKBOOK, ""},
                { Constants.TOTAL_WORKBOOK, ", (SELECT ISNULL((SELECT  COUNT(DISTINCT wbt.EntityId) FROM  WorkBookProgress wbs JOIN WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId WHERE wbs.UserId IN ((SELECT u.Id UNION SELECT * FROM getChildUsers (u.Id))) AND wbs.WorkBookId=wb.id ),0)) AS TotalTasks"},

                { Constants.COMPLETED_WORKBOOK, ", (SELECT ISNULL((SELECT  COUNT(DISTINCT wbt.EntityId) FROM  WorkBookProgress wbs JOIN WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId WHERE wbs.UserId IN ((SELECT u.Id UNION SELECT * FROM getChildUsers (u.Id))) AND wbs.WorkBookId=wb.id GROUP BY wbt.WorkBookId HAVING (SELECT SUM(www.NumberCompleted) FROM WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) = (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbt.WorkBookId )),0)) AS CompletedWorkbooks"},

                { Constants.ROLE, ", r.Name AS Role "},

                { Constants.NUMBER_COMPLETED, ", wbp.NumberCompleted"},

                { Constants.LAST_ATTEMPT_DATE, ", wbp.LastAttemptDate"},

                { Constants.FIRST_ATTEMPT_DATE, ", wbp.FirstAttemptDate"},

                { Constants.REPETITIONS, ", wbc.Repetitions"},

                { Constants.WORKBOOK_ASSIGNED_DATE, ", uwb.DateAdded"},

                { Constants.LAST_SIGNOFF_BY, ", wbp.LastSignOffBy"}
        };

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<string, string> workbookFields = new Dictionary<string, string>()
        {
            {Constants.WORKBOOK_NAME, "wb.Name" },
            {Constants.DESCRIPTION, "wb.Description" },
            {Constants.WORKBOOK_CREATED, "wb.DateCreated" },
            {Constants.WORKBOOK_ISENABLED, "wb.isenabled" },
            {Constants.WORKBOOK_CREATED_BY, "wb.createdby" },
            {Constants.DAYS_TO_COMPLETE, "wb.Daystocomplete" },
            {Constants.ASSIGNED_TO, "u.FName" },
            {Constants.DUE_DATE, "wb.Id" },
        };


        private readonly Dictionary<string, List<string>> tableJoins = new Dictionary<string, List<string>>()
        {
            { "LEFT JOIN UserWorkbook uwb on uwb.UserId=u.Id", new List<string> {Constants.ROLEID, Constants.ROLE} },
            { "LEFT JOIN dbo.[user] u on u.Id=wb.Id", new List<string> {Constants.ROLEID, Constants.ROLE} },
            { "LEFT JOIN WorkbookProgress wbp on wbp.WorkbookId=wb.Id", new List<string> {Constants.ROLEID, Constants.ROLE} },
            { "LEFT JOIN WorkbookContent wbc on wbc.WorkbookId=wb.Id", new List<string> {Constants.ROLEID, Constants.ROLE} },
            { "LEFT JOIN UserRole ur on ur.UserId=u.Id LEFT JOIN Role r on r.Id=ur.roleId" , new List<string> {Constants.ROLEID, Constants.ROLE} },
        };


        /// <summary>
        /// Get employee details based on the input
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse GetWorkbookDetails(string requestBody, int companyId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            string query = string.Empty, tableJoin = string.Empty, selectQuery = string.Empty, whereQuery = string.Empty;
            List<WorkbookResponse> workbookDetails;
            List<string> fieldList = new List<string>();
            try
            {
                selectQuery = "SELECT  ";
                QueryBuilderRequest queryRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(requestBody);

                //getting column List
                query = string.Join("", (from column in workbookColumnList
                                         where queryRequest.ColumnList.Any(x => x == column.Key)
                                         select column.Value));
                query = query.TrimStart(',');
                query = selectQuery + query;
                query += "  FROM Workbook wb  ";

                //get table joins
                fieldList = queryRequest.ColumnList.ToList();

                fieldList.AddRange(queryRequest.Fields.Select(x => x.Name.ToUpper()).ToArray());

                tableJoin = string.Join("", from joins in tableJoins
                                            where fieldList.Any(x => joins.Value.Any(y => y == x))
                                            select joins.Key);

                query += tableJoin;


                //getting where conditions
                whereQuery = string.Join("",   from emplyoee in queryRequest.Fields
                                               select (!string.IsNullOrEmpty(emplyoee.Bitwise) ? (" " + emplyoee.Bitwise + " ") : string.Empty) + workbookFields.Where(x => x.Key == emplyoee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault() + emplyoee.Operator + ("'" + emplyoee.Value + "'"));
                whereQuery = (!string.IsNullOrEmpty(whereQuery)) ? (" WHERE " + whereQuery) : string.Empty;

                query += whereQuery;

                workbookDetails = ReadWorkBookDetails(query);
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
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        /// Read workbook details from sql 
        /// </summary>
        /// <returns>WorkbookResponse</returns>
        public List<WorkbookResponse> ReadWorkBookDetails(string query)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<WorkbookResponse> workbookList = new List<WorkbookResponse>();
            try
            {
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        WorkbookResponse workbookResponse = new WorkbookResponse
                        {


                        };
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
