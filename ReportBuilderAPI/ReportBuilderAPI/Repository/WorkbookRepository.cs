using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.DatabaseManager;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.IRepository;



// <copyright file="WorkbookRepository.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Shoba Eswar</author>
// <date>23-10-2018</date>
// <summary>Repository that helps to read the Workbook data from the Table</summary>
namespace ReportBuilderAPI.Repository
{
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
                query = WorkbookQueries.ReadWorkbookDetails(userId);
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
                            PercentageCompleted = (Convert.ToInt32(sqlDataReader["CompletedWorkbooks"]) / Convert.ToInt32(sqlDataReader["totalTasks"])) * 100,
                            DueDate = Convert.ToDateTime(sqlDataReader["DueDate"]),
                            UserId= Convert.ToInt32(sqlDataReader["UserId"])
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
