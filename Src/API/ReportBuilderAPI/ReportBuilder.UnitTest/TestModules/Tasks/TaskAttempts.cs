//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Amazon.Lambda.APIGatewayEvents;
//using Newtonsoft.Json;
//using ReportBuilderAPI.Handlers.FunctionHandler;
//using ReportBuilder.Models.Response;
//using ReportBuilder.UnitTest.Helpers;


///*******************************************************************************
//<copyright file="TaskAttempts.cs">
//    Copyright (c) 2018 All Rights Reserved
//</copyright>
//<author>Shoba Eswar</author>
//<date>27-Nov-2018</date>
//<summary>
//    This class covers test cases for list of attempts made over a task for a user under a workbook.
//    It has three mandatory input fields, i.e., userId, workbookId and taskId
//    All tests assert the response based on requested input.
//    The class covers following test cases :
//        - Correct inputs which yields a well defined success response
//        - Trying to get attempts made over a task with incorrect inputs which does not contains any. Hence, expecting empty response with success code
//        - Incorrect datatype of inputs, which yields an error response
//        - NULL is given as mandatory input, which yields an error response
//        - A request without mandatory input, which yields an error response
//</summary>
//*********************************************************************************/

//namespace ReportBuilder.UnitTest.TestModules.Tasks
//{
//    /// <summary> 
//    /// Test to get list of attempts made over a task for a user under a workbook
//    /// [Inputs]        Correct inputs (userId, workbookId, taskId) are given
//    /// [Expectations]  A well defined response with attempts list on task and success code
//    /// [Assertions]    Success response code as 200
//    ///                 Response contains list of attempts
//    /// </summary>
//    [TestClass]
//    public class TaskAttempts
//    {
//        [TestMethod]
//        public void GetTaskAttemptsList()
//        {

//            Function function = new Function();
//            Dictionary<string, string> pathValues = new Dictionary<string, string>
//            {
//                { "userId", "18" },
//                {"workbookId","18" },
//                {"taskId","606" }
//            };

//            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
//            {
//                PathParameters = pathValues
//            };
//            APIGatewayProxyResponse attemptsResponse = function.GetTaskAttemptsDetails(aPIGatewayProxyRequest, null);
//            string responseTasks = Convert.ToString(attemptsResponse.Body);
//            List<AttemptsResponse> attemptslist = JsonConvert.DeserializeObject<List<AttemptsResponse>>(responseTasks);
//            Assert.AreEqual(200, attemptsResponse.StatusCode);
//            Assert.AreNotEqual(0, attemptslist.Count);
//        }
        
//        /// <summary> 
//        /// Test to get list of attempts made over a task for a user under a workbook
//        /// [Inputs]        Incorrect inputs are given, which does not yield any attempts on task
//        /// [Expectations]  An empty response with success code
//        /// [Assertions]    Success response code as 200
//        ///                 An empty response
//        /// </summary>
//        [TestMethod]
//        public void GetTaskAttemptsForUnsubscribedUser()
//        {

//            Function function = new Function();
//            Dictionary<string, string> pathValues = new Dictionary<string, string>
//            {
//                { "userId", "5" },
//                {"workbookId","18" },
//                {"taskId","606" }
//            };

//            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
//            {
//                PathParameters = pathValues
//            };
//            APIGatewayProxyResponse attemptsResponse = function.GetTaskAttemptsDetails(aPIGatewayProxyRequest, null);
//            string responseTasks = Convert.ToString(attemptsResponse.Body);
//            List<AttemptsResponse> attemptslist = JsonConvert.DeserializeObject<List<AttemptsResponse>>(responseTasks);
//            Assert.AreEqual(200, attemptsResponse.StatusCode);
//            Assert.IsFalse(attemptslist.Count > 0, "Empty response");
//        }
        
//        /// <summary> 
//        /// Test to get list of attempts made over a task for a user under a workbook
//        /// [Inputs]        Invalid mandatory inputs (userId, workbookId) are given i.e., different datatype value
//        /// [Expectations]  An error response with bad request status code
//        /// [Assertions]    Response code as 400
//        ///                 Response contains custom error code as 1
//        ///                 Response message contains error statement
//        /// </summary>
//        [TestMethod]
//        public void GetTaskAttemptsWithInvalidId()
//        {

//            Function function = new Function();
//            Dictionary<string, string> pathValues = new Dictionary<string, string>
//            {
//                { "userId", "ITS" },
//                {"workbookId","Its" },
//                {"taskId","606" }

//            };

//            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
//            {
//                PathParameters = pathValues
//            };
//            APIGatewayProxyResponse attemptsResponse = function.GetTaskAttemptsDetails(aPIGatewayProxyRequest, null);
//            string responseTasks = Convert.ToString(attemptsResponse.Body);
//            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseTasks);
//            Assert.AreEqual(400, attemptsResponse.StatusCode);
//            Assert.AreEqual(errorRes.Code, 1);
//            StringAssert.Contains(errorRes.Message, "Invalid input");
//        }
        
//        /// <summary> 
//        /// Test to get list of attempts made over a task for a user under a workbook
//        /// [Inputs]        Invalid mandatory inputs (userId, taskId) are given i.e., null
//        /// [Expectations]  An error response with bad request status code
//        /// [Assertions]    Response code as 400
//        ///                 Response contains custom error code as 1
//        ///                 Response message contains error statement
//        /// </summary>
//        [TestMethod]
//        public void GetTaskAttemptsWithNullId()
//        {

//            Function function = new Function();
//            Dictionary<string, string> pathValues = new Dictionary<string, string>
//            {
//                { "userId", null },
//                {"workbookId","Its" },
//                {"taskId", null }
//            };

//            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
//            {
//                PathParameters = pathValues
//            };
//            APIGatewayProxyResponse attemptsResponse = function.GetTaskAttemptsDetails(aPIGatewayProxyRequest, null);
//            string responseTasks = Convert.ToString(attemptsResponse.Body);
//            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseTasks);
//            Assert.AreEqual(400, attemptsResponse.StatusCode);
//            Assert.AreEqual(errorRes.Code, 1);
//            StringAssert.Contains(errorRes.Message, "Invalid input");
//        }
        
//        /// <summary> 
//        /// Test to get list of attempts made over a task for a user under a workbook
//        /// [Inputs]        'taskId' is not given as it is mandatory
//        /// [Expectations]  An error response with bad request status code
//        /// [Assertions]    Response code as 400
//        ///                 Response contains custom error code as 1
//        ///                 Response message contains error statement
//        /// </summary>
//        [TestMethod]
//        public void GetTaskAttemptsWithoutTaskId()
//        {

//            Function function = new Function();
//            Dictionary<string, string> pathValues = new Dictionary<string, string>
//            {
//                { "userId", "18" },
//                {"workbookId","18" }
//            };

//            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
//            {
//                PathParameters = pathValues
//            };
//            APIGatewayProxyResponse attemptsResponse = function.GetTaskAttemptsDetails(aPIGatewayProxyRequest, null);
//            string responseTasks = Convert.ToString(attemptsResponse.Body);
//            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseTasks);
//            Assert.AreEqual(400, attemptsResponse.StatusCode);
//            Assert.AreEqual(errorRes.Code, 1);
//            StringAssert.Contains(errorRes.Message, "Invalid input");
//        }
//    }
//}
