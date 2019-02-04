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
<copyright file="Employees.cs">
    Copyright (c) 2018 All Rights Reserved
</copyright>
<author>Shoba Eswar</author>
<date>22-Nov-2018</date>
<summary>
    This class covers test cases for list of employees under a user.
    It has one mandatory input field, i.e., userId
    All tests assert the response based on requested userId.
    The class covers following test cases :
        - Correct userId which yields a well defined success response
        - Trying to get employees list under a user which does not have any. Hence, expecting empty response with success code
        - Incorrect datatype of input, which yields an error response
        - NULL is given as mandatory input, which yields an error response
        - A request without mandatory input, which yields an error response
</summary>
*********************************************************************************/

namespace ReportBuilder.UnitTest.TestModules.Employees
{
    [TestClass]
    public class Employees
    {
        /// <summary> 
        /// Test to get employees list under a user
        /// [Inputs]        Correct userId is given
        /// [Expectations]  A well defined response with list of employees and success code
        /// [Assertions]    Success response code as 200
        ///                 Response contains list of employees
        /// </summary>
        [TestMethod]
        public void GetEmployeeList()
        {
            Function function = new Function();
            var APIRequest = RequestBuilder.PathParamsRequest("userId", "6");
            var userResponse = function.GetEmployees(APIRequest, null);
            string responseEmpl = Convert.ToString(userResponse.Body);
            List<EmployeeResponse> empl = JsonConvert.DeserializeObject<List<EmployeeResponse>>(responseEmpl);
            Assert.AreEqual(200, userResponse.StatusCode);
            Assert.IsTrue(empl.Count > 0, "Should not be empty one");
        }
        
        /// <summary> 
        /// Test to get employees list under a user
        /// [Inputs]        Incorrect userId is given, which does not yield employees list
        /// [Expectations]  An empty response with success code
        /// [Assertions]    Success response code as 200
        ///                 An empty response
        /// </summary>
        [TestMethod]
        public void GetEmployeesForUnsubscribedtUser()
        {
            Function function = new Function();
            var APIRequest = RequestBuilder.PathParamsRequest("userId", "145");
            var userResponse = function.GetEmployees(APIRequest, null);
            string responseEmpl = Convert.ToString(userResponse.Body);
            List<EmployeeResponse> empl = JsonConvert.DeserializeObject<List<EmployeeResponse>>(responseEmpl);
            Assert.AreEqual(200, userResponse.StatusCode);
            Assert.IsFalse(empl.Count > 0, "Should be empty");
        }
        
        /// <summary> 
        /// Test to get employees list under a user
        /// [Inputs]        Invalid userId is given i.e., different datatype value
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 Response contains custom error code as 1
        ///                 Response message contains error statement
        /// </summary>
        [TestMethod]
        public void GetEmployeesWithInvalidUserId()
        {
            Function function = new Function();
            var APIRequest = RequestBuilder.PathParamsRequest("userId", "LMS");
            var userResponse = function.GetEmployees(APIRequest, null);
            string responseEmpl = Convert.ToString(userResponse.Body);
            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseEmpl);
            Assert.AreEqual(400, userResponse.StatusCode);
            Assert.AreEqual(errorRes.Code, 1);
            StringAssert.Contains(errorRes.Message, "Invalid input");
        }
        
        /// <summary> 
        /// Test to get employees list under a user
        /// [Inputs]        Invalid userId is given i.e., null
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 Response contains custom error code as 1
        ///                 Response message contains error statement
        /// </summary>
        [TestMethod]
        public void GetEmployeesWithNullUserId()
        {
            Function function = new Function();
            var APIRequest = RequestBuilder.PathParamsRequest("userId", null);
            var userResponse = function.GetEmployees(APIRequest, null);
            string responseEmpl = Convert.ToString(userResponse.Body);
            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseEmpl);
            Assert.AreEqual(400, userResponse.StatusCode);
            Assert.AreEqual(errorRes.Code, 1);
            StringAssert.Contains(errorRes.Message, "Invalid input");
        }
        
        /// <summary> 
        /// Test to get employees list under a user
        /// [Inputs]        'userId' is not given as it is mandatory
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 Response contains custom error code as 1
        ///                 Response message contains error statement
        /// </summary>
        [TestMethod]
        public void GetEmployeesWithoutUserId()
        {
            Function function = new Function();
            var APIRequest = RequestBuilder.PathParamsRequest("WorkbookId", "34");
            var userResponse = function.GetEmployees(APIRequest, null);
            string responseEmpl = Convert.ToString(userResponse.Body);
            ErrorResponse errorRes = JsonConvert.DeserializeObject<ErrorResponse>(responseEmpl);
            Assert.AreEqual(400, userResponse.StatusCode);
            Assert.AreEqual(errorRes.Code, 1);
            StringAssert.Contains(errorRes.Message, "Invalid input");
        }

        /*
         * **********************
         * COMMENTED TEST AS IT IS NOT FOR WORKING API
         * 
         * This test checks the selection of query string based on input parameter.
         * Here, we are sending additional parameter 'query' which yields no list with success response.
         * 
         * ***********************
        /// <summary> 
        /// Test to get employees list under a user
        /// Incorrect inputs are given and expected an empty response with success code.
        /// </summary>
        [TestMethod]
        public void GetEmployeesWithQueryParam()
        {
            Function function = new Function();
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "userId", "6" }

            };

            Dictionary<string, string> query = new Dictionary<string, string>
            {
                { "param", "accepteddate=20/11/2017 and storypoints<= 3 or severity=High" }

            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues,
                QueryStringParameters= query
            };
            var userResponse = function.GetEmployees(aPIGatewayProxyRequest, null);
            string responseEmpl = Convert.ToString(userResponse.Body);
            List<EmployeeResponse> empl = JsonConvert.DeserializeObject<List<EmployeeResponse>>(responseEmpl);
            Assert.AreEqual(200, userResponse.StatusCode);
            Assert.IsFalse(empl.Count > 0, "Empty response");
        }
        */
    }
}
