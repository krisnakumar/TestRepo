using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using ReportBuilderAPI.Handlers.FunctionHandler;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilder.UnitTest.Helpers;


/*******************************************************************************
<copyright file="Login.cs">
    Copyright (c) 2018 All Rights Reserved
</copyright>
<author>Pawan Kumar</author>
<date>29-Nov-2018</date>
<summary>
    This class covers test cases to login the user.
    It has two mandatory input fields, i.e., UserName and Password
    All tests assert the response based on requested input.
    The class covers following test cases :
        - Correct credentials which yields a well defined success response with tokens
        - Incorrect credentials, which yields an error response
        - A request without mandatory inputs, which yields an error response
</summary>
*********************************************************************************/

namespace ReportBuilder.UnitTest.TestModules.Authentication
{
    [TestClass]
    public class Login
    {
        /// <summary> 
        /// Test to Login the user first time
        /// [Inputs]        Correct credentials (UserName, Password) are given
        /// [Expectations]  A well defined response with tokens and success code
        /// [Assertions]    Success response code as 200
        ///                 Response contains IdentityToken (not null)
        ///                 Validating userId (should not be 0) in response
        ///                 Response body contains custom error code as 0
        /// </summary>
        [TestMethod]
        public void UserLogin()
        {
            Function function = new Function();
            UserRequest userRequest = new UserRequest
            {
                UserName = "shoba.eswar@in.sysvine.com",
                Password = "Demo@2017"
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(userRequest),
            };
            APIGatewayProxyResponse authResponse = function.Login(aPIGatewayProxyRequest, null);
            string responseAuth = Convert.ToString(authResponse.Body);
            UserResponse userLoggedIn = JsonConvert.DeserializeObject<UserResponse>(responseAuth);
            Assert.AreEqual(200, authResponse.StatusCode);
            Assert.IsNotNull( userLoggedIn.IdentityToken);
            Assert.IsTrue(userLoggedIn.UserId > 0, "user id");
            Assert.AreEqual(userLoggedIn.Code, 0);
        }
        
        /// <summary> 
        /// Test to Login the user first time
        /// [Inputs]        Incorrect credentials (UserName, Password) are given
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 Response contains IdentityToken as NULL
        ///                 Response body contains custom error code as 1
        ///                 Response body contains error message
        /// </summary>
        [TestMethod]
        public void LoginWithInvalidCredentials()
        {
            Function function = new Function();
            UserRequest userRequest = new UserRequest
            {
                UserName = "shobha.eswar@in.sysvine.com",
                Password = "Demo@2017#"
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(userRequest),
            };
            APIGatewayProxyResponse authResponse = function.Login(aPIGatewayProxyRequest, null);
            string responseAuth = Convert.ToString(authResponse.Body);
            UserResponse userLoggedIn = JsonConvert.DeserializeObject<UserResponse>(responseAuth);
            Assert.AreEqual(400, authResponse.StatusCode);
            Assert.IsNull(userLoggedIn.IdentityToken);
            Assert.AreEqual(userLoggedIn.Code, 1);
            StringAssert.Contains(userLoggedIn.Message, "Invalid input");
        }

        /// <summary> 
        /// Test to Login the user
        /// [Inputs]        Credentials (UserName, Password) are not given
        /// [Expectations]  An error response with bad request status code
        /// [Assertions]    Response code as 400
        ///                 Response contains IdentityToken as NULL
        ///                 Response body contains custom error code as 1
        ///                 Response body contains error message
        /// </summary>
        [TestMethod]
        public void LoginWithoutCredentials()
        {
            Function function = new Function();
            UserRequest userRequest = new UserRequest { };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(userRequest),
            };
            APIGatewayProxyResponse authResponse = function.Login(aPIGatewayProxyRequest, null);
            string responseAuth = Convert.ToString(authResponse.Body);
            UserResponse userLoggedIn = JsonConvert.DeserializeObject<UserResponse>(responseAuth);
            Assert.AreEqual(400, authResponse.StatusCode);
            Assert.IsNull(userLoggedIn.IdentityToken);
            Assert.AreEqual(userLoggedIn.Code, 1);
            StringAssert.Contains(userLoggedIn.Message, "Invalid input");
        }
    }
}
