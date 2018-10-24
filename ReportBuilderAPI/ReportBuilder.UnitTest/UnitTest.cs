using Amazon.Lambda.APIGatewayEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ReportBuilder.Models.Request;
using ReportBuilderAPI.Handlers.FunctionHandler;
using System.Collections.Generic;

namespace ReportBuilder.UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Login()
        {
            Function function = new Function();
            UserRequest userRequest = new UserRequest
            {
                UserName = "ITS",
                Password = "Demo@2017"
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(userRequest),
            };
            var userResponse = function.Login(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }



        /// <summary>
        ///  Test method for DeleteCompany
        /// </summary>
        [TestMethod]
        public void GetEmployeeList()
        {

            Function function = new Function();
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "userId", "6" }

            };

            Dictionary<string, string> query = new Dictionary<string, string>
            {
                { "PastDueWorkBook", "90" }

            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues,
                QueryStringParameters= query
            };
            var userResponse = function.GetEmployees(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }


        /// <summary>
        ///  Test method to get the workbook details
        /// </summary>
        [TestMethod]
        public void GetWorkbookList()
        {

            Function function = new Function();
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "userId", "10" }

            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues
            };
            var userResponse = function.GetWorkbookDetails(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }
    }
}
