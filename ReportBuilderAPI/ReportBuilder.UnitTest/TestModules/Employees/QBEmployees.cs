using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using ReportBuilderAPI.Handlers.FunctionHandler;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Utilities;

namespace ReportBuilder.UnitTest.TestModules.Employees
{

    [TestClass]
    public class QBEmployees
    {
        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190103.6487    //
        //                                                             //
        /////////////////////////////////////////////////////////////////

        [TestMethod]
        public void GetEmployeesWithSingleField()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.TOTAL_EMPLOYEES, Constants.EMAIL },
                Fields = employeeList
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "7",
                Operator = "="
            };
            employeeList.Add(employeeModel1);

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
            Assert.AreEqual(1, userResponse.Count);
            Assert.AreEqual("Manager", userResponse[0].Role);
            Assert.AreNotEqual("", userResponse[0].AlternateName);
            StringAssert.Matches(userResponse[0].UserName, new Regex(@"(?i)\Bobby13\b"));
        }

        [TestMethod]
        public void GetEmployeesWithMultipleFields()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.TOTAL_EMPLOYEES, Constants.EMAIL },
                Fields = employeeList
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "7",
                Operator = ">="
            };
            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.USER_CREATED_DATE,
                Value = "01/21/1995",
                Operator = ">",
                Bitwise = "and"
            };
            EmployeeModel employeeModel3 = new EmployeeModel
            {
                Name = Constants.ROLE,
                Value = "manager",
                Operator = "contains",
                Bitwise = "and"
            };
            employeeList.Add(employeeModel1);
            employeeList.Add(employeeModel2);
            employeeList.Add(employeeModel3);

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
            Assert.AreNotEqual(0, userResponse.Count);
            Assert.AreNotEqual("", userResponse[0].UserName);
            StringAssert.Matches(userResponse[0].Role, new Regex(@"(?i)\b(.*?)manager(.*?)\b"));
        }

        [TestMethod]
        public void GetEmployeesWithAdditionalColumns()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.TOTAL_EMPLOYEES, Constants.EMAIL, Constants.USERID, Constants.USER_CREATED_DATE, Constants.TOTAL_EMPLOYEES },
                Fields = employeeList
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "7",
                Operator = ">="
            };
            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.USER_CREATED_DATE,
                Value = "01/21/1995",
                Operator = ">",
                Bitwise = "and"
            };
            EmployeeModel employeeModel3 = new EmployeeModel
            {
                Name = Constants.ROLE,
                Value = "manager",
                Operator = "contains",
                Bitwise = "and"
            };
            employeeList.Add(employeeModel1);
            employeeList.Add(employeeModel2);
            employeeList.Add(employeeModel3);

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
            Assert.AreNotEqual(0, userResponse.Count);
            Assert.AreNotEqual("", userResponse[0].UserName);
            Assert.AreNotSame("Completed", userResponse[0].UserId);
            Assert.AreNotEqual(null, userResponse[0].UserCreatedDate);
            Assert.AreNotEqual(null, userResponse[0].TotalEmployees);
            StringAssert.Matches(userResponse[0].Role, new Regex(@"(?i)\b(.*?)manager(.*?)\b"));
        }

        [TestMethod]
        public void GetEmployeesWithInvalidCompany()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.USERID, Constants.TOTAL_EMPLOYEES, Constants.EMAIL },
                Fields = employeeList
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "7",
                Operator = ">="
            };
            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.USER_CREATED_DATE,
                Value = "01/21/1995",
                Operator = ">",
                Bitwise = "and"
            };
            EmployeeModel employeeModel3 = new EmployeeModel
            {
                Name = Constants.ROLE,
                Value = "manager",
                Operator = "contains",
                Bitwise = "and"
            };
            employeeList.Add(employeeModel1);
            employeeList.Add(employeeModel2);
            employeeList.Add(employeeModel3);

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "8" } // Valid one is 6
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(employeeRequest),
                PathParameters = pathValues
            };
            APIGatewayProxyResponse usersResponse = function.GetEmployeesQuerBuilder(aPIGatewayProxyRequest, null);
            List<EmployeeResponse> userResponse = JsonConvert.DeserializeObject<List<EmployeeResponse>>(usersResponse.Body);
            Assert.AreEqual(200, usersResponse.StatusCode);
            Assert.AreEqual(0, userResponse.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => userResponse[0]);
        }

        [TestMethod]
        public void GetEmployeesWithInvalidInputs()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.USERID, Constants.TOTAL_EMPLOYEES, Constants.EMAIL },
                Fields = employeeList
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "7",
                Operator = ">="
            };
            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.ROLE,
                Value = "user",
                Operator = "contains",
                Bitwise = "and"
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
            Assert.AreEqual(0, userResponse.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => userResponse[0]);
        }

        [TestMethod]
        public void GetEmployeesWithOperatorCombinations()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.USERID, Constants.TOTAL_EMPLOYEES, Constants.EMAIL },
                Fields = employeeList
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "7",
                Operator = ">="
            };
            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.USER_CREATED_DATE,
                Value = "01/21/1995",
                Operator = ">",
                Bitwise = "and"
            };
            EmployeeModel employeeModel3 = new EmployeeModel
            {
                Name = Constants.ROLE,
                Value = "manager",
                Operator = "contains",
                Bitwise = "and"
            };
            EmployeeModel employeeModel4 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "14",
                Operator = "<=",
                Bitwise = "or"
            };
            EmployeeModel employeeModel5 = new EmployeeModel
            {
                Name = Constants.ROLE,
                Value = "manager",
                Operator = "contains",
                Bitwise = "and"
            };
            employeeList.Add(employeeModel1);
            employeeList.Add(employeeModel2);
            employeeList.Add(employeeModel3);
            employeeList.Add(employeeModel4);
            employeeList.Add(employeeModel5);

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
            Assert.AreNotEqual(0, userResponse.Count);
            Assert.AreNotEqual(null, userResponse[0].UserId);
            StringAssert.Matches(userResponse[0].Role, new Regex(@"(?i)\b(.*?)manager(.*?)\b"));
        }

        [TestMethod]
        public void GetEmployeesWithoutColumns()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { },
                Fields = employeeList
            };

            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "7",
                Operator = ">="
            };
            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.USER_CREATED_DATE,
                Value = "01/21/1995",
                Operator = ">",
                Bitwise = "and"
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
            Assert.AreEqual(0, userResponse.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => userResponse[0]);
        }

        // Test for try-catch block

        [TestMethod]
        public void RequestWithoutColumnList()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                Fields = employeeList
            };

            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "7",
                Operator = ">="
            };
            employeeList.Add(employeeModel1);

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
            ErrorResponse empResponse = JsonConvert.DeserializeObject<ErrorResponse>(usersResponse.Body);
            Assert.AreEqual(500, usersResponse.StatusCode);
            Assert.IsTrue(empResponse.Code == 33);
            StringAssert.Matches(empResponse.Message, new Regex(@"(?i)\b(.*?)error(.*?)\b"));
        }
    }
}
