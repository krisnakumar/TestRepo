using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using ReportBuilderAPI.Handlers.FunctionHandler;
using ReportBuilder.Models.Response;
using ReportBuilder.UnitTest.Helpers;


/*******************************************************************************
<copyright file="Tasks.cs">
    Copyright (c) 2018 All Rights Reserved
</copyright>
<author>Pawan Kumar</author>
<date>27-Nov-2018</date>
<summary>
    This class covers test cases for tasks list for a user under a workbook.
    It has two mandatory input fields, i.e., userId and workbookId
    All tests assert the response based on requested input.
    The class covers following test cases :
        - Correct inputs which yields a well defined success response
        - Trying to get tasks list for a user under a workbook which does not contains any. Hence, expecting empty response with success code
        - Incorrect datatype of inputs, which yields an error response
        - NULL is given as mandatory input, which yields an error response
        - A request without mandatory input, which yields an error response
</summary>
*********************************************************************************/

namespace ReportBuilder.UnitTest.TestModules.Tasks
{
    [TestClass]
    public class Tasks
    {
        /// <summary> 
        /// Test to get tasks list for a user under a workbook
        /// [Inputs]        Correct inputs (userId, workbookId) are given
        /// [Expectations]  A well defined response with list of tasks and success code
        /// [Assertions]    Success response code as 200
        ///                 Response contains list of tasks
        /// </summary>
        [TestMethod]
        public void GetTaskList()
        {
            Function function = new Function();
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "userId", "18" },
                {"workbookId","18" }
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues
            };
            APIGatewayProxyResponse taskResponse = function.GetTaskList(aPIGatewayProxyRequest, null);
            string responseTasks = Convert.ToString(taskResponse.Body);
            List<TaskResponse> tasklist = JsonConvert.DeserializeObject<List<TaskResponse>>(responseTasks);
            Assert.AreEqual(200, taskResponse.StatusCode);
            Assert.AreNotEqual(0, tasklist.Count);
        }
        
        /// <summary> 
        /// Test to get tasks list for a user under a workbook
        /// [Inputs]        Incorrect inputs are given, which does not yield any tasks
        /// [Expectations]  An empty response with success code
        /// [Assertions]    Success response code as 200
        ///                 An empty response
        /// </summary>
        [TestMethod]
        public void GetTasksForUnsubscribedUser()
        {
            Function function = new Function();
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "userId", "5" },
                {"workbookId","18" }
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues
            };
            APIGatewayProxyResponse taskResponse = function.GetTaskList(aPIGatewayProxyRequest, null);
            string responseTasks = Convert.ToString(taskResponse.Body);
            List<TaskResponse> tasklist = JsonConvert.DeserializeObject<List<TaskResponse>>(responseTasks);
            Assert.AreEqual(200, taskResponse.StatusCode);
            Assert.IsFalse(tasklist.Count > 0, "Empty response");
        }
        
        /// <summary> 
        /// Test to get tasks list for a user under a workbook
        /// [Inputs]        Invalid mandatory inputs (userId, workbookId) are given i.e., different datatype value
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 Response contains custom error code as 1
        ///                 Response message contains error statement
        /// </summary>
        [TestMethod]
        public void GetTasksWithInvalidId()
        {
            Function function = new Function();
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "userId", "LMS" },
                {"workbookId","LMS" }

            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues
            };
            APIGatewayProxyResponse taskResponse = function.GetTaskList(aPIGatewayProxyRequest, null);
            string responseTasks = Convert.ToString(taskResponse.Body);
            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseTasks);
            Assert.AreEqual(400, taskResponse.StatusCode);
            Assert.AreEqual(errorRes.Code, 1);
            StringAssert.Contains(errorRes.Message, "Invalid input");
        }
        
        /// <summary> 
        /// Test to get tasks list for a user under a workbook
        /// [Inputs]        Invalid mandatory inputs (userId, workbookId) are given i.e., null
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 Response contains custom error code as 1
        ///                 Response message contains error statement
        /// </summary>
        [TestMethod]
        public void GetTasksWithNullId()
        {
            Function function = new Function();
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "userId", null },
                {"workbookId",null }

            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues
            };
            APIGatewayProxyResponse taskResponse = function.GetTaskList(aPIGatewayProxyRequest, null);
            string responseTasks = Convert.ToString(taskResponse.Body);
            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseTasks);
            Assert.AreEqual(400, taskResponse.StatusCode);
            Assert.AreEqual(errorRes.Code, 1);
            StringAssert.Contains(errorRes.Message, "Invalid input");
        }
        
        /// <summary> 
        /// Test to get tasks list for a user under a workbook
        /// [Inputs]        'workbookId' is not given as it is mandatory
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 Response contains custom error code as 1
        ///                 Response message contains error statement
        /// </summary>
        [TestMethod]
        public void GetTasksWithoutWorkbookId()
        {
            Function function = new Function();
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "userId", "18" }

            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues
            };
            APIGatewayProxyResponse taskResponse = function.GetTaskList(aPIGatewayProxyRequest, null);
            string responseTasks = Convert.ToString(taskResponse.Body);
            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseTasks);
            Assert.AreEqual(400, taskResponse.StatusCode);
            Assert.AreEqual(errorRes.Code, 1);
            StringAssert.Contains(errorRes.Message, "Invalid input");
        }
    }
}
