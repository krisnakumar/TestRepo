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
<copyright file="PastDueWorkbooks.cs">
    Copyright (c) 2018 All Rights Reserved
</copyright>
<author>Pawan Kumar</author>
<date>26-Nov-2018</date>
<summary>
    This class covers test cases for past due workbooks under a user.
    It has one input field, i.e., userId
    All tests assert the response based on requested userId.
    The class covers following test cases :
        - Correct userId which yields a well defined success response
        - Trying to get past due workbook list for a user which does not have any. Hence, expecting empty response with success code
        - Incorrect datatype of userId, which yields an error response
        - NULL is given as userId, which yields an error response
        - A request without userId, which yields an error response
</summary>
*********************************************************************************/

namespace ReportBuilder.UnitTest.TestModules.Workbooks
{
    [TestClass]
    public class PastDueWorkbooks
    {
        
        /// <summary> 
        /// Test to get past due workbooks list for a user
        /// [Inputs]        userId is correctly given
        /// [Expectations]  A well defined response with list of wokrbooks and success code
        /// [Assertions]    Success response code as 200
        ///                 Response contains list of workbooks
        /// </summary>
        [TestMethod]
        public void GetDueWorkbookList()
        {
            Function function = new Function();
            var APIRequest = RequestBuilder.PathParamsRequest("userId", "6");
            var workbookResponse = function.GetPastDueWorkbookDetails(APIRequest, null);
            string responseWorkBooks = Convert.ToString(workbookResponse.Body);
            List<WorkbookResponse> wblist = JsonConvert.DeserializeObject<List<WorkbookResponse>>(responseWorkBooks);
            Assert.AreEqual(200, workbookResponse.StatusCode);
            Assert.AreNotEqual(0, wblist.Count);
        }
        
        /// <summary> 
        /// Test to get past due workbooks list for a user
        /// [Inputs]        userId is given, which does not have any past due workbooks
        /// [Expectations]  An empty response with success code
        /// [Assertions]    Success response code as 200
        ///                 An empty response
        /// </summary>
        [TestMethod]
        public void GetDueWorkbookForUnsubscribedUser()
        {
            Function function = new Function();
            var APIRequest = RequestBuilder.PathParamsRequest("userId", "601");
            var workbookResponse = function.GetPastDueWorkbookDetails(APIRequest, null);
            string responseWorkBooks = Convert.ToString(workbookResponse.Body);
            List<WorkbookResponse> wblist = JsonConvert.DeserializeObject<List<WorkbookResponse>>(responseWorkBooks);
            Assert.AreEqual(200, workbookResponse.StatusCode);
            Assert.IsFalse(wblist.Count > 0, "Empty response");
        }
        
        /// <summary> 
        /// Test to get  past due workbooks list for a user
        /// [Inputs]        Invalid 'UserId' is given i.e., different datatype value
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 Response contains custom error code as 1
        ///                 Response message contains error statement
        /// </summary>
        [TestMethod]
        public void GetDueWorkbooksWithInvalidUserId()
        {
            Function function = new Function();
            var APIRequest = RequestBuilder.PathParamsRequest("userId", "LMS");
            var workbookResponse = function.GetPastDueWorkbookDetails(APIRequest, null);
            string responseWorkBooks = Convert.ToString(workbookResponse.Body);
            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseWorkBooks);
            Assert.AreEqual(400, workbookResponse.StatusCode);
            Assert.AreEqual(errorRes.Code, 1);
            StringAssert.Contains(errorRes.Message, "Invalid input");
        }
        
        /// <summary> 
        /// Test to get past due workbooks list for a user
        /// [Inputs]        Invalid 'UserId' is given i.e., null
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 Response contains custom error code as 1
        ///                 Response message contains error statement
        /// </summary>
        [TestMethod]
        public void GetDueWorkbooksWithNullUserId()
        {
            Function function = new Function();
            var APIRequest = RequestBuilder.PathParamsRequest("userId", null);
            var workbookResponse = function.GetPastDueWorkbookDetails(APIRequest, null);
            string responseWorkBooks = Convert.ToString(workbookResponse.Body);
            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseWorkBooks);
            Assert.AreEqual(400, workbookResponse.StatusCode);
            Assert.AreEqual(errorRes.Code, 1);
            StringAssert.Contains(errorRes.Message, "Invalid input");
        }
        
        /// <summary> 
        /// Test to get past due workbooks list for a user
        /// [Inputs]        'UserId' is not given as it is mandatory
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 Response contains custom error code as 1
        ///                 Response message contains error statement
        /// </summary>
        [TestMethod]
        public void GetDueWorkbooksWithoutUserId()
        {
            Function function = new Function();
            var APIRequest = RequestBuilder.PathParamsRequest("workbookId", null);
            var workbookResponse = function.GetPastDueWorkbookDetails(APIRequest, null);
            string responseWorkBooks = Convert.ToString(workbookResponse.Body);
            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseWorkBooks);
            Assert.AreEqual(400, workbookResponse.StatusCode);
            Assert.AreEqual(errorRes.Code, 1);
            StringAssert.Contains(errorRes.Message, "Invalid input");
        }
    }
}
