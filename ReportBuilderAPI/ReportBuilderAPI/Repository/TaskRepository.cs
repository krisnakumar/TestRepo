using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Helpers;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ReportBuilderAPI.Repository
{
    /// <summary>
    ///     Class that handles to read task(s) details for specific user and workbook from database
    /// </summary>
    public class TaskRepository : ITask
    {
        /// <summary>
        /// Create the query to depends upon the request
        /// </summary>
        /// <param name="queryRequest"></param>
        /// <returns></returns>
        public string CreateTaskQuery(QueryBuilderRequest queryBuilderRequest)
        {
            string query = string.Empty, tableJoin = string.Empty, selectQuery = string.Empty, whereQuery = string.Empty, supervisorId = string.Empty;
            List<string> fieldList = new List<string>();
            int companyId = 0;
            try
            {
                companyId = queryBuilderRequest.CompanyId;
                selectQuery = "SELECT  DISTINCT";

                //getting column List
                query = string.Join("", (from column in taskColumnList
                                         where queryBuilderRequest.ColumnList.Any(x => x == column.Key)
                                         select column.Value));
                query = query.TrimStart(',');
                query = selectQuery + query;

                //if (queryBuilderRequest.ColumnList.Contains(Constants.ASSIGNED_COMPANY_QUALIFICATION))
                //{
                //    query += " FROM dbo.DashboardReportDN dr ";
                //}
                //else
                //{
                //    query += "  FROM dbo.Task t LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=t.Id ";
                //}

                //get table joins
                fieldList = queryBuilderRequest.ColumnList.ToList();

                fieldList.AddRange(queryBuilderRequest.Fields.Select(x => x.Name.ToUpper()).ToArray());

                tableJoin = string.Join("", from joins in tableJoins
                                            where fieldList.Any(x => joins.Value.Any(y => y == x))
                                            select joins.Key);

                supervisorId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.SUPERVISOR_ID).Select(x => x.Value).FirstOrDefault();

                query += tableJoin;

                if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.SUPERVISOR_ID).ToList().Count > 0 && queryBuilderRequest.Fields.Where(x => x.Name == Constants.USERID).ToList().Count > 0)
                {
                    EmployeeModel userDetails = queryBuilderRequest.Fields.Where(x => x.Name == Constants.USERID).FirstOrDefault();
                    queryBuilderRequest.Fields.Remove(userDetails);
                    queryBuilderRequest.Fields.Select(x => x.Name == Constants.SUPERVISOR_ID ? x.Name = Constants.SUPERVISOR_USER : x.Name).ToList();
                }

                if (queryBuilderRequest.ColumnList.Contains(Constants.TOTAL_EMPLOYEES) && !string.IsNullOrEmpty(supervisorId))
                {
                    queryBuilderRequest.Fields.Select(x => x.Name == Constants.SUPERVISOR_ID ? x.Name = Constants.SUPERVISOR_SUB : x.Name).ToList();
                }

                if (queryBuilderRequest.ColumnList.Contains(Constants.ASSIGNED_COMPANY_QUALIFICATION))
                {
                    queryBuilderRequest.Fields.Select(x => x.Name == Constants.USERID ? x.Name = Constants.COMPANY_USER_ID : x.Name).ToList();
                }

                if (!queryBuilderRequest.ColumnList.Contains(Constants.ASSIGNED_COMPANY_QUALIFICATION) && !queryBuilderRequest.ColumnList.Contains(Constants.COMPLETED_ROLE_QUALIFICATION))
                {
                    //getting where conditions
                    whereQuery = string.Join("", from employee in queryBuilderRequest.Fields
                                                 select (!string.IsNullOrEmpty(employee.Bitwise) ? (" " + employee.Bitwise + " ") : string.Empty) + (!string.IsNullOrEmpty(taskFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault()) ? (taskFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault() + OperatorHelper.CheckOperator(employee.Operator, employee.Value.Trim(), employee.Name)) : string.Empty));

                    whereQuery = !queryBuilderRequest.ColumnList.Contains(Constants.COMPANY_NAME) ? ((!string.IsNullOrEmpty(whereQuery)) ? (" WHERE uc.CompanyId IN (" + companyId + ") AND  (" + whereQuery + ")") : (" WHERE uc.CompanyId=" + companyId)) : (" WHERE (" + whereQuery + ")");
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
        ///  Get list of task(s) based on input field and column(s) for specific company [QueryBuilder]
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <returns>TaskResponse</returns>
        public TaskResponse GetQueryTaskDetails(QueryBuilderRequest queryBuilderRequest)
        {
            string query = string.Empty;
            Dictionary<string, string> parameterList;
            TaskResponse taskResponse = new TaskResponse();
            int companyId = 0;
            try
            {
                //Assign the request details to corresponding objects
                companyId = Convert.ToInt32(queryBuilderRequest.CompanyId);
                queryBuilderRequest = queryBuilderRequest.Payload;

                //Assign the companyId to the new object
                queryBuilderRequest.CompanyId = companyId;

                //Generates the query
                query = CreateTaskQuery(queryBuilderRequest);

                //Get the parameters  to send into the sql query
                parameterList = ParameterHelper.Getparameters(queryBuilderRequest);

                taskResponse.Tasks = ReadTaskDetails(query, parameterList);
                if (taskResponse.Tasks != null)
                {
                    return taskResponse;
                }
                else
                {
                    taskResponse.Error = ResponseBuilder.InternalError();
                    return taskResponse;
                }
            }
            catch (Exception getEmployeeDetails)
            {
                LambdaLogger.Log(getEmployeeDetails.ToString());
                taskResponse.Error = ResponseBuilder.InternalError();
                return taskResponse;
            }
        }



        /// <summary>
        ///  Creating response object after reading task(s) details from database
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns>TaskModel</returns>
        public List<TaskModel> ReadTaskDetails(string query, Dictionary<string, string> parameters)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<TaskModel> taskList = new List<TaskModel>();
            try
            {
                //Read the data from the database
                using (SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query, ParameterHelper.CreateSqlParameter(parameters)))
                {
                    if (sqlDataReader != null)
                    {
                        if (sqlDataReader.HasRows)
                        {
                            while (sqlDataReader.Read())
                            {
                                DataTable dataTable = sqlDataReader.GetSchemaTable();
                                //get the taskcomment from the payload
                                TaskModel taskComment = (dataTable.Select("ColumnName = 'Comments'").Count() == 1) ? JsonConvert.DeserializeObject<TaskModel>(Convert.ToString(sqlDataReader["Comments"])) : null;

                                //Get the task details from the database
                                TaskModel taskModel = new TaskModel
                                {
                                    TaskId = (dataTable.Select("ColumnName = 'taskId'").Count() == 1) ? (sqlDataReader["taskId"] != DBNull.Value ? (int?)sqlDataReader["taskId"] : 0) : null,
                                    TaskName = (dataTable.Select("ColumnName = 'taskName'").Count() == 1) ? Convert.ToString(sqlDataReader["taskName"]) : null,
                                    AssignedTo = (dataTable.Select("ColumnName = 'assignee'").Count() == 1) ? Convert.ToString(sqlDataReader["assignee"]) : null,
                                    EvaluatorName = (dataTable.Select("ColumnName = 'evaluatorName'").Count() == 1) ? Convert.ToString(sqlDataReader["evaluatorName"]) : null,
                                    Status = (dataTable.Select("ColumnName = 'status'").Count() == 1) ? Convert.ToString(sqlDataReader["status"]) : null,
                                    ExpirationDate = (dataTable.Select("ColumnName = 'DateExpired'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(sqlDataReader["DateExpired"])) ? Convert.ToDateTime(sqlDataReader["DateExpired"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,
                                    TaskCode = (dataTable.Select("ColumnName = 'Code'").Count() == 1) ? Convert.ToString(sqlDataReader["Code"]) : null,
                                    CompletedTasksCount = (dataTable.Select("ColumnName = 'CompletedTasks'").Count() == 1) ? (sqlDataReader["CompletedTasks"] != DBNull.Value ? (int?)sqlDataReader["CompletedTasks"] : 0) : null,
                                    TotalTasks = (dataTable.Select("ColumnName = 'TotalTasks'").Count() == 1) ? (sqlDataReader["TotalTasks"] != DBNull.Value ? (int?)sqlDataReader["TotalTasks"] : 0) : null,
                                    IncompletedTasksCount = (dataTable.Select("ColumnName = 'InCompleteTask'").Count() == 1) ? (sqlDataReader["InCompleteTask"] != DBNull.Value ? (int?)sqlDataReader["InCompleteTask"] : 0) : null,
                                    UserId = (dataTable.Select("ColumnName = 'userId'").Count() == 1) ? (sqlDataReader["userId"] != DBNull.Value ? (int?)sqlDataReader["userId"] : 0) : null,
                                    WorkbookId = (dataTable.Select("ColumnName = 'workbookId'").Count() == 1) ? (sqlDataReader["workbookId"] != DBNull.Value ? (int?)sqlDataReader["workbookId"] : 0) : null,
                                    NumberofAttempts = (dataTable.Select("ColumnName = 'Attempt'").Count() == 1) ? Convert.ToString(sqlDataReader["Attempt"]) : null,
                                    LastAttemptDate = (dataTable.Select("ColumnName = 'lastAttemptDate'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(sqlDataReader["lastAttemptDate"])) ? Convert.ToDateTime(sqlDataReader["lastAttemptDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,
                                    Location = (dataTable.Select("ColumnName = 'location'").Count() == 1) ? Convert.ToString(sqlDataReader["location"]) : null,
                                    Comments = taskComment?.Comment,
                                    EmployeeName = (dataTable.Select("ColumnName = 'employeeName'").Count() == 1) ? Convert.ToString(sqlDataReader["employeeName"]) : null,
                                    Role = (dataTable.Select("ColumnName = 'role'").Count() == 1) ? Convert.ToString(sqlDataReader["role"]) : null,
                                    AssignedQualification = (dataTable.Select("ColumnName = 'AssignedQualification'").Count() == 1) ? (sqlDataReader["AssignedQualification"] != DBNull.Value ? (int?)sqlDataReader["AssignedQualification"] : 0) : null,
                                    CompletedQualification = (dataTable.Select("ColumnName = 'CompletedQualification'").Count() == 1) ? (sqlDataReader["CompletedQualification"] != DBNull.Value ? (int?)sqlDataReader["CompletedQualification"] : 0) : null,
                                    InDueQualification = (dataTable.Select("ColumnName = 'InDueQualification'").Count() == 1) ? (sqlDataReader["InDueQualification"] != DBNull.Value ? (int?)sqlDataReader["InDueQualification"] : 0) : null,
                                    PastDueQualification = (dataTable.Select("ColumnName = 'PastDueQualification'").Count() == 1) ? (sqlDataReader["PastDueQualification"] != DBNull.Value ? (int?)sqlDataReader["PastDueQualification"] : 0) : null,
                                    IncompleteQualification = (dataTable.Select("ColumnName = 'IncompleteQualification'").Count() == 1) ? (sqlDataReader["IncompleteQualification"] != DBNull.Value ? (int?)sqlDataReader["IncompleteQualification"] : 0) : null,

                                    TotalEmployees = (dataTable.Select("ColumnName = 'TotalEmployees'").Count() == 1) ? (sqlDataReader["TotalEmployees"] != DBNull.Value ? (int?)sqlDataReader["TotalEmployees"] : 0) : null,
                                    AssignedDate = (dataTable.Select("ColumnName = 'AssignedDate'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(sqlDataReader["AssignedDate"])) ? Convert.ToDateTime(sqlDataReader["AssignedDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,
                                    CompanyName = (dataTable.Select("ColumnName = 'companyName'").Count() == 1) ? Convert.ToString(sqlDataReader["companyName"]) : null,
                                    CompanyId = (dataTable.Select("ColumnName = 'companyId'").Count() == 1) ? (sqlDataReader["companyId"] != DBNull.Value ? (int?)sqlDataReader["companyId"] : 0) : null,
                                    Score = (dataTable.Select("ColumnName = 'score'").Count() == 1) ? Convert.ToString(sqlDataReader["score"]) : null,
                                    Duration = (dataTable.Select("ColumnName = 'Duration'").Count() == 1) ? Convert.ToString(sqlDataReader["Duration"]) : null,
                                    CompletedRoleQualification = (dataTable.Select("ColumnName = 'CompletedRoleQualification'").Count() == 1) ? (sqlDataReader["CompletedRoleQualification"] != DBNull.Value ? (int?)sqlDataReader["CompletedRoleQualification"] : 0) : null,



                                    SuspendedQualification = (dataTable.Select("ColumnName = 'SuspendedQualification'").Count() == 1) ? (sqlDataReader["SuspendedQualification"] != DBNull.Value ? (int?)sqlDataReader["SuspendedQualification"] : 0) : null,
                                    DisqualifiedQualification = (dataTable.Select("ColumnName = 'DisqualifiedQualification'").Count() == 1) ? (sqlDataReader["DisqualifiedQualification"] != DBNull.Value ? (int?)sqlDataReader["DisqualifiedQualification"] : 0) : null,
                                    InCompletedRoleQualification = (dataTable.Select("ColumnName = 'NotCompletedRoleQualification'").Count() == 1) ? (sqlDataReader["NotCompletedRoleQualification"] != DBNull.Value ? (int?)sqlDataReader["NotCompletedRoleQualification"] : 0) : null,
                                    RoleId = (dataTable.Select("ColumnName = 'roleId'").Count() == 1) ? (sqlDataReader["roleId"] != DBNull.Value ? (int?)sqlDataReader["roleId"] : 0) : null,
                                    CompletedCompanyQualification = (dataTable.Select("ColumnName = 'CompletedCompanyUsers'").Count() == 1) ? (sqlDataReader["CompletedCompanyUsers"] != DBNull.Value ? (int?)sqlDataReader["CompletedCompanyUsers"] : 0) : null,
                                    InCompletedCompanyQualification = (dataTable.Select("ColumnName = 'NotCompletedCompanyUsers'").Count() == 1) ? (sqlDataReader["NotCompletedCompanyUsers"] != DBNull.Value ? (int?)sqlDataReader["NotCompletedCompanyUsers"] : 0) : null,
                                    TotalCompanyQualification = (dataTable.Select("ColumnName = 'TotalCompanyUsers'").Count() == 1) ? (sqlDataReader["TotalCompanyUsers"] != DBNull.Value ? (int?)sqlDataReader["TotalCompanyUsers"] : 0) : null,
                                };
                                // Adding each task details in array list
                                taskList.Add(taskModel);
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                return taskList;
            }
            catch (Exception readTaskDetailsException)
            {
                LambdaLogger.Log(readTaskDetailsException.ToString());
                return null;
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
                { Constants.TASK_NAME, ",  t.Name AS taskName" },
                { Constants.TASK_ID, ", t.Id AS taskId"},
                { Constants.STATUS, ", ss.[desc] AS status"},
                { Constants.DATE_EXPIRED, ", sa.DateExpired"},
                { Constants.EVALUATOR_NAME, ", (SELECT Full_Name_Format1 FROM dbo.[UserDetails_RB] usr WHERE usr.User_Id=sa.Evaluator) AS evaluatorName"},
                { Constants.ASSIGNED_TO, ",  (SELECT usr.User_Name AS UserName FROM dbo.[UserDetails_RB] usr WHERE usr.User_Id=sa.UserId) AS assignee"},
                { Constants.IP, ", sam.IP"},
                { Constants.LOCATION, ",   concat((CASE WHEN sam.street IS NOT NULL THEN (sam.street + ',') ELSE '' END),   (CASE WHEN sam.City IS NOT NULL THEN (sam.city+ ',') ELSE '' END), (CASE WHEN sam.State IS NOT NULL THEN (sam.State + ',') ELSE '' END),   (CASE WHEN sam.Zip IS NOT NULL THEN (sam.Zip + ',') ELSE '' END),  sam.country) AS location"},
                { Constants.DURATION, ", sam.duration"},
                { Constants.SCORE, ", sam.score"},
                { Constants.DATE_TAKEN, ", sa.DateTaken"},
                { Constants.USERID, ", u.User_Id as userId"},
                { Constants.WORKBOOK_ID, ", wbc.workbookId"},
                { Constants.COMPLETION_DATE, ", u.DateCreated"},
                { Constants.LAST_ATTEMPT_DATE, ", sa.DateTaken AS lastAttemptDate"},
                { Constants.CREATED_BY, ", (SELECT us.User_Name AS UserName FROM dbo.[UserDetails_RB] us WHERE us.User_Id=sa.Createdby) AS CreatedBy"},
                { Constants.DELETED_BY, ", (SELECT us.User_Name AS UserName FROM dbo.[UserDetails_RB] us WHERE us.User_Id=sa.deletedby) AS DeletedBy"},
                { Constants.PARENT_TASK_NAME, ", u.Email"},
                { Constants.CHILD_TASK_NAME, ", u.City"},
                { Constants.NUMBER_OF_ATTEMPTS, ", sa.Attempt"},
                { Constants.EXPIRATION_DATE, ", sa.DateExpired"},
                { Constants.COMMENTS, ", sam.Payload AS Comments"},
                { Constants.TASK_CODE, ", t.Code"},

                { Constants.COMPLETED_TASK, ", (SELECT ISNULL((SELECT SUM(wbc.Repetitions) FROM dbo.[UserDetails_RB] us LEFT JOIN dbo.UserWorkBook uwb ON uwb.UserId=us.User_Id LEFT JOIN dbo.WorkBookContent wbc ON wbc.WorkBookId=uwb.WorkBookId LEFT JOIN  dbo.Task tk ON tk.Id=wbc.EntityId LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=tk.Id LEFT JOIN dbo.TaskSkill tss ON tss.TaskVersionId=tv.Id LEFT JOIN dbo.SkillActivity sa ON sa.SkillId=tss.SkillId LEFT JOIN dbo.SCOStatus ss ON ss.status=sa.Status WHERE  us.User_Id IN ((u.User_Id))  AND uwb.IsEnabled=1 AND wbc.WorkBookId=@workbookId AND tk.Id=t.Id AND ss.[desc]='COMPLETED'),0)) AS CompletedTasks"},

                { Constants.INCOMPLETE_TASK, ", (SELECT ISNULL((SELECT COUNT(DISTINCT tk.Id) FROM dbo.[UserDetails_RB] us LEFT JOIN dbo.UserWorkBook uwb ON uwb.UserId=us.User_Id LEFT JOIN dbo.WorkBookContent wbc ON wbc.WorkBookId=uwb.WorkBookId LEFT JOIN  dbo.Task tk ON tk.Id=wbc.EntityId LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=tk.Id LEFT JOIN dbo.TaskSkill tss ON tss.TaskVersionId=tv.Id LEFT JOIN dbo.SkillActivity sa ON sa.SkillId=tss.SkillId LEFT JOIN dbo.SCOStatus ss ON ss.status=sa.Status WHERE  u.User_Id IN ((u.User_Id))  AND uwb.IsEnabled=1 AND tk.Id=t.Id AND wbc.WorkBookId=@workbookId AND ss.[desc]='FAILED'),0))  AS InCompleteTask"},

                { Constants.TOTAL_TASK, ", (SELECT ISNULL((SELECT SUM(wbc.Repetitions) FROM dbo.[UserDetails_RB] us LEFT JOIN dbo.UserWorkBook uwb ON  uwb.UserId=us.User_Id LEFT JOIN dbo.WorkBookContent wbc ON wbc.WorkBookId=uwb.WorkBookId LEFT JOIN  dbo.Task tk ON tk.Id=wbc.EntityId LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=tk.Id WHERE  us.User_Id IN ((u.User_Id))  AND uwb.IsEnabled=1 AND wbc.WorkBookId=@workbookId AND tk.Id=t.Id),0)) AS TotalTasks"},

                { Constants.ASSIGNED_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskversionId) FROM dbo.courseAssignment cs  WHERE UserId =u.User_Id AND cs.CompanyId=@companyId ) ,0)) AS  AssignedQualification" },

                { Constants.COMPLETED_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskId) FROM dbo.TranscriptSkillsDN ts WHERE Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5 AND Date_Knowledge_Cert_Expired > GETDATE() AND ts.User_Id = u.User_Id AND ts.CompanyId=@companyId  ) ,0)) AS  CompletedQualification" },

                 { Constants.DISQUALIFIED_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskId) FROM dbo.TranscriptSkillsDN ts WHERE Knowledge_Cert_Status = 2 AND ts.User_Id = u.User_Id AND ts.CompanyId=@companyId  ) ,0)) AS  DisqualifiedQualification" },


                 { Constants.SUSPENDED_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskId) FROM dbo.TranscriptSkillsDN ts WHERE Knowledge_Cert_Status = 4 AND ts.User_Id = u.User_Id AND ts.CompanyId=@companyId  ) ,0)) AS  SuspendedQualification" },


                { Constants.IN_COMPLETE_QUALIFICATION,",  (SELECT ISNULL((SELECT COUNT(DISTINCT taskId) FROM dbo.TranscriptSkillsDN ts WHERE  Knowledge_Cert_Status IN (3, 2,0,4, 255) AND  ts.User_Id = u.User_Id AND ts.CompanyId=@companyId ) ,0)) AS IncompleteQualification" },

                { Constants.PAST_DUE_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskversionId) FROM dbo.courseAssignment cs  WHERE UserId = u.User_Id AND cs.IsEnabled = 1 AND cs.Status = 0 AND cs.IsCurrent = 1 AND cs.ExpirationDate < GETDATE() AND  ABS(DATEDIFF(DAY,GETDATE(),cs.ExpirationDate))<=@duedays AND cs.CompanyId=@companyId AND cs.FirstAccessed IS NULL ) ,0)) AS PastDueQualification" },

                { Constants.IN_DUE_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT(DISTINCT taskversionId) FROM dbo.courseAssignment cs  WHERE UserId = u.User_Id AND cs.IsEnabled = 1 AND cs.Status = 0 AND cs.IsCurrent = 1 AND cs.ExpirationDate > GETDATE() AND  ABS(DATEDIFF(DAY,GETDATE(),cs.ExpirationDate))<=@duedays AND cs.CompanyId=@companyId AND cs.FirstAccessed IS NULL ) ,0)) AS InDueQualification" },

                { Constants.TOTAL_EMPLOYEES,", (SELECT COUNT(ss.UserId) FROM dbo.Supervisor ss LEFT JOIN dbo.UserCompany uc ON uc.UserId = ss.UserId AND uc.IsEnabled = 1 AND IsDirectReport=1 AND uc.IsVisible = 1 AND uc.Status = 1 WHERE ss.SupervisorId = @userId AND ss.UserId <> @userId AND uc.CompanyId = @companyId AND ss.IsEnabled = 1 AND ss.SupervisorId <>  @userId ) AS TotalEmployees" },

                { Constants.ASSIGNED_COMPANY_QUALIFICATION,", SUM(dr.Assigned_Qual_Count) AS AssignedQualification" },

                { Constants.COMPLETED_COMPANY_QUALIFICATION,", SUM(dr.Qual_Count) as CompletedQualification" },

                { Constants.IN_COMPLETE_COMPANY_QUALIFICATION,",  SUM(dr.Disqual_Count)   AS IncompleteQualification" },

                { Constants.PAST_DUE_COMPANY_QUALIFICATION,", 0 AS PastDueQualification" },

                { Constants.IN_DUE_COMPANY_QUALIFICATION,", SUM(dr.Qual_Count_30) AS InDueQualification" },

                { Constants.TOTAL_COMPANY_EMPLOYEES,",  (SELECT COUNT(ss.UserId) FROM dbo.Supervisor ss   LEFT JOIN dbo.UserCompany uc ON uc.UserId = ss.UserId AND uc.IsEnabled = 1 AND IsDirectReport=1  AND uc.IsVisible = 1 AND uc.Status = 1        WHERE ss.UserId IN (select sr.userId from Supervisor sr WHERE  sr.SupervisorId=@userId AND sr.IsDirectReport=1 AND sr.IsEnabled = 1)  AND uc.CompanyId = @companyId AND ss.IsEnabled = 1 ) AS TotalEmployees" },

                { Constants.COMPLETED_ROLE_QUALIFICATION,", (SELECT COUNT(DISTINCT CompanyId)FROM dbo.TranscriptSkillsDN WHERE IsEnabled=1 AND  Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5 AND User_ID=ur.UserId) AS CompletedRoleQualification" },

                { Constants.NOT_COMPLETED_ROLE_QUALIFICATION,", (SELECT COUNT(DISTINCT CompanyId)FROM dbo.TranscriptSkillsDN WHERE IsEnabled=1 AND  Knowledge_Cert_Status != 1 AND Knowledge_Status_Code != 5 AND User_ID=ur.UserId) as NotCompletedRoleQualification" },

                { Constants.EMPLOYEE_NAME,", Full_Name_Format1  as employeeName" },

                { Constants.COMPLETED_COMPANY_USERS,",  (SELECT ISNULL(( SELECT COUNT(ts.UserId) FROM dbo.CourseAssignment ts  JOIN dbo.UserRole ur ON ur.UserId=ts.UserId WHERE ur.RoleId=@roleId AND companyId IN (Select ClientCompany from dbo.companyClient WHERE ownerCompany=@companyId)  GROUP BY  TaskversionId HAVING COUNT(TaskversionId)= (SELECT COUNT(TaskversionId) FROM dbo.TranscriptSkillsDN t   WHERE Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5 and CompanyId   IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=@companyId))) ,0)) AS CompletedCompanyUsers" },

                { Constants.NOT_COMPLETED_COMPANY_USERS,", (SELECT ISNULL(( SELECT COUNT(ts.UserId) FROM dbo.CourseAssignment ts JOIN dbo.UserRole ur ON ur.UserId=ts.UserId WHERE ur.RoleId=@roleId AND  companyId IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=@companyId) AND TaskversionId    NOT IN(SELECT TaskversionId FROM dbo.TranscriptSkillsDN t   WHERE Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5    AND CompanyId IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=@companyId)) ) ,0)) AS NotCompletedCompanyUsers" },

                { Constants.TOTAL_COMPLETED_COMPANY_USERS,",    (SELECT ISNULL(( SELECT COUNT(ts.UserId) FROM dbo.CourseAssignment ts  JOIN dbo.UserRole ur ON ur.UserId=ts.UserId WHERE ur.RoleId=@roleId AND companyId IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=@companyId))  ,0)) AS TotalCompanyUsers" },

                { Constants.ROLE,", (SELECT STUFF((SELECT DISTINCT ', ' + r.Name FROM dbo.UserRole ur JOIN dbo.Role r ON r.Id=ur.roleId  WHERE ur.UserId=u.User_Id FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 2, '')) As Role" },
                {Constants.ROLE_ID, ", r.Id as roleId" },
                {Constants.ASSIGNED_DATE, ", ca.DateCreated as AssignedDate " },
                {Constants.LOCK_OUT_REASON, ", ca.LockoutReason as LockoutReason " },

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
            {Constants.EVALUATOR_NAME, " (SELECT usr.User_Name  AS evaluatorName FROM dbo.[UserDetails_RB] usr WHERE usr.User_Id=sa.Evaluator) " },
            {Constants.CITY, "sam.City" },
            {Constants.USERID, "u.User_Id" },
            {Constants.STATE, "sam.State" },
            {Constants.ZIP, "sam.Zip" },
            {Constants.COUNTRY, "sam.Country"  },
            {Constants.IP, "sam.IP" },
            {Constants.DATECREATED, "CONVERT(DATE,t.DateCreated,101)" },
            {Constants.ISENABLED, "t.isenabled" },
            {Constants.CREATED_BY, " (SELECT us.User_Name AS UserName FROM dbo.[UserDetails_RB] us WHERE us.User_Id=sa.Createdby) " },
            {Constants.DELETED_BY, " (SELECT us.User_Name AS UserName FROM dbo.[UserDetails_RB] us WHERE us.User_Id=sa.Deletedby) " },
            {Constants.ASSIGNED_TO, " sa.userId" },
            {Constants.REPETITIONS_COUNT, "" },
            {Constants.SUPERVISOR_ID, " u.User_Id = @userId " },
            {Constants.WORKBOOK_ID, " uwb.IsEnabled=1 AND wbc.WorkbookId" },
            {Constants.SUPERVISOR_SUB, " s.supervisorId " },
            {Constants.ROLE, " r.Name " },
            {Constants.CAN_CERTIFY, "  ca.IsEnabled=1 AND c.CanCertify " },
            {Constants.COMPLETED, "  Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5 AND Date_Knowledge_Cert_Expired > GETDATE()" },

            {Constants.PAST_DUE, "  ca.IsEnabled = 1 	AND ca.Status = 0 AND ca.IsCurrent = 1 AND ca.ExpirationDate < GETDATE() AND  ABS(DATEDIFF(DAY,GETDATE(),ca.ExpirationDate)) <=@duedays AND ca.FirstAccessed IS NULL " },

            { Constants.IN_DUE, "  ca.IsEnabled = 1 	AND ca.Status = 0 AND ca.IsCurrent = 1 AND ca.ExpirationDate > GETDATE() AND   ABS(DATEDIFF(DAY,GETDATE(),ca.ExpirationDate)) <=@duedays  AND ca.FirstAccessed IS NULL " },
            {Constants.IN_COMPLETE, "  Knowledge_Cert_Status IN (3, 2,0,4, 255) " },

            {Constants.SUSPENDED, "  Knowledge_Cert_Status IN (4) " },

            {Constants.DISQUALIFIED, "  Knowledge_Cert_Status IN (2) " },

            {Constants.NOT_COMPLETED_COMPANY_USERS, "  ca.UserId IN(SELECT USERID FROM dbo.CourseAssignment ts WHERE companyId IN (SELECT ClientCompany FROM      dbo.companyClient WHERE ownerCompany=6) AND TaskversionId    NOT IN(SELECT TaskversionId FROM dbo.TranscriptSkillsDN t WHERE Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5    and CompanyId IN (Select ClientCompany from dbo.companyClient WHERE ownerCompany=6))) " },
            {Constants.COMPLETED_COMPANY_USERS, "  ca.UserId in(SELECT ts.UserId FROM dbo.CourseAssignment ts WHERE companyId IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=6)    GROUP BY ts.UserId, TaskversionId HAVING COUNT(TaskversionId)= (SELECT COUNT(TaskversionId) FROM dbo.TranscriptSkillsDN t   WHERE Knowledge_Cert_Status = 1    AND Knowledge_Status_Code = 5 and CompanyId   IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=6))) " },
            {Constants.SUPERVISOR_USER, " u.User_Id = @userId" },
            { Constants.IS_SHARED, " r.IsShared"},
            { Constants.ROLE_ID, " r.Id"},
            { Constants.ROLES, " r.Id IN (Select * from dbo.fnSplit_RB(@roles))"},
            { Constants.COMPANIES, " cy.Id IN (Select * from dbo.fnSplit_RB(@companies))"}
        };



        /// <summary>
        ///     Dictionary having list of tables that requried to get the columns for task(s) result
        /// </summary>
        private readonly Dictionary<string, List<string>> tableJoins = new Dictionary<string, List<string>>()
        {
             { " FROM dbo.Task t LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=t.Id  LEFT JOIN dbo.TaskSkill ts ON ts.TaskVersionId=tv.Id LEFT JOIN dbo.SkillActivity sa ON sa.SkillId=ts.SkillId    LEFT JOIN dbo.SCOStatus ss ON ss.status=sa.Status LEFT JOIN dbo.SkillActivityMetrics sam ON sam.SkillActivityId=sa.Id   LEFT JOIN dbo.WorkBookContent wbc ON wbc.EntityId=t.Id    LEFT JOIN dbo.UserWorkBook uwb ON uwb.WorkbookId=wbc.WorkbookId   LEFT JOIN dbo.[UserDetails_RB] u on u.User_Id=uwb.UserId LEFT JOIN dbo.Usercompany uc on uc.userId=u.User_Id AND uc.IsDefault=1 AND uc.IsEnabled=1" , new List<string> {Constants.DATE_EXPIRED, Constants.DATE_TAKEN, Constants.CITY, Constants.STATE, Constants.ZIP, Constants.COUNTRY, Constants.IP, Constants.SCORE, Constants.DURATION, Constants.EVALUATOR_NAME, Constants.ASSIGNED_TO, Constants.COMPLETION_DATE, Constants.LAST_ATTEMPT_DATE, Constants.NUMBER_OF_ATTEMPTS, Constants.EXPIRATION_DATE, Constants.EVALUATOR_NAME , Constants.CREATED_BY, Constants.DELETED_BY, Constants.WORKBOOK_ID, Constants.EVALUATOR_NAME, Constants.STATUS } },

              { " FROM dbo.Task t LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=t.Id  JOIN dbo.CourseAssignment ca on ca.TaskversionId=tv.Id    JOIN  dbo.UserRole ur on ur.UserId=ca.UserId    JOIN TranscriptSkillsDN ts on ts.User_Id=ur.UserId  JOIN dbo.Role r on r.Id=ur.roleId   JOIN dbo.UserCompany uc on uc.UserId=ur.UserId    AND  uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 AND uc.IsDefault=1 LEFT JOIN dbo.[UserDetails_RB] u on u.User_Id=uc.UserId  LEFT JOIN dbo.CompanyClient cc ON uc.CompanyId=cc.OwnerCompany AND cc.IsEnabled=1 AND cc.ClientCompany!=uc.CompanyId   JOIN dbo.Company cy on cy.Id=uc.CompanyId AND cy.IsEnabled=1 " , new List<string> {Constants.COURSE_EXPIRATION_DATE} },

              { " FROM dbo.Task t LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=t.Id  JOIN dbo.CourseAssignment ca on ca.TaskversionId=tv.Id   JOIN  dbo.UserRole ur on ur.UserId=ca.UserId   JOIN dbo.Role r on r.Id=ur.roleId   JOIN dbo.UserCompany uc on uc.UserId=ur.UserId    AND  uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 AND uc.IsDefault=1 LEFT JOIN dbo.[UserDetails_RB] u on u.User_Id=uc.UserId  LEFT JOIN dbo.CompanyClient cc ON uc.CompanyId=cc.OwnerCompany AND cc.IsEnabled=1 AND cc.ClientCompany!=uc.CompanyId   JOIN dbo.Company cy on cy.Id=uc.CompanyId AND cy.IsEnabled=1 " , new List<string> {Constants.COMPLETED_COMPANY_USERS, Constants.NOT_COMPLETED_COMPANY_USERS, Constants.TOTAL_COMPLETED_COMPANY_USERS, Constants.ASSIGNED_DATE } },

             { " FROM dbo.Usercompany uc LEFT JOIN dbo.CompanyClient cc ON uc.CompanyId=cc.OwnerCompany AND cc.IsEnabled=1 AND cc.ClientCompany!=uc.CompanyId AND  uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 AND uc.IsDefault=1 FULL OUTER JOIN dbo.[UserDetails_RB] u ON u.User_Id=uc.UserId FULL OUTER JOIN dbo.Supervisor s ON s.userId=u.User_Id  AND IsDirectReport=1   LEFT JOIN dbo.Company cy ON cy.Id=uc.CompanyId AND cy.IsEnabled=1 " , new List<string> {Constants.ASSIGNED_QUALIFICATION, Constants.COMPLETED_QUALIFICATION, Constants.IN_DUE_QUALIFICATION, Constants.PAST_DUE_QUALIFICATION, Constants.IN_COMPLETE_QUALIFICATION,   Constants.LOCK_OUT_REASON } },

               { "  FROM dbo.DashboardReportDN dr  LEFT JOIN  dbo.Usercompany uc on uc.userId=uc.userId  AND  uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 AND uc.IsDefault=1 AND userid=@userId  LEFT JOIN dbo.[UserDetails_RB] u on u.User_Id=uc.UserId   LEFT JOIN dbo.CompanyClient cc ON uc.CompanyId=cc.OwnerCompany AND cc.IsEnabled=1 AND cc.ClientCompany!=uc.CompanyId  LEFT JOIN dbo.Company cy ON cy.Id=cc.ClientCompany AND cy.IsEnabled=1 WHERE dr.User_Id IN (SELECT @userId UNION select sr.userId from Supervisor sr WHERE  sr.SupervisorId=@userId AND sr.IsDirectReport=1 AND sr.IsEnabled = 1) and  uc.CompanyId=@companyId GROUP BY u.User_Id,cy.Name, cy.Id" , new List<string> { Constants.ASSIGNED_COMPANY_QUALIFICATION, Constants.COMPLETED_COMPANY_QUALIFICATION, Constants.IN_COMPLETE_COMPANY_QUALIFICATION, Constants.PAST_DUE_COMPANY_QUALIFICATION, Constants.IN_DUE_COMPANY_QUALIFICATION, Constants.TOTAL_COMPANY_EMPLOYEES } },

                 { "  FROM   dbo.UserRole ur       JOIN dbo.Role r  ON r.id = ur.roleid       JOIN dbo.UserCompany uc         ON uc.userid = ur.userid            AND  uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 AND uc.IsDefault=1   JOIN dbo.CompanyClient cc ON uc.CompanyId=cc.OwnerCompany    JOIN dbo.Company c ON c.Id = cc.ClientCompany WHERE uc.IsDefault=1     AND uc.IsEnabled=1        AND cc.IsEnabled=1    and c.IsEnabled=1    AND cc.ClientCompany!=uc.CompanyId AND  uc.companyid IN ( @companyId )       AND ( r.isshared = '1' ) 	   Group by ur.userId, ur.RoleId, r.name, r.Id" , new List<string> {      Constants.COMPLETED_ROLE_QUALIFICATION, Constants.NOT_COMPLETED_ROLE_QUALIFICATION, Constants.IS_SHARED } }



        };
    }
}
