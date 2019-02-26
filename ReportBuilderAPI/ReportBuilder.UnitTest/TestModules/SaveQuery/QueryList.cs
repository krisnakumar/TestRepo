using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using ReportBuilderAPI.Handlers.FunctionHandler;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Utilities;

namespace ReportBuilder.UnitTest.TestModules.SaveQuery
{
    [TestClass]
    public class QueryList
    {
        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190103.6487    //
        //                                                             //
        /////////////////////////////////////////////////////////////////

        [TestMethod]
        public void RequestAllSavedQueries()
        {
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                UserId = 6
            };
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse queryResponse = function.GetQueries(aPIGatewayProxyRequest, null);
            List<QueryResponse> saveQueryList = JsonConvert.DeserializeObject<List<QueryResponse>>(queryResponse.Body);
            Assert.AreEqual(200, queryResponse.StatusCode);
            Assert.IsTrue(saveQueryList.Count > 0);
        }

        [TestMethod]
        public void RequestSavedQueriesWithoutUserId()
        {
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                //UserId = 6
            };
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse queryResponse = function.GetQueries(aPIGatewayProxyRequest, null);
            ErrorResponse errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(queryResponse.Body);
            Assert.AreEqual(400, queryResponse.StatusCode);
            StringAssert.Contains(errorResponse.Message, "Invalid input: UserId");
        }

        [TestMethod]
        public void RequestSavedQueriesWithInvalidCompanyId()
        {
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                UserId = 6
            };
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "8" } // Correct one is 6
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse queryResponse = function.GetQueries(aPIGatewayProxyRequest, null);
            List<QueryResponse> saveQueryList = JsonConvert.DeserializeObject<List<QueryResponse>>(queryResponse.Body);
            Assert.AreEqual(200, queryResponse.StatusCode);
            Assert.AreEqual(0, saveQueryList.Count);
        }
    }
}
