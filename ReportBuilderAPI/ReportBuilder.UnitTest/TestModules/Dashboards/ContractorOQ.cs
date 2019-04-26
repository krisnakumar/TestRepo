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

namespace ReportBuilder.UnitTest.TestModules.Dashboards
{
    [TestClass]
    public class ContractorOQ
    {
        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190103.6487    //
        //                                                             //
        /////////////////////////////////////////////////////////////////

        [TestMethod]
        public void GetCompanyQualificationsDetails()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.COMPANY_ID, Constants.COMPANY_NAME, Constants.ASSIGNED_COMPANY_QUALIFICATION, Constants.COMPLETED_COMPANY_QUALIFICATION, Constants.IN_COMPLETE_COMPANY_QUALIFICATION, Constants.PAST_DUE_COMPANY_QUALIFICATION, Constants.IN_DUE_COMPANY_QUALIFICATION, Constants.TOTAL_COMPANY_EMPLOYEES, Constants.LOCK_OUT_COUNT },
                    Fields = tasksList,
                    AppType = Constants.OQ_DASHBOARD
                }
            };
            var supervisor_company_id = "6";
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = supervisor_company_id,
                Operator = "=",
                Bitwise = ""
            };
            tasksList.Add(task1);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            //int userIndex = tasksResponse.FindIndex(x => x.UserId.ToString() == supervisor_company_id);
            //Assert.IsTrue(userIndex >= 0);
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreNotEqual("", tasksResponse[0].CompanyName);
            Assert.AreNotEqual(null, tasksResponse[0].LockoutCount);
            Assert.IsTrue(tasksResponse[0].TotalEmployees >= 0);
        }

        [TestMethod]
        public void GetEmployeesQualificationsDetails()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.USERID, Constants.ROLE, Constants.COMPANY_ID, Constants.EMPLOYEE_NAME, Constants.TOTAL_EMPLOYEES, Constants.ASSIGNED_QUALIFICATION, Constants.COMPLETED_QUALIFICATION, Constants.IN_DUE_QUALIFICATION, Constants.PAST_DUE_QUALIFICATION, Constants.IN_COMPLETE_QUALIFICATION },
                    Fields = tasksList,
                    AppType = Constants.OQ_DASHBOARD
                }
            };
            var supervisor_company_id = "6";
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.SUPERVISOR_ID,
                Value = supervisor_company_id,
                Operator = "=",
                Bitwise = ""
            };
            tasksList.Add(task1);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            int userIndex = tasksResponse.FindIndex(x => x.UserId.ToString() == supervisor_company_id);
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreNotEqual("", tasksResponse[0].Role);
            Assert.AreNotEqual("", tasksResponse[0].EmployeeName);
            Assert.IsTrue(userIndex < 0);
            Assert.IsTrue(tasksResponse[0].TotalEmployees >= 0);
        }

        [TestMethod]
        public void GetAssignedQualificationsForCompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.ASSIGNED_DATE },
                    Fields = tasksList,
                    AppType = Constants.OQ_DASHBOARD,
                    EntityName = "Task"
                }
            };

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.AreNotEqual(0, tasksResponse.Count);
            Assert.AreNotEqual(null, tasksResponse[0].TaskId);
            Assert.AreNotEqual(null, tasksResponse[0].TaskName);
            Assert.AreNotEqual(null, tasksResponse[0].TaskCode);
            Assert.AreNotEqual(null, tasksResponse[0].EmployeeName);
            Assert.AreNotEqual("", tasksResponse[0].AssignedDate);
        }

        [TestMethod]
        public void GetPassedQualificationsForCompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE },
                    Fields = tasksList,
                    AppType = Constants.OQ_DASHBOARD,
                    EntityName = "Task"
                }
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.COMPLETED,
                Operator = "=",
                Value = "true"
            };
            tasksList.Add(task1);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.AreNotEqual(0, tasksResponse.Count);
            Assert.IsTrue(tasksResponse[0].TaskId > 0);
            Assert.AreNotEqual(null, tasksResponse[0].TaskName);
            Assert.AreNotEqual(null, tasksResponse[0].TaskCode);
            Assert.AreNotEqual(null, tasksResponse[0].EmployeeName);
            Assert.AreNotEqual("", tasksResponse[0].ExpirationDate);
        }

        [TestMethod]
        public void GetDisqualificationsForCompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.ASSIGNED_DATE },
                    Fields = tasksList,
                    AppType = Constants.OQ_DASHBOARD,
                    EntityName = "Task"
                }
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.IN_COMPLETE,
                Operator = "=",
                Value = "true"
            };
            tasksList.Add(task1);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.AreNotEqual(0, tasksResponse.Count);
            Assert.IsTrue(tasksResponse[0].TaskId > 0);
            Assert.AreNotEqual("", tasksResponse[0].TaskName);
            Assert.AreNotEqual("", tasksResponse[0].TaskCode);
            Assert.AreNotEqual("", tasksResponse[0].EmployeeName);
            Assert.AreNotEqual("", tasksResponse[0].AssignedDate);
        }

        [TestMethod]
        public void GetExpiredQualificationsForCompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE },
                    Fields = tasksList,
                    AppType = Constants.QUERY_BUILDER,
                    EntityName = "Task"
                }
            };

            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.PAST_DUE,
                Operator = "=",
                Value = "30"
            };
            tasksList.Add(task1);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.AreNotEqual(0, tasksResponse.Count);
            Assert.IsTrue(tasksResponse[0].TaskId > 0);
            Assert.AreNotEqual(null, tasksResponse[0].TaskName);
            Assert.AreNotEqual(null, tasksResponse[0].TaskCode);
            Assert.AreNotEqual(null, tasksResponse[0].EmployeeName);
            Assert.AreNotEqual(null, tasksResponse[0].ExpirationDate);
        }

        [TestMethod]
        public void Get30DaysInDueQualificationsForCompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE },
                    Fields = tasksList,
                    AppType = Constants.QUERY_BUILDER,
                    EntityName = "Task"
                }
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.IN_DUE,
                Operator = "=",
                Value = "30"
            };
            tasksList.Add(task1);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.AreNotEqual(0, tasksResponse.Count);
            Assert.IsTrue(tasksResponse[0].TaskId > 0);
            Assert.AreNotEqual(null, tasksResponse[0].TaskName);
            Assert.AreNotEqual(null, tasksResponse[0].TaskCode);
            Assert.AreNotEqual(null, tasksResponse[0].EmployeeName);
            Assert.AreNotEqual(null, tasksResponse[0].ExpirationDate);
        }
    }
}
