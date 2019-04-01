using Amazon.Lambda.APIGatewayEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.FunctionHandler;
using ReportBuilderAPI.Utilities;
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
                UserName = "devtester@its-training.com",
                Password = "Demo@2017"
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(userRequest),
            };
            var userResponse = function.Login(userRequest, null);
            //Assert.AreEqual(200, userResponse.StatusCode);
        }



        [TestMethod]
        public void GetRoles()
        {
            Function function = new Function();
            RoleRequest userRequest = new RoleRequest
            {
                CompanyId=6,
                UserId = 6
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(userRequest),
            };
            var userResponse = function.GetRoles(userRequest, null);
            //Assert.AreEqual(200, userResponse.StatusCode);
        }


        //        [TestMethod]
        //        public void RefreshToken()
        //        {
        //            Function function = new Function();
        //            UserRequest userRequest = new UserRequest
        //            {
        //                UserName = "devtester@its-training.com",

        //                IdToken = "eyJraWQiOiJIbWRVVUFybjRQbWxVRDAyQUw0ZTJtRm5kTzBWN1V5ZzJFSTdpeW1iTUh3PSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiJlZGUyMTQ3ZS0wNmNhLTQ2YmEtODE3Ni1iZmVhNjI3ZGM2ODIiLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6XC9cL2NvZ25pdG8taWRwLnVzLXdlc3QtMi5hbWF6b25hd3MuY29tXC91cy13ZXN0LTJfbnJyWmFvVEFzIiwicGhvbmVfbnVtYmVyX3ZlcmlmaWVkIjp0cnVlLCJjb2duaXRvOnVzZXJuYW1lIjoiZWRlMjE0N2UtMDZjYS00NmJhLTgxNzYtYmZlYTYyN2RjNjgyIiwiYXVkIjoiNGVmb3VnYjhucWo3Zjcya3UxODNydWRtcW0iLCJldmVudF9pZCI6ImFmNGVmOWFjLTRiMGMtMTFlOS1iMjVhLTNmOTY3ZjNhYTdiYiIsInRva2VuX3VzZSI6ImlkIiwiYXV0aF90aW1lIjoxNTUzMDg1MzQ2LCJwaG9uZV9udW1iZXIiOiIrOTE5OTYyMDg1ODQ2IiwiZXhwIjoxNTUzMDg4OTQ2LCJpYXQiOjE1NTMwODUzNDYsImVtYWlsIjoiZGV2dGVzdGVyQGl0cy10cmFpbmluZy5jb20ifQ.dbK9FCfC1f7gUtwGDakHcnqC5O3Q0Lf7oFMCNR9DQJB05HmfiVnRPmWnBssRpV5gTXyNsKfgjlmeW2u7VUNoB6hkQMOdlJpVYuh8zB1qvPBjJknFOBovConL-pIUZ_ngBqOIQVpo9WtOvK4YeHtpDIyv6tWlCGM1rQrcRcsjpNd5tHE8wdFteB4TfEBa1EijOYVFpf3ecmTbAvcXBLyReBato75oC9h26E2Oldf4pkkCeZi9l2ILD3dUumIYTjF-C3ZcS2Si91lscDdOH6SyVeRJyE2YBdhyIqpgHiCBvb9WvSePOey2waXxBoY75ucpDqgKD9IaDiArGN1vSUS94w",
        //                AccessToken= "eyJraWQiOiJoanBkbzVTeDkxTDcyUkYxajRJQVFiQmI3cGJwbEFlYjRYRWJJVXdmTUlBPSIsImFsZyI6IlJTMjU2In0.eyJzdWIiOiJlZGUyMTQ3ZS0wNmNhLTQ2YmEtODE3Ni1iZmVhNjI3ZGM2ODIiLCJldmVudF9pZCI6ImFmNGVmOWFjLTRiMGMtMTFlOS1iMjVhLTNmOTY3ZjNhYTdiYiIsInRva2VuX3VzZSI6ImFjY2VzcyIsInNjb3BlIjoiYXdzLmNvZ25pdG8uc2lnbmluLnVzZXIuYWRtaW4iLCJhdXRoX3RpbWUiOjE1NTMwODUzNDYsImlzcyI6Imh0dHBzOlwvXC9jb2duaXRvLWlkcC51cy13ZXN0LTIuYW1hem9uYXdzLmNvbVwvdXMtd2VzdC0yX25yclphb1RBcyIsImV4cCI6MTU1MzA4ODk0NiwiaWF0IjoxNTUzMDg1MzQ2LCJqdGkiOiIyY2ZhNmNhZC1lN2FmLTQxZjctYjNkNS0xNDhjYzNmMmI0NTciLCJjbGllbnRfaWQiOiI0ZWZvdWdiOG5xajdmNzJrdTE4M3J1ZG1xbSIsInVzZXJuYW1lIjoiZWRlMjE0N2UtMDZjYS00NmJhLTgxNzYtYmZlYTYyN2RjNjgyIn0.y_F8Xazreu95Y9OzcsRsDc-R92X1LtTrzpkNGl-MIak9mQ819_iiwhNfgYytVxSFEmlkbRRArGv_ggM_EPTmi1G_JNH6TL9er3Cefrpk1YsNWqehBlxdgwRAn0AXkyIXujGIKbg8uVqDvZG4rgYn3O7ObedmaTfzodkh3IN7c8DAOtOt2rqY0-kejK_wdnzs8NMo5z0v1QvzlAqBEBfCytyfLnOp6QddPn9BeUX333NRjOPw7PhsUsEsjwylvkTlUcrT6-9GMeJtP9v5DegMIcdvt7TPurADPEmsHdkqnXBfkJVFRJbztLj3T2s3h7WKS2Z-oTsbVEjL8Oor9vRB3w",
        //                RefreshToken="eyJjdHkiOiJKV1QiLCJlbmMiOiJBMjU2R0NNIiwiYWxnIjoiUlNBLU9BRVAifQ.HaGQKOq68qK-e_cFbjc2T0tlm_SEsEwl1RD23syp43b494J2c6xovnqP3LBujXWDBj8x4Z7Ifqt-1d9Sv8QTAn_df-s91yh5NEZnrn_6pdsTzXnVm3N9q6m-sC2OiotBspNBa7muJMi8xXzt2aKgYLldtl9OypGAYF_zcu4fystBBU9iD3WH3GGf95KuwLPrbA0_AmrVOqfyI4dOXh3Qc_TLMBdEgs-RUUfN_NYHd9Z8JJdmSM5NVf0OuPwSIKsBUETcq42pJY13tmCX6mOaSgoyK4qbBXo57Ayhl7uWK7VBsKWBVDj2ZSc55iyDAEhicwAJgoXwKZjqrpeivERtbg.CoNWujtjCc49gH1T.mo0lnj5mqKlqnISaT_u1zaYK26GOVRuyJ028XjctLpQ3FtlMfkoUj2M47nL2uMnRmgIZeKeJ_Ub88Hm0mIXLi0TjQ6GYxXRGlPqoNGxpvxJL0FHbi4kh86fKHlUJOaBHpZkEXPQ6ESM8H_UXPWDdOt42owdjfbyX_SzOm8RiT4wa5j6eaTkzmLqt4X5SlbfPWNfhMQ_Ais5HRYKgSlzidd94_cUjW5fUrFH_MJzb63re862o56xIUR4dGza6Au__jCxFnPkXiVJrd6NwtsACdkMQy0TbQghh3I_vy4sNvL1KVyOxbWwNYCnyfBILR8bwGA6Sopf3bvSizeaHM4v9yYSkcWq-Eqt93kR6jHCZSD5ShXM3tBWHbR366aumnrrN5U88L3xtrn8JN8e4VOwdvvsWgswtG4SVLE2Phi89-m2Jn-sYmsHpIcDYTb8GOadhGYhPqHALcAqzfdqtSBVQv_RlYc_tDeLCegIlPtjqAg_WlTP-Nj1CBMpbWqjLwIDqGINjYLjYclTrftpoIgE4Y_p1iCv6pHtyqqoZ7yEk23P-nymFHJSzwhRR9TEN5R7dZcQZQta8zBy6RShwZM1s8vltUA01Uxu9cgIDPq1FCL7vVOqr0ejYAbQ5XnDUAbcskvl6wpTxo4MK0P7CxQ_ONQYW6s_ychHRXPfbas73jznIBY56EWbZ6J5cf--EfR3i1W4cV120KUUdyEMyPaJraspYwGzeHMKlHkVBeDu8BhlPn7fwU_eqVTlU4V9u6L9S1Y_n_EbX9-YcY89O9Mc1Ddrc5Kq2bvTnRdIYk0FOlp_PakhOjkDHX6Oh2GCPW-ZxabcLuZQC_BICbLH-FmpytcmEptP1k_kpMQW4Z_WdCpleyqDyQkxriGhR_x13g7oJB2UXEyRzfeFweWWYDxMG087biUoiDTGeGMsFyn8HHeYag2_BWUxJQlrL3gj-gUTVR1d9Wqw4BMN4U7IW0BXWwqrBENFC8Xp3mVpwIpHduj5d11w6T_kLUhmFYjZ1zwa3D2vJZjAx3EzL1WbjZ-Ab-wLYarTuBLOcdXBBTIr2RiEe5Suip-K_XRcVnhjYt4xzin-9wUHFj3p2v8YRF9ZZXrKuwygdLvElcRVyT9iWP4Zdw_9tSh6zcqChYd9XoX_MZjjmeX1HLALB7tc4dReA6xFvMDaVVEv69OTpZ4N6jHk7hc6uGb6RQJSyRIcyhdzS2mgmcoOQ6W92Hu-c_Z6XLoROp1mKYIoKQ67QqqW_AbmzboOfHm-ETanXjOFhYFIcKcwjTMuJkn-IJz8xr6nt2A9ZlgHFXokrbuIXOTegF8C-k7-UeJh8PtWtkP0.aqvR-vZ2vxMv-w8O7maHPg"
        //            };

        //            var userResponse = function.SilentAuth(userRequest, null);
        //         //   Assert.AreEqual(userResponse.StatusCode, userResponse.StatusCode);
        //        }



        //        /// <summary>
        //        ///  Test method for DeleteCompany
        //        /// </summary>
        //        [TestMethod]
        //        public void GetEmployeeList()
        //        {

        //            //Function function = new Function();
        //            //Dictionary<string, string> pathValues = new Dictionary<string, string>
        //            //{
        //            //    { "userId", "6" }

        //            //};

        //            //Dictionary<string, string> query = new Dictionary<string, string>
        //            //{
        //            //    { "param", "accepteddate=20/11/2017 and storypoints<= 3 or severity=High" }

        //            //};
        //            //APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //            //{
        //            //    PathParameters = pathValues,
        //            //    QueryStringParameters = query
        //            //};
        //            //APIGatewayProxyResponse userResponse = function.GetEmployees(aPIGatewayProxyRequest, null);
        //            //Assert.AreEqual(200, userResponse.StatusCode);
        //        }


        ///// <summary>
        /////  Test method to get the workbook details
        ///// </summary>
        //[TestMethod]
        //public void GetWorkbookList()
        //{

        //    Function function = new Function();
        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "userId", "10" }

        //    };

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        PathParameters = pathValues
        //    };
        //    APIGatewayProxyResponse userResponse = function.GetWorkbookDetails(aPIGatewayProxyRequest, null);
        //    Assert.AreEqual(200, userResponse.StatusCode);
        //}


        //        ///// <summary>
        //        /////  Test method to get the workbook details
        //        ///// </summary>
        //        //[TestMethod]
        //        //public void GetCompletedWorkbookList()
        //        //{

        //        //    Function function = new Function();
        //        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //        //    {
        //        //        { "userId", "24" }

        //        //    };

        //        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //        //    {
        //        //        PathParameters = pathValues
        //        //    };
        //        //    APIGatewayProxyResponse userResponse = function.GetCompletedWorkbookDetails(aPIGatewayProxyRequest, null);
        //        //    Assert.AreEqual(200, userResponse.StatusCode);
        //        //}


        //        /////  Test method to get the workbook details
        //        ///// </summary>
        //        //[TestMethod]
        //        //public void GetTaskList()
        //        //{

        //        //    Function function = new Function();
        //        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //        //    {
        //        //        { "userId", "18" },
        //        //        {"workbookId","18" }

        //        //    };

        //        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //        //    {
        //        //        PathParameters = pathValues
        //        //    };
        //        //    APIGatewayProxyResponse userResponse = function.GetTaskList(aPIGatewayProxyRequest, null);
        //        //    Assert.AreEqual(200, userResponse.StatusCode);
        //        //}


        //        ///// </summary>
        //        //[TestMethod]
        //        //public void GetTaskAttemptsList()
        //        //{

        //        //    Function function = new Function();
        //        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //        //    {
        //        //        { "userId", "18" },
        //        //        {"workbookId","18" },
        //        //        {"taskId","606" }

        //        //    };





        //        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //        //    {
        //        //        PathParameters = pathValues
        //        //    };
        //        //    APIGatewayProxyResponse userResponse = function.GetTaskAttemptsDetails(aPIGatewayProxyRequest, null);
        //        //    Assert.AreEqual(200, userResponse.StatusCode);
        //        //}


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
                PathParameters = pathValues
            };

            string rr = "{    \"CompanyId\" : \"6\",    \"Payload\" : {\"Fields\":[{\"Value\":\"contractor\",\"Operator\":\"=\",\"Name\":\"ROLE\",\"Bitwise\":\"\"}],\"ColumnList\":[\"EMPLOYEE_NAME\",\"ROLE\",\"USER_ID\",\"USERNAME\",\"EMAIL\",\"ALTERNATE_USERNAME\",\"TOTAL_EMPLOYEES\"],\"AppType\":\"QUERY_BUILDER\"},    \"UserName\" :  \"\",    \"UserId\":\"6\"}";

            aPIGatewayProxyRequest.Body = "{\"Fields\":[{\"Value\":\"ME_AND_DIRECT_SUBORDINATES\",\"Operator\":\"=\",\"Name\":\"USERNAME\",\"Bitwise\":\"\"},{\"Value\":\"6\",\"Operator\":\"=\",\"Name\":\"CURRENT_USER\",\"BitWise\":\"AND\"},{\"Value\":\"Sheila136\",\"Operator\":\"=\",\"Name\":\"USERNAME\",\"Bitwise\":\"or\"}],\"ColumnList\":[\"USER_CREATED_DATE\",\"ROLE\",\"USER_ID\",\"USERNAME\",\"EMAIL\",\"ALTERNATE_USERNAME\",\"TOTAL_EMPLOYEES\"]}";
            var userResponse = function.GetEmployeesQueryBuilder(JsonConvert.DeserializeObject<QueryBuilderRequest>(rr), null);

            //    Assert.AreEqual(200, userResponse.StatusCode);
        }


        //        ///// </summary>
        //        //[TestMethod]
        //        //public void GetWorkbookDetails()
        //        //{
        //        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //        //    Function function = new Function();
        //        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //        //    {
        //        //        ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.WORKBOOK_DUE, Constants.PAST_DUE_WORKBOOK, Constants.TOTAL_EMPLOYEES, Constants.COMPLETED_WORKBOOK },
        //        //        Fields = employeeList
        //        //    };

        //        //    EmployeeModel employeeModel = new EmployeeModel
        //        //    {
        //        //        Name = Constants.SUPERVISOR_ID,
        //        //        Value = "6",
        //        //        Operator = "="
        //        //    };

        //        //    //EmployeeModel employeeModel2 = new EmployeeModel
        //        //    //{
        //        //    //    Name = Constants.USERID,
        //        //    //    Value = "14",
        //        //    //    Operator = "=",
        //        //    //    Bitwise = "and"
        //        //    //};
        //        //    //EmployeeModel employeeModel3 = new EmployeeModel
        //        //    //{
        //        //    //    Name = Constants.PAST_DUE,
        //        //    //    Value = "60",
        //        //    //    Operator = "=",
        //        //    //    Bitwise="and"
        //        //    //};

        //        //    employeeList.Add(employeeModel);
        //        //    //employeeList.Add(employeeModel2);
        //        //    //employeeList.Add(employeeModel3);

        //        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //        //    {
        //        //        { "companyId", "6" }

        //        //    };


        //        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //        //    {
        //        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        //        PathParameters = pathValues
        //        //    };

        //        //    aPIGatewayProxyRequest.Body = "{\"Fields\":[{\"Value\":\"6\",\"Operator\":\"=\",\"Name\":\"SUPERVISOR_ID\",\"Bitwise\":\"\"}],\"ColumnList\":[\"WORKBOOK_ID\",\"WORKBOOK_NAME\",\"DESCRIPTION\",\"WORKBOOK_CREATED_BY\",\"DAYS_TO_COMPLETE\"]}";

        //        //    APIGatewayProxyResponse userResponse = function.GetWorkbookQuerBuilder(aPIGatewayProxyRequest, null);
        //        //    Assert.AreEqual(200, userResponse.StatusCode);
        //        //}



        //        /// </summary>
        //        [TestMethod]
        //        public void GetTaskQueryDetails()
        //        {
        //            List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //            Function function = new Function();



        //            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //            {
        //                ColumnList = new string[] { Constants.COMPANY_ID, Constants.ASSIGNED_COMPANY_QUALIFICATION, Constants.COMPLETED_COMPANY_QUALIFICATION, Constants.IN_COMPLETE_COMPANY_QUALIFICATION, Constants.PAST_DUE_COMPANY_QUALIFICATION, Constants.IN_DUE_COMPANY_QUALIFICATION, Constants.TOTAL_COMPANY_EMPLOYEES, Constants.COMPANY_NAME },
        //                Fields = employeeList
        //            };
        //            EmployeeModel employeeModel = new EmployeeModel
        //            {
        //                Name = Constants.USERID,
        //                Value = "6",
        //                Operator = "="
        //            };



        //            employeeList.Add(employeeModel);
        //            Dictionary<string, string> pathValues = new Dictionary<string, string>
        //            {
        //                { "companyId", "6" }

        //            };

        //            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //            {
        //                Body = JsonConvert.SerializeObject(employeeRequest),
        //                PathParameters = pathValues
        //            };

        //            var req = "{    \"CompanyId\" : \"6\",    \"Payload\" : {\"Fields\":[{\"Value\":\"a\",\"Operator\":\"contains\",\"Name\":\"TASK_NAME\",\"Bitwise\":\"\"}],\"ColumnList\":[\"TASK_ID\",\"TASK_NAME\",\"ASSIGNED_TO\",\"EVALUATOR_NAME\",\"EXPIRATION_DATE\"]},    \"UserName\" :  \"\",    \"UserId\":\"6\"}";
        //            var userResponse = function.GetTaskQueryBuilder(JsonConvert.DeserializeObject<QueryBuilderRequest>(req), null);
        //            //Assert.AreEqual(200, userResponse.StatusCode);
        //        }

        //        //[TestMethod]
        //        //public void GetEmployeesWithMultipleFields()
        //        //{
        //        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();
        //        //    Function function = new Function();

        //        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //        //    {
        //        //        ColumnList = new string[] { "EMPLOYEE_NAME", "ROLE", "USERNAME", "ALTERNATE_USERNAME", "TOTAL_EMPLOYEES", "EMAIL" },
        //        //        Fields = employeeList
        //        //    };

        //        //    EmployeeModel employeeModel1 = new EmployeeModel
        //        //    {
        //        //        Name = "User_Id",
        //        //        Value = "20",
        //        //        Operator = ">"
        //        //    };

        //        //    EmployeeModel employeeModel2 = new EmployeeModel
        //        //    {
        //        //        Name = "User_Id",
        //        //        Value = "14",
        //        //        Operator = "=",
        //        //        Bitwise = "OR"
        //        //    };
        //        //    employeeList.Add(employeeModel1);
        //        //    employeeList.Add(employeeModel2);

        //        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //        //    {
        //        //        { "companyId", "6" }
        //        //    };

        //        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //        //    {
        //        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        //        PathParameters = pathValues
        //        //    };
        //        //    APIGatewayProxyResponse usersResponse = function.GetEmployeesQuerBuilder(aPIGatewayProxyRequest, null);
        //        //    List<EmployeeResponse> userResponse = JsonConvert.DeserializeObject<List<EmployeeResponse>>(usersResponse.Body);
        //        //    Assert.AreEqual(200, usersResponse.StatusCode);
        //        //    Assert.AreNotEqual(1, userResponse.Count);
        //        //    Assert.AreNotEqual("", userResponse[0].Role);
        //        //    Assert.AreNotEqual("", userResponse[0].UserName);
        //        //}


        //        ///// </summary>
        //        //[TestMethod]
        //        //public void SaveQuery()
        //        //{
        //        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //        //    Function function = new Function();
        //        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //        //    {
        //        //        ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
        //        //        Fields = employeeList,
        //        //        EntityName = Constants.EMPLOYEE,
        //        //        QueryName = "Shoba-test"
        //        //    };

        //        //    EmployeeModel employeeModel = new EmployeeModel
        //        //    {
        //        //        Name = Constants.USER_CREATED_DATE,
        //        //        Value = "04/02/1990 and 04/02/2019",
        //        //        Operator = "Between"
        //        //    };
        //        //    EmployeeModel employeeModel2 = new EmployeeModel
        //        //    {
        //        //        Name = Constants.USERID,
        //        //        Value = "6",
        //        //        Operator = "=",
        //        //        Bitwise = "and"
        //        //    };
        //        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //        //    {
        //        //        { "companyId", "6" }

        //        //    };
        //        //    employeeList.Add(employeeModel);
        //        //    employeeList.Add(employeeModel2);

        //        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //        //    {
        //        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        //        PathParameters = pathValues
        //        //    };

        //        //    APIGatewayProxyResponse userResponse = function.SaveQuery(aPIGatewayProxyRequest, null);

        //        //    Assert.AreEqual(400, userResponse.StatusCode);
        //        //}


        //        /////// </summary>
        //        ////[TestMethod]
        //        ////public void GetQueries()
        //        ////{
        //        ////    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //        ////    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //        ////    {
        //        ////        { "companyId", "6" }

        //        ////    };

        //        ////    Function function = new Function();
        //        ////    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //        ////    {
        //        ////        ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
        //        ////        Fields = employeeList,
        //        ////        EntityName = Constants.EMPLOYEE,
        //        ////        QueryName = "Shoba"
        //        ////    };


        //        ////    EmployeeModel employeeModel2 = new EmployeeModel
        //        ////    {
        //        ////        Name = Constants.USERID,
        //        ////        Value = "6",
        //        ////        Operator = "="
        //        ////    };


        //        ////    employeeList.Add(employeeModel2);

        //        ////    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //        ////    {
        //        ////        Body = JsonConvert.SerializeObject(employeeRequest),
        //        ////        PathParameters=pathValues
        //        ////    };

        //        ////    aPIGatewayProxyRequest.Body = "{ \"Fields\": [ { \"Name\": \"USER_ID\", \"Value\": \"6\", \"Operator\": \"=\" } ]}";
        //        ////    APIGatewayProxyResponse userResponse = function.GetQueries(aPIGatewayProxyRequest, null);

        //        ////    Assert.AreEqual(200, userResponse.StatusCode);
        //        ////}


        //        /////// </summary>
        //        ////[TestMethod]
        //        ////public void RenameQuery()
        //        ////{
        //        ////    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //        ////    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //        ////    {
        //        ////        { "companyId", "6" }

        //        ////    };

        //        ////    Function function = new Function();
        //        ////    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //        ////    {
        //        ////        ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
        //        ////        Fields = employeeList,
        //        ////        EntityName = Constants.EMPLOYEE,
        //        ////        QueryName = "Shoba-test-New",
        //        ////        QueryId = "1e23a802-f6fa-4aa8-b77c-186db54ebbce",
        //        ////        UserId=6
        //        ////    };


        //        ////    EmployeeModel employeeModel2 = new EmployeeModel
        //        ////    {
        //        ////        Name = Constants.USERID,
        //        ////        Value = "6",
        //        ////        Operator = "="
        //        ////    };

        //        ////    employeeList.Add(employeeModel2);

        //        ////    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //        ////    {
        //        ////        Body = JsonConvert.SerializeObject(employeeRequest),
        //        ////        PathParameters = pathValues
        //        ////    };

        //        ////    APIGatewayProxyResponse userResponse = function.RenameQuery(aPIGatewayProxyRequest, null);

        //        ////    Assert.AreEqual(500, userResponse.StatusCode);
        //        ////}


        //        //[TestMethod]
        //        //public void DeleteQuery()
        //        //{
        //        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //        //    {
        //        //        { "companyId", "6" }

        //        //    };

        //        //    Function function = new Function();
        //        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //        //    {
        //        //        ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
        //        //        Fields = employeeList,
        //        //        EntityName = Constants.EMPLOYEE,
        //        //        QueryName = "Shoba-test-New",
        //        //        QueryId = "7bf5f925-ec41-49a8-8d97-61e4c4033703"
        //        //    };


        //        //    EmployeeModel employeeModel2 = new EmployeeModel
        //        //    {
        //        //        Name = Constants.USERID,
        //        //        Value = "6",
        //        //        Operator = "="
        //        //    };

        //        //    employeeList.Add(employeeModel2);

        //        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //        //    {
        //        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        //        PathParameters = pathValues
        //        //    };

        //        //    APIGatewayProxyResponse userResponse = function.DeleteQuery(aPIGatewayProxyRequest, null);

        //        //    Assert.AreEqual(400, userResponse.StatusCode);
        //        //}



        //        //[TestMethod]
        //        //public void GetQuery()
        //        //{
        //        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //        //    {
        //        //        { "companyId", "6" }

        //        //    };

        //        //    Function function = new Function();
        //        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //        //    {
        //        //        ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
        //        //        Fields = employeeList,
        //        //        EntityName = Constants.EMPLOYEE,
        //        //        QueryName = "Shoba-test-New",
        //        //        QueryId = "998d83e9-80c3-4b24-840a-a727d1366d0f"
        //        //    };


        //        //    EmployeeModel employeeModel2 = new EmployeeModel
        //        //    {
        //        //        Name = Constants.USERID,
        //        //        Value = "6",
        //        //        Operator = "="
        //        //    };

        //        //    employeeList.Add(employeeModel2);

        //        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //        //    {
        //        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        //        PathParameters = pathValues
        //        //    };

        //        //    APIGatewayProxyResponse userResponse = function.GetQuery(aPIGatewayProxyRequest, null);

        //        //    Assert.AreEqual(200, userResponse.StatusCode);
        //        //}
    }
}
