﻿using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Helpers;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Logger;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ReportBuilderAPI.Repository
{
    /// <summary>
    ///     Class that handles to read task(s) details for specific user and workbook from database
    /// </summary>
    public class TaskRepository : DatabaseWrapper, ITask
    {

        /// <summary>
        /// Create the query to depends upon the request
        /// </summary>
        /// <param name="queryRequest"></param>
        /// <returns></returns>
        public string CreateTaskQuery(QueryBuilderRequest queryBuilderRequest)
        {
            string query = string.Empty, tableJoin = string.Empty, selectQuery = string.Empty, whereQuery = string.Empty, supervisorId = string.Empty, dashboardName = string.Empty, adminId = string.Empty;
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

                //Remove student details from the List
                if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.STUDENT_DETAILS).ToList().Count > 0)
                {
                    EmployeeModel userDetails = queryBuilderRequest.Fields.Where(x => x.Name == Constants.STUDENT_DETAILS).FirstOrDefault();
                    queryBuilderRequest.Fields.Remove(userDetails);
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

                query = query.Replace("@dashboard", queryBuilderRequest.AppType);
                if (queryBuilderRequest.AppType.ToUpper() == Constants.OQ_DASHBOARD && queryBuilderRequest.ColumnList.Contains(Constants.ASSIGNED_QUALIFICATION))
                {
                    query += " GROUP BY u.User_id, u.full_name_format1, cy.id  ";
                }
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
            int companyId = 0, reportId = 0, userId = 0;
            string contractorCompanyId = string.Empty, adminId = string.Empty;
            string status = string.Empty;
            try
            {
                if (queryBuilderRequest.CompanyId == 0)
                {
                    throw new ArgumentException("CompanyId");
                }

                if (queryBuilderRequest.UserId == 0)
                {
                    throw new ArgumentException("UserId");
                }

                if (queryBuilderRequest.Payload == null)
                {
                    throw new ArgumentException("Fields and ColumnList");
                }

                if (queryBuilderRequest.Payload.Fields == null)
                {
                    throw new ArgumentException("Fields ");
                }

                if (queryBuilderRequest.Payload.ColumnList == null)
                {
                    throw new ArgumentException("Columns");
                }

                //Assign the request details to corresponding objects
                companyId = Convert.ToInt32(queryBuilderRequest.CompanyId);
                userId = Convert.ToInt32(queryBuilderRequest.UserId);
                queryBuilderRequest = queryBuilderRequest.Payload;

                //Assign the companyId to the new object
                queryBuilderRequest.CompanyId = companyId;
                queryBuilderRequest.UserId = userId;
                parameterList = ParameterHelper.Getparameters(queryBuilderRequest);

                if (queryBuilderRequest.AppType == Constants.TRAINING_DASHBOARD)
                {
                    adminId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.ADMIN_ID).Select(x => x.Value).FirstOrDefault();
                    adminId = !string.IsNullOrEmpty(adminId) ? adminId : "0";

                    if (queryBuilderRequest.ColumnList.Contains(Constants.COMPLETED_ROLE_QUALIFICATION))
                    {
                        reportId = ExecuteScalar("SELECT Id FROM Reporting_Dashboard WHERE DashboardName='TRAINING_DASHBOARD'");
                        query = "EXEC  dbo.ContractorManagement_TaskProfile_GetRoleStatusByRole @operatorCompanyId =" + companyId + " , @reportId = " + reportId + " , @adminId = " + adminId;
                    }

                    else if (queryBuilderRequest.ColumnList.Contains(Constants.COMPLETED_COMPANY_USERS))
                    {

                        string tag = queryBuilderRequest.Fields.Where(x => x.Name == Constants.STATUS).Select(y => y.Value).FirstOrDefault();
                        status = tag == Constants.COMPLETED_COMPANY_USERS ? "1" : tag == Constants.NOT_COMPLETED_COMPANY_USERS ? "0" : "null";
                        reportId = ExecuteScalar("SELECT Id FROM Reporting_Dashboard WHERE DashboardName='TRAINING_DASHBOARD'");
                        query = "EXEC dbo.ContractorManagement_TaskProfile_GetRoleStatusByCompany @operatorCompanyId = " + companyId + ", @reportId = " + reportId + ", @parentRoleId =" + parameterList["role"].ToString() + ", @completionStatus=" + status + " , @adminId = " + adminId;
                    }

                    else if (queryBuilderRequest.ColumnList.Contains(Constants.ASSIGNED_COMPANY_QUALIFICATION))
                    {
                        string tag = queryBuilderRequest.Fields.Where(x => x.Name == Constants.STATUS).Select(y => y.Value).FirstOrDefault();
                        status = tag == Constants.COMPLETED ? "1" : tag == Constants.IN_COMPLETE ? "0" : "null";
                        contractorCompanyId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.CONTRACTOR_COMPANY).Select(x => x.Value).FirstOrDefault();
                        contractorCompanyId = !string.IsNullOrEmpty(contractorCompanyId) ? contractorCompanyId : "0";
                        reportId = ExecuteScalar("SELECT Id FROM Reporting_Dashboard WHERE DashboardName='TRAINING_DASHBOARD'");
                        query = " EXEC dbo.ContractorManagement_TaskProfile_GetRoleStatusByUser @operatorCompanyId = " + companyId + ", @reportId = " + reportId + " , @parentRoleId = " + parameterList["role"].ToString() + ", @contractorCompanyId = " + contractorCompanyId + ", @completionStatus = " + status + " , @adminId = " + adminId;
                    }
                    else
                    {
                        string tag = queryBuilderRequest.Fields.Where(x => x.Name == Constants.STATUS).Select(y => y.Value).FirstOrDefault();
                        status = tag == Constants.COMPLETED ? "1" : tag == Constants.IN_COMPLETE ? "0" : "null";
                        contractorCompanyId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.CONTRACTOR_COMPANY).Select(x => x.Value).FirstOrDefault();
                        contractorCompanyId = !string.IsNullOrEmpty(contractorCompanyId) ? contractorCompanyId : "0";
                        reportId = ExecuteScalar("SELECT Id FROM Reporting_Dashboard WHERE DashboardName='TRAINING_DASHBOARD'");
                        query = "EXEC dbo.ContractorManagement_TaskProfile_GetRoleStatusByTask @operatorCompanyId = " + companyId + ", @reportId = " + reportId + " , @parentRoleId  = " + parameterList["role"].ToString() + ", @contractorCompanyId = " + contractorCompanyId + ", @taskCompletionStatus = " + status + ", @contractorEmployeeId = " + parameterList["userId"].ToString() + " , @adminId = " + adminId;
                    }
                }
                else if (queryBuilderRequest.AppType == Constants.OQ_DASHBOARD)
                {
                    query = OQDashboardQuery(parameterList, queryBuilderRequest, companyId);
                }
                else
                {
                    //Generates the query
                    query = CreateTaskQuery(queryBuilderRequest);
                }
                //Get the parameters  to send into the sql query


                taskResponse.Tasks = ReadTaskDetails(query, parameterList, queryBuilderRequest);
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
                taskResponse.Error = new ExceptionHandler(getEmployeeDetails).ExceptionResponse();
                return taskResponse;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterList"></param>
        public string OQDashboardQuery(Dictionary<string, string> parameterList, QueryBuilderRequest queryBuilderRequest, int companyId)
        {
            string query = string.Empty, userId = string.Empty, contractorCompanyId = string.Empty, role = string.Empty, adminId = string.Empty, studentId = string.Empty;
            try
            {
                userId = Convert.ToString(parameterList["userId"]);
                role = Convert.ToString(parameterList["roles"]);
                if (string.IsNullOrEmpty(userId))
                {
                    userId = queryBuilderRequest.UserId.ToString();
                }

                if (queryBuilderRequest.ColumnList.Contains(Constants.ASSIGNED_COMPANY_QUALIFICATION))
                {
                    query = "EXEC  dbo.ContractorManagement_QualsDashboard_GetAllContractors   @operatorCompanyId   =" + companyId + " , @viewedByUserId   = " + userId;
                }
                if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.ASSIGNED).Select(y => y.Name).FirstOrDefault() == Constants.ASSIGNED)
                {
                    adminId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.ADMIN_ID).Select(x => x.Value).FirstOrDefault();
                    contractorCompanyId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.CONTRACTOR_COMPANY).Select(x => x.Value).FirstOrDefault();
                    contractorCompanyId = !string.IsNullOrEmpty(contractorCompanyId) ? contractorCompanyId : "0";
                    studentId = userId;

                    if (string.IsNullOrEmpty(adminId))
                    {
                        studentId = "null";
                        adminId = userId;
                    }

                    query = "EXEC  dbo.ContractorManagement_QualsDashboard_GetAssignedQualifications  @viewedByUserId = " + adminId + ", @studentId  = " + studentId + ",     @contractorCompanyId = " + contractorCompanyId + ", @operatorCompanyId = " + companyId;
                }

                if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.COMPLETED).Select(y => y.Name).FirstOrDefault() == Constants.COMPLETED)
                {
                    adminId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.ADMIN_ID).Select(x => x.Value).FirstOrDefault();
                    contractorCompanyId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.CONTRACTOR_COMPANY).Select(x => x.Value).FirstOrDefault();
                    contractorCompanyId = !string.IsNullOrEmpty(contractorCompanyId) ? contractorCompanyId : "0";

                    studentId = userId;
                    if (string.IsNullOrEmpty(adminId))
                    {
                        studentId = "null";
                        adminId = userId;
                    }

                    query = "EXEC  dbo.ContractorManagement_QualsDashboard_GetActiveQualifications   @viewedByUserId = " + adminId + ",   @studentId  = " + studentId + ",     @contractorCompanyId = " + contractorCompanyId + ", @operatorCompanyId = " + companyId;
                }


                if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.LOCKOUT_COUNT).Select(y => y.Name).FirstOrDefault() == Constants.LOCKOUT_COUNT)
                {
                    adminId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.ADMIN_ID).Select(x => x.Value).FirstOrDefault();
                    studentId = userId;
                    if (string.IsNullOrEmpty(adminId))
                    {
                        studentId = "null";
                        adminId = userId;
                    }
                    contractorCompanyId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.CONTRACTOR_COMPANY).Select(x => x.Value).FirstOrDefault();
                    contractorCompanyId = !string.IsNullOrEmpty(contractorCompanyId) ? contractorCompanyId : "0";
                    query = "EXEC  dbo.ContractorManagement_QualsDashboard_GetLockedQualifications	  @viewedByUserId = " + adminId + ",  @studentId  = " + studentId + ",     @contractorCompanyId = " + contractorCompanyId + ", @operatorCompanyId = " + companyId;


                }

                if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.IN_DUE).Select(y => y.Name).FirstOrDefault() == Constants.IN_DUE)
                {
                    adminId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.ADMIN_ID).Select(x => x.Value).FirstOrDefault();
                    contractorCompanyId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.CONTRACTOR_COMPANY).Select(x => x.Value).FirstOrDefault();
                    contractorCompanyId = !string.IsNullOrEmpty(contractorCompanyId) ? contractorCompanyId : "0";
                    studentId = userId;
                    if (string.IsNullOrEmpty(adminId))
                    {
                        studentId = "null";
                        adminId = userId;
                    }
                    query = "EXEC  dbo.ContractorManagement_QualsDashboard_GetExpiringQualifications   @viewedByUserId = " + adminId + ", @studentId  = " + studentId + ",     @contractorCompanyId = " + contractorCompanyId + ", @operatorCompanyId = " + companyId + ", @expiringInDaysStart = 0, @expiringInDaysEnd = " + parameterList["duedays"].ToString();
                }


                if (queryBuilderRequest.ColumnList.Contains(Constants.ASSIGNED_QUALIFICATION))
                {
                    contractorCompanyId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.CONTRACTOR_COMPANY).Select(x => x.Value).FirstOrDefault();
                    contractorCompanyId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.CONTRACTOR_COMPANY).Select(x => x.Value).FirstOrDefault();
                    contractorCompanyId = !string.IsNullOrEmpty(contractorCompanyId) ? contractorCompanyId : "0";
                    query = "EXEC  dbo.ContractorManagement_QualsDashboard_GetContractor  @viewedByUserId = " + userId + ", @contractorCompanyId = " + contractorCompanyId + ", @operatorCompanyId = " + companyId;
                }

                if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.IN_COMPLETE).Select(y => y.Name).FirstOrDefault() == Constants.IN_COMPLETE)
                {
                    adminId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.ADMIN_ID).Select(x => x.Value).FirstOrDefault();
                    studentId = userId;
                    if (string.IsNullOrEmpty(adminId))
                    {
                        studentId = "null";
                        adminId = userId;
                    }
                    contractorCompanyId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.CONTRACTOR_COMPANY).Select(x => x.Value).FirstOrDefault();
                    contractorCompanyId = !string.IsNullOrEmpty(contractorCompanyId) ? contractorCompanyId : "0";
                    query = "EXEC  dbo.ContractorManagement_QualsDashboard_GetDisqualifications   @viewedByUserId = " + adminId + ",  @studentId  = " + studentId + ",     @contractorCompanyId = " + contractorCompanyId + ", @operatorCompanyId = " + companyId;
                }

                if (!string.IsNullOrEmpty(query) && !string.IsNullOrEmpty(role))
                {
                    query += " , @parentRoleId = " + role;
                }
                if (!string.IsNullOrEmpty(query) && string.IsNullOrEmpty(role))
                {
                    query += " , @parentRoleId = null";
                }
                return query;
            }
            catch (Exception queryException)
            {
                LambdaLogger.Log(queryException.ToString());
                return query;
            }
        }



        /// <summary>
        ///  Creating response object after reading task(s) details from database
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns>TaskModel</returns>
        public List<TaskModel> ReadTaskDetails(string query, Dictionary<string, string> parameters, QueryBuilderRequest queryBuilderRequest)
        {
            List<TaskModel> taskList = new List<TaskModel>();
            try
            {
                System.Data.SqlClient.SqlParameter[] _parameters = ParameterHelper.CreateSqlParameter(parameters);
                //Read the data from the database
                using (IDataReader dataReader = ExecuteDataReader(query, _parameters))
                {
                    if (dataReader != null)
                    {
                        while (dataReader.Read())
                        {
                            DataTable dataTable = dataReader.GetSchemaTable();

                            List<string> columnList = GetColumnList(dataReader);

                            //Get the taskcomment from the payload
                            TaskModel taskComment = columnList.Contains("Comments") ? JsonConvert.DeserializeObject<TaskModel>(Convert.ToString(dataReader["Comments"])) : null;

                            //Get the task details from the database
                            TaskModel taskModel = new TaskModel
                            {
                                TaskId = (dataTable.Select("ColumnName = 'taskId'").Count() == 1) ? (dataReader["taskId"] != DBNull.Value ? (int?)dataReader["taskId"] : 0) : (dataTable.Select("ColumnName = 'Task_Id'").Count() == 1) ? (dataReader["Task_Id"] != DBNull.Value ? (int?)dataReader["Task_Id"] : 0) : null,

                                TaskName = (dataTable.Select("ColumnName = 'taskName'").Count() == 1) ? Convert.ToString(dataReader["taskName"]) : (dataTable.Select("ColumnName = 'Task_Name'").Count() == 1) ? Convert.ToString(dataReader["Task_Name"]) : null,

                                AssignedTo = (dataTable.Select("ColumnName = 'assignee'").Count() == 1) ? Convert.ToString(dataReader["assignee"]) : null,
                                EvaluatorName = (dataTable.Select("ColumnName = 'evaluatorName'").Count() == 1) ? Convert.ToString(dataReader["evaluatorName"]) : null,

                                Status = (dataTable.Select("ColumnName = 'status'").Count() == 1) ? Convert.ToString(dataReader["status"]) : (dataTable.Select("ColumnName = 'Task_Status'").Count() == 1) ? Convert.ToString(dataReader["Task_Status"]) : null,

                                ExpirationDate = (dataTable.Select("ColumnName = 'Date_Expired'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["Date_Expired"])) ? Convert.ToDateTime(dataReader["Date_Expired"]).ToString("MM/dd/yyyy") : null : (dataTable.Select("ColumnName = 'Task_Expiration_Date'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["Task_Expiration_Date"])) ? Convert.ToDateTime(dataReader["Task_Expiration_Date"]).ToString("MM/dd/yyyy") : null : null,


                                TaskCode = (dataTable.Select("ColumnName = 'Code'").Count() == 1) ? Convert.ToString(dataReader["Code"]) : (dataTable.Select("ColumnName = 'Task_Code'").Count() == 1) ? Convert.ToString(dataReader["Task_Code"]) : null,

                                CompletedTasksCount = (dataTable.Select("ColumnName = 'CompletedTasks'").Count() == 1) ? (dataReader["CompletedTasks"] != DBNull.Value ? (int?)dataReader["CompletedTasks"] : 0) : null,

                                TotalTasks = (dataTable.Select("ColumnName = 'TotalTasks'").Count() == 1) ? (dataReader["TotalTasks"] != DBNull.Value ? (int?)dataReader["TotalTasks"] : 0) : null,

                                IncompletedTasksCount = (dataTable.Select("ColumnName = 'InCompleteTask'").Count() == 1) ? (dataReader["InCompleteTask"] != DBNull.Value ? (int?)dataReader["InCompleteTask"] : 0) : null,

                                UserId = (dataTable.Select("ColumnName = 'userId'").Count() == 1) ? (dataReader["userId"] != DBNull.Value ? (int?)dataReader["userId"] : 0) : (dataTable.Select("ColumnName = 'Contractor_User_Id'").Count() == 1) ? (dataReader["Contractor_User_Id"] != DBNull.Value ? (int?)dataReader["Contractor_User_Id"] : 0) : null,

                                WorkbookId = (dataTable.Select("ColumnName = 'workbookId'").Count() == 1) ? (dataReader["workbookId"] != DBNull.Value ? (int?)dataReader["workbookId"] : 0) : null,

                                NumberofAttempts = (dataTable.Select("ColumnName = 'Attempt'").Count() == 1) ? Convert.ToString(dataReader["Attempt"]) : null,

                                LastAttemptDate = (dataTable.Select("ColumnName = 'lastAttemptDate'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["lastAttemptDate"])) ? Convert.ToDateTime(dataReader["lastAttemptDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,
                                Location = (dataTable.Select("ColumnName = 'location'").Count() == 1) ? Convert.ToString(dataReader["location"]) : null,
                                Comments = (dataTable.Select("ColumnName = 'Comments'").Count() == 1) ? taskComment != null ? taskComment.Comment : string.Empty : null,

                                EmployeeName = (dataTable.Select("ColumnName = 'employeeName'").Count() == 1) ? Convert.ToString(dataReader["employeeName"]) : (dataTable.Select("ColumnName = 'Contractor_Full_Name'").Count() == 1) ? Convert.ToString(dataReader["Contractor_Full_Name"]) : null,

                                Role = (dataTable.Select("ColumnName = 'role'").Count() == 1) ? Convert.ToString(dataReader["role"]) : (dataTable.Select("ColumnName = 'Parent_Role_Name'").Count() == 1) ? Convert.ToString(dataReader["Parent_Role_Name"]) : (dataTable.Select("ColumnName = 'Job_Title'").Count() == 1) ? Convert.ToString(dataReader["Job_Title"]) : null,

                                EmployeeRole = (dataTable.Select("ColumnName = 'Job_Title'").Count() == 1) ? Convert.ToString(dataReader["Job_Title"]) : null,

                                AssignedQualification = (dataTable.Select("ColumnName = 'AssignedQualification'").Count() == 1) ? (dataReader["AssignedQualification"] != DBNull.Value ? (int?)dataReader["AssignedQualification"] : 0) : (dataTable.Select("ColumnName = 'Assignment_count'").Count() == 1) ? (dataReader["Assignment_count"] != DBNull.Value ? (int?)dataReader["Assignment_count"] : 0) : null,

                                UserName = (dataTable.Select("ColumnName = 'Contractor_User_Name'").Count() == 1) ? Convert.ToString(dataReader["Contractor_User_Name"]) : null,

                                CompletedQualification = (dataTable.Select("ColumnName = 'CompletedQualification'").Count() == 1) ? (dataReader["CompletedQualification"] != DBNull.Value ? (int?)dataReader["CompletedQualification"] : 0) : (dataTable.Select("ColumnName = 'Qualification_Count'").Count() == 1) ? (dataReader["Qualification_Count"] != DBNull.Value ? (int?)dataReader["Qualification_Count"] : 0) : null,

                                IncompleteQualification = (dataTable.Select("ColumnName = 'IncompleteQualification'").Count() == 1) ? (dataReader["IncompleteQualification"] != DBNull.Value ? (int?)dataReader["IncompleteQualification"] : 0) : null,

                                CompletedUserQualification = (dataTable.Select("ColumnName = 'Task_Completion_Count'").Count() == 1) ? (dataReader["Task_Completion_Count"] != DBNull.Value) ? (dataTable.Select("ColumnName = 'Task_Status'").Count() == 1) ? Convert.ToString(dataReader["Task_Status"]).ToUpper() == Constants.QUALIFIED ? (int?)dataReader["Task_Completion_Count"] : null : null : null : null,

                                RoleStatus = (dataTable.Select("ColumnName = 'Completion_Status'").Count() == 1) ? Convert.ToString(dataReader["Completion_Status"]) : null,


                                TaskStatus = (dataTable.Select("ColumnName = 'Task_Status'").Count() == 1) ? Convert.ToString(dataReader["Task_Status"]) : null,

                                IncompleteUserQualification = (dataTable.Select("ColumnName = 'Task_Completion_Count'").Count() == 1) ? (dataReader["Task_Completion_Count"] != DBNull.Value) ? (dataTable.Select("ColumnName = 'Task_Status'").Count() == 1) ? Convert.ToString(dataReader["Task_Status"]).ToUpper() == Constants.NOT_QUALIFIED ? (int?)dataReader["Task_Completion_Count"] : null : null : null : null,


                                PastDueQualification = (dataTable.Select("ColumnName = 'PastDueQualification'").Count() == 1) ? (dataReader["PastDueQualification"] != DBNull.Value ? (int?)dataReader["PastDueQualification"] : 0) : null,


                                UnlockDate = (dataTable.Select("ColumnName = 'Date_Unlocked'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["Date_Unlocked"])) ? Convert.ToDateTime(dataReader["Date_Unlocked"]).ToString("MM/dd/yyyy") : null : null,

                                InDueQualification = (dataTable.Select("ColumnName = 'InDueQualification'").Count() == 1) ? (dataReader["InDueQualification"] != DBNull.Value ? (int?)dataReader["InDueQualification"] : 0) : (dataTable.Select("ColumnName = 'Expiration_Count_30'").Count() == 1) ? (dataReader["Expiration_Count_30"] != DBNull.Value ? (int?)dataReader["Expiration_Count_30"] : 0) : null,

                                TotalEmployees = (dataTable.Select("ColumnName = 'TotalEmployees'").Count() == 1) ? (dataReader["TotalEmployees"] != DBNull.Value ? (int?)dataReader["TotalEmployees"] : 0) : (dataTable.Select("ColumnName = 'User_Count'").Count() == 1) ? (dataReader["User_Count"] != DBNull.Value ? (int?)dataReader["User_Count"] : 0) : null,


                                AssignedDate = (dataTable.Select("ColumnName = 'AssignedDate'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["AssignedDate"])) ? Convert.ToDateTime(dataReader["AssignedDate"]).ToString("MM/dd/yyyy") : null : (dataTable.Select("ColumnName = 'Date_Assigned'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["Date_Assigned"])) ? Convert.ToDateTime(dataReader["Date_Assigned"]).ToString("MM/dd/yyyy") : null : null,

                                QualificationAssignedDate = (dataTable.Select("ColumnName = 'Date'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["Date"])) ? Convert.ToDateTime(dataReader["Date"]).ToString("MM/dd/yyyy") : string.Empty : null,

                                CompanyName = (dataTable.Select("ColumnName = 'companyName'").Count() == 1) ? Convert.ToString(dataReader["companyName"]) : (dataTable.Select("ColumnName = 'Contractor_Company_Name'").Count() == 1) ? Convert.ToString(dataReader["Contractor_Company_Name"]) : (dataTable.Select("ColumnName = 'Company_Name'").Count() == 1) ? Convert.ToString(dataReader["Company_Name"]) : null,

                                CompanyId = (dataTable.Select("ColumnName = 'companyId'").Count() == 1) ? (dataReader["companyId"] != DBNull.Value ? (int?)dataReader["companyId"] : 0) : (dataTable.Select("ColumnName = 'Contractor_Company_Id'").Count() == 1) ? (dataReader["Contractor_Company_Id"] != DBNull.Value ? (int?)dataReader["Contractor_Company_Id"] : 0) : (dataTable.Select("ColumnName = 'Company_Id'").Count() == 1) ? (dataReader["Company_Id"] != DBNull.Value ? (int?)dataReader["Company_Id"] : 0) : null,


                                Score = (dataTable.Select("ColumnName = 'score'").Count() == 1) ? Convert.ToString(dataReader["score"]) : null,
                                Duration = (dataTable.Select("ColumnName = 'Duration'").Count() == 1) ? Convert.ToString(dataReader["Duration"]) : null,


                                CompletedRoleQualification = (dataTable.Select("ColumnName = 'Number_of_Companies'").Count() == 1) ? (dataReader["Number_of_Companies"] != DBNull.Value) ? (dataTable.Select("ColumnName = 'Completion_Status'").Count() == 1) ? Convert.ToString(dataReader["Completion_Status"]).ToUpper() == Constants.COMPLETED ? (int?)dataReader["Number_of_Companies"] : null : null : null : null,

                                InCompletedRoleQualification = (dataTable.Select("ColumnName = 'Number_of_Companies'").Count() == 1) ? (dataReader["Number_of_Companies"] != DBNull.Value) ? (dataTable.Select("ColumnName = 'Completion_Status'").Count() == 1) ? Convert.ToString(dataReader["Completion_Status"]).ToUpper() == Constants.NOT_COMPLETED ? (int?)dataReader["Number_of_Companies"] : null : null : null : null,

                                CompletedCompanyQualification = (dataTable.Select("ColumnName = 'Number_of_Employees'").Count() == 1) ? (dataReader["Number_of_Employees"] != DBNull.Value) ? (dataTable.Select("ColumnName = 'Completion_Status'").Count() == 1) ? Convert.ToString(dataReader["Completion_Status"]).ToUpper() == Constants.COMPLETED ? (int?)dataReader["Number_of_Employees"] : null : null : null : null,

                                InCompletedCompanyQualification = (dataTable.Select("ColumnName = 'Number_of_Employees'").Count() == 1) ? (dataReader["Number_of_Employees"] != DBNull.Value) ? (dataTable.Select("ColumnName = 'Completion_Status'").Count() == 1) ? Convert.ToString(dataReader["Completion_Status"]).ToUpper() == Constants.NOT_COMPLETED ? (int?)dataReader["Number_of_Employees"] : null : null : null : null,


                                SuspendedQualification = (dataTable.Select("ColumnName = 'Manual_Disqualification_Count'").Count() == 1) ? (dataReader["Manual_Disqualification_Count"] != DBNull.Value ? (int?)dataReader["Manual_Disqualification_Count"] : 0) : null,

                                DisQualification = (dataTable.Select("ColumnName = 'Disqualification_Count'").Count() == 1) ? (dataReader["Disqualification_Count"] != DBNull.Value ? (int?)dataReader["Disqualification_Count"] : 0) : null,

                                RoleId = (dataTable.Select("ColumnName = 'roleId'").Count() == 1) ? (dataReader["roleId"] != DBNull.Value ? (int?)dataReader["roleId"] : 0) : (dataTable.Select("ColumnName = 'Parent_Role_Id'").Count() == 1) ? (dataReader["Parent_Role_Id"] != DBNull.Value ? (int?)dataReader["Parent_Role_Id"] : 0) : null,


                                TotalCompanyQualification = (dataTable.Select("ColumnName = 'TotalCompanyUsers'").Count() == 1) ? (dataReader["TotalCompanyUsers"] != DBNull.Value ? (int?)dataReader["TotalCompanyUsers"] : 0) : null,

                                LockoutReason = (dataTable.Select("ColumnName = 'LockoutReason'").Count() == 1) ? Convert.ToString(dataReader["LockoutReason"]) : null,

                                LockoutCount = (dataTable.Select("ColumnName = 'LockOut_Count'").Count() == 1) ? Convert.ToString(dataReader["LockOut_Count"]) : null
                            };


                            if (queryBuilderRequest.AppType == Constants.TRAINING_DASHBOARD)
                            {
                                if (queryBuilderRequest.ColumnList.Contains(Constants.COMPLETED_ROLE_QUALIFICATION))
                                {
                                    TaskModel task = taskList.Where(x => x.Role == taskModel.Role).Select(x => x).FirstOrDefault();
                                    if (task != null)
                                    {
                                        if (taskModel.RoleStatus.ToUpper() == Constants.COMPLETED)
                                        {
                                            task.CompletedRoleQualification = taskModel.CompletedRoleQualification;
                                        }
                                        else
                                        {
                                            task.InCompletedRoleQualification = taskModel.InCompletedRoleQualification;
                                        }
                                    }
                                    else
                                    {
                                        taskList.Add(taskModel);
                                    }
                                }

                                else if (queryBuilderRequest.ColumnList.Contains(Constants.COMPLETED_COMPANY_USERS))
                                {
                                    TaskModel task = taskList.Where(x => x.CompanyName == taskModel.CompanyName).Select(x => x).FirstOrDefault();
                                    if (task != null)
                                    {
                                        if (taskModel.RoleStatus.ToUpper() == Constants.COMPLETED)
                                        {
                                            task.CompletedCompanyQualification = taskModel.CompletedCompanyQualification;
                                        }
                                        else
                                        {
                                            task.InCompletedCompanyQualification = taskModel.InCompletedCompanyQualification;
                                        }
                                    }
                                    else
                                    {
                                        taskList.Add(taskModel);
                                    }
                                }

                                else if (queryBuilderRequest.ColumnList.Contains(Constants.ASSIGNED_COMPANY_QUALIFICATION))
                                {
                                    TaskModel task = taskList.Where(x => x.UserId == taskModel.UserId).Select(x => x).FirstOrDefault();
                                    if (task != null)
                                    {
                                        if (taskModel.TaskStatus.ToUpper() == Constants.QUALIFIED)
                                        {
                                            task.CompletedUserQualification = taskModel.CompletedUserQualification;
                                        }
                                        else
                                        {
                                            task.IncompleteUserQualification = taskModel.IncompleteUserQualification;
                                        }
                                    }
                                    else
                                    {
                                        taskList.Add(taskModel);
                                    }
                                }
                                else
                                {
                                    taskList.Add(taskModel);
                                }
                            }
                            else
                            {
                                taskList.Add(taskModel);
                            }
                        }
                    }
                    else
                    {
                        return null;
                    }
                    return taskList;
                }
            }
            catch (Exception readTaskDetailsException)
            {
                LambdaLogger.Log(readTaskDetailsException.ToString());
                return null;
            }
        }

        public virtual List<string> GetColumnList(IDataReader dataReader)
        {
            List<string> columnList = new List<string>();
            try
            {                
                columnList = dataReader.GetSchemaTable().Rows.Cast<DataRow>().Select(r => (string)r["ColumnName"]).ToList();
                return columnList;
            }
            catch (Exception getColumnListException)
            {
                LambdaLogger.Log(getColumnListException.ToString());
                return columnList;
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
                { Constants.EVALUATOR_NAME, ", (SELECT Full_Name_Format1 FROM dbo.[UserDetails_RB] usr WHERE usr.User_Id=sa.Evaluator AND usr.Is_Enabled = 1) AS evaluatorName"},
                { Constants.ASSIGNED_TO, ",  (SELECT usr.User_Name AS UserName FROM dbo.[UserDetails_RB] usr WHERE usr.User_Id=sa.UserId AND usr.Is_Enabled = 1) AS assignee"},
                { Constants.IP, ", sam.IP"},
                { Constants.LOCATION, ",   concat((CASE WHEN sam.street IS NOT NULL THEN (sam.street + ',') ELSE '' END),   (CASE WHEN sam.City IS NOT NULL THEN (sam.city+ ',') ELSE '' END), (CASE WHEN sam.State IS NOT NULL THEN (sam.State + ',') ELSE '' END),   (CASE WHEN sam.Zip IS NOT NULL THEN (sam.Zip + ',') ELSE '' END),  sam.country) AS location"},
                { Constants.DURATION, ", sam.duration"},
                { Constants.SCORE, ", sam.score"},
                { Constants.DATE_TAKEN, ", sa.DateTaken"},
                { Constants.USERID, ", u.User_Id as userId"},
                { Constants.WORKBOOK_ID, ", wbc.workbookId"},
                { Constants.COMPLETION_DATE, ", u.DateCreated"},
                { Constants.LAST_ATTEMPT_DATE, ", sa.DateTaken AS lastAttemptDate"},
                { Constants.CREATED_BY, ", (SELECT us.User_Name AS UserName FROM dbo.[UserDetails_RB] us WHERE us.User_Id=sa.Createdby AND us.Is_Enabled = 1) AS CreatedBy"},
                { Constants.DELETED_BY, ", (SELECT us.User_Name AS UserName FROM dbo.[UserDetails_RB] us WHERE us.User_Id=sa.deletedby AND us.Is_Enabled = 1) AS DeletedBy"},
                { Constants.PARENT_TASK_NAME, ", u.Email"},
                { Constants.CHILD_TASK_NAME, ", u.City"},
                { Constants.NUMBER_OF_ATTEMPTS, ", sa.Attempt"},
                { Constants.EXPIRATION_DATE, ", sa.DateExpired as Date_Expired"},
                { Constants.COMMENTS, ", sam.Payload AS Comments"},
                { Constants.TASK_CODE, ", t.Code"},

                { Constants.COMPLETED_TASK, ", (SELECT ISNULL((SELECT SUM(wbc.Repetitions) FROM dbo.[UserDetails_RB] us LEFT JOIN dbo.UserWorkBook uwb ON uwb.UserId=us.User_Id AND us.Is_Enabled = 1 LEFT JOIN dbo.WorkBookContent wbc ON wbc.WorkBookId=uwb.WorkBookId LEFT JOIN  dbo.Task tk ON tk.Id=wbc.EntityId LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=tk.Id LEFT JOIN dbo.TaskSkill tss ON tss.TaskVersionId=tv.Id LEFT JOIN dbo.SkillActivity sa ON sa.SkillId=tss.SkillId LEFT JOIN dbo.SCOStatus ss ON ss.status=sa.Status WHERE  us.User_Id IN ((u.User_Id))  AND uwb.IsEnabled=1 AND wbc.WorkBookId=@workbookId AND tk.Id=t.Id AND ss.[desc]='COMPLETED'),0)) AS CompletedTasks"},

                { Constants.INCOMPLETE_TASK, ", (SELECT ISNULL((SELECT COUNT(tk.Id) FROM dbo.[UserDetails_RB] us LEFT JOIN dbo.UserWorkBook uwb ON uwb.UserId=us.User_Id AND us.Is_Enabled = 1 LEFT JOIN dbo.WorkBookContent wbc ON wbc.WorkBookId=uwb.WorkBookId LEFT JOIN  dbo.Task tk ON tk.Id=wbc.EntityId LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=tk.Id LEFT JOIN dbo.TaskSkill tss ON tss.TaskVersionId=tv.Id LEFT JOIN dbo.SkillActivity sa ON sa.SkillId=tss.SkillId LEFT JOIN dbo.SCOStatus ss ON ss.status=sa.Status WHERE  u.User_Id IN ((u.User_Id))  AND uwb.IsEnabled=1 AND tk.Id=t.Id AND wbc.WorkBookId=@workbookId AND ss.[desc]='FAILED'),0))  AS InCompleteTask"},

                { Constants.TOTAL_TASK, ", (SELECT ISNULL((SELECT SUM(wbc.Repetitions) FROM dbo.[UserDetails_RB] us LEFT JOIN dbo.UserWorkBook uwb ON  uwb.UserId=us.User_Id AND us.Is_Enabled = 1 LEFT JOIN dbo.WorkBookContent wbc ON wbc.WorkBookId=uwb.WorkBookId LEFT JOIN  dbo.Task tk ON tk.Id=wbc.EntityId LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=tk.Id WHERE  us.User_Id IN ((u.User_Id))  AND uwb.IsEnabled=1 AND wbc.WorkBookId=@workbookId AND tk.Id=t.Id),0)) AS TotalTasks"},

                { Constants.ASSIGNED_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT( taskversionId) FROM dbo.courseAssignment cs  WHERE UserId =u.User_Id AND cs.CompanyId=@companyId AND cs.IsEnabled = 1 AND cs.Status = 0 AND cs.IsCurrent = 1 ) ,0)) AS  AssignedQualification" },

                { Constants.COMPLETED_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT( taskId) FROM dbo.TranscriptSkillsDN ts WHERE Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5 AND ts.IsEnabled = 1 AND Date_Knowledge_Cert_Expired > GETDATE() AND ts.User_Id = u.User_Id AND ts.CompanyId=@companyId  ) ,0)) AS  CompletedQualification" },

                 { Constants.DISQUALIFIED_QUALIFICATION,", Sum(dr.Disqual_Count)    AS  DisqualifiedQualification" },


                 { Constants.SUSPENDED_QUALIFICATION,", 0 AS  SuspendedQualification" },


                { Constants.IN_COMPLETE_QUALIFICATION,",  (SELECT ISNULL((SELECT COUNT( taskId) FROM dbo.TranscriptSkillsDN ts WHERE  Knowledge_Cert_Status IN (3, 2,0,4, 255) AND  ts.User_Id = u.User_Id AND ts.CompanyId=@companyId AND ts.IsEnabled = 1 ) ,0)) AS IncompleteQualification" },

                { Constants.PAST_DUE_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT( taskversionId) FROM dbo.courseAssignment cs  WHERE UserId = u.User_Id AND cs.IsEnabled = 1 AND cs.Status = 0 AND cs.IsCurrent = 1 AND cs.ExpirationDate < GETDATE() AND  ABS(DATEDIFF(DAY,GETDATE(),cs.ExpirationDate))<=@duedays AND cs.CompanyId=@companyId AND cs.FirstAccessed IS NULL ) ,0)) AS PastDueQualification" },

                { Constants.IN_DUE_QUALIFICATION,", (SELECT ISNULL((SELECT COUNT( taskversionId) FROM dbo.courseAssignment cs  WHERE UserId = u.User_Id AND cs.IsEnabled = 1 AND cs.Status = 0 AND cs.IsCurrent = 1 AND cs.ExpirationDate > GETDATE() AND  ABS(DATEDIFF(DAY,GETDATE(),cs.ExpirationDate))<=@duedays AND cs.CompanyId=@companyId AND cs.FirstAccessed IS NULL ) ,0)) AS InDueQualification" },

                { Constants.TOTAL_EMPLOYEES,", (SELECT COUNT(ss.UserId) FROM dbo.Supervisor ss LEFT JOIN dbo.UserCompany uc ON uc.UserId = ss.UserId AND uc.IsEnabled = 1 AND IsDirectReport=1 AND uc.IsVisible = 1 AND uc.Status = 1 WHERE ss.SupervisorId = u.User_Id AND uc.CompanyId = @companyId AND ss.IsEnabled = 1 ) AS TotalEmployees" },

                { Constants.ASSIGNED_COMPANY_QUALIFICATION,", SUM(dr.Assigned_Qual_Count) AS AssignedQualification" },

                { Constants.COMPLETED_COMPANY_QUALIFICATION,", SUM(dr.Qual_Count) as CompletedQualification" },

                { Constants.IN_COMPLETE_COMPANY_QUALIFICATION,",  SUM(dr.Disqual_Count)   AS IncompleteQualification" },

                { Constants.PAST_DUE_COMPANY_QUALIFICATION,", 0 AS PastDueQualification" },

                { Constants.IN_DUE_COMPANY_QUALIFICATION,", SUM(dr.Qual_Count_30) AS InDueQualification" },

                { Constants.TOTAL_COMPANY_EMPLOYEES,",  (SELECT COUNT(ss.UserId) FROM  dbo.Supervisor ss  LEFT JOIN dbo.UserCompany uc ON uc.UserId = ss.UserId AND uc.IsEnabled = 1 AND IsDirectReport=1  AND uc.IsVisible = 1 AND uc.Status = 1    WHERE uc.CompanyId = cy.Id  AND ss.IsEnabled = 1 ) AS TotalEmployees" },

                { Constants.COMPLETED_ROLE_QUALIFICATION,", (SELECT COUNT(CompanyId)FROM dbo.TranscriptSkillsDN WHERE IsEnabled=1 AND  Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5 AND User_ID=ur.UserId) AS CompletedRoleQualification" },

                { Constants.NOT_COMPLETED_ROLE_QUALIFICATION,", (SELECT COUNT(CompanyId)FROM dbo.TranscriptSkillsDN WHERE IsEnabled=1 AND  Knowledge_Cert_Status != 1 AND Knowledge_Status_Code != 5 AND User_ID=ur.UserId) as NotCompletedRoleQualification" },

                { Constants.EMPLOYEE_NAME,", u.Full_Name_Format1  as employeeName" },

                { Constants.COMPLETED_COMPANY_USERS,",  (SELECT ISNULL(( SELECT COUNT(ts.UserId) FROM dbo.CourseAssignment ts  JOIN dbo.UserRole ur ON ur.UserId=ts.UserId WHERE ur.RoleId=@roleId AND companyId IN (Select ClientCompany from dbo.companyClient WHERE ownerCompany=@companyId)  GROUP BY  TaskversionId HAVING COUNT(TaskversionId)= (SELECT COUNT(TaskversionId) FROM dbo.TranscriptSkillsDN t   WHERE Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5  AND t.IsEnabled = 1and CompanyId   IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=@companyId))) ,0)) AS CompletedCompanyUsers" },

                { Constants.NOT_COMPLETED_COMPANY_USERS,", (SELECT ISNULL(( SELECT COUNT(ts.UserId) FROM dbo.CourseAssignment ts JOIN dbo.UserRole ur ON ur.UserId=ts.UserId WHERE ur.RoleId=@roleId AND  companyId IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=@companyId) AND TaskversionId    NOT IN(SELECT TaskversionId FROM dbo.TranscriptSkillsDN t   WHERE Knowledge_Cert_Status = 1 AND Knowledge_Status_Code = 5  AND t.IsEnabled = 1  AND CompanyId IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=@companyId)) ) ,0)) AS NotCompletedCompanyUsers" },

                { Constants.TOTAL_COMPLETED_COMPANY_USERS,",    (SELECT ISNULL(( SELECT COUNT(ts.UserId) FROM dbo.CourseAssignment ts  JOIN dbo.UserRole ur ON ur.UserId=ts.UserId WHERE ur.RoleId=@roleId AND companyId IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=@companyId))  ,0)) AS TotalCompanyUsers" },

                { Constants.ROLE,", (SELECT STUFF((SELECT DISTINCT ', ' + r.Name FROM dbo.UserRole ur JOIN dbo.Role r ON r.Id=ur.roleId  WHERE ur.UserId=u.User_Id FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 2, '')) As Role" },
                {Constants.ROLE_ID, ", r.Id as roleId" },
                {Constants.TRAINING_ROLE, ", r.Name as Role" },
                {Constants.ASSIGNED_DATE, ", ca.DateCreated as AssignedDate " },
                {Constants.LOCK_OUT_REASON, ", ca.LockoutReason as LockoutReason " },
                {Constants.LOCK_OUT_COUNT, ", SUM(dr.Lockout_Count)   AS LockOutCount" },
                { Constants.COURSE_EXPIRATION_DATE, ", ca. ExpirationDate as Date_Expired"},
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
            {Constants.EVALUATOR_NAME, " (SELECT usr.User_Name  AS evaluatorName FROM dbo.[UserDetails_RB] usr WHERE usr.User_Id=sa.Evaluator AND usr.Is_Enabled = 1) " },
            {Constants.CITY, "sam.City" },
            {Constants.USERID, "u.User_Id" },
            {Constants.STATE, "sam.State" },
            {Constants.ZIP, "sam.Zip" },
            {Constants.COUNTRY, "sam.Country"  },
            {Constants.IP, "sam.IP" },
            {Constants.DATECREATED, "CONVERT(DATE,t.DateCreated,101)" },
            {Constants.ISENABLED, "t.isenabled" },
            {Constants.CREATED_BY, " (SELECT us.User_Name AS UserName FROM dbo.[UserDetails_RB] us WHERE us.User_Id=sa.Createdby AND us.Is_Enabled = 1) " },
            {Constants.DELETED_BY, " (SELECT us.User_Name AS UserName FROM dbo.[UserDetails_RB] us WHERE us.User_Id=sa.Deletedby AND us.Is_Enabled = 1) " },
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

            {Constants.NOT_COMPLETED_COMPANY_USERS, "  ca.UserId IN(SELECT USERID FROM dbo.CourseAssignment ts WHERE companyId IN (SELECT ClientCompany FROM      dbo.companyClient WHERE ownerCompany=@companyId) AND TaskversionId    NOT IN(SELECT TaskversionId FROM dbo.TranscriptSkillsDN t WHERE Knowledge_Cert_Status = 1 AND t.IsEnabled = 1 AND Knowledge_Status_Code = 5    and CompanyId IN (Select ClientCompany from dbo.companyClient WHERE ownerCompany=@companyId))) " },
            {Constants.COMPLETED_COMPANY_USERS, "  ca.UserId in(SELECT ts.UserId FROM dbo.CourseAssignment ts WHERE companyId IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=@companyId)    GROUP BY ts.UserId, TaskversionId HAVING COUNT(TaskversionId)= (SELECT COUNT(TaskversionId) FROM dbo.TranscriptSkillsDN t   WHERE Knowledge_Cert_Status = 1    AND t.IsEnabled = 1 AND Knowledge_Status_Code = 5 and CompanyId   IN (SELECT ClientCompany FROM dbo.companyClient WHERE ownerCompany=@companyId))) " },
            {Constants.SUPERVISOR_USER, " u.User_Id = @userId" },
            { Constants.ROLES, " r.Id IN (Select * from dbo.fnSplit_RB(@roles))"},
            { Constants.COMPANIES, " cy.Id IN (Select * from dbo.fnSplit_RB(@companies))"}
        };



        /// <summary>
        ///     Dictionary having list of tables that requried to get the columns for task(s) result
        /// </summary>
        private readonly Dictionary<string, List<string>> tableJoins = new Dictionary<string, List<string>>()
        {
             { " FROM dbo.Task t LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=t.Id  LEFT JOIN dbo.TaskSkill ts ON ts.TaskVersionId=tv.Id LEFT JOIN dbo.SkillActivity sa ON sa.SkillId=ts.SkillId    LEFT JOIN dbo.SCOStatus ss ON ss.status=sa.Status LEFT JOIN dbo.SkillActivityMetrics sam ON sam.SkillActivityId=sa.Id   LEFT JOIN dbo.WorkBookContent wbc ON wbc.EntityId=t.Id    LEFT JOIN dbo.UserWorkBook uwb ON uwb.WorkbookId=wbc.WorkbookId   LEFT JOIN dbo.[UserDetails_RB] u on u.User_Id=uwb.UserId AND u.Is_Enabled = 1 LEFT JOIN dbo.Usercompany uc on uc.userId=u.User_Id AND uc.IsDefault=1 AND uc.IsEnabled=1" , new List<string> {Constants.DATE_EXPIRED, Constants.DATE_TAKEN, Constants.CITY, Constants.STATE, Constants.ZIP, Constants.COUNTRY, Constants.IP, Constants.SCORE, Constants.DURATION, Constants.EVALUATOR_NAME, Constants.ASSIGNED_TO, Constants.COMPLETION_DATE, Constants.LAST_ATTEMPT_DATE, Constants.NUMBER_OF_ATTEMPTS, Constants.EXPIRATION_DATE, Constants.EVALUATOR_NAME , Constants.CREATED_BY, Constants.DELETED_BY, Constants.WORKBOOK_ID, Constants.EVALUATOR_NAME, Constants.STATUS } },

              { " FROM dbo.Task t LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=t.Id  JOIN dbo.CourseAssignment ca on ca.TaskversionId=tv.Id    AND ca.IsEnabled = 1 AND ca.Status = 0 AND ca.IsCurrent = 1 JOIN  dbo.UserRole ur on ur.UserId=ca.UserId  AND ur.IsEnabled = 1   JOIN TranscriptSkillsDN ts on ts.User_Id=ur.UserId AND ts.IsEnabled = 1 JOIN dbo.Role r on r.Id=ur.roleId AND r.IsEnabled = 1  JOIN dbo.UserCompany uc on uc.UserId=ur.UserId    AND uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 AND uc.IsDefault=1 LEFT JOIN dbo.[UserDetails_RB] u on u.User_Id=uc.UserId AND u.Is_Enabled = 1   LEFT JOIN dbo.CompanyClient cc ON uc.CompanyId=cc.OwnerCompany AND cc.IsEnabled=1 AND cc.ClientCompany!=uc.CompanyId   JOIN dbo.Company cy on cy.Id=uc.CompanyId AND cy.IsEnabled=1 " , new List<string> {Constants.COURSE_EXPIRATION_DATE} },

              { " FROM dbo.Task t LEFT JOIN dbo.TaskVersion tv ON tv.TaskId=t.Id  JOIN dbo.CourseAssignment ca on ca.TaskversionId=tv.Id AND ca.IsEnabled = 1 AND ca.Status = 0 AND ca.IsCurrent = 1   JOIN  dbo.UserRole ur on ur.UserId=ca.UserId AND ur.IsEnabled = 1   JOIN dbo.Role r on r.Id=ur.roleId AND r.IsEnabled = 1  JOIN dbo.UserCompany uc on uc.UserId=ur.UserId    AND  uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 AND uc.IsDefault=1 LEFT JOIN dbo.[UserDetails_RB] u on u.User_Id=uc.UserId AND u.Is_Enabled = 1  LEFT JOIN dbo.CompanyClient cc ON uc.CompanyId=cc.OwnerCompany AND cc.IsEnabled=1 AND cc.ClientCompany!=uc.CompanyId   JOIN dbo.Company cy on cy.Id=uc.CompanyId AND cy.IsEnabled=1 " , new List<string> {Constants.COMPLETED_COMPANY_USERS, Constants.NOT_COMPLETED_COMPANY_USERS, Constants.TOTAL_COMPLETED_COMPANY_USERS, Constants.ASSIGNED_DATE } },


             { " FROM   dbo.usercompany uc  JOIN dbo.[userdetails_rb] u  ON u.user_id = uc.userid AND uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 AND uc.IsDefault=1 AND u.is_enabled = 1  JOIN dbo.dashboardreportdn dr ON uc.userid = dr.user_id AND uc.companyid = dr.company_id  JOIN dbo.supervisor s  ON s.userid = u.user_id  AND isdirectreport = 1 AND s.IsEnabled=1 JOIN dbo.company cy ON cy.id = uc.companyid AND cy.isenabled = 1 " , new List<string> {Constants.ASSIGNED_QUALIFICATION, Constants.COMPLETED_QUALIFICATION, Constants.IN_DUE_QUALIFICATION, Constants.PAST_DUE_QUALIFICATION, Constants.IN_COMPLETE_QUALIFICATION,   Constants.LOCK_OUT_REASON } },

               { "FROM (SELECT c.Id AS [company_id] FROM dbo.UserCompany uc  JOIN dbo.CompanyClient cc ON uc.CompanyId=cc.OwnerCompany JOIN dbo.Company c ON c.Id = cc.ClientCompany  WHERE uc.IsDefault=1	AND uc.IsEnabled=1	AND uc.UserId=@userId AND cc.IsEnabled=1	and c.IsEnabled=1  AND cc.ClientCompany!=uc.CompanyId)  As ClientCompanies JOIN dbo.usercompany uc ON uc.companyid = ClientCompanies.[company_id] JOIN dbo.dashboardreportdn dr ON uc.userid = dr.user_id AND uc.companyid = dr.company_id JOIN dbo.company cy ON ClientCompanies.[company_id]=cy.id GROUP  BY cy.NAME, cy.id,uc.companyid		" , new List<string> { Constants.ASSIGNED_COMPANY_QUALIFICATION, Constants.COMPLETED_COMPANY_QUALIFICATION, Constants.IN_COMPLETE_COMPANY_QUALIFICATION, Constants.PAST_DUE_COMPANY_QUALIFICATION, Constants.IN_DUE_COMPANY_QUALIFICATION, Constants.TOTAL_COMPANY_EMPLOYEES } },

                 { "  FROM   Reporting_Dashboard rd JOIN dbo.Reporting_DashboardRole rdr ON rdr. DashboardId=rd.Id JOIN Role r ON r.id=rdr.RoleId AND r.IsEnabled=1 JOIN dbo.UserRole ur ON r.Id = ur.roleid AND ur.isenabled = 1 JOIN dbo.UserCompany uc  ON uc.userid = ur.userid  AND uc.isenabled = 1  AND uc.status = 1 AND uc.isvisible = 1 AND uc.isdefault = 1 JOIN [userdetails_rb] u ON u.user_id = uc.userid AND u.is_enabled = 1 JOIN dbo.companyclient cc ON uc.companyid = cc.ownercompany JOIN dbo.company c ON c.id = cc.clientcompany WHERE  uc.isdefault = 1 AND uc.isenabled = 1  AND cc.isenabled = 1 AND c.isenabled = 1 AND cc.clientcompany != uc.companyid AND uc.companyid IN ( @companyId ) AND DashboardName='@dashboard' GROUP  BY ur.userid, ur.roleid,  r.NAME, r.id, u.User_Id " , new List<string> {      Constants.COMPLETED_ROLE_QUALIFICATION, Constants.NOT_COMPLETED_ROLE_QUALIFICATION, Constants.IS_SHARED } }



        };
    }
}
