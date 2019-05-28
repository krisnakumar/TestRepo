
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
    public class SmartParams
    {
        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190103.6487    //
        //                                                             //
        /////////////////////////////////////////////////////////////////

        string[] DefaultColumnsList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.TOTAL_EMPLOYEES, Constants.EMAIL };
        
        [TestMethod]
        public void GetOnlyLoggedInUserDetails()
        {
            List < EmployeeModel > employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 111,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = DefaultColumnsList,
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERNAME,
                Value = Constants.ME,
                Operator = "="
            };
            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.CURRENT_USER,
                Value = "111",
                Operator = "=",
                Bitwise = "AND"
            };
            employeeList.Add(employeeModel1);
            employeeList.Add(employeeModel2);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            Assert.AreEqual(1, userResponse.Count);
            Assert.IsTrue(userResponse[0].UserId == 111);
            Assert.AreEqual("supervisor", userResponse[0].Role);
            Assert.AreNotEqual("", userResponse[0].AlternateName);
        }

        [TestMethod]
        public void GetLoggedInUserAndSubordinateDetails()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 111,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = DefaultColumnsList,
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERNAME,
                Value = Constants.ME_AND_DIRECT_SUBORDINATES,
                Operator = "="
            };
            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.CURRENT_USER,
                Value = "111",
                Operator = "=",
                Bitwise = "AND"
            };
            employeeList.Add(employeeModel1);
            employeeList.Add(employeeModel2);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            int userIndex = userResponse.FindIndex(x => x.UserId == 111);
            Assert.IsTrue(userIndex >= 0);
            Assert.AreNotEqual(0, userResponse.Count);
        }

        [TestMethod]
        public void GetOnlySubordinateDetails()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 111,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = DefaultColumnsList,
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USERNAME,
                Value = Constants.DIRECT_SUBORDINATES,
                Operator = "="
            };
            EmployeeModel employeeModel2 = new EmployeeModel
            {
                Name = Constants.CURRENT_USER,
                Value = "111",
                Operator = "=",
                Bitwise = "AND"
            };
            employeeList.Add(employeeModel1);
            employeeList.Add(employeeModel2);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            int userIndex = userResponse.FindIndex(x => x.UserId == 111);
            Assert.IsTrue(userIndex < 0);
            Assert.AreNotEqual(0, userResponse.Count);
        }
        
        // Test cases on Date params
        [TestMethod]
        public void GetUserDetailsCreatedBetweenDates()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 111,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = DefaultColumnsList,
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USER_CREATED_DATE,
                Value = "01/01/1985 and 12/31/2018",
                Operator = "Between"
            };
            employeeList.Add(employeeModel1);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            Assert.AreNotEqual(0, userResponse.Count);
        }

        [TestMethod]
        public void GetUserDetailsCreatedBeforeToday()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 111,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = DefaultColumnsList,
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.USER_CREATED_DATE,
                Value = "04/22/2019",
                Operator = "<"
            };
            employeeList.Add(employeeModel1);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            Assert.AreNotEqual(0, userResponse.Count);
        }

        // Test cases on Role params
        [TestMethod]
        public void GetUsersWithSpecificRole()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 111,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = DefaultColumnsList,
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.ROLE,
                Value = Constants.SUPERVISOR,
                Operator = "="
            };
            employeeList.Add(employeeModel1);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            Assert.AreNotEqual(0, userResponse.Count);
            Assert.IsTrue(userResponse[0].Role.ToUpper() == Constants.SUPERVISOR);
        }

        // Test case(s) on user photo availability
        [TestMethod]
        public void GetUsersListWithPhoto()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 111,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = DefaultColumnsList,
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.PHOTO,
                Value = Constants.YES,
                Operator = "="
            };
            employeeList.Add(employeeModel1);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            Assert.AreNotEqual(0, userResponse.Count);
        }

        // Test case(s) on user QR Code availability
        [TestMethod]
        public void GetUsersListWithQRCode()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 111,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = DefaultColumnsList,
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.QR_CODE,
                Value = Constants.YES,
                Operator = "="
            };
            employeeList.Add(employeeModel1);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            Assert.AreNotEqual(0, userResponse.Count);
        }

        // Test case(s) on user Department
        [TestMethod]
        public void GetUsersListHavingDepartment()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 111,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = DefaultColumnsList,
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.DEPARTMENT,
                Value = Constants.YES,
                Operator = "="
            };
            employeeList.Add(employeeModel1);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            Assert.AreNotEqual(0, userResponse.Count);
        }

        //  Smart params combination
        [TestMethod]
        public void SmartParamCombinationTest()
        {
            List<EmployeeModel> employeeList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest employeeRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 111,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = DefaultColumnsList,
                    Fields = employeeList,
                    EntityName = "Employee",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel employeeModel1 = new EmployeeModel
            {
                Name = Constants.DEPARTMENT,
                Value = Constants.YES,
                Operator = "="
            };
            employeeList.Add(employeeModel1);

            EmployeeResponse usersResponse = function.GetEmployeesQueryBuilder(employeeRequest, null);
            List<EmployeeQueryModel> userResponse = usersResponse.Employees;
            Assert.AreNotEqual(0, userResponse.Count);
        }
    }
}
