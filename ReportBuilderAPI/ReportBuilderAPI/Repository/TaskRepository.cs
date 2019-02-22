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
                    DataSet dataSet = databaseWrapper.ExecuteAdapter(query);
                    foreach (DataRow rows in dataSet.Tables[0].Rows)
                    {
                        TaskResponse employeeResponse = new TaskResponse
                        {
                            TaskId = Convert.ToInt32(rows["TaskId"]),
                            UserId = userId,
                            WorkbookId = workbookId,
                            TaskCode = Convert.ToString(rows["TaskCode"]),
                            TaskName = Convert.ToString(rows["TaskName"]),
                            CompletedTasksCount = Convert.ToInt32(rows["CompletedWorkbooks"]),
                            IncompletedTasksCount = Convert.ToInt32(rows["InCompletedWorkbooks"]),
                            TotalTasks = Convert.ToInt32(rows["TotalTasks"]),
                            CompletionPrecentage = (int)Math.Round((double)(100 * (Convert.ToInt32(rows["CompletedWorkbooks"]))) / (Convert.ToInt32(rows["TotalTasks"]))),
                        };
                        taskList.Add(employeeResponse);
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
                    DataSet dataSet = databaseWrapper.ExecuteAdapter(query);
                    foreach (DataRow rows in dataSet.Tables[0].Rows)
                    {
                        TaskModel taskModel = JsonConvert.DeserializeObject<TaskModel>(Convert.ToString(rows["Comments"]));
                        AttemptsResponse employeeResponse = new AttemptsResponse
                        {
                            Attempt = Convert.ToString(rows["Attempt"]),
                            Status = Convert.ToString(rows["Status"]),
                            DateTime = Convert.ToDateTime(rows["DateTaken"]).ToString("MM/dd/yyyy"),
                            Location = Convert.ToString(rows["Street1"]) + "," + Convert.ToString(rows["Street2"]) + "," + Convert.ToString(rows["City"]) + "," + Convert.ToString(rows["State"]) + "," + Convert.ToString(rows["Zip"]),
                            Evaluator = Convert.ToString(rows["EvaluatorName"]),
                            Comments = taskModel.Comment
                        };
                        attemptsList.Add(employeeResponse);
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
        /// Create the query to depends upon the request
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="companyId"></param>
        /// <returns>string</returns>
        public string CreateTaskQuery(QueryBuilderRequest queryRequest, int companyId)
        {
            string query = string.Empty, tableJoin = string.Empty, selectQuery = string.Empty, whereQuery = string.Empty, supervisorId = string.Empty;            
            List<string> fieldList = new List<string>();
            EmployeeRepository employeeRepository = new EmployeeRepository();            

            try
            {
                selectQuery = "SELECT  DISTINCT";
           
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

                supervisorId = queryRequest.Fields.Where(x => x.Name.ToUpper() == Constants.SUPERVISOR_ID).Select(x => x.Value).FirstOrDefault();
               
                query += tableJoin;
              
                if (queryRequest.Fields.Where(x => x.Name == Constants.SUPERVISOR_ID).ToList().Count > 0 && queryRequest.Fields.Where(x => x.Name == Constants.USERID).ToList().Count > 0)
                {
                    EmployeeModel userDetails = queryRequest.Fields.Where(x => x.Name == Constants.USERID).FirstOrDefault();
                    queryRequest.Fields.Remove(userDetails);
                    queryRequest.Fields.Select(x => x.Name == Constants.SUPERVISOR_ID ? x.Name = Constants.SUPERVISOR_USER : x.Name).ToList();
                }

                if (queryRequest.ColumnList.Contains(Constants.TOTAL_EMPLOYEES) && !string.IsNullOrEmpty(supervisorId))
                {
                    queryRequest.Fields.Select(x => x.Name == Constants.SUPERVISOR_ID ? x.Name = Constants.SUPERVISOR_SUB : x.Name).ToList();
                }

                //getting where conditions
                whereQuery = string.Join("", from employee in queryRequest.Fields
                                             select (!string.IsNullOrEmpty(employee.Bitwise) ? (" " + employee.Bitwise + " ") : string.Empty) + (!string.IsNullOrEmpty(taskFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault()) ? (taskFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault() + employeeRepository.CheckOperator(employee.Operator, employee.Value.Trim(), employee.Name)) : string.Empty));

                whereQuery = !queryRequest.ColumnList.Contains(Constants.COMPANY_NAME) ? ((!string.IsNullOrEmpty(whereQuery)) ? (" WHERE ucs.CompanyId=" + companyId + " and  (" + whereQuery + ")") : (" WHERE ucs.CompanyId=" + companyId)) : (" WHERE (" + whereQuery + ")");

                if (queryRequest.Fields.Count == 1 && queryRequest.Fields.Where(x => x.Value == Constants.SUPERVISOR).ToList().Count > 0)
                {
                    whereQuery += "and u.Id in (SELECT u.Id FROM dbo.[User] u join userCompany uc on uc.UserId = u.Id JOIN UserRole ur on ur.UserId = u.Id JOIN ROle r on r.Id = ur.roleId WHERE r.Name = 'SUPERVISOR' and uc.companyId = @companyId))";
                }
                query += whereQuery;
                return query;
            }
            catch (Exception createTaskQueryException)
            {
                LambdaLogger.Log(createTaskQueryException.ToString());
                return string.Empty;
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
            string query = string.Empty;
            List<TaskResponse> workbookDetails;            
            Dictionary<string, string> parameterList;
            try
            {
                QueryBuilderRequest queryRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(requestBody);
                
                //Generates the query
                query = CreateTaskQuery(queryRequest, companyId);

                //Get the parameters  to send into the sql query
                parameterList = Getparameters(queryRequest, companyId);

                workbookDetails = ReadTaskDetails(query, parameterList);
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
        /// Get parameter exception
        /// </summary>
        public Dictionary<string, string>  Getparameters(QueryBuilderRequest queryRequest, int companyId)
        {
            string  supervisorId = string.Empty, workbookId = string.Empty, taskId = string.Empty, dueDays = string.Empty;
            Dictionary<string, string> parameterList=new Dictionary<string, string> { };
            try
            {
                //Get the list of Id details
                supervisorId = queryRequest.Fields.Where(x => x.Name.ToUpper() == Constants.SUPERVISOR_ID).Select(x => x.Value).FirstOrDefault();
                workbookId = queryRequest.Fields.Where(x => x.Name.ToUpper() == Constants.WORKBOOK_ID).Select(x => x.Value).FirstOrDefault();
                taskId = queryRequest.Fields.Where(x => x.Name.ToUpper() == Constants.TASK_ID).Select(x => x.Value).FirstOrDefault();
                dueDays = Convert.ToString(queryRequest.Fields.Where(x => x.Name.ToUpper() == (Constants.WORKBOOK_IN_DUE) || x.Name.ToUpper() == (Constants.PAST_DUE)).Select(x => x.Value).FirstOrDefault());
                if (string.IsNullOrEmpty(dueDays))
                {
                    dueDays = "30";
                }

                //Get the parameter dictionary
                parameterList = new Dictionary<string, string>() { { "userId", Convert.ToString(supervisorId) }, { "companyId", Convert.ToString(companyId) }, { "workbookId", Convert.ToString(workbookId) }, { "taskId", Convert.ToString(taskId) }, { "duedays", Convert.ToString(dueDays) } };
                return parameterList;
            }
            catch (Exception getParameterException)
            {
                LambdaLogger.Log(getParameterException.ToString());
                return parameterList;
            }
        }

        /// <summary>
        ///     Creating response object after reading task(s) details from database
        /// </summary>
        /// <param name="query"></param>
        /// <returns>TaskResponse</returns>
        public List<TaskResponse> ReadTaskDetails(string query, Dictionary<string, string> parameters)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            TaskResponse taskResponse;
            List<TaskResponse> taskList = new List<TaskResponse>();
            try
            {
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query, parameters);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        TaskModel taskModel = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Comments'").Count() == 1) ? JsonConvert.DeserializeObject<TaskModel>(Convert.ToString(sqlDataReader["Comments"])) : null;
                        taskResponse = new TaskResponse
                        {
                            TaskId = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'taskId'").Count() == 1) ? (sqlDataReader["taskId"]!=DBNull.Value ? (int?)sqlDataReader["taskId"]:0) : null,
                            TaskName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'taskName'").Count() == 1) ? Convert.ToString(sqlDataReader["taskName"]) : null,
                            AssignedTo = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'assignee'").Count() == 1) ? Convert.ToString(sqlDataReader["assignee"]) : null,
                            EvaluatorName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'evaluatorName'").Count() == 1) ? Convert.ToString(sqlDataReader["evaluatorName"]) : null,
                            Status = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'status'").Count() == 1) ? Convert.ToString(sqlDataReader["status"]) : null,
                            ExpirationDate = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'DateExpired'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(sqlDataReader["DateExpired"])) ? Convert.ToDateTime(sqlDataReader["DateExpired"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,
                            TaskCode = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Code'").Count() == 1) ? Convert.ToString(sqlDataReader["Code"]) : null,
                            CompletedTasksCount = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'CompletedTasks'").Count() == 1) ? (int?)sqlDataReader["CompletedTasks"] : null,
                            TotalTasks = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'TotalTasks'").Count() == 1) ? (int?)sqlDataReader["TotalTasks"] : null,
                            IncompletedTasksCount = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'InCompleteTask'").Count() == 1) ? (int?)sqlDataReader["InCompleteTask"] : null,
                            UserId = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'userId'").Count() == 1) ? (int?)sqlDataReader["userId"] : null,
                            WorkbookId = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'workbookId'").Count() == 1) ? (int?)sqlDataReader["workbookId"] : null,
                            NumberofAttempts = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'Attempt'").Count() == 1) ? Convert.ToString(sqlDataReader["Attempt"]) : null,
                            LastAttemptDate = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'lastAttemptDate'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(sqlDataReader["lastAttemptDate"])) ? Convert.ToDateTime(sqlDataReader["lastAttemptDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,

                            Location = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'location'").Count() == 1) ? Convert.ToString(sqlDataReader["location"]) : null,
                            Comments = taskModel?.Comment,


                            EmployeeName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'employeeName'").Count() == 1) ? Convert.ToString(sqlDataReader["employeeName"]) : null,

                            Role = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'role'").Count() == 1) ? Convert.ToString(sqlDataReader["role"]) : null,

                            AssignedQualification = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'AssignedQualification'").Count() == 1) ? (int?)sqlDataReader["AssignedQualification"] : null,

                            CompletedQualification = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'CompletedQualification'").Count() == 1) ? (int?)sqlDataReader["CompletedQualification"] : null,

                            InDueQualification = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'InDueQualification'").Count() == 1) ? (int?)sqlDataReader["InDueQualification"] : null,


                            PastDueQualification = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'PastDueQualification'").Count() == 1) ? (int?)sqlDataReader["PastDueQualification"] : null,

                            IncompleteQualification = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'IncompleteQualification'").Count() == 1) ? (int?)sqlDataReader["IncompleteQualification"] : null,


                            TotalEmployees = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'TotalEmployees'").Count() == 1) ? (int?)sqlDataReader["TotalEmployees"] : null,

                            AssignedDate = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'AssignedDate'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(sqlDataReader["AssignedDate"])) ? Convert.ToDateTime(sqlDataReader["AssignedDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,

                            CompanyName = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'companyName'").Count() == 1) ? Convert.ToString(sqlDataReader["companyName"]) : null,

                            CompanyId = (sqlDataReader.GetSchemaTable().Select("ColumnName = 'companyId'").Count() == 1) ? (int?)(sqlDataReader["companyId"]) : null

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
                { Constants.STATUS, ", ss.[desc] as status"},
                { Constants.DATE_EXPIRED, ", sa.DateExpired"},
                { Constants.EVALUATOR_NAME, ", (SELECT (ISNULL(NULLIF(u.LName, '') + ', ', '') + u.Fname) as evaluatorName FROM dbo.[User] usr WHERE usr.Id=sa.Evaluator) as evaluatorName"},
                { Constants.ASSIGNED_TO, ",  (SELECT usr.UserName FROM dbo.[User] usr WHERE usr.Id=sa.UserId) as assignee"},
                { Constants.IP, ", sam.IP"},
                { Constants.LOCATION, ",   concat((CASE WHEN sam.street IS NOT NULL THEN (sam.street + ',') ELSE '' END),   (CASE WHEN sam.City IS NOT NULL THEN (sam.city+ ',') ELSE '' END), (CASE WHEN sam.State IS NOT NULL THEN (sam.State + ',') ELSE '' END),   (CASE WHEN sam.Zip IS NOT NULL THEN (sam.Zip + ',') ELSE '' END),  sam.country) as location"},
                { Constants.DURATION, ", sam.duration"},
                { Constants.SCORE, ", sam.score"},
                { Constants.DATE_TAKEN, ", sa.DateTaken"},
                { Constants.USERID, ", u.Id as userId"},
                { Constants.WORKBOOK_ID, ", wbc.workbookId"},
                { Constants.COMPLETION_DATE, ", u.DateCreated"},
                { Constants.LAST_ATTEMPT_DATE, ", sa.DateTaken as lastAttemptDate"},
                { Constants.CREATED_BY, ", (Select us.Username from dbo.[User] us WHERE us.Id=sa.Createdby) as CreatedBy"},
                { Constants.DELETED_BY, ", (Select us.Username from dbo.[User] us WHERE us.Id=sa.deletedby) as DeletedBy"},
                { Constants.PARENT_TASK_NAME, ", u.Email"},
                { Constants.CHILD_TASK_NAME, ", u.City"},
                { Constants.NUMBER_OF_ATTEMPTS, ", sa.Attempt"},
                { Constants.EXPIRATION_DATE, ", sa.DateExpired"},
                { Constants.COMMENTS, ", sam.Payload AS Comments"},
                { Constants.TASK_CODE, ", t.Code"},
                { Constants.COMPLETED_TASK, ", (SELECT ISNULL((SELECT SUM(wbc.Repetitions) FROM [user] us LEFT JOIN UserWorkBook uwb ON uwb.UserId=us.Id LEFT JOIN WorkBookContent wbc ON wbc.WorkBookId=uwb.WorkBookId LEFT JOIN  dbo.Task tk ON tk.Id=wbc.EntityId LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=tk.Id LEFT JOIN dbo.TaskSkill tss ON tss.TaskVersionId=tv.Id LEFT JOIN dbo.SkillActivity sa ON sa.SkillId=tss.SkillId LEFT JOIN dbo.SCOStatus ss ON ss.status=sa.Status WHERE  us.Id IN ((u.Id))  AND uwb.IsEnabled=1 AND wbc.WorkBookId=@workbookId AND tk.Id=t.Id AND ss.[desc]='COMPLETED'),0)) AS CompletedTasks"},

                { Constants.INCOMPLETE_TASK, ", (SELECT ISNULL((SELECT COUNT(DISTINCT tk.Id) FROM [user] us LEFT JOIN UserWorkBook uwb ON uwb.UserId=us.Id LEFT JOIN WorkBookContent wbc ON wbc.WorkBookId=uwb.WorkBookId LEFT JOIN  dbo.Task tk ON tk.Id=wbc.EntityId LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=tk.Id LEFT JOIN dbo.TaskSkill tss ON tss.TaskVersionId=tv.Id LEFT JOIN dbo.SkillActivity sa ON sa.SkillId=tss.SkillId LEFT JOIN dbo.SCOStatus ss ON ss.status=sa.Status WHERE  u.Id IN ((u.Id))  AND uwb.IsEnabled=1 AND tk.Id=t.Id AND wbc.WorkBookId=@workbookId AND ss.[desc]='FAILED'),0))  AS InCompleteTask"},

                { Constants.TOTAL_TASK, ", (SELECT ISNULL((SELECT SUM(wbc.Repetitions) FROM [user] us LEFT JOIN UserWorkBook uwb ON  uwb.UserId=us.Id LEFT JOIN WorkBookContent wbc ON wbc.WorkBookId=uwb.WorkBookId LEFT JOIN  dbo.Task tk ON tk.Id=wbc.EntityId LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=tk.Id WHERE  us.Id IN ((u.Id))  AND uwb.IsEnabled=1 AND wbc.WorkBookId=@workbookId AND tk.Id=t.Id),0)) AS TotalTasks"},

                { Constants.ASSIGNED_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskversionId) FROM courseAssignment cs  WHERE UserId In (SELECT u.id UNION SELECT * FROM getchildUsers(u.id)) AND cs.CompanyId=@companyId ) ,0)) as AssignedQualification" },

                { Constants.COMPLETED_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskId) FROM TranscriptSkillsDN ts WHERE Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5 AND Date_Knowledge_Cert_Expired > GETDATE() AND ts.User_Id In (SELECT u.Id UNION SELECT * FROM getchildUsers(u.Id)) AND ts.CompanyId=@companyId  ) ,0)) as CompletedQualification" },


                { Constants.IN_COMPLETE_QUALIFICATION,",  (SELECT ISNULL((SELECT COUNT(DISTINCT taskId) FROM TranscriptSkillsDN ts WHERE  Knowledge_Cert_Status IN (3, 2,0,4, 255) AND  ts.User_Id IN (SELECT u.Id UNION SELECT * FROM getchildUsers(u.Id)) AND ts.CompanyId=@companyId ) ,0)) as IncompleteQualification" },


                { Constants.PAST_DUE_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskversionId) FROM courseAssignment cs  WHERE UserId In (SELECT u.id UNION SELECT * FROM getchildUsers( u.id)) AND cs.IsEnabled = 1 AND cs.Status = 0 AND cs.IsCurrent = 1 AND cs.ExpirationDate < GETDATE() AND  ABS(DATEDIFF(DAY,GETDATE(),cs.ExpirationDate))<=@duedays AND cs.CompanyId=@companyId AND cs.FirstAccessed IS NULL ) ,0)) as PastDueQualification" },

                { Constants.IN_DUE_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskversionId) FROM courseAssignment cs  WHERE UserId In (SELECT u.id UNION SELECT * FROM getchildUsers(u.id)) AND cs.IsEnabled = 1 AND cs.Status = 0 AND cs.IsCurrent = 1 AND cs.ExpirationDate > GETDATE() AND  ABS(DATEDIFF(DAY,GETDATE(),cs.ExpirationDate))<=@duedays AND cs.CompanyId=@companyId AND cs.FirstAccessed IS NULL ) ,0)) as InDueQualification" },

                { Constants.TOTAL_EMPLOYEES,", (SELECT COUNT(*) FROM dbo.Supervisor ss LEFT JOIN UserCompany uc on uc.UserId=ss.UserId   WHERE ss.SupervisorId = u.Id and uc.companyId=@companyId) AS TotalEmployees" },


                { Constants.ASSIGNED_COMPANY_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskversionId) FROM courseAssignment cs  WHERE cs.CompanyId=@companyId ) ,0)) as AssignedQualification" },

                { Constants.COMPLETED_COMPANY_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskId) FROM TranscriptSkillsDN ts WHERE Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5 AND Date_Knowledge_Cert_Expired > GETDATE() AND ts.CompanyId=@companyId  ) ,0)) as CompletedQualification" },


                { Constants.IN_COMPLETE_COMPANY_QUALIFICATION,",  (SELECT ISNULL((SELECT COUNT(DISTINCT taskId) FROM TranscriptSkillsDN ts WHERE  Knowledge_Cert_Status IN (3, 2,0,4, 255) AND ts.CompanyId=@companyId ) ,0)) as IncompleteQualification" },


                { Constants.PAST_DUE_COMPANY_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskversionId) FROM courseAssignment cs  WHERE  cs.IsEnabled = 1 AND cs.Status = 0 AND cs.IsCurrent = 1 AND cs.ExpirationDate < GETDATE() AND  ABS(DATEDIFF(DAY,GETDATE(),cs.ExpirationDate))<=@duedays AND cs.CompanyId=@companyId AND cs.FirstAccessed IS NULL ) ,0)) as PastDueQualification" },

                { Constants.IN_DUE_COMPANY_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskversionId) FROM courseAssignment cs  WHERE cs.IsEnabled = 1 AND cs.Status = 0 AND cs.IsCurrent = 1 AND cs.ExpirationDate > GETDATE() AND  ABS(DATEDIFF(DAY,GETDATE(),cs.ExpirationDate))<=@duedays AND cs.CompanyId=@companyId AND cs.FirstAccessed IS NULL ) ,0)) as InDueQualification" },

                { Constants.TOTAL_COMPANY_EMPLOYEES,", (SELECT COUNT(*) FROM dbo.Supervisor ss LEFT JOIN UserCompany uc on uc.UserId=ss.UserId   WHERE uc.companyId=@companyId) AS TotalEmployees" },



                { Constants.EMPLOYEE_NAME,", (ISNULL(NULLIF(u.LName, '') + ', ', '') + u.Fname)  as employeeName" },

                { Constants.ROLE,", r.Name as role" },

                {Constants.ASSIGNED_DATE, ", ca.DateCreated as AssignedDate " },
                { Constants.COURSE_EXPIRATION_DATE, ", ca. ExpirationDate as DateExpired"},
                {Constants.COMPANY_NAME, ", cy.Name as companyName  " },

                { Constants.COMPANY_ID, ", cy.Id as companyId  " }

        };

        /// <summary>
        ///     Dictionary having fields that requried for the task entity.
        ///     Based on fields, query is being formed/updated
        /// </summary>
        private readonly Dictionary<string, string> taskFields = new Dictionary<string, string>()
        {
            {Constants.TASK_NAME, "t.Name " },
            {Constants.TASK_CODE, "t.code " },
            {Constants.TASK_ID, "t.Id " },
            {Constants.TASK_CREATED, "CONVERT(DATE,t.DateCreated,101)" },
            {Constants.ATTEMPT_DATE, "CONVERT(DATE,sa.dateTaken,101)" },
            {Constants.DATE_TAKEN, "sa.dateTaken" },
            {Constants.STATUS, "ss.[desc] " },
            {Constants.EVALUATOR_NAME, " (SELECT usr.UserName as evaluatorName FROM dbo.[User] usr WHERE usr.Id=sa.Evaluator) " },
            {Constants.CITY, "sam.City" },
            {Constants.USERID, "u.Id" },
            {Constants.STATE, "sam.State" },
            {Constants.ZIP, "sam.Zip" },
            {Constants.COUNTRY, "sam.Country"  },
            {Constants.IP, "sam.IP" },
            {Constants.DATECREATED, "CONVERT(DATE,t.DateCreated,101)" },
            {Constants.ISENABLED, "t.isenabled" },
            {Constants.CREATED_BY, " (Select us.UserName from dbo.[User] us WHERE us.Id=sa.Createdby) " },
            {Constants.DELETED_BY, " (Select us.UserName from dbo.[User] us WHERE us.Id=sa.Deletedby) " },
            {Constants.ASSIGNED_TO, " sa.userId" },
            {Constants.REPETITIONS_COUNT, "" },
            {Constants.SUPERVISOR_ID, " u.Id IN ((SELECT @userId UNION SELECT * FROM getChildUsers (@userId))) " },
            {Constants.WORKBOOK_ID, " uwb.IsEnabled=1 and wbc.WorkbookId" },
            {Constants.SUPERVISOR_SUB, " s.supervisorId " },
            {Constants.ROLE, " r.Name " },
            {Constants.CAN_CERTIFY, "  ca.IsEnabled=1 and c.CanCertify " },
            {Constants.COMPLETED, "  Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5 AND Date_Knowledge_Cert_Expired > GETDATE()" },
            {Constants.PAST_DUE, "  ca.IsEnabled = 1 	AND ca.Status = 0 AND ca.IsCurrent = 1 AND ca.ExpirationDate < GETDATE() AND  ABS(DATEDIFF(DAY,GETDATE(),ca.ExpirationDate)) <=@duedays AND ca.FirstAccessed IS NULL " },
            {Constants.IN_DUE, "  ca.IsEnabled = 1 	AND ca.Status = 0 AND ca.IsCurrent = 1 AND ca.ExpirationDate > GETDATE() AND   ABS(DATEDIFF(DAY,GETDATE(),ca.ExpirationDate)) <=@duedays  AND ca.FirstAccessed IS NULL " },
            {Constants.IN_COMPLETE, "  Knowledge_Cert_Status IN (3, 2,0,4, 255) " },
           {Constants.SUPERVISOR_USER, " u.Id IN (SELECT  @userId UNION SELECT * FROM getChildUsers (@userId))  " }
        };



        /// <summary>
        ///     Dictionary having list of tables that requried to get the columns for task(s) result
        /// </summary>
        private readonly Dictionary<string, List<string>> tableJoins = new Dictionary<string, List<string>>()
        {
             { " LEFT JOIN dbo.TaskSkill ts ON ts.TaskVersionId=tv.Id LEFT JOIN dbo.SkillActivity sa ON sa.SkillId=ts.SkillId    LEFT JOIN dbo.SCOStatus ss ON ss.status=sa.Status LEFT JOIN dbo.SkillActivityMetrics sam ON sam.SkillActivityId=sa.Id   LEFT JOIN WorkBookContent wbc ON wbc.EntityId=t.Id    LEFT JOIN UserWorkBook uwb ON uwb.WorkbookId=wbc.WorkbookId   LEFT JOIN dbo.[User] u on u.Id=uwb.UserId LEFT JOIn Usercompany ucs on ucs.userId=u.Id " , new List<string> {Constants.DATE_EXPIRED, Constants.DATE_TAKEN, Constants.CITY, Constants.STATE, Constants.ZIP, Constants.COUNTRY, Constants.IP, Constants.SCORE, Constants.DURATION, Constants.EVALUATOR_NAME, Constants.ASSIGNED_TO, Constants.COMPLETION_DATE, Constants.LAST_ATTEMPT_DATE, Constants.NUMBER_OF_ATTEMPTS, Constants.EXPIRATION_DATE, Constants.EVALUATOR_NAME , Constants.CREATED_BY, Constants.DELETED_BY, Constants.WORKBOOK_ID, Constants.EVALUATOR_NAME, Constants.STATUS } },            



             { " LEFT JOIN TaskSkill ts ON ts.TaskversionId=tv.Id 	 LEFT JOIN CourseAssignment ca ON ca.taskversionId=ts.taskversionId  	  LEFT JOIN COurse c on c.Id=ca.courseId LEFT JOIN transcriptSkillsDN tss on tss.taskId=t.Id       FULL OUTER JOIN userCompanySeries ucs ON ucs.userId=ca.userId  LEFT JOIN  Usercompany uc on uc.userId=ucs.userId    FULL OUTER JOIN dbo.[User] u ON u.Id=ucs.UserId FULL OUTER JOIn Supervisor s ON s.userId=u.Id    LEFT JOIN dbo.Company cy ON cy.Id=ucs.CompanyId  " , new List<string> {Constants.ASSIGNED_QUALIFICATION, Constants.COMPLETED_QUALIFICATION, Constants.IN_DUE_QUALIFICATION, Constants.PAST_DUE_QUALIFICATION, Constants.IN_COMPLETE_QUALIFICATION, Constants.EMPLOYEE_NAME , Constants.ASSIGNED_DATE, Constants.COURSE_EXPIRATION_DATE, Constants.ASSIGNED_COMPANY_QUALIFICATION, Constants.COMPLETED_COMPANY_QUALIFICATION, Constants.IN_COMPLETE_COMPANY_QUALIFICATION, Constants.PAST_DUE_COMPANY_QUALIFICATION, Constants.IN_DUE_COMPANY_QUALIFICATION, Constants.TOTAL_COMPANY_EMPLOYEES} },

             { "  FULL OUTER JOIN UserRole ur ON ur.userId=u.Id FULL OUTER JOIN role r ON r.Id=ur.RoleId ", new List<string> {Constants.ROLE} },
        };
    }
}
