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
using System.Text.RegularExpressions;



// <copyright file="EmployeeRepository.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Shoba Eswar</author>
// <date>10-10-2018</date>
// <summary>Repository that helps to read the data from the Table</summary>
namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Repository that helps to read the data from the table
    /// </summary>
    public class EmployeeRepository : IEmployee
    {
        /// <summary>
        /// Get list of employee who currently working under the specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetEmployeeList(int userId, QueryStringModel queryStringModel)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<EmployeeResponse> employeeList = new List<EmployeeResponse>();
            string query = string.Empty;
            try
            {
                var r = "//<![CDATA[location.href =\"onboardconnect://onboard_success=true&message=Yay!&v62_response=%7B%22UserName%22%3A%22aravi%40its-training.com%22%2C%22FullName%22%3A%22Aruna%20Ravi%22%2C%22PrivateKey%22%3A%22crANzX6%2F3rU3Yj7A48FCxghiyITghGmhh3sW5Nkit%2F4xcZq%2FYYJY%2Bp8TOC1n977OY5Q%2Fl%2B24xYsMdbvjvVSlLKq3sp%2FXTs8t%2B4nqMoD65C8%3D%22%2C%22Token%22%3A%229BMiN6geXexQJTaBVu%2FxZhKKr%2FssMmGZqVNLVE5GiPlIdXrCy7u%2FyrURSo59Kh04%22%2C%22UserId%22%3A272758%2C%22Key%22%3A%22UXUyakxAJFpOU2NSZzdwVzZkLjNNMUh5VjBGNXRhRTgoNC45KSotaw%3D%3D%22%2C%22UploadAccessId%22%3A%22%22%2C%22UploadSecretKey%22%3A%22%22%2C%22Permissions%22%3A%7B%22User%22%3A37879803%2C%22Company%22%3A339890%2C%22Course%22%3A2154493%2C%22Report%22%3A2147483647%2C%22System%22%3A0%7D%2C%22SessionTimeOutPeriod%22%3A1%7D&v610_response=%7B%22userid%22%3A272758%2C%22username%22%3A%22aravi%40its-training.com%22%2C%22fullname%22%3A%22Aruna%20Ravi%22%2C%22companyid%22%3A1%2C%22token%22%3A%22camX0ejxfFCcYknOnhRPr7S1PdmNNqpB1R0rtEn%2BPr2YRYxd8WihF22oRu1yXv7d1DxuOIi45J2huzIcvTmiUQ%3D%3D%22%2C%22testsessionid%22%3A0%7D\"" + ";//]]>";

                var regex = new Regex("=\"(.*?)\"");
                var output = regex.Match(r);
                string result = output.Groups[1].Value;

                if (queryStringModel != null)
                {
                    query = Employee.GetWorkBookDetails(userId, queryStringModel.CompletedWorkBooks, queryStringModel.WorkBookInDue, queryStringModel.PastDueWorkBook);
                }
                else
                {
                    query = Employee.ReadEmployeeDetails(userId);
                }

                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(query);
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        EmployeeResponse employeeResponse = new EmployeeResponse
                        {
                            FirstName = Convert.ToString(sqlDataReader["FirstName"]),
                            LastName = Convert.ToString(sqlDataReader["LastName"]),
                            Role = Convert.ToString(sqlDataReader["Role"]),
                            WorkbookName = Convert.ToString(sqlDataReader["WorkbookName"]),
                            AssignedWorkBooks = Convert.ToInt32(sqlDataReader["AssignedWorkbooks"]),
                            InDueWorkBooks = Convert.ToInt32(sqlDataReader["WorkbooksinDue"]),
                            PastDueWorkBooks = Convert.ToInt32(sqlDataReader["PastDueWorkbooks"]),
                            CompletedWorkBooks = Convert.ToInt32(sqlDataReader["CompletedWorkbooks"]),
                            EmployeeCount = Convert.ToInt32(sqlDataReader["TotalEmployees"]),
                            UserId = Convert.ToInt32(sqlDataReader["UserId"])
                        };
                        employeeList.Add(employeeResponse);
                    }
                }
                return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(employeeList), 0);
            }
            catch (Exception getEmployeeException)
            {
                LambdaLogger.Log(getEmployeeException.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }
    }
}
