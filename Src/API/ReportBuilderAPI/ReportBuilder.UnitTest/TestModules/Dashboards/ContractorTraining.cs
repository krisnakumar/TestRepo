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
    public class ContractorTraining
    {
        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190103.6487    //
        //                                                             //
        /////////////////////////////////////////////////////////////////

        [TestMethod]
        public void GetTrainingProgressByRole()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 2288,
                UserId = 331535,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.COMPLETED_ROLE_QUALIFICATION, Constants.NOT_COMPLETED_ROLE_QUALIFICATION, Constants.ROLE_ID, Constants.TRAINING_ROLE },
                    Fields = tasksList,
                    AppType = Constants.TRAINING_DASHBOARD
                }
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.IS_SHARED,
                Value = "1",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.ADMIN_ID,
                Value = "331535",
                Operator = "=",
                Bitwise = "AND"
            };
            tasksList.Add(task1);
            tasksList.Add(task2);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
        }

        [TestMethod]
        public void GetIncompleteCompaniesForARole()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 2288,
                UserId = 331535,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.COMPLETED_COMPANY_USERS, Constants.NOT_COMPLETED_COMPANY_USERS, Constants.TOTAL_COMPLETED_COMPANY_USERS, Constants.COMPANY_NAME, Constants.ROLE_ID, Constants.COMPANY_ID },
                    Fields = tasksList,
                    AppType = Constants.TRAINING_DASHBOARD
                }
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.ROLE_ID,
                Value = "28649",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.ADMIN_ID,
                Value = "331535",
                Operator = "=",
                Bitwise = "AND"
            };
            EmployeeModel task3 = new EmployeeModel
            {
                Name = Constants.STATUS,
                Value = Constants.NOT_COMPLETED_COMPANY_USERS,
                Operator = "=",
                Bitwise = "AND"
            };
            tasksList.Add(task1);
            tasksList.Add(task2);
            tasksList.Add(task3);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreEqual(28649, tasksResponse[0].RoleId);
        }

        [TestMethod]
        public void GetIncompleteUsersForACompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 2288,
                UserId = 331535,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { "USER_ID", Constants.COMPANY_ID, Constants.EMPLOYEE_NAME, Constants.ASSIGNED_COMPANY_QUALIFICATION, Constants.COMPLETED_COMPANY_QUALIFICATION, Constants.IN_COMPLETE_COMPANY_QUALIFICATION },
                    Fields = tasksList,
                    AppType = Constants.TRAINING_DASHBOARD
                }
            };
            EmployeeModel task4 = new EmployeeModel
            {
                Name = Constants.CONTRACTOR_COMPANY,
                Value = "2128",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.ROLE_ID,
                Value = "28649",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.ADMIN_ID,
                Value = "331535",
                Operator = "=",
                Bitwise = "AND"
            };
            EmployeeModel task3 = new EmployeeModel
            {
                Name = Constants.STATUS,
                Value = Constants.IN_COMPLETE,
                Operator = "=",
                Bitwise = "AND"
            };
            tasksList.Add(task4);
            tasksList.Add(task1);
            tasksList.Add(task2);
            tasksList.Add(task3);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreEqual(28649, tasksResponse[0].RoleId);
            Assert.AreEqual(2128, tasksResponse[0].CompanyId); 
        }

        [TestMethod]
        public void GetIncompleteTasksForAUser()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 2288,
                UserId = 331535,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_NAME, Constants.TASK_CODE, Constants.STATUS },
                    Fields = tasksList,
                    AppType = Constants.TRAINING_DASHBOARD
                }
            };
            EmployeeModel task5 = new EmployeeModel
            {
                Name = "USER_ID",
                Value = "375520",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task4 = new EmployeeModel
            {
                Name = Constants.CONTRACTOR_COMPANY,
                Value = "2128",
                Operator = "=",
                Bitwise = "AND"
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.ROLE_ID,
                Value = "28649",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.ADMIN_ID,
                Value = "331535",
                Operator = "=",
                Bitwise = "AND"
            };
            EmployeeModel task3 = new EmployeeModel
            {
                Name = Constants.STATUS,
                Value = Constants.IN_COMPLETE,
                Operator = "=",
                Bitwise = "AND"
            };
            tasksList.Add(task5);
            tasksList.Add(task4);
            tasksList.Add(task1);
            tasksList.Add(task2);
            tasksList.Add(task3);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreEqual(28649, tasksResponse[0].RoleId);
            Assert.AreEqual(2128, tasksResponse[0].CompanyId);
            Assert.AreEqual(375520, tasksResponse[0].UserId);
            Assert.IsTrue(tasksResponse[0].Status.ToUpper() == Constants.NOT_QUALIFIED);
        }

        [TestMethod]
        public void GetCompletedCompaniesProgress()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 2288,
                UserId = 331535,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.COMPLETED_COMPANY_USERS, Constants.NOT_COMPLETED_COMPANY_USERS, Constants.TOTAL_COMPLETED_COMPANY_USERS, Constants.COMPANY_NAME, Constants.ROLE_ID, Constants.COMPANY_ID },
                    Fields = tasksList,
                    AppType = Constants.TRAINING_DASHBOARD
                }
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.ROLE_ID,
                Value = "28649",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.ADMIN_ID,
                Value = "331535",
                Operator = "=",
                Bitwise = "AND"
            };
            EmployeeModel task3 = new EmployeeModel
            {
                Name = Constants.STATUS,
                Value = Constants.COMPLETED_COMPANY_USERS,
                Operator = "=",
                Bitwise = "AND"
            };
            tasksList.Add(task1);
            tasksList.Add(task2);
            tasksList.Add(task3);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreEqual(28649, tasksResponse[0].RoleId);
        }

        [TestMethod]
        public void GetCompletedUsersProgress()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 2288,
                UserId = 331535,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { "USER_ID", Constants.COMPANY_ID, Constants.EMPLOYEE_NAME, Constants.ASSIGNED_COMPANY_QUALIFICATION, Constants.COMPLETED_COMPANY_QUALIFICATION, Constants.IN_COMPLETE_COMPANY_QUALIFICATION },
                    Fields = tasksList,
                    AppType = Constants.TRAINING_DASHBOARD
                }
            };
            EmployeeModel task4 = new EmployeeModel
            {
                Name = Constants.CONTRACTOR_COMPANY,
                Value = "2128",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.ROLE_ID,
                Value = "28649",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.ADMIN_ID,
                Value = "331535",
                Operator = "=",
                Bitwise = "AND"
            };
            EmployeeModel task3 = new EmployeeModel
            {
                Name = Constants.STATUS,
                Value = Constants.COMPLETED,
                Operator = "=",
                Bitwise = "AND"
            };
            tasksList.Add(task4);
            tasksList.Add(task1);
            tasksList.Add(task2);
            tasksList.Add(task3);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreEqual(28649, tasksResponse[0].RoleId);
            Assert.AreEqual(2128, tasksResponse[0].CompanyId);
        }

        [TestMethod]
        public void GetCompletedTasksForAUser()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 2288,
                UserId = 331535,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_NAME, Constants.TASK_CODE, Constants.STATUS },
                    Fields = tasksList,
                    AppType = Constants.TRAINING_DASHBOARD
                }
            };
            EmployeeModel task5 = new EmployeeModel
            {
                Name = "USER_ID",
                Value = "373769",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task4 = new EmployeeModel
            {
                Name = Constants.CONTRACTOR_COMPANY,
                Value = "2128",
                Operator = "=",
                Bitwise = "AND"
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.ROLE_ID,
                Value = "28649",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.ADMIN_ID,
                Value = "331535",
                Operator = "=",
                Bitwise = "AND"
            };
            EmployeeModel task3 = new EmployeeModel
            {
                Name = Constants.STATUS,
                Value = Constants.COMPLETED,
                Operator = "=",
                Bitwise = "AND"
            };
            tasksList.Add(task5);
            tasksList.Add(task4);
            tasksList.Add(task1);
            tasksList.Add(task2);
            tasksList.Add(task3);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreEqual(28649, tasksResponse[0].RoleId);
            Assert.AreEqual(2128, tasksResponse[0].CompanyId);
            Assert.AreEqual(375520, tasksResponse[0].UserId);
            Assert.IsTrue(tasksResponse[0].Status.ToUpper() == Constants.QUALIFIED);
        }

        [TestMethod]
        public void EmptyListCompletedUsersForACompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 2288,
                UserId = 331535,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { "USER_ID", Constants.COMPANY_ID, Constants.EMPLOYEE_NAME, Constants.ASSIGNED_COMPANY_QUALIFICATION, Constants.COMPLETED_COMPANY_QUALIFICATION, Constants.IN_COMPLETE_COMPANY_QUALIFICATION },
                    Fields = tasksList,
                    AppType = Constants.TRAINING_DASHBOARD
                }
            };
            EmployeeModel task4 = new EmployeeModel
            {
                Name = Constants.CONTRACTOR_COMPANY,
                Value = "4424",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.ROLE_ID,
                Value = "28649",
                Operator = "=",
                Bitwise = ""
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.ADMIN_ID,
                Value = "331535",
                Operator = "=",
                Bitwise = "AND"
            };
            EmployeeModel task3 = new EmployeeModel
            {
                Name = Constants.STATUS,
                Value = Constants.COMPLETED,
                Operator = "=",
                Bitwise = "AND"
            };
            tasksList.Add(task4);
            tasksList.Add(task1);
            tasksList.Add(task2);
            tasksList.Add(task3);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count == 0);
        }
    }
}
