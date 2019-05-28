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
using ReportBuilder.UnitTest.Utilities;

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
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.TOTAL_EMPLOYEES, Constants.EMAIL },
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "27",
                Operator = "="
            };
            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.ROLE,
                Value = "contractor",
                Operator = "="
            };
            EmployeeModel employeeModel5 = new EmployeeModel
            {
                Name = Constants.ROLE,
                Value = "Admin",
                Operator = "=",
                Bitwise = "AND"
            };
            EmployeeModel employeeModel3 = new EmployeeModel
            {
                Name = Constants.SUPERVISOR_ID,
                Value = "6",
                Operator = "="
            };
            EmployeeModel employeeModel4 = new EmployeeModel
            {
                Name = Constants.QR_CODE,
                Value = "no",
                Operator = "="
            };
            employeeList.Add(employeeModel1);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            Assert.AreEqual(1, userResponse.Count);
            Assert.IsTrue(userResponse[0].UserId == 27);
            StringAssert.Contains(userResponse[0].Role, "Manager");
        }

        [TestMethod]
        public void GetEmployeesWithMultipleFields()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.TOTAL_EMPLOYEES, Constants.EMAIL },
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
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

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
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
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.TOTAL_EMPLOYEES, Constants.EMAIL, Constants.USERID, Constants.USER_CREATED_DATE },
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
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
            
            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            Assert.AreNotEqual(0, userResponse.Count);
            Assert.AreNotEqual("", userResponse[0].UserName);
            Assert.AreNotSame("Completed", userResponse[0].UserId);
            //Assert.AreNotEqual(null, userResponse[0].UserCreatedDate);
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
                CompanyId = 8, // Valid one is 6
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.USERID, Constants.TOTAL_EMPLOYEES, Constants.EMAIL },
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
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

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            ErrorResponse userResponse = usersResponse.Error;
            Assert.AreEqual(403, userResponse.Status);
            Assert.AreEqual(14, userResponse.Code);
            StringAssert.Contains(userResponse.Message, TestConstants.PERMISSION_DENIED);
        }

        [TestMethod]
        public void GetEmployeesWithInvalidInputs()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.USERID, Constants.TOTAL_EMPLOYEES, Constants.EMAIL },
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
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

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
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
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.USERID, Constants.TOTAL_EMPLOYEES, Constants.EMAIL },
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
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

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            Assert.AreNotEqual(0, userResponse.Count);
            Assert.AreNotEqual(null, userResponse[0].UserId);
            StringAssert.Matches(userResponse[0].Role, new Regex(@"(?i)\b(.*?)manager(.*?)\b"));
        }

        [TestMethod]
        public void GetEmployeesWithEmptyColumnsList()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { },
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "7",
                Operator = ">="
            };
            employeeList.Add(employeeModel1);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            ErrorResponse usrResponse = usersResponse.Error;
            Assert.AreEqual(500, usrResponse.Status);
            Assert.AreEqual(33, usrResponse.Code);
            StringAssert.Contains(usrResponse.Message, TestConstants.SYSTEM_ERROR);
        }
      
        [TestMethod]
        public void RequestWithoutColumnsList()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "7",
                Operator = ">="
            };
            employeeList.Add(employeeModel1);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            ErrorResponse userResponse = usersResponse.Error;
            Assert.AreEqual(500, userResponse.Status);
            Assert.AreEqual(33, userResponse.Code);
            StringAssert.Contains(userResponse.Message, TestConstants.SYSTEM_ERROR);
        }
    }
}
