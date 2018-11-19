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
                            WorkBookId=Convert.ToInt32(sqlDataReader["workbookId"])
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
                            WorkBookId= Convert.ToInt32(sqlDataReader["workbookId"])
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
                            WorkBookId= Convert.ToInt32(sqlDataReader["workbookId"])

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
                            WorkBookId= Convert.ToInt32(sqlDataReader["workbookId"])
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
    }
}
