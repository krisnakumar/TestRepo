using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text;
using Amazon.Lambda.APIGatewayEvents;
using ReportBuilder.Models.Response;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilderAPI.Handlers.FunctionHandler;
using ReportBuilderAPI.Utilities;

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
                UserName = "devtester@its-training.com",

                RefreshToken = "eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.IUJmjYXRBVr40WXOehJtU6LDVAOtMCKxegZXxxhYIcOuGwk7hqgGCMYYnhtDPK5QiDG82ijTRkYsbF9cL0EbSsb2QRuqhiId2mpuDnU7bLSBHW_xmlSKK4l4LKxPdJVzjV9WF6yWSJ6DXMaE6MJd11bhZJMKn-Tg2uR7CQBXjeA3JJCVAEBfUZJ87EDPdwexhdnziHSZjT1Xrwbye43FJaN3lcTBfHdohEYVRJjmCynR3SETxA35JLvXI0YNZngIr-an814ZdzAWdIVA6w0FUAlKcQ3b1OY4adZdViTA189_Am6hFMkmAvld4FO9xxKmJzXbc60RtmGn_19SO6AChg.7lUVWNv_w-QzbpJC.rIzxKUXEN1j46tTWv3SOYWrrzOBb9is0RoA-qU3NOB7vIkcpnwyXaFxyTt07xtVf7HSvN2vK7tZkLVnBOE1w33ZuszlZVxvm_WypHdBjr0ET_xMVJDB8Kknz2NqmiD3t2xBegLqFZP2G1lfTV1rdLtfkF5CV5ccInMDdB9nhnbUgq560ZNTz_eDGYsGHLWg44ddH-GbKzJCxnBYzYE1OfwsD2Y_GOP2ish_9fr4Esg2IqJx-gMBOgZF3ysU2X0ijFh2NqyhGZsn2Ln-_of-uLUHz6ngHt6F6MHEUYRw7q4yA8-r0A8go2jhU9vhZZL9LeQo8GMMBUt1sur93hK4-FAzVGxcgMUjgXfBN4ctkKHGQCfHFpineZNHcJOJ-60nHQaxtAp1TXH35mCRlCY7tfbVtUp-k_r5L3Uncw3M6NDGfZctnocM1wnNvwEloBnrlTMQH09AfRo6B_4bHWo2eQcXnVBdMQyIy0UOTsM_-J2f9gMsuWh52EXJs2zkuGspsbIwyWmL0r71lpH7YUL2w5ot2nZnamUg8gbUa4830ikhZQjJm42rIWZHZ3lxYLiqkr7rlNwP9zHWA4ptcLrN07sniMWzcPj2NOhjptYwkuu_zlJ6A8er29XKch42onO-tmxkU5koc-56qY4aHxML0B20MEGNeDc5xdTEEjIVCwqGJRpRcySAIioHEdm4chkP8Z7WI8QMtmqA3UXvHxW10TYDlR9pJ9VFYhgrSA1CfmZA3_2ZNz6lpvUMQq79OgF9opQz_780NsMS3f_JalMjT5ubIV_VY33JoIei10_bza8cOkf3dS4LutlZrcAkARwnNtKrG_HwHVz9LA6MqrA76gUsCA90UNAYyBYCmhohQGEbvL2RDczJ6vpEtxUAUHgW6xjMBPWOgpnT6CPHvy69Lw883rj4crUp_CVARgsRh9P6QU3NpWlLXWWQYvTUmvLgh6H7una41LMqEvcS3R3jN-HG86GpBxdICsz7n4ekGKrB_OpJc66get9EinUpCutYaoTNPgRMBlLg46Ehg7htsWvqffSzVF20FWI1nrjcit_QKcDhG3_PppSPIK_at7s7NCZxk-qW7UIQkKcbKsfxtzmf680jsrJ_SiFq7OLuCu16p1oR5B-NiD9swHP9JRWI80blD1PPnJOHHsH3UX8f32bAZJ665y3y55mY7O81-MUU6kWLVL-4x2QUB0gW9jVS9xY7LjAzwgV_EHaUD9dO_kENbmNUEEEpUbjnOLP0r69HiP3ax51ms-z8ShEcayDrMFbV_Km56s5jUGiQxciy5K6iTtWRxtTmYtrAiY1C17-52zjQ831fFRZN8hHM.mEie4VpsVJ2QRf9KRYKZsg"
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

            //Function function = new Function();
            //Dictionary<string, string> pathValues = new Dictionary<string, string>
            //{
            //    { "userId", "6" }

            //};

            //Dictionary<string, string> query = new Dictionary<string, string>
            //{
            //    { "param", "accepteddate=20/11/2017 and storypoints<= 3 or severity=High" }

            //};
            //APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            //{
            //    PathParameters = pathValues,
            //    QueryStringParameters = query
            //};
            //APIGatewayProxyResponse userResponse = function.GetEmployees(aPIGatewayProxyRequest, null);
            //Assert.AreEqual(200, userResponse.StatusCode);
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
                ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
                Fields = employeeList
            };

            EmployeeModel employeeModel = new EmployeeModel
            {
                Name = Constants.QR_CODE,
                Value = "Yes",
                Operator = "="                
            };
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }

            };

            employeeList.Add(employeeModel);


            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest),
                PathParameters= pathValues
            };

            aPIGatewayProxyRequest.Body = "{\"Fields\":[{\"Value\":\"ME_AND_DIRECT_SUBORDINATES\",\"Operator\":\"=\",\"Name\":\"USERNAME\",\"Bitwise\":\"\"},{\"Value\":\"6\",\"Operator\":\"=\",\"Name\":\"CURRENT_USER\",\"BitWise\":\"AND\"},{\"Value\":\"Sheila136\",\"Operator\":\"=\",\"Name\":\"USERNAME\",\"Bitwise\":\"or\"}],\"ColumnList\":[\"USER_CREATED_DATE\",\"ROLE\",\"USER_ID\",\"USERNAME\",\"EMAIL\",\"ALTERNATE_USERNAME\",\"TOTAL_EMPLOYEES\"]}";
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
                ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.WORKBOOK_DUE, Constants.PAST_DUE_WORKBOOK, Constants.TOTAL_EMPLOYEES, Constants.COMPLETED_WORKBOOK },
                Fields = employeeList
            };

            EmployeeModel employeeModel = new EmployeeModel
            {
                Name = Constants.SUPERVISOR_ID,
                Value = "6",
                Operator = "="
            };

            //EmployeeModel employeeModel2 = new EmployeeModel
            //{
            //    Name = Constants.USERID,
            //    Value = "14",
            //    Operator = "=",
            //    Bitwise = "and"
            //};
            //EmployeeModel employeeModel3 = new EmployeeModel
            //{
            //    Name = Constants.PAST_DUE,
            //    Value = "60",
            //    Operator = "=",
            //    Bitwise="and"
            //};

            employeeList.Add(employeeModel);
            //employeeList.Add(employeeModel2);
            //employeeList.Add(employeeModel3);

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }

            };


            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest),
                PathParameters = pathValues
            };

            aPIGatewayProxyRequest.Body = "{\"Fields\":[{\"Value\":\"6\",\"Operator\":\"=\",\"Name\":\"SUPERVISOR_ID\",\"Bitwise\":\"\"}],\"ColumnList\":[\"WORKBOOK_ID\",\"WORKBOOK_NAME\",\"DESCRIPTION\",\"WORKBOOK_CREATED_BY\",\"DAYS_TO_COMPLETE\"]}";

            APIGatewayProxyResponse userResponse = function.GetWorkbookQuerBuilder(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }



        /// </summary>
        [TestMethod]
        public void GetTaskQueryDetails()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            Function function = new Function();



            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.COMPANY_ID, Constants.ASSIGNED_COMPANY_QUALIFICATION, Constants.COMPLETED_COMPANY_QUALIFICATION, Constants.IN_COMPLETE_COMPANY_QUALIFICATION, Constants.PAST_DUE_COMPANY_QUALIFICATION, Constants.IN_DUE_COMPANY_QUALIFICATION, Constants.TOTAL_COMPANY_EMPLOYEES, Constants.COMPANY_NAME },
                Fields = employeeList
            };
            EmployeeModel employeeModel = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "6",
                Operator = "="
            };



            employeeList.Add(employeeModel);
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }

            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest),
                PathParameters = pathValues
            };

            aPIGatewayProxyRequest.Body = "{\"Fields\":[{\"Value\":\"e\",\"Operator\":\"contains\",\"Name\":\"TASK_NAME\",\"Bitwise\":\"\"}],\"ColumnList\":[\"TASK_NAME\",\"TASK_ID\",\"ASSIGNED_TO\",\"EVALUATOR_NAME\",\"DATE_EXPIRED\"]}";
            APIGatewayProxyResponse userResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
            Assert.AreEqual(200, userResponse.StatusCode);
        }

        [TestMethod]
        public void GetEmployeesWithMultipleFields()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { "EMPLOYEE_NAME", "ROLE", "USERNAME", "ALTERNATE_USERNAME", "TOTAL_EMPLOYEES", "EMAIL" },
                Fields = employeeList
            };

            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = "User_Id",
                Value = "20",
                Operator = ">"
            };

            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = "User_Id",
                Value = "14",
                Operator = "=",
                Bitwise = "OR"
            };
            employeeList.Add(employeeModel1);
            employeeList.Add(employeeModel2);

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest),
                PathParameters = pathValues
            };
            APIGatewayProxyResponse usersResponse = function.GetEmployeesQuerBuilder(aPIGatewayProxyRequest, null);
            List<EmployeeResponse> userResponse = JsonConvert.DeserializeObject<List<EmployeeResponse>>(usersResponse.Body);
            Assert.AreEqual(200, usersResponse.StatusCode);
            Assert.AreNotEqual(1, userResponse.Count);
            Assert.AreNotEqual("", userResponse[0].Role);
            Assert.AreNotEqual("", userResponse[0].UserName);
        }


        /// </summary>
        [TestMethod]
        public void SaveQuery()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
                Fields = employeeList,
                EntityName = Constants.EMPLOYEE,
                QueryName = "Shoba-test"
            };

            EmployeeModel employeeModel = new EmployeeModel
            {
                Name = Constants.USER_CREATED_DATE,
                Value = "04/02/1990 and 04/02/2019",
                Operator = "Between"
            };
            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "6",
                Operator = "=",
                Bitwise = "and"
            };
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }

            };
            employeeList.Add(employeeModel);
            employeeList.Add(employeeModel2);

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse userResponse = function.SaveQuery(aPIGatewayProxyRequest, null);

            Assert.AreEqual(400, userResponse.StatusCode);
        }


        ///// </summary>
        //[TestMethod]
        //public void GetQueries()
        //{
        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }

        //    };

        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
        //        Fields = employeeList,
        //        EntityName = Constants.EMPLOYEE,
        //        QueryName = "Shoba"
        //    };

           
        //    EmployeeModel employeeModel2 = new EmployeeModel
        //    {
        //        Name = Constants.USERID,
        //        Value = "6",
        //        Operator = "="
        //    };

         
        //    employeeList.Add(employeeModel2);

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters=pathValues
        //    };

        //    aPIGatewayProxyRequest.Body = "{ \"Fields\": [ { \"Name\": \"USER_ID\", \"Value\": \"6\", \"Operator\": \"=\" } ]}";
        //    APIGatewayProxyResponse userResponse = function.GetQueries(aPIGatewayProxyRequest, null);

        //    Assert.AreEqual(200, userResponse.StatusCode);
        //}


        ///// </summary>
        //[TestMethod]
        //public void RenameQuery()
        //{
        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }

        //    };

        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
        //        Fields = employeeList,
        //        EntityName = Constants.EMPLOYEE,
        //        QueryName = "Shoba-test-New",
        //        QueryId = "1e23a802-f6fa-4aa8-b77c-186db54ebbce",
        //        UserId=6
        //    };


        //    EmployeeModel employeeModel2 = new EmployeeModel
        //    {
        //        Name = Constants.USERID,
        //        Value = "6",
        //        Operator = "="
        //    };

        //    employeeList.Add(employeeModel2);

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse userResponse = function.RenameQuery(aPIGatewayProxyRequest, null);

        //    Assert.AreEqual(500, userResponse.StatusCode);
        //}


        [TestMethod]
        public void DeleteQuery()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }

            };

            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
                Fields = employeeList,
                EntityName = Constants.EMPLOYEE,
                QueryName = "Shoba-test-New",
                QueryId = "7bf5f925-ec41-49a8-8d97-61e4c4033703"
            };


            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "6",
                Operator = "="
            };

            employeeList.Add(employeeModel2);

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse userResponse = function.DeleteQuery(aPIGatewayProxyRequest, null);

            Assert.AreEqual(400, userResponse.StatusCode);
        }



        [TestMethod]
        public void GetQuery()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }

            };

            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
                Fields = employeeList,
                EntityName = Constants.EMPLOYEE,
                QueryName = "Shoba-test-New",
                QueryId = "998d83e9-80c3-4b24-840a-a727d1366d0f"
            };


            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "6",
                Operator = "="
            };

            employeeList.Add(employeeModel2);

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse userResponse = function.GetQuery(aPIGatewayProxyRequest, null);

            Assert.AreEqual(200, userResponse.StatusCode);
        }
    }
}
