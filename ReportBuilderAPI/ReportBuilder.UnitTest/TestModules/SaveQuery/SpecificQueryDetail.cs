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

namespace ReportBuilder.UnitTest.TestModules.SaveQuery
{
    [TestClass]
    public class SpecificQueryDetail
    {
        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190103.6487    //
        //                                                             //
        /////////////////////////////////////////////////////////////////

        //[TestMethod]
        //public void GetSpecificQueryDetail()
        //{
        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        QueryId = "c6901bc5-7fda-4098-9e25-04af806bab4c"
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

        //    APIGatewayProxyResponse queryResponse = function.GetQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse RenameQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(queryResponse.Body);
        //    Assert.AreEqual(200, queryResponse.StatusCode);
        //    Assert.AreEqual("Query has been deleted succcessfully!", RenameQueryResponse.Message);
        //}

        [TestMethod]
        public void GetSpecificQueryWithEmptyQueryId()
        {
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                QueryId = ""
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

            APIGatewayProxyResponse queryResponse = function.GetQuery(aPIGatewayProxyRequest, null);
            ErrorResponse RenameQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(queryResponse.Body);
            Assert.AreEqual(400, queryResponse.StatusCode);
            Assert.AreEqual("Invalid input: QueryId", RenameQueryResponse.Message);
        }

        //[TestMethod]
        //public void GetSpecificQueryWithInvalidQueryId()
        //{
        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        QueryId = "ITS-Delete-Query"
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

        //    APIGatewayProxyResponse queryResponse = function.GetQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse RenameQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(queryResponse.Body);
        //    Assert.AreEqual(400, queryResponse.StatusCode);
        //    Assert.AreEqual("Invalid input: Please check input", RenameQueryResponse.Message);
        //}
    }
}
