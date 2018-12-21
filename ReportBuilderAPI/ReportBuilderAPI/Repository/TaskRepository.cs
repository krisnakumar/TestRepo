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


/* 
<copyright file="TaskRepository.cs">
    Copyright (c) 2018 All Rights Reserved
</copyright>
<author>Shoba Eswar</author>
<date>02-11-2018</date>
<summary> 
    Repository that helps to read task(s) details for specific user and workbook from database.
</summary>
*/
namespace ReportBuilderAPI.Repository
{
    /// <summary>
    ///     Class that handles to read task(s) details for specific user and workbook from database
    /// </summary>
    public class TaskRepository : ITask
    {

        /// <summary>
        ///     Get task(s) details under a workbook for a user [ReportBuilder]
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="workbookId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetTaskDetails(int userId, int workbookId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<TaskResponse> taskList = new List<TaskResponse>();
            string query = string.Empty;
            try
            {
                if (userId != 0 && workbookId != 0)
                {
                    query = Task.GetTaskList(userId, workbookId);
                    SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                    if (sqlDataReader != null && sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            TaskResponse employeeResponse = new TaskResponse
                            {
                                TaskId = Convert.ToInt32(sqlDataReader["TaskId"]),
                                UserId = userId,
                                WorkbookId = workbookId,
                                TaskCode = Convert.ToString(sqlDataReader["TaskCode"]),
                                TaskName = Convert.ToString(sqlDataReader["TaskName"]),
                                CompletedTasksCount = Convert.ToInt32(sqlDataReader["CompletedWorkbooks"]),
                                IncompletedTasksCount = Convert.ToInt32(sqlDataReader["InCompletedWorkbooks"]),
                                TotalTasks = Convert.ToInt32(sqlDataReader["TotalTasks"]),
                                CompletionPrecentage = (int)Math.Round((double)(100 * (Convert.ToInt32(sqlDataReader["CompletedWorkbooks"]))) / (Convert.ToInt32(sqlDataReader["TotalTasks"]))),
                            };
                            taskList.Add(employeeResponse);
                        }
                    }
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(taskList), 0);
                }
                else
                {
                    return ResponseBuilder.BadRequest("Request parameters");
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
        ///     Get list of attempt(s) made over a task under a workbook for specific user [ReportBuilder]
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="workbookId"></param>
        /// <param name="taskId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetTaskAttemptsDetails(int userId, int workbookId, int taskId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<AttemptsResponse> attemptsList = new List<AttemptsResponse>();
            string query = string.Empty;
            try
            {
                if (userId != 0 && workbookId != 0 && taskId != 0)
                {
                    query = Task.GetTaskAttemptsDetails(userId, workbookId, taskId);
                    SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                    if (sqlDataReader != null && sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            TaskModel taskModel = JsonConvert.DeserializeObject<TaskModel>(Convert.ToString(sqlDataReader["Comments"]));
                            AttemptsResponse employeeResponse = new AttemptsResponse
                            {
                                Attempt = Convert.ToString(sqlDataReader["Attempt"]),
                                Status = Convert.ToString(sqlDataReader["Status"]),
                                DateTime = Convert.ToDateTime(sqlDataReader["DateTaken"]).ToString("MM/dd/yyyy"),
                                Location = Convert.ToString(sqlDataReader["Street1"]) + "," + Convert.ToString(sqlDataReader["Street2"]) + "," + Convert.ToString(sqlDataReader["City"]) + "," + Convert.ToString(sqlDataReader["State"]) + "," + Convert.ToString(sqlDataReader["Zip"]),
                                Evaluator = Convert.ToString(sqlDataReader["EvaluatorName"]),
                                Comments = taskModel.Comment
                            };
                            attemptsList.Add(employeeResponse);
                        }
                    }
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(attemptsList), 0);
                }
                else
                {
                    return ResponseBuilder.BadRequest("Request parameters");
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
        ///     Get list of task(s) based on input field and column(s) for specific company [QueryBuilder]
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="companyId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetQueryTaskDetails(string requestBody, int companyId)
        {
            string query = string.Empty, tableJoin = string.Empty, selectQuery = string.Empty, whereQuery = string.Empty;
            List<TaskResponse> workbookDetails;
            List<string> fieldList = new List<string>();
            EmployeeRepository employeeRepository = new EmployeeRepository();
            try
            {
                selectQuery = "SELECT  ";
                QueryBuilderRequest queryRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(requestBody);

                //getting column List
                query = string.Join("", (from column in taskColumnList
                                         where queryRequest.ColumnList.Any(x => x == column.Key)
                                         select column.Value));
                query = query.TrimStart(',');
                query = selectQuery + query;
                query += "  FROM Task t LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=t.Id ";

                //get table joins
                fieldList = queryRequest.ColumnList.ToList();

                fieldList.AddRange(queryRequest.Fields.Select(x => x.Name.ToUpper()).ToArray());

                tableJoin = string.Join("", from joins in tableJoins
                                            where fieldList.Any(x => joins.Value.Any(y => y == x))
                                            select joins.Key);

                query += tableJoin;

                //getting where conditions
                whereQuery = string.Join("", from employee in queryRequest.Fields
                                             select (!string.IsNullOrEmpty(employee.Bitwise) ? (" " + employee.Bitwise + " ") : string.Empty) + (!string.IsNullOrEmpty(taskFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault()) ? (taskFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault() + employeeRepository.CheckOperator(employee.Operator, employee.Value.Trim())) : string.Empty));
                whereQuery = (!string.IsNullOrEmpty(whereQuery)) ? (" WHERE tv.CompanyId=" + companyId + " and  (" + whereQuery) : string.Empty;
                query += whereQuery + " )";
                workbookDetails = ReadTaskDetails(query);
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
        ///     Creating response object after reading task(s) details from database
        /// </summary>
        /// <param name="query"></param>
        /// <returns>TaskResponse</returns>
        private List<TaskResponse> ReadTaskDetails(string query)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            TaskResponse taskResponse;
            List<TaskResponse> taskList = new List<TaskResponse>();
            try
            {
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        taskResponse = new TaskResponse
                        {
                            TaskId = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'taskId'").Count() == 1) ? (int?)sqlDataReader["taskId"] : null,
                            TaskName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'taskName'").Count() == 1) ? Convert.ToString(sqlDataReader["taskName"]) : null,
                            AssignedTo = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'assignee'").Count() == 1) ? Convert.ToString(sqlDataReader["assignee"]) : null,
                            EvaluatorName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'evaluatorName'").Count() == 1) ? Convert.ToString(sqlDataReader["evaluatorName"]) : null,
                            Status = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'status'").Count() == 1) ? Convert.ToString(sqlDataReader["status"]) : null,
                            ExpirationDate = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'DateExpired'").Count() == 1) ? Convert.ToString(sqlDataReader["DateExpired"]) : null
                        };
                        // Adding each task details in array list
                        taskList.Add(taskResponse);
                    }
                }
                return taskList;
            }
            catch (Exception readEmployeeDetailsException)
            {
                LambdaLogger.Log(readEmployeeDetailsException.ToString());
                return taskList;
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }

        /// <summary>
        ///     Dictionary having Column list for the task(s) details.
        ///     Based on column name, query is being formed/updated
        /// </summary>
        private readonly Dictionary<string, string> taskColumnList = new Dictionary<string, string>()
        {
                { Constants.TASK_NAME, ",  t.Name as taskName" },
                { Constants.TASK_ID, ", t.Id as taskId"},
                { Constants.STATUS, ", ss.desc"},
                { Constants.DATE_EXPIRED, ", sa.DateExpired"},
                { Constants.EVALUATOR_NAME, ", (SELECT (ISNULL(NULLIF(u.LName, '') + ', ', '') + u.Fname) as evaluatorName FROM dbo.[User] usr WHERE usr.Id=sa.Evaluator) as evaluatorName"},
                { Constants.ASSIGNED_TO, ",  (SELECT (ISNULL(NULLIF(u.LName, '') + ', ', '') + u.Fname) FROM dbo.[User] usr WHERE usr.Id=sa.UserId) as assignee"},
                { Constants.IP, ", sam.IP"},
                { Constants.LOCATION, ", w.datecreated"},
                { Constants.DURATION, ", sam.duration"},
                { Constants.SCORE, ", sam.score"},
                { Constants.DATE_TAKEN, ", wb.dateTaken"},
                { Constants.COMPLETION_DATE, ", u.DateCreated"},
                { Constants.LAST_ATTEMPT_DATE, ", u.FName"},
                { Constants.CREATED_BY, ", u.MName"},
                { Constants.DELETED_BY, ", u.LName"},
                { Constants.PARENT_TASK_NAME, ", u.Email"},
                { Constants.CHILD_TASK_NAME, ", u.City"},
                { Constants.NUMBER_OF_ATTEMPTS, ", sa.Attempt"},
                { Constants.EXPIRATION_DATE, ", sa.DateExpired"}
        };
        
        /// <summary>
        ///     Dictionary having fields that requried for the task entity.
        ///     Based on fields, query is being formed/updated
        /// </summary>
        private readonly Dictionary<string, string> taskFields = new Dictionary<string, string>()
        {
            {Constants.TASK_NAME, "t.Name " },
            {Constants.TASK_ID, "t.Id " },
            {Constants.TASK_CREATED, "CONVERT(VARCHAR,t.DateCreated,101)" },
            {Constants.ATTEMPT_DATE, "CONVERT(VARCHAR,sa.dateTaken,101)" },
            {Constants.DATE_TAKEN, "sa.dateTaken" },
            {Constants.STATUS, "ss.desc as status" },
            {Constants.EVALUATOR_NAME, "u.Fname" },
            {Constants.CITY, "sam.City" },
            {Constants.STATE, "sam.State" },
            {Constants.ZIP, "sam.Zip" },
            {Constants.COUNTRY, "sam.Country"  },
            {Constants.IP, "sam.IP" },            
            {Constants.DATECREATED, "CONVERT(VARCHAR,t.DateCreated,101)" },
            {Constants.ISENABLED, "t.isenabled" },

            {Constants.CREATED_BY, "u.FName" },
            {Constants.DELETED_BY, "" },
            {Constants.ASSIGNED_TO, "u.FName" },
            {Constants.REPETITIONS_COUNT, "" },
        };

        /// <summary>
        ///     Dictionary having list of tables that requried to get the columns for task(s) result
        /// </summary>
        private readonly Dictionary<string, List<string>> tableJoins = new Dictionary<string, List<string>>()
        {
            { " LEFT JOIN dbo.TaskSkill ts ON ts.TaskVersionId=tv.Id LEFT JOIN dbo.SkillActivity sa ON sa.SkillId=ts.SkillId   LEFT JOIN dbo.SkillActivityMetrics sam ON sam.SkillActivityId=sa.Id  LEFT JOIN dbo.[User] u on u.Id=sa.evaluator " , new List<string> {Constants.DATE_EXPIRED, Constants.DATE_TAKEN, Constants.CITY, Constants.STATE, Constants.ZIP, Constants.COUNTRY, Constants.IP, Constants.SCORE, Constants.DURATION, Constants.EVALUATOR_NAME, Constants.ASSIGNED_TO, Constants.COMPLETION_DATE, Constants.LAST_ATTEMPT_DATE, Constants.NUMBER_OF_ATTEMPTS, Constants.EXPIRATION_DATE, Constants.EVALUATOR_NAME , Constants.CREATED_BY, Constants.DELETED_BY} },
            { " JOIN dbo.SCOStatus ss ON ss.status=sa.Status" , new List<string> {Constants.STATUS} }
        };
    }
}
