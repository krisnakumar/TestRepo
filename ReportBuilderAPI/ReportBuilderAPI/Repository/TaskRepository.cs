using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.DatabaseManager;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;

namespace ReportBuilderAPI.Repository
{
    public class TaskRepository : ITask
    {

        /// <summary>
        /// Get task details
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="workbookId"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse GetTaskDetails(int userId, int workbookId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<TaskResponse> taskList = new List<TaskResponse>();
            string query = string.Empty;
            try
            {
                query = TaskQueries.GetTaskList(userId, workbookId);
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        TaskResponse employeeResponse = new TaskResponse
                        {
                            TaskCode = Convert.ToString(sqlDataReader["TaskCode"]),
                            TaskName = Convert.ToString(sqlDataReader["TaskName"]),
                            CompletedTasksCount = Convert.ToInt32(sqlDataReader["CompletedWorkbooks"]),
                            IncompletedTasksCount = Convert.ToInt32(sqlDataReader["InCompletedWorkbooks"]),
                            TotalTasks = Convert.ToInt32(sqlDataReader["TotalTasks"]),
                            CompletionPrecentage = (Convert.ToInt32(sqlDataReader["CompletedWorkbooks"]) / Convert.ToInt32(sqlDataReader["TotalTasks"])) * 100,
                        };
                        taskList.Add(employeeResponse);
                    }
                }
                return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(taskList), 0);
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
        /// <returns></returns>
        public APIGatewayProxyResponse GetTaskAttemptsDetails(int userId, int workbookId, int taskId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<AttemptsResponse> attemptsList = new List<AttemptsResponse>();
            string query = string.Empty;
            try
            {
                query = TaskQueries.GetTaskAttemptsDetails(userId, taskId);
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        AttemptsResponse employeeResponse = new AttemptsResponse
                        {
                            Status = Convert.ToString(sqlDataReader[""]),
                            DateTime = Convert.ToDateTime(sqlDataReader[""]),
                            Location = Convert.ToString(sqlDataReader[""]),
                            Evaluator = Convert.ToString(sqlDataReader[""]),
                            Comments = Convert.ToString(sqlDataReader[""])
                        };
                        attemptsList.Add(employeeResponse);
                    }
                }
                return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(attemptsList), 0);
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
