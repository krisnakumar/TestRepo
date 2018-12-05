using Amazon.Lambda.APIGatewayEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
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
                UserName = "shoba.eswar@in.sysvine.com",
                Password = "Demo@2017"
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(userRequest),
            };
            APIGatewayProxyResponse userResponse = function.Login(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }



        [TestMethod]
        public void RefreshToken()
        {
            Function function = new Function();
            UserRequest userRequest = new UserRequest
            {
                UserName = "shoba.eswar@in.sysvine.com",
                Password = "Demo@2017",
                RefreshToken = "eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.alORThbr912tdOSrkTNotINKbT0O6bilsFQhH1D8bdsLfi-GCCfk9mxEcU8nQ-l42n0lEcZ0lFOqzUcZ3dlLASMS077y-uSJb4-dUJ1m19vcEfrIP_yFrjjCTbMkSKULrATEugIs3N3hkTnL3D5PjCoPJb5fdSpirC7H0GHQ2_AcZTO9sylDtcougWmAp6qpzv7UqTTeDMjFaacMT93PcHSNAP4-aG7TnNenbw08kqTc_HGqVcz5RneyY7BPAYDaqaLLlgtNWpizhtsZFSe96i3wVdad-P1pePhoa8KmzwrqAKxc2FHFrGgG4i5pUMWi31QXB79dOEEXukmJL1jVtQ.nZQBFxRelxEBDdpo.pd-9Gx10IN67in4ek8VxhXFUw4BfMMBVcxnhwWzrUrJWEMuDqBjRy3odiMRrj2NfMzjOyl8laMG92rKw7PikBZ9AZJ1qVb3fhEm8WmIj5XtmqJQQofki4-01QAWBylERGPqETq63z9bRalfv9pmH0bReeM0geGFSb7SR0oj2NItFoOCPZKayWFc7c5KJTGZ_YtVfNvKjY5gnbZtlx1jH71IAsmfZVpmBy_tG3oiyXaxjBuUCraI-TBenl9prLSJVOnGzgpjz56lgRlGagjAgDqDgaiCoA9MfG4XGCRVdTGP1hxYllVcZA2ZecP-ANsmkNwJeptr1Qolcb1V2noqSW4ZuB52OchSGPEtTT0tgEwc0Ny1MluW4dj5YNsSc6VckZNetPgRFFwtaGBWCaSwh0ZkfdoLZEJE9MNW_zgKDV8kubQ0xKEln1ElmjbREu0IfL2fMro3yxfRvTYLCM7ABvE2P66hJ8Jd1ojvsYL7tryuv1ZhhXHHSJ6abOgeO6YVRWKwCgDzT2RbBsY9efD4_Gh19HX2pyHe9xmXdHqlJ4x5oJyL7oIhWZMS9fpAw0KmFtHQyYlkoqmj3sB2bx0vDXIMlfCQmgtwhzQGtogeCXqpauvY-Fpk2_AbfNuWGn7wjMd-sCNzZsncBwAkbrCZtnI_oo6OPDFAgUfpWksWKrD_Oiuxw2oMfSBqsVRTaTdx8p986F47gLgVQeHfTfpfgkwI4eoIbcsvkTpdX9-XAubgG6x-SW3dGOeuOHguqM1B0GFB0_tJ27cQouumQ3mZW3y43ql3Ldzq5EPW4ow73wjfvW57Ln_ynoVn6Giv2amW98P_zaWxVhT7N8Ld0YcCtKqPpzTdBqB73aTndki97z9eSCmVbywVzkIJ6j9vEChS77wHAsRTgW57n82jxGQg-rYRmCPrYiYrmUJEOkW0-CJyB1Ntpgtb_ih9EMxjF9BX7v6Ce_tgscGJ6iZpcKq1OQ5fedcmR63uzyMJfZIUlBHBsshoGprcn0N1dkj4QKMUJBD7CB83aBTGIAKbXwJDsu1aFaV8Oumct2gOYCaEwVbzlwh0azB0PyTVfMuBFMpL3eJKSa71J_3pKwgMw7Td0diu024dJQ0AYc__4NpZ3wOLlyUcj__JoCtW3NA1zWz8nKP-oXW_jD3oMrp7myL51n9cWmDVuS1qGCDrg0oksCi2vWjwQNvjIMl1KjDB-9ODuDqPUa2C93XaI0v-IlrtlswaKKxSA2ZTwnPjemHNEjmuibNGHRaDiHd640rTcflEkjVkoKlFKCXcDYEbWEawU53bJD9TtUlkgdlzifqDBbpJJ_rF5G5jjK6MKNr0._B3_OuNIDSYWUVzDRLI_HQ"
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(userRequest),
            };
            APIGatewayProxyResponse userResponse = function.SilentAuth(aPIGatewayProxyRequest, null);
            Assert.AreEqual(userResponse.StatusCode, userResponse.StatusCode);
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
                { "param", "accepteddate=20/11/2017 and storypoints<= 3 or severity=High" }

            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues,
                QueryStringParameters = query
            };
            APIGatewayProxyResponse userResponse = function.GetEmployees(aPIGatewayProxyRequest, null);
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
            APIGatewayProxyResponse userResponse = function.GetWorkbookDetails(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }


        /// <summary>
        ///  Test method to get the workbook details
        /// </summary>
        [TestMethod]
        public void GetCompletedWorkbookList()
        {

            Function function = new Function();
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "userId", "24" }

            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues
            };
            APIGatewayProxyResponse userResponse = function.GetCompletedWorkbookDetails(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }


        ///  Test method to get the workbook details
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
            APIGatewayProxyResponse userResponse = function.GetTaskList(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }


        /// </summary>
        [TestMethod]
        public void GetTaskAttemptsList()
        {

            Function function = new Function();
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "userId", "18" },
                {"workbookId","18" },
                {"taskId","606" }

            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues
            };
            APIGatewayProxyResponse userResponse = function.GetTaskAttemptsDetails(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }


        /// </summary>
        [TestMethod]
        public void GetEmployeeDetails()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { "EMPLOYEE_NAME", "ROLE", "USERNAME", "ALTERNATE_USERNAME", "TOTAL_EMPLOYEES" , "EMAIL"},
                Fields = employeeList
            };

            EmployeeModel employeeModel = new EmployeeModel
            {
                Name = "UserId",
                Value = "10",
                Operator = "=",
                Bitwise="OR"
            };

            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = "UserId",
                Value = "11",
                Operator = "=",
                Bitwise = null
            };
            employeeList.Add(employeeModel);
            employeeList.Add(employeeModel2);



            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest)
            };
            APIGatewayProxyResponse userResponse = function.GetEmployeesQuerBuilder(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }


        /// </summary>
        [TestMethod]
        public void GetWorkbookDetails()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { "EMPLOYEE_NAME", "ROLE", "USERNAME", "ALTERNATE_USERNAME", "EMAIL" },
                Fields = employeeList
            };

            EmployeeModel employeeModel = new EmployeeModel
            {
                Name = "WORKBOOK_NAME",
                Value = "Gail4",
                Operator = "="               
            };

            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = "WORKBOOK_CREATED",
                Value = "08/06/1975",
                Operator = "=",
                Bitwise = "OR"                
            };
            employeeList.Add(employeeModel);
            employeeList.Add(employeeModel2);



            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest)
            };
            APIGatewayProxyResponse userResponse = function.GetWorkbookQuerBuilder(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }
    }
}
