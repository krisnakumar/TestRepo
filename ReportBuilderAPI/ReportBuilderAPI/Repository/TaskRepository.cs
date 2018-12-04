using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.DatabaseManager;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;


// <copyright file="EmployeeRepository.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Shoba Eswar</author>
// <date>02-11-2018</date>
// <summary>Repository that helps to read the task from the Table</summary>
namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Class that handles the task details
    /// </summary>
    public class TaskRepository : ITask
    {

        /// <summary>
        /// Get task details
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
        /// Get list of task attempts details
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
    }
}
