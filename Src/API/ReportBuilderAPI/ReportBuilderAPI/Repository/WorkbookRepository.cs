﻿using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using OnBoardLMS.WebAPI.Models;
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
    ///     Class that handles to read workbook(s) details for specific user from database
    /// </summary>
    public class WorkbookRepository : DatabaseWrapper, IWorkbook
    {

        /// <summary>
        ///     Dictionary having Column list for the workbook(s) details.
        ///     Based on column name, query is being formed/updated
        /// </summary>
        private readonly Dictionary<string, string> workbookColumnList = new Dictionary<string, string>()
        {
                { Constants.EMPLOYEE_NAME, ", Full_Name_Format1  AS employeeName" },
                { Constants.WORKBOOK_NAME, ",  wb.name AS workbookName" },
                { Constants.DESCRIPTION, ", wb.Description"},
                { Constants.WORKBOOK_CREATED, ", wb.datecreated"},
                { Constants.WORKBOOK_ISENABLED, ", wb.isEnabled"},
                { Constants.WORKBOOK_ID, ", wb.Id"},
                { Constants.WORKBOOK_CREATED_BY, ", (SELECT us.User_Name AS UserName FROM dbo.[UserDetails_RB] us WHERE us.User_Id=wb.Createdby) AS Createdby"},
                { Constants.DAYS_TO_COMPLETE, ", wb.daystocomplete"},
                { Constants.DUE_DATE, ", DATEADD(DAY, wb.DaysToComplete, wbp.FirstAttemptDate) AS DueDate"},
                { Constants.USERNAME, ", u.User_Name AS UserName"},
                { Constants.USERNAME2, ", u.Alternate_User_Name AS UserName2"},
                { Constants.USER_CREATED_DATE, ", u.Date_Created AS DateCreated"},
                { Constants.FIRSTNAME, ", u.First_Name AS FName"},
                { Constants.MIDDLENAME, ", u.Middle_Name AS MName"},
                { Constants.LASTNAME, ", u.Last_Name AS LName"},
                { Constants.EMAIL, ", u.Email"},
                { Constants.CITY, ", u.City"},
                { Constants.STATE, ", u.State"},
                { Constants.ZIP, ", u.Zip"},
                { Constants.PHONE, ", u.phone"},
                { Constants.USERID, ", u.User_Id as userId"},
                { Constants.ENTITY_COUNT, ", (SELECT COUNT(DISTINCT EntityId) FROM dbo.WorkbookProgress WHERE WorkbookId=wb.Id) AS entityCount"},
                { Constants.USER_COUNT, ", (SELECT COUNT(DISTINCT UserId) FROM dbo.UserWorkbook WHERE WorkbookId=wb.Id) AS userCount"},

                { Constants.ASSIGNED_WORKBOOK, ", (SELECT COUNT(uwt.WorkBookId) AS WorkbookCount FROM dbo.UserWorkBook uwt   INNER  JOIN dbo.UserCompany uc ON uc.UserId= uwt.UserId AND uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1  INNER JOIN dbo.Supervisor s ON uc.UserId = s.UserId AND s.UserId IN  (SELECT u.User_Id UNION SELECT sss.UserID FROM Supervisor sss WHERE sss.SupervisorId=u.User_Id AND SupervisorId <>  @currentuserId AND sss.UserID <> u.User_Id) AND s.IsEnabled = 1 WHERE uc.companyId=@companyId AND uwt.IsEnabled=1  )  AS AssignedWorkbooks"},

                { Constants.PAST_DUE_WORKBOOK, ", (SELECT ISNULL((SELECT COUNT(uwb.WorkBookId) FROM  dbo.WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId  AND uwb.IsEnabled=1 JOIN dbo.Supervisor s ON uwb.UserId = s.UserId AND s.UserId IN (SELECT u.User_Id UNION SELECT UserID FROM Supervisor WHERE SupervisorId=u.User_Id AND SupervisorId <> @currentuserId) AND s.IsEnabled = 1 JOIN dbo.UserCompany uc on uc.UserId= s.UserId   AND uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 JOIN dbo.WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId JOIN dbo.WorkBook wb ON wb.Id=wbt.WorkBookId WHERE uc.companyId=@companyId AND ABS(DATEDIFF(DAY,GETDATE(),CONVERT(DATE,DATEADD(DAY, wb.DaysToComplete, wbs.FirstAttemptDate),101))) < @duedays AND wb.DaysToComplete <= ABS(DATEDIFF(DAY,  GETDATE(), DateAdded)) AND (SELECT SUM(www.NumberCompleted) FROM dbo.WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) < (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbt.WorkBookId)),0)) AS PastDueWorkbooks"},

                { Constants.WORKBOOK_DUE, ", (SELECT ISNULL((SELECT COUNT(uwb.WorkBookId) FROM  dbo.WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId  AND uwb.IsEnabled=1 JOIN dbo.Supervisor s ON uwb.UserId = s.UserId AND s.UserId IN (SELECT u.User_Id UNION SELECT UserID FROM Supervisor WHERE SupervisorId=u.User_Id AND SupervisorId <> @currentuserId) AND s.IsEnabled = 1 JOIN dbo.UserCompany uc on uc.UserId= s.UserId   AND uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 JOIN dbo.WorkBookContent wbt ON wbt.WorkBookId=uwb.WorkBookId JOIN dbo.WorkBook wb ON wb.Id=uwb.WorkBookId WHERE uc.companyId=@companyId AND ABS(DATEDIFF(DAY,GETDATE(),CONVERT(DATE,DATEADD(DAY, wb.DaysToComplete, wbs.FirstAttemptDate),101))) < @duedays AND wb.DaysToComplete >= ABS(DATEDIFF(DAY,  GETDATE(), DateAdded)) AND (SELECT SUM(www.NumberCompleted) FROM dbo.WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) < (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbt.WorkBookId)),0)) AS InDueWorkbooks"},

                { Constants.INCOMPLETE_WORKBOOK, ", (SELECT ISNULL((SELECT COUNT(wbt.EntityId) FROM  dbo.WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId  AND uwb.IsEnabled=1 JOIN dbo.Supervisor s ON uwb.UserId = s.UserId AND s.UserId IN (SELECT u.User_Id UNION SELECT UserID FROM Supervisor WHERE SupervisorId=u.User_Id AND SupervisorId <> @currentuserId) AND s.IsEnabled = 1 JOIN dbo.UserCompany uc on uc.UserId= s.UserId   AND uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 JOIN dbo.WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId JOIN dbo.WorkBook wb ON wb.Id=wbt.WorkBookId WHERE uc.companyId=@companyId AND (SELECT SUM(www.NumberCompleted) FROM dbo.WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) < (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbt.WorkBookId)),0)) AS InCompletedWorkbooks"},

                { Constants.TOTAL_WORKBOOK, ", (SELECT ISNULL((SELECT COUNT(uwb.WorkBookId) FROM  dbo.WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId  AND uwb.IsEnabled=1 JOIN dbo.Supervisor s ON uwb.UserId = s.UserId AND s.UserId IN (SELECT u.User_Id UNION SELECT UserID FROM Supervisor WHERE SupervisorId=u.User_Id AND SupervisorId <> @currentuserId) AND s.IsEnabled = 1 JOIN dbo.UserCompany uc on uc.UserId= s.UserId   AND uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 JOIN dbo.WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId JOIN dbo.WorkBook wb ON wb.Id=wbt.WorkBookId WHERE uc.companyId=@companyId),0)) AS TotalWorkbooks"},

                { Constants.TOTAL_TASK, ", (SELECT ISNULL((SELECT COUNT(wbt.EntityId) FROM  dbo.WorkBookContent wbt JOIN dbo.UserWorkBook uwb ON uwb.workbookId=wbt.WorkbookId AND uwb.IsEnabled=1  JOIN dbo.UserCompany uc on uc.UserId= uwb.UserId   AND uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1  WHERE uc.companyId=@companyId AND wbt.IsEnabled=1 and wbt.WorkbookId=wb.Id  AND uwb.UserId=u.User_Id),0)) AS TotalTasks"},

                { Constants.TOTAL_EMPLOYEES, ", (SELECT COUNT(ss.UserId) FROM dbo.Supervisor ss LEFT JOIN dbo.UserCompany uc ON uc.UserId = ss.UserId AND uc.IsEnabled = 1 AND IsDirectReport=1 AND uc.IsVisible = 1 AND uc.Status = 1 WHERE ss.SupervisorId = u.User_Id AND ss.UserId <> u.User_Id AND uc.CompanyId = @companyId AND ss.IsEnabled = 1 AND ss.SupervisorId <> @currentuserId) AS TotalEmployees"},

                { Constants.COMPLETED_WORKBOOK, ",  (SELECT ISNULL((SELECT COUNT(uwb.WorkBookId) FROM  dbo.WorkBookProgress wbs JOIN dbo.UserWorkBook uwb ON uwb.UserId=wbs.UserId AND uwb.WorkBookId=wbs.WorkBookId  AND uwb.IsEnabled=1 JOIN dbo.Supervisor s ON uwb.UserId = s.UserId AND s.UserId IN (SELECT u.User_Id UNION SELECT UserID FROM Supervisor WHERE SupervisorId=u.User_Id AND SupervisorId <> @currentuserId) AND s.IsEnabled = 1 JOIN dbo.UserCompany uc on uc.UserId= s.UserId   AND uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1 JOIN dbo.WorkBookContent wbt ON wbt.WorkBookId=wbs.WorkBookId JOIN dbo.WorkBook wb ON wb.Id=wbt.WorkBookId WHERE uc.companyId=@companyId AND (SELECT SUM(www.NumberCompleted) FROM dbo.WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) >= (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbt.WorkBookId)),0)) AS CompletedWorkbooks"},

                { Constants.COMPLETED_TASK, ", (SELECT ISNULL((SELECT COUNT(wbt.EntityId) FROM  dbo.WorkBookContent wbt JOIN dbo.UserWorkBook uwb ON uwb.workbookId=wbt.WorkbookId AND uwb.IsEnabled=1  JOIN dbo.UserCompany uc on uc.UserId= uwb.UserId   AND uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1  WHERE uc.companyId=@companyId AND wbt.IsEnabled=1 and wbt.WorkbookId=wb.Id  AND uwb.UserId=u.User_Id AND (SELECT SUM(www.NumberCompleted) FROM dbo.WorkBookProgress www WHERE www.WorkBookId=wbt.WorkBookId) >= (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbt.WorkBookId)),0)) AS CompletedTasks"},

                { Constants.ROLE, ", (SELECT STUFF((SELECT DISTINCT ', ' + r.Name FROM dbo.UserRole ur JOIN dbo.Role r ON r.Id=ur.roleId  WHERE ur.UserId=u.User_Id FOR XML PATH(''), TYPE).value('.', 'VARCHAR(MAX)'), 1, 2, '')) As Role"},

                { Constants.NUMBER_COMPLETED, ", wbp.NumberCompleted"},
                { Constants.LAST_ATTEMPT_DATE, ", (SELECT  MAX(LastAttemptDate) FROM dbo.WorkBookProgress WHERE WorkBookId=wbp.WorkBookId) AS LastAttemptDate"},
                { Constants.FIRST_ATTEMPT_DATE, ", wbp.FirstAttemptDate"},
                { Constants.REPETITIONS, ", wbc.Repetitions"},
                { Constants.WORKBOOK_ASSIGNED_DATE, ", uwb.DateAdded"},
                { Constants.LAST_SIGNOFF_BY, ", (SELECT us.User_Name AS UserName FROM dbo.[UserDetails_RB] us WHERE us.User_Id=wbp.LastSignOffBy) AS LastSignOffBy"}
        };

        /// <summary>
        ///     Dictionary having fields that requried for the workbook entity.
        ///     Based on fields, query is being formed/updated
        /// </summary>
        private readonly Dictionary<string, string> workbookFields = new Dictionary<string, string>()
        {
            {Constants.WORKBOOK_ID, "wb.ID " },
            {Constants.SUPERVISOR_ID, " u.User_Id IN (SELECT UserID FROM Supervisor WHERE SupervisorId=@userId AND SupervisorId <> @currentuserId)" },
            {Constants.NOT_SUPERVISORID, " u.User_Id NOT IN (SELECT UserID FROM Supervisor WHERE SupervisorId=@userId)  " },
            {Constants.SUPERVISOR_USER, " u.User_Id IN (SELECT @userId  UNION SELECT UserID FROM Supervisor WHERE SupervisorId=@userId  AND SupervisorId <> @currentuserId)  " },
            {Constants.SUPERVISOR_SUB, " s.supervisorId " },
            {Constants.USERID, " u.User_Id " },
            {Constants.WORKBOOK_NAME, "wb.Name " },
            {Constants.DESCRIPTION, "wb.Description " },
            {Constants.WORKBOOK_CREATED, "CONVERT(DATE,wb.DateCreated,101) " },
            {Constants.WORKBOOK_ISENABLED, "wb.isenabled " },
            {Constants.DAYS_TO_COMPLETE, "wb.Daystocomplete " },
            {Constants.DUE_DATE, "CONVERT(DATE,DATEADD(DAY, wb.DaysToComplete, wbp.FirstAttemptDate),101) " },
            {Constants.DATE_ADDED, "uwb.DateAdded " },
            { Constants.WORKBOOK_CREATED_BY, "(SELECT us.User_Name AS UserName FROM dbo.[UserDetails_RB] us WHERE us.User_Id=wb.createdby) " },
            {Constants.ASSIGNED_TO, "uwb.UserId " },
            {Constants.ASSIGNED, " u.User_Id IN (SELECT UserID FROM Supervisor WHERE SupervisorId=@userId) " },
            {Constants.WORKBOOK_IN_DUE, " uwb.IsEnabled=1 AND CONVERT(date,(DATEADD(DAY, wb.DaysToComplete, uwb.DateAdded)))  BETWEEN CONVERT(date,GETDATE())  AND CONVERT(date,DATEADD(DAY,CONVERT(INT, @duedays), GETDATE())) AND (SELECT SUM(www.NumberCompleted) FROM dbo.WorkBookProgress www WHERE www.WorkBookId=wbc.WorkBookId) < (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbc.WorkBookId) " },
            {Constants.PAST_DUE, " uwb.IsEnabled=1 AND CONVERT(date,(DATEADD(DAY, wb.DaysToComplete, uwb.DateAdded)))  BETWEEN CONVERT(date,DATEADD(DAY, (CONVERT(INT, @duedays) * -1), GETDATE())) AND CONVERT(date,GETDATE())  AND  (SELECT SUM(www.NumberCompleted) FROM dbo.WorkBookProgress www WHERE www.WorkBookId=wbc.WorkBookId) < (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbc.WorkBookId) " },
            {Constants.COMPLETED, " uwb.IsEnabled=1 AND (SELECT SUM(www.NumberCompleted) FROM dbo.WorkBookProgress www WHERE www.WorkBookId=wbc.WorkBookId) >= (SELECT SUM(tre.Repetitions) FROM dbo.WorkBookContent tre WHERE tre.WorkBookId=wbc.WorkBookId) " },
            { Constants.ROLES, " r.Id IN (Select * from dbo.fnSplit_RB(@roles)) "}

        };


        /// <summary>
        ///     Dictionary having list of tables that requried to get the columns for workbook(s) result
        /// </summary>
        private readonly Dictionary<string, List<string>> tableJoins = new Dictionary<string, List<string>>()
        {

            { " FULL OUTER JOIN dbo.[UserDetails_RB] u ON u.User_Id=uc.UserId ", new List<string> { Constants.USERNAME, Constants.USERNAME2, Constants.USER_CREATED_DATE, Constants.FIRSTNAME, Constants.MIDDLENAME, Constants.LASTNAME, Constants.EMAIL, Constants.CITY, Constants.STATE, Constants.ZIP, Constants.PHONE, Constants.ASSIGNED_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.ASSIGNED_TO, Constants.ROLEID, Constants.ROLE, Constants.USERID, Constants.EMPLOYEE_NAME, Constants.SUPERVISOR_ID, Constants.CREATED_BY } },
            { " FULL OUTER JOIN dbo.Supervisor s ON s.UserId=u.User_Id  AND IsDirectReport=1 AND s.IsEnabled = 1 ", new List<string> {Constants.SUPERVISOR_ID} },
            { " FULL OUTER JOIN dbo.WorkbookProgress wbp ON wbp.WorkbookId=wb.Id ", new List<string> {Constants.NUMBER_COMPLETED, Constants.LAST_ATTEMPT_DATE, Constants.FIRST_ATTEMPT_DATE, Constants.LAST_SIGNOFF_BY, Constants.DATE_ADDED, Constants.DUE_DATE} },
            { " FULL OUTER JOIN dbo.WorkbookContent wbc ON wbc.WorkbookId=wb.Id ", new List<string> {Constants.REPETITIONS, Constants.USERNAME, Constants.USERNAME2, Constants.USER_CREATED_DATE, Constants.FIRSTNAME, Constants.MIDDLENAME, Constants.LASTNAME, Constants.EMAIL, Constants.CITY, Constants.STATE, Constants.ZIP, Constants.PHONE, Constants.ASSIGNED_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.ASSIGNED_TO, Constants.ROLEID, Constants.ROLE, Constants.USERID, Constants.EMPLOYEE_NAME, Constants.SUPERVISOR_ID } },
            { "FULL OUTER JOIN dbo.UserRole ur ON ur.UserId=u.User_Id LEFT JOIN Role r ON r.Id=ur.roleId " , new List<string> {Constants.ROLEID, Constants.ROLE} },
        };

        /// <summary>
        ///  Create SQL Query for the workbook based on the request
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <returns>string</returns>
        public string CreateWorkbookQuery(QueryBuilderRequest queryBuilderRequest)
        {
            string query = string.Empty, tableJoin = string.Empty, selectQuery = string.Empty, whereQuery = string.Empty, companyQuery = string.Empty, supervisorId = string.Empty, currentUserId = string.Empty;
            List<string> fieldList = new List<string>();
            try
            {

                //Select statement for the Query
                selectQuery = "SELECT  DISTINCT ";

                //getting column List
                query = string.Join("", (from column in workbookColumnList
                                         where queryBuilderRequest.ColumnList.Any(x => x == column.Key)
                                         select column.Value));
                query = query.TrimStart(',');
                query = selectQuery + query;

                //Append the workbook tables with select statement
                query += "  FROM dbo.Workbook wb FULL OUTER JOIN dbo.UserWorkBook uwb ON uwb.workbookId=wb.Id  FULL OUTER JOIN  dbo.UserCompany uc ON uc.UserId=uwb.UserId AND  uc.IsEnabled = 1 AND uc.Status = 1 AND uc.IsVisible = 1";

                currentUserId = queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.CURRENT_USER).Select(x => x.Value).FirstOrDefault();
                if (string.IsNullOrEmpty(currentUserId))
                {
                    currentUserId = "0";
                }
                //get table joins
                fieldList = queryBuilderRequest.ColumnList.ToList();

                fieldList.AddRange(queryBuilderRequest.Fields.Select(x => x.Name.ToUpper()).ToArray());

                tableJoin = string.Join("", from joins in tableJoins
                                            where fieldList.Any(x => joins.Value.Any(y => y == x))
                                            select joins.Key);

                query += tableJoin;

                //Read the supervisorId based on the request
                supervisorId = Convert.ToString(queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.SUPERVISOR_ID).Select(x => x.Value).FirstOrDefault());

                //handles the Supervisor Id depends upon the Request
                if (queryBuilderRequest.ColumnList.Contains(Constants.TOTAL_EMPLOYEES) && !string.IsNullOrEmpty(supervisorId))
                {
                    queryBuilderRequest.Fields.Select(x => x.Name == Constants.SUPERVISOR_ID ? x.Name = Constants.SUPERVISOR_SUB : x.Name).ToList();
                }
                else
                {
                    if (!string.IsNullOrEmpty(supervisorId))
                    {
                        queryBuilderRequest.Fields.Select(x => x.Name == Constants.SUPERVISOR_ID && x.Operator == "!=" ? x.Name = Constants.NOT_SUPERVISORID : x.Name).ToList();
                    }
                }

                //handles the Supervisor Id depends upon the Request
                if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.SUPERVISOR_ID).ToList().Count > 0 && queryBuilderRequest.Fields.Where(x => x.Name == Constants.USERID).ToList().Count > 0)
                {
                    EmployeeModel userDetails = queryBuilderRequest.Fields.Where(x => x.Name == Constants.USERID).FirstOrDefault();
                    queryBuilderRequest.Fields.Remove(userDetails);
                    queryBuilderRequest.Fields.Select(x => x.Name == Constants.SUPERVISOR_ID ? x.Name = Constants.SUPERVISOR_USER : x.Name).ToList();
                }

                if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.CURRENT_USER).ToList().Count > 0)
                {
                    EmployeeModel userDetails = queryBuilderRequest.Fields.Where(x => x.Name == Constants.CURRENT_USER).FirstOrDefault();
                    queryBuilderRequest.Fields.Remove(userDetails);
                }


                //Remove student details from the List
                if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.STUDENT_DETAILS).ToList().Count > 0)
                {
                    EmployeeModel userDetails = queryBuilderRequest.Fields.Where(x => x.Name == Constants.STUDENT_DETAILS).FirstOrDefault();
                    queryBuilderRequest.Fields.Remove(userDetails);
                }

                //getting where conditions
                whereQuery = string.Join("", from employee in queryBuilderRequest.Fields
                                             select (!string.IsNullOrEmpty(employee.Bitwise) ? (" " + employee.Bitwise + " ") : string.Empty) + (!string.IsNullOrEmpty(workbookFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault()) ? (workbookFields.Where(x => x.Key == employee.Name.ToUpper()).Select(x => x.Value).FirstOrDefault() + OperatorHelper.CheckOperator(employee.Operator, employee.Value.Trim(), employee.Name)) : string.Empty));

                //Add the where condition for the company
                companyQuery = (" WHERE  uc.status=1 AND  uc.CompanyId=" + queryBuilderRequest.CompanyId);


                if (queryBuilderRequest.ColumnList.Contains(Constants.WORKBOOK_NAME))
                {
                    whereQuery += "AND uwb.isEnabled=1";
                }

                //Create the final query 
                query += (!string.IsNullOrEmpty(whereQuery)) ? (companyQuery + " and (" + whereQuery) + ")" : string.Empty;
                query = query.Replace("@currentuserId", currentUserId);
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
        /// <param name="queryBuilderRequest"></param>
        /// <returns>WorkbookResponse</returns>
        public WorkbookResponse GetWorkbookDetails(QueryBuilderRequest queryBuilderRequest)
        {
            string query = string.Empty;
            int companyId = 0;
            Dictionary<string, string> parameterList;
            WorkbookResponse workbookResponse = new WorkbookResponse();
            string userId = string.Empty;
            string status = string.Empty;
            string workbookID = string.Empty;
            string taskId = string.Empty, role = string.Empty;

            try
            {
                if (queryBuilderRequest.CompanyId == 0) throw new ArgumentException("CompanyId");
                if (queryBuilderRequest.UserId == 0) throw new ArgumentException("UserId");
                if (queryBuilderRequest.Payload == null) throw new ArgumentException("Fields and ColumnList");

                if (queryBuilderRequest.Payload.Fields == null) throw new ArgumentException("Fields ");
                if (queryBuilderRequest.Payload.ColumnList == null) throw new ArgumentException("Columns");

                //Assign the request details to corresponding objects
                companyId = Convert.ToInt32(queryBuilderRequest.CompanyId);
                queryBuilderRequest = queryBuilderRequest.Payload;

                //Assign the companyId to the new object
                queryBuilderRequest.CompanyId = companyId;
                parameterList = ParameterHelper.Getparameters(queryBuilderRequest);
                role = Convert.ToString(parameterList["roles"]);

                if (queryBuilderRequest.AppType == Constants.WORKBOOK_DASHBOARD)
                {
                    if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.WORKBOOK_DETAILS).Select(y => y.Name).FirstOrDefault() == Constants.WORKBOOK_DETAILS)
                    {
                        userId = parameterList["userId"].ToString();
                        query = "EXEC  dbo.Training_OJT_Dashboard_GetDashboardSummary  @companyId  =" + companyId + " , @supervisorId  = " + userId + " , @showTopLevel = " + CheckSupervisor(companyId, Convert.ToInt32(userId));
                    }

                    else if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.WORKBOOK_IN_DUE).Select(y => y.Name).FirstOrDefault() == Constants.WORKBOOK_IN_DUE)
                    {
                        userId = parameterList["userId"].ToString();
                        query = "EXEC  dbo.Training_OJT_Dashboard_GetAssignedOJTs  @companyId  =" + companyId + " , @supervisorId  = " + userId + ", @dueInDays  = " + parameterList["duedays"].ToString() + ",@status = 0";
                    }

                    else if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.PAST_DUE).Select(y => y.Name).FirstOrDefault() == Constants.PAST_DUE)
                    {
                        userId = parameterList["userId"].ToString();
                        query = "EXEC  dbo.Training_OJT_Dashboard_GetAssignedOJTs   @companyId  =" + companyId + " , @supervisorId  = " + userId + ", @dueInDays  = " + parameterList["duedays"].ToString() + " ,@status = 1";
                    }

                    else if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.COMPLETED).Select(y => y.Name).FirstOrDefault() == Constants.COMPLETED)
                    {
                        userId = parameterList["userId"].ToString();
                        query = "EXEC  dbo.Training_OJT_Dashboard_GetAssignedOJTs   @companyId  =" + companyId + " , @supervisorId  = " + userId + " , @status   = " + 2;
                    }

                    else if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.ASSIGNED_WORKBOOK).Select(y => y.Name).FirstOrDefault() == Constants.ASSIGNED_WORKBOOK)
                    {
                        userId = parameterList["userId"].ToString();
                        query = "EXEC  dbo.Training_OJT_Dashboard_GetAssignedOJTs   @companyId  =" + companyId + " , @supervisorId  = " + userId;
                    }

                    else if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.TASK_PROGRESS).Select(y => y.Name).FirstOrDefault() == Constants.TASK_PROGRESS)
                    {
                        userId = parameterList["userId"].ToString();
                        workbookID = parameterList["workbookId"].ToString();
                        query = "EXEC  dbo.Training_OJT_Dashboard_GetTaskProgress   @companyId  =" + companyId + " , @studentId   = " + userId + ", @OJTId =" + workbookID;
                    }

                    else if (queryBuilderRequest.Fields.Where(x => x.Name == Constants.REP_PROGRESS).Select(y => y.Name).FirstOrDefault() == Constants.REP_PROGRESS)
                    {
                        userId = parameterList["userId"].ToString();
                        workbookID = parameterList["workbookId"].ToString();
                        taskId = parameterList["taskId"].ToString();
                        string tag = queryBuilderRequest.Fields.Where(x => x.Name == Constants.STATUS).Select(y => y.Value).FirstOrDefault();
                        status = tag == Constants.COMPLETED ? "1" : tag == Constants.FAILED ? "0" : "null";
                        query = "EXEC  dbo.Training_OJT_Dashboard_GetRepProgress   @companyId  =" + companyId + " , @studentId  = " + userId + ", @OJTId =" + workbookID + ", @taskId=" + taskId + ", @completionStatus=" + status;
                    }
                    if (!string.IsNullOrEmpty(query) && !string.IsNullOrEmpty(role))
                    {
                        query += " , @roleId  = " + role;
                    }
                }
                else
                {
                    query = CreateWorkbookQuery(queryBuilderRequest);
                }
                //Read the SQL Parameters value



                //Create the dictionary to pass the parameter value

                workbookResponse.Workbooks = ReadWorkBookDetails(query, parameterList);

                //Send the response depends upon the workboook details
                if (workbookResponse.Workbooks != null)
                {
                    return workbookResponse;
                }
                else
                {
                    workbookResponse.Error = ResponseBuilder.InternalError();
                    return workbookResponse;
                }
            }
            catch (Exception getEmployeeDetails)
            {
                LambdaLogger.Log(getEmployeeDetails.ToString());
                workbookResponse.Error = new ExceptionHandler(getEmployeeDetails).ExceptionResponse();
                return workbookResponse;
            }
        }

        /// <summary>
        ///     Creating response object after reading workbook(s) details from database
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns>WorkbookModel</returns>
        public List<WorkbookModel> ReadWorkBookDetails(string query, Dictionary<string, string> parameters)
        {
            List<WorkbookModel> workbookList = new List<WorkbookModel>();
            try
            {
                //Read the data from the database
                using (IDataReader dataReader = ExecuteDataReader(query, ParameterHelper.CreateSqlParameter(parameters)))
                {
                    if (dataReader != null)
                    {
                        while (dataReader.Read())
                        {
                            DataTable dataTable = dataReader.GetSchemaTable();
                            //Get the workbook details from the database
                            TaskModel taskComment = (dataTable.Select("ColumnName = 'Attempt_Comment'").Count() == 1) ? JsonConvert.DeserializeObject<TaskModel>(Convert.ToString(dataReader["Attempt_Comment"])) : null;

                            WorkbookModel workbookResponse = new WorkbookModel
                            {

                                EmployeeName = (dataTable.Select("ColumnName = 'employeeName'").Count() == 1) ? Convert.ToString(dataReader["employeeName"]) : (dataTable.Select("ColumnName = 'Employee_Full_Name'").Count() == 1) ? Convert.ToString(dataReader["Employee_Full_Name"]) : null,

                                WorkBookName = (dataTable.Select("ColumnName = 'workbookName'").Count() == 1) ? Convert.ToString(dataReader["workbookName"]) : (dataTable.Select("ColumnName = 'OJT_Name'").Count() == 1) ? Convert.ToString(dataReader["OJT_Name"]) : null,

                                Description = (dataTable.Select("ColumnName = 'Description'").Count() == 1) ? Convert.ToString(dataReader["Description"]) : null,
                                WorkbookCreated = (dataTable.Select("ColumnName = 'datecreated'").Count() == 1) ? Convert.ToString(dataReader["datecreated"]) : null,
                                WorkbookEnabled = (dataTable.Select("ColumnName = 'isEnabled'").Count() == 1) ? (bool?)(dataReader["isEnabled"]) : null,

                                WorkBookId = (dataTable.Select("ColumnName = 'Id'").Count() == 1) ? (dataReader["Id"] != DBNull.Value ? (int?)dataReader["Id"] : 0) : (dataTable.Select("ColumnName = 'OJT_Id'").Count() == 1) ? (dataReader["OJT_Id"] != DBNull.Value ? (int?)dataReader["OJT_Id"] : 0) : null,

                                RepsRequired = (dataTable.Select("ColumnName = 'OJT_Reps_Required_Count'").Count() == 1) ? (dataReader["OJT_Reps_Required_Count"] != DBNull.Value ? (int?)dataReader["OJT_Reps_Required_Count"] : 0) : null,

                                RepsCompleted = (dataTable.Select("ColumnName = 'OJT_Reps_Completed_Count'").Count() == 1) ? (dataReader["OJT_Reps_Completed_Count"] != DBNull.Value ? (int?)dataReader["OJT_Reps_Completed_Count"] : 0) : null,

                                LastSignoffBy = (dataTable.Select("ColumnName = 'LastSignOffBy'").Count() == 1) ? Convert.ToString((dataReader["LastSignOffBy"])) : null,
                                WorkbookAssignedDate = (dataTable.Select("ColumnName = 'DateAdded'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["DateAdded"])) ? Convert.ToDateTime(dataReader["DateAdded"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,
                                Repetitions = (dataTable.Select("ColumnName = 'Repetitions'").Count() == 1) ? Convert.ToString((dataReader["Repetitions"])) : null,
                                FirstAttemptDate = (dataTable.Select("ColumnName = 'FirstAttemptDate'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["FirstAttemptDate"])) ? Convert.ToDateTime(dataReader["FirstAttemptDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,
                                LastAttemptDate = (dataTable.Select("ColumnName = 'LastAttemptDate'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["LastAttemptDate"])) ? Convert.ToDateTime(dataReader["LastAttemptDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,
                                NumberCompleted = (dataTable.Select("ColumnName = 'NumberCompleted'").Count() == 1) ? (int?)(dataReader["NumberCompleted"]) : null,

                                Role = (dataTable.Select("ColumnName = 'Role'").Count() == 1) ? Convert.ToString(dataReader["Role"]) : (dataTable.Select("ColumnName = 'Employee_Role'").Count() == 1) ? Convert.ToString(dataReader["Employee_Role"]) : null,

                                CompletedWorkbook = (dataTable.Select("ColumnName = 'OJT_Completed'").Count() == 1) ? (int?)(dataReader["OJT_Completed"]) : null,

                                TotalTasks = (dataTable.Select("ColumnName = 'OJT_Task_Count'").Count() == 1) ? (int?)(dataReader["OJT_Task_Count"]) : null,


                                TotalWorkbook = (dataTable.Select("ColumnName = 'TotalWorkbooks'").Count() == 1) ? (int?)(dataReader["TotalWorkbooks"]) : null,
                                PastDueWorkBook = (dataTable.Select("ColumnName = 'OJT_Past_Due_Count'").Count() == 1) ? (int?)(dataReader["OJT_Past_Due_Count"]) : null,
                                InDueWorkBook = (dataTable.Select("ColumnName = 'OJT_Due_Count'").Count() == 1) ? (int?)(dataReader["OJT_Due_Count"]) : null,
                                AssignedWorkBook = (dataTable.Select("ColumnName = 'OJT_Assigned_Count'").Count() == 1) ? (int?)(dataReader["OJT_Assigned_Count"]) : null,
                                UserCount = (dataTable.Select("ColumnName = 'userCount'").Count() == 1) ? (int?)(dataReader["userCount"]) : null,
                                EntityCount = (dataTable.Select("ColumnName = 'entityCount'").Count() == 1) ? (int?)(dataReader["entityCount"]) : null,
                                UserName = (dataTable.Select("ColumnName = 'UserName'").Count() == 1) ? Convert.ToString(dataReader["UserName"]) : (dataTable.Select("ColumnName = 'Employee_User_Name'").Count() == 1) ? Convert.ToString(dataReader["Employee_User_Name"]) : null,
                                Status = (dataTable.Select("ColumnName = 'status'").Count() == 1) ? Convert.ToString(dataReader["status"]) : null,
                                DaysToComplete = (dataTable.Select("ColumnName = 'daystocomplete'").Count() == 1) ? Convert.ToString(dataReader["daystocomplete"]) : null,
                                AlternateName = (dataTable.Select("ColumnName = 'UserName2'").Count() == 1) ? Convert.ToString(dataReader["UserName2"]) : null,
                                Email = (dataTable.Select("ColumnName = 'email'").Count() == 1) ? Convert.ToString(dataReader["email"]) : null,
                                CreatedBy = (dataTable.Select("ColumnName = 'CreatedBy'").Count() == 1) ? Convert.ToString(dataReader["CreatedBy"]) : null,
                                Address = (dataTable.Select("ColumnName = 'Address'").Count() == 1) ? Convert.ToString(dataReader["Address"]) : null,
                                Phone = (dataTable.Select("ColumnName = 'Phone'").Count() == 1) ? Convert.ToString(dataReader["Phone"]) : null,
                                TotalEmployees = (dataTable.Select("ColumnName = 'Subordinate_Count'").Count() == 1) ? (int?)(dataReader["Subordinate_Count"]) : null,

                                InCompleteWorkBook = (dataTable.Select("ColumnName = 'InCompletedWorkbooks'").Count() == 1) ? (int?)(dataReader["InCompletedWorkbooks"]) : null,

                                UserId = (dataTable.Select("ColumnName = 'UserId'").Count() == 1) ? (dataReader["UserId"] != DBNull.Value ? (int?)dataReader["UserId"] : 0) : (dataTable.Select("ColumnName = 'Employee_Id'").Count() == 1) ? (dataReader["Employee_Id"] != DBNull.Value ? (int?)dataReader["Employee_Id"] : 0) : null,

                                DueDate = (dataTable.Select("ColumnName = 'OJT_Due_Date'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["OJT_Due_Date"])) ? Convert.ToDateTime(dataReader["OJT_Due_Date"]).ToString("MM/dd/yyyy") : null : null,

                                TaskId = (dataTable.Select("ColumnName = 'taskId'").Count() == 1) ? (dataReader["taskId"] != DBNull.Value ? (int?)dataReader["taskId"] : 0) : (dataTable.Select("ColumnName = 'Task_Id'").Count() == 1) ? (dataReader["Task_Id"] != DBNull.Value ? (int?)dataReader["Task_Id"] : 0) : null,

                                TaskName = (dataTable.Select("ColumnName = 'taskName'").Count() == 1) ? Convert.ToString(dataReader["taskName"]) : (dataTable.Select("ColumnName = 'Task_Name'").Count() == 1) ? Convert.ToString(dataReader["Task_Name"]) : null,

                                TaskCode = (dataTable.Select("ColumnName = 'Code'").Count() == 1) ? Convert.ToString(dataReader["Code"]) : (dataTable.Select("ColumnName = 'Task_Code'").Count() == 1) ? Convert.ToString(dataReader["Task_Code"]) : null,

                                NumberofAttempts = (dataTable.Select("ColumnName = 'Attempt'").Count() == 1) ? Convert.ToString(dataReader["Attempt"]) : null,

                                LastAttemptDate_tasks = (dataTable.Select("ColumnName = 'Date_Attempted'").Count() == 1) ? !string.IsNullOrEmpty(Convert.ToString(dataReader["Date_Attempted"])) ? Convert.ToDateTime(dataReader["Date_Attempted"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy") : null,

                                Location = (dataTable.Select("ColumnName = 'Attempt_Location'").Count() == 1) ? Convert.ToString(dataReader["Attempt_Location"]) : null,
                                Comments = taskComment?.Comment,

                                EvaluatorName = (dataTable.Select("ColumnName = 'Submitted_By_User_Id'").Count() == 1) ? Convert.ToString(dataReader["Submitted_By_User_Id"]) : null,

                                CompletedTasks = (dataTable.Select("ColumnName = 'OJT_Task_Completed_Count'").Count() == 1) ? Convert.ToString((dataReader["OJT_Task_Completed_Count"])) : null
                            };
                            // Adding each workbook details in array list
                            workbookList.Add(workbookResponse);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                return workbookList;
            }
            catch (Exception readWorkbookDetailsException)
            {
                LambdaLogger.Log(readWorkbookDetailsException.ToString());
                return null;
            }
        }

        /// <summary>
        /// Check if the logged in user is supervisor or not
        /// </summary>
        public int CheckSupervisor(int companyId, int userId)
        {
            int showTopLevel = 0;
            try
            {
                using (DBEntity context = new DBEntity())
                {
                    //check whether the user has access to the company  
                    UserCompany userCompany = (from uc in context.UserCompany
                                               where uc.CompanyId == companyId
                                               && uc.IsEnabled && uc.Status == 1
                                               && uc.UserId == userId
                                               select uc).FirstOrDefault();
                    if (userCompany != null)
                    {
                        PermissionManager permissionManager = new PermissionManager(Convert.ToInt64(userCompany.ReportsPerms));
                        if (permissionManager.Contains(ReportPerms.ShowDashBoardTopLevel))
                        {
                            showTopLevel = 1;
                        }
                    }
                }
                return showTopLevel;
            }
            catch (Exception checkSupervisorException)
            {
                LambdaLogger.Log(checkSupervisorException.ToString());
                return showTopLevel;
            }
        }
    }
}
