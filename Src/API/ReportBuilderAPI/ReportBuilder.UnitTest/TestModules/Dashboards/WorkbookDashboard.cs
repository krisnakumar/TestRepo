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
using ReportBuilder.UnitTest.Utilities;
using ReportBuilderAPI.Utilities;

namespace ReportBuilder.UnitTest.TestModules.Dashboards
{
    [TestClass]
    public class WorkbookDashboard
    {
        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190103.6487    //
        //                                                             //
        /////////////////////////////////////////////////////////////////
        
        [TestMethod]
        public void GetDashboardForAUser()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.USERID, Constants.SUPERVISOR_ID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.WORKBOOK_DUE, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES };

            EmployeeModel wbFilter1 = testExecute.CreateFields(Constants.SUPERVISOR_ID, "331535", "=", "");
            EmployeeModel wbFilter2 = testExecute.CreateFields(Constants.CURRENT_USER, "331535", "=", "AND");
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);

            WorkbookResponse wbResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.WORKBOOK_DASHBOARD, wbList);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.IsTrue(workbookList.Count > 0);
        }

        [TestMethod]
        public void GetAssignedWorkbooksForAUser()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.USERID, Constants.WORKBOOK_ID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.WORKBOOK_NAME, Constants.COMPLETED_TASK, Constants.DUE_DATE, Constants.TOTAL_TASK };

            EmployeeModel wbFilter1 = testExecute.CreateFields(Constants.SUPERVISOR_ID, "384658", "=", "");
            EmployeeModel wbFilter2 = testExecute.CreateFields(Constants.USERID, "384658", "=", "AND");
            EmployeeModel wbFilter3 = testExecute.CreateFields(Constants.ASSIGNED_WORKBOOK, "true", "=", "AND");
            EmployeeModel wbFilter4 = testExecute.CreateFields(Constants.CURRENT_USER, "331535", "=", "AND");
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);
            wbList.Add(wbFilter3);
            wbList.Add(wbFilter4);

            WorkbookResponse wbResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.WORKBOOK_DASHBOARD, wbList);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.IsTrue(workbookList.Count > 0);
        }

        [TestMethod]
        public void GetInDueWorkbooksForAUser()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.USERID, Constants.WORKBOOK_ID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.WORKBOOK_NAME, Constants.COMPLETED_TASK, Constants.DUE_DATE, Constants.TOTAL_TASK };

            EmployeeModel wbFilter1 = testExecute.CreateFields(Constants.SUPERVISOR_ID, "384658", "=", "");
            EmployeeModel wbFilter2 = testExecute.CreateFields(Constants.USERID, "384658", "=", "AND");
            EmployeeModel wbFilter3 = testExecute.CreateFields(Constants.WORKBOOK_IN_DUE, "30", "=", "AND");
            EmployeeModel wbFilter4 = testExecute.CreateFields(Constants.CURRENT_USER, "331535", "=", "AND");
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);
            wbList.Add(wbFilter3);
            wbList.Add(wbFilter4);

            WorkbookResponse wbResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.WORKBOOK_DASHBOARD, wbList);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.IsTrue(workbookList.Count > 0);
        }

        [TestMethod]
        public void GetPastDueWorkbooksForAUser()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.USERID, Constants.WORKBOOK_ID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.WORKBOOK_NAME, Constants.COMPLETED_TASK, Constants.DUE_DATE, Constants.TOTAL_TASK };

            EmployeeModel wbFilter1 = testExecute.CreateFields(Constants.SUPERVISOR_ID, "384658", "=", "");
            EmployeeModel wbFilter2 = testExecute.CreateFields(Constants.USERID, "384658", "=", "AND");
            EmployeeModel wbFilter3 = testExecute.CreateFields(Constants.PAST_DUE, "30", "=", "AND");
            EmployeeModel wbFilter4 = testExecute.CreateFields(Constants.CURRENT_USER, "331535", "=", "AND");
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);
            wbList.Add(wbFilter3);
            wbList.Add(wbFilter4);

            WorkbookResponse wbResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.WORKBOOK_DASHBOARD, wbList);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.IsTrue(workbookList.Count > 0);
        }

        [TestMethod]
        public void GetTasksProgressForAWorkbook()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.USERID, Constants.WORKBOOK_ID, Constants.TASK_ID, Constants.TASK_CODE, Constants.TASK_NAME, Constants.COMPLETED_TASK, Constants.INCOMPLETE_TASK, Constants.TOTAL_TASK };
            
            EmployeeModel wbFilter1 = testExecute.CreateFields(Constants.USERID, "384666", "=", "");
            EmployeeModel wbFilter2 = testExecute.CreateFields(Constants.WORKBOOK_ID, "22", "=", "AND");
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);

            WorkbookResponse wbResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.WORKBOOK_DASHBOARD, wbList);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.IsTrue(workbookList.Count > 0);
        }

        [TestMethod]
        public void GetRepititionsDetailForACompletedTask()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.NUMBER_OF_ATTEMPTS, Constants.STATUS, Constants.LAST_ATTEMPT_DATE, Constants.LOCATION, Constants.EVALUATOR_NAME, Constants.COMMENTS };

            EmployeeModel wbFilter1 = testExecute.CreateFields(Constants.USERID, "384666", "=", "");
            EmployeeModel wbFilter2 = testExecute.CreateFields(Constants.STATUS, Constants.COMPLETED, "=", "AND");
            EmployeeModel wbFilter4 = testExecute.CreateFields(Constants.WORKBOOK_ID, "22", "=", "AND");
            EmployeeModel wbFilter3 = testExecute.CreateFields(Constants.TASK_ID, "32931", "=", "AND");
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);
            wbList.Add(wbFilter3);
            wbList.Add(wbFilter4);

            WorkbookResponse wbResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.WORKBOOK_DASHBOARD, wbList);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.IsTrue(workbookList.Count > 0);
            Assert.IsTrue(Int32.Parse(workbookList[0].NumberofAttempts) > 0);
            Assert.AreEqual(Constants.COMPLETED, workbookList[0].Status.ToUpper());
        }

        [TestMethod]
        public void RepititionsDetailForUnattemptedTask()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            TestExecution testExecute = new TestExecution();
            string[] ColumnList = new string[] { Constants.NUMBER_OF_ATTEMPTS, Constants.STATUS, Constants.LAST_ATTEMPT_DATE, Constants.LOCATION, Constants.EVALUATOR_NAME, Constants.COMMENTS };

            EmployeeModel wbFilter1 = testExecute.CreateFields(Constants.USERID, "384666", "=", "");
            EmployeeModel wbFilter2 = testExecute.CreateFields(Constants.STATUS, Constants.COMPLETED, "=", "AND");
            EmployeeModel wbFilter4 = testExecute.CreateFields(Constants.WORKBOOK_ID, "22", "=", "AND");
            EmployeeModel wbFilter3 = testExecute.CreateFields(Constants.TASK_ID, "32934", "=", "AND");
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);
            wbList.Add(wbFilter3);
            wbList.Add(wbFilter4);

            WorkbookResponse wbResponse = testExecute.ExecuteTests(2288, 331535, ColumnList, Constants.WORKBOOK_DASHBOARD, wbList);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.IsTrue(workbookList.Count == 0);
        }
    }
}
