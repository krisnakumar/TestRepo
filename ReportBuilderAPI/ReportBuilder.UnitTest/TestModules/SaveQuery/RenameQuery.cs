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
    public class RenameQuery
    {
        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190103.6487    //
        //                                                             //
        /////////////////////////////////////////////////////////////////

        //[TestMethod]
        //public void RenameAQuery()
        //{
        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        UserId = 6,
        //        QueryName = "22-FEB-Employee-Set",
        //        QueryId = "905736be-d8f2-4418-b8c8-c25b57211e4a"
        //    };
        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse queryResponse = function.RenameQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse RenameQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(queryResponse.Body);
        //    Assert.AreEqual(200, queryResponse.StatusCode);
        //    Assert.AreEqual("Query name has been updated successfully!", RenameQueryResponse.Message);
        //}

        //[TestMethod]
        //public void AttemptingToRenameWithEmptyQueryName()
        //{
        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        UserId = 6,
        //        QueryName = "",
        //        QueryId = "905736be-d8f2-4418-b8c8-c25b57211e4a"
        //    };
        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse queryResponse = function.RenameQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse RenameQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(queryResponse.Body);
        //    Assert.AreEqual(400, queryResponse.StatusCode);
        //    Assert.AreEqual("Invalid input: Check inputs", RenameQueryResponse.Message);
        //}

        //[TestMethod]
        //public void AttemptingToRenameWithInvalidQueryId()
        //{
        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        UserId = 6,
        //        QueryName = "Renaming Query",
        //        QueryId = "Its-Query-Id"
        //    };
        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse queryResponse = function.RenameQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse RenameQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(queryResponse.Body);
        //    Assert.AreEqual(400, queryResponse.StatusCode);
        //    Assert.AreEqual("Invalid input: Check inputs", RenameQueryResponse.Message);
        //}

        [TestMethod]
        public void AttemptingToRenameWithExistingName()
        {
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                UserId = 6,
                QueryName = "Employees created between",
                QueryId = "905736be-d8f2-4418-b8c8-c25b57211e4a"
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

            APIGatewayProxyResponse queryResponse = function.RenameQuery(aPIGatewayProxyRequest, null);
            ErrorResponse RenameQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(queryResponse.Body);
            Assert.AreEqual(400, queryResponse.StatusCode);
            Assert.AreEqual("Invalid input: Query Name already exist! Please select different one!", RenameQueryResponse.Message);
        }

        //[TestMethod]
        //public void RenameQueryWithSpecialCharacters()
        //{
        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        UserId = 6,
        //        QueryName = "&*^%!$#;:<,,,,?@~`_+=-",
        //        QueryId = "905736be-d8f2-4418-b8c8-c25b57211e4a"
        //    };
        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse queryResponse = function.RenameQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse RenameQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(queryResponse.Body);
        //    Assert.AreEqual(200, queryResponse.StatusCode);
        //    Assert.AreEqual("Query name has been updated successfully!", RenameQueryResponse.Message);
        //}
    }
}
