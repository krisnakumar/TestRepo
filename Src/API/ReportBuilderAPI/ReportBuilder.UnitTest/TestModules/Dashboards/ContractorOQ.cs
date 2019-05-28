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
        public void GetOQDashboardForAUser()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.COMPANY_ID, Constants.COMPANY_NAME, Constants.SUSPENDED_QUALIFICATION, Constants.DISQUALIFIED_QUALIFICATION, Constants.ASSIGNED_COMPANY_QUALIFICATION, Constants.COMPLETED_COMPANY_QUALIFICATION, Constants.IN_COMPLETE_COMPANY_QUALIFICATION, Constants.PAST_DUE_COMPANY_QUALIFICATION, Constants.IN_DUE_COMPANY_QUALIFICATION, Constants.TOTAL_COMPANY_EMPLOYEES, Constants.LOCK_OUT_COUNT };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.USERID, "331535", "=", "");
            tasksList.Add(taskFilter1);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            //int userIndex = tasksResponse.FindIndex(x => x.UserId.ToString() == supervisor_company_id);
            //Assert.IsTrue(userIndex >= 0);
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreNotEqual("", tasksResponse[0].CompanyName);
            Assert.AreNotEqual(null, tasksResponse[0].LockoutCount);
            Assert.IsTrue(tasksResponse[0].TotalEmployees >= 0);
        }

        [TestMethod]
        public void GetEmployeesQualificationsUnderACompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.USERID, Constants.COMPANY_ID, Constants.SUSPENDED_QUALIFICATION, Constants.DISQUALIFIED_QUALIFICATION, Constants.ROLE, Constants.EMPLOYEE_NAME, Constants.TOTAL_EMPLOYEES, Constants.ASSIGNED_QUALIFICATION, Constants.COMPLETED_QUALIFICATION, Constants.IN_DUE_QUALIFICATION, Constants.PAST_DUE_QUALIFICATION, Constants.IN_COMPLETE_QUALIFICATION, Constants.LOCK_OUT_COUNT };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            tasksList.Add(taskFilter1);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreNotEqual("", tasksResponse[0].UserId);
            Assert.AreNotEqual(null, tasksResponse[0].LockoutCount);
        }

        [TestMethod]
        public void GetAllQualificationsForACompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.ASSIGNED_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.ASSIGNED, "true", "=", "AND");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreNotEqual("", tasksResponse[0].UserId);
            Assert.AreNotEqual("", tasksResponse[0].TaskCode);
        }

        [TestMethod]
        public void GetQualifiedForACompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.COMPLETED, "true", "=", "AND");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreNotEqual("", tasksResponse[0].UserId);
            Assert.AreEqual(Constants.QUALIFIED, tasksResponse[0].Status.ToUpper());
        }

        [TestMethod]
        public void GetDisqualifiedForACompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.ASSIGNED_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.IN_COMPLETE, "true", "=", "AND");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreNotEqual("", tasksResponse[0].UserId);
            Assert.AreEqual(Constants.NOT_QUALIFIED, tasksResponse[0].Status.ToUpper());
        }

        [TestMethod]
        public void GetLockOutForACompanyHavingLockOutData()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.LOCKOUT_COUNT, "true", "=", "AND");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreNotEqual(null, tasksResponse[0].UnlockDate);
        }

        [TestMethod]
        public void GetLockOutForACompanyWithoutLockOutData()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4738", "=", "");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.LOCKOUT_COUNT, "true", "=", "AND");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count == 0);
        }

        [TestMethod]
        public void GetInDueQualificationsForACompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.IN_DUE, "30", "=", "AND");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreNotEqual(null, tasksResponse[0].ExpirationDate);
        }

        [TestMethod]
        public void GetAllQualificationsForASupervisor()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.ASSIGNED_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.ADMIN_ID, "331535", "=", "AND");
            EmployeeModel taskFilter3 = testExecute.CreateFields(Constants.SUPERVISOR_ID, "370043", "=", "");
            EmployeeModel taskFilter4 = testExecute.CreateFields(Constants.ASSIGNED, "true", "=", "AND");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);
            tasksList.Add(taskFilter3);
            tasksList.Add(taskFilter4);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreEqual(370043, tasksResponse[0].UserId);
        }

        [TestMethod]
        public void GetQualifiedForASupervisor()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.ADMIN_ID, "331535", "=", "AND");
            EmployeeModel taskFilter3 = testExecute.CreateFields(Constants.SUPERVISOR_ID, "370043", "=", "");
            EmployeeModel taskFilter4 = testExecute.CreateFields(Constants.COMPLETED, "true", "=", "AND");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);
            tasksList.Add(taskFilter3);
            tasksList.Add(taskFilter4);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreEqual(370043, tasksResponse[0].UserId);
            Assert.AreEqual(Constants.QUALIFIED, tasksResponse[0].Status.ToUpper());
        }

        [TestMethod]
        public void GetDisqualifiedForASupervisor()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.ASSIGNED_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.ADMIN_ID, "331535", "=", "AND");
            EmployeeModel taskFilter3 = testExecute.CreateFields(Constants.SUPERVISOR_ID, "370043", "=", "");
            EmployeeModel taskFilter4 = testExecute.CreateFields(Constants.IN_COMPLETE, "true", "=", "AND");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);
            tasksList.Add(taskFilter3);
            tasksList.Add(taskFilter4);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreEqual(370043, tasksResponse[0].UserId);
            Assert.AreEqual(Constants.NOT_QUALIFIED, tasksResponse[0].Status.ToUpper());
        }

        [TestMethod]
        public void GetLockOutForASupervisorHavingLockOutData()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            EmployeeModel taskFilter4 = testExecute.CreateFields(Constants.LOCKOUT_COUNT, "true", "=", "AND");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.ADMIN_ID, "331535", "=", "AND");
            EmployeeModel taskFilter3 = testExecute.CreateFields(Constants.SUPERVISOR_ID, "370043", "=", "");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);
            tasksList.Add(taskFilter3);
            tasksList.Add(taskFilter4);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count > 0);
            Assert.AreEqual(370043, tasksResponse[0].UserId);
            Assert.AreNotEqual(null, tasksResponse[0].UnlockDate);
        }

        [TestMethod]
        public void GetLockOutForASupervisorWithoutLockOutData()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            EmployeeModel taskFilter4 = testExecute.CreateFields(Constants.LOCKOUT_COUNT, "true", "=", "AND");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.ADMIN_ID, "331535", "=", "AND");
            EmployeeModel taskFilter3 = testExecute.CreateFields(Constants.SUPERVISOR_ID, "377151", "=", "");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);
            tasksList.Add(taskFilter3);
            tasksList.Add(taskFilter4);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count == 0);
        }

        [TestMethod]
        public void GetInDueQualificationForASupervisor()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE, "REASON" };

            EmployeeModel taskFilter1 = testExecute.CreateFields(Constants.CONTRACTOR_COMPANY, "4065", "=", "");
            EmployeeModel taskFilter2 = testExecute.CreateFields(Constants.ADMIN_ID, "331535", "=", "AND");
            EmployeeModel taskFilter3 = testExecute.CreateFields(Constants.SUPERVISOR_ID, "377151", "=", "");
            EmployeeModel taskFilter4 = testExecute.CreateFields(Constants.IN_DUE, "30", "=", "AND");
            tasksList.Add(taskFilter1);
            tasksList.Add(taskFilter2);
            tasksList.Add(taskFilter3);
            tasksList.Add(taskFilter4);

            TaskResponse taskResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.OQ_DASHBOARD, tasksList);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.IsTrue(tasksResponse.Count == 0);
            Assert.AreEqual(377151, tasksResponse[0].UserId);
            Assert.AreNotEqual(null, tasksResponse[0].ExpirationDate);
        }
    }
}
