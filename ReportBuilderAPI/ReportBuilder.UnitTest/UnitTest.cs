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

                RefreshToken = "eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.a40fcGx7A6aqc9_uXPLG_oqXDTstqDdxMCe7gipIEP3isUxQuM-e_aGgrMGxo9HZ_PBS1I2t9GV-mRf5AakZTMlZJqfYiPIhbPU9AeSUfM32k3Vx191bHPKkNpkOzj20-o7xzUI1rfF0r8x1It-V1sfyp54ybAtOh2ZbodujtVCWKekF6tCtxt4SdLCSjIPTRGNs2QDWe8AV_5JhDybIWuqDe3-Wj3J3T3GXyHYJNKMOdH1UvF8m4BH7r5MU0SWbR8cy80Ll0Cco2CwbzMCKDs-BoWH9AwfZZrqjHQZe6L8bB1EhwiSO7EmBNaKivw_AcfjvlLaN3RAO14dYiSOj9Q.RllyQxB-i2DGAPco.58FX3agH0re38YxenRZoJUy7FV4b1iYXzb6AWBqG1_T8LyCcXlc_W7PzDLXOrceQirewmi8xP_uxYl4bnrFvgGC9O-s16nlLvmQuOrWedfi02LNR4fCUUcNQz_U9uz3nAfIWt-uf98SBuJ7If2WbwtK2NqFjAyGXRujGDxEqxw5aQ3P9wSxVHNdJZPTao8aG4VHlOP9zbaAEmLt_ZEgZ2tbhurwHlEatPDFhoRh_As1a3owNvBDOOVLIUR1VNObKGXFpr7ZQW8bUHM2LN8WWd9bNBoy8t54g7PHBuHNf7h0VlPdZNFGDbLSsAW0U3WM8yV501WPgReBjNOB8y-wTJIBdLXok1OQtD6kbb5ixzro3BiV7WeY-c8FBI_IugFMODZaEQ3k0Fhgg_cU86wvW1L-tl7A9VM0vT9qVVjYdAC2OkLfKYFeHW1CBMDWD_GiZ_tq494bwWpFshVORA2SmkG05uFEBlwDKfwrAXuOoJp6Zc2Z86v456RkeCK5hTDwjtq0l8peXoJNQCpMinN07Aw9_sN9pP-noRI3XTSKZ0XTKEUJtkLOfQrOupV7P8dOc4SxffgnUc9Rf1Qruku-3_jaUZ71QTm_z9AriF-QCrXT33lX7eDKpYeCBAIBKJp1Pa3HT6O3tJ6e4O-4TG6j-XzRr0D2GNaZudmR4UEpy-jrlgJwjZB1UGON4iCJ2xyWEx6dIpeVD-da8TJcYdoamMO2y2Hmu2vPEtsNGTFTCoFga_3PQObFvsKIzv_PyEsyVWgqGbit2GOc6aOXE8uFhrlOnbp0hfKxzKRisUwKUrQBcERZvUa1mEL0dEwRLqwADkxIyn2pwvK-iWDaEfG4suCE0vBj6u56DO-2kDWTKf7Slq6ZeUQYW_epHcZFtiVH8iNIE4hmI6ygTe_XJDzf3HjHssrdyPDmImT3_EwUbCGDukNf-OEuk9R3poQPo_SI90qqj9xHVhA6SX8eHapPPApKCgdcimoVHNurwrsdRUefVQt-UIZy4xkAx7DQ-nufOBSgekWoWFrL9L2BzMo8mHXTeteFBaD6e-6TyUBVmezUwNnBXXT6QTpyBlIpSRfR1kjTllLqXvruVftZwa4r3OL26DXZu18KdsmfV2k_OOrTiFbrrAYot5prVjhETRpEDrYA3Y7V36zt1BuujoHf7vo664-t2Nv01yREr2Gautdei9_-T2S9J4uhvCJULLgYMPdx8NfaxuJ9DSVp_q4f5JMZaW_pjpq05KHOqBGqrbDdYtOe1GOnlhZlA1CV7NRGnvppWEuJE4xsQsGJJMWhQmf42qgtVPRRdMzmx36RJykRrwrP3NAHrO7FWj5U.URj_nE2qRt2cx147jAhypg"
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


            employeeList.Add(employeeModel);


            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest)
            };

            aPIGatewayProxyRequest.Body = "{\"Fields\":[{\"Value\":\"1\",\"Operator\":\"!=\",\"Name\":\"USER_ID\",\"Bitwise\":\"\"},{\"Value\":\"ME\",\"Operator\":\"=\",\"Name\":\"USERNAME\",\"Bitwise\":\"or\"},{\"Value\":\"6\",\"Operator\":\"=\",\"Name\":\"USER_ID\",\"BitWise\":\"AND\"}],\"ColumnList\":[\"EMPLOYEE_NAME\",\"ROLE\",\"USER_ID\",\"USERNAME\",\"ALTERNATE_USERNAME\",\"TOTAL_EMPLOYEES\",\"EMAIL\"]}";
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

            aPIGatewayProxyRequest.Body = "{\"Fields\":[{\"Name\":\"USER_ID\",\"Value\":\"6\",\"Operator\":\"=\"}],\"ColumnList\":[\"USER_ID\",\"COMPANY_NAME\",\"COMPANY_ID\",\"ASSIGNED_COMPANY_QUALIFICATION\",\"COMPLETED_COMPANY_QUALIFICATION\",\"IN_COMPLETE_COMPANY_QUALIFICATION\",\"PAST_DUE_COMPANY_QUALIFICATION\",\"IN_DUE_COMPANY_QUALIFICATION\",\"TOTAL_COMPANY_EMPLOYEES\"]}";

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

            aPIGatewayProxyRequest.Body = "{\"Fields\": [{\"Name\": \"TASK_ID\",\"Value\": \"6\",\"Operator\": \">\"}],\"ColumnsList\": [\"TASK_ID\", \"TASK_NAME\", \"ASSIGNED_TO\", \"EVALUATOR_NAME\", \"EXPIRATION_DATE\"]}";
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
                QueryName = "Shoba"
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
                PathParameters=pathValues
            };

            APIGatewayProxyResponse userResponse = function.SaveQuery(aPIGatewayProxyRequest, null);

            Assert.AreEqual(200, userResponse.StatusCode);
        }


        /// </summary>
        [TestMethod]
        public void GetQueries()
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
                QueryName = "Shoba"
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
                PathParameters=pathValues
            };

            aPIGatewayProxyRequest.Body = "{ \"Fields\": [ { \"Name\": \"USER_ID\", \"Value\": \"6\", \"Operator\": \"=\" } ]}";
            APIGatewayProxyResponse userResponse = function.GetQueries(aPIGatewayProxyRequest, null);

            Assert.AreEqual(200, userResponse.StatusCode);
        }


        /// </summary>
        [TestMethod]
        public void RenameQuery()
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

            APIGatewayProxyResponse userResponse = function.RenameQuery(aPIGatewayProxyRequest, null);

            Assert.AreEqual(200, userResponse.StatusCode);
        }


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

            Assert.AreEqual(200, userResponse.StatusCode);
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
