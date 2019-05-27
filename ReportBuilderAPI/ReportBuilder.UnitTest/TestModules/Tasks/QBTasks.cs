using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ReportBuilder.Models.Models;
using ReportBuilderAPI.Handlers.FunctionHandler;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Utilities;
using ReportBuilder.UnitTest.TestModules.Utilities;

namespace ReportBuilder.UnitTest.TestModules.Tasks
{
    [TestClass]
    public class QBTasks
    {
        ///////////////////////////////////////////////////////////////
        //                                                           //
        //  All test cases are tested upto build v0.9.190118.6595    //
        //                                                           //
        ///////////////////////////////////////////////////////////////

        [TestMethod]
        public void GetTasksWithSingleField()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest {
                    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, Constants.ASSIGNED_TO, Constants.EVALUATOR_NAME, Constants.EXPIRATION_DATE },
                    Fields = tasksList,
                    AppType = Constants.QUERY_BUILDER,
                    EntityName = "Task"
                }
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.TASK_ID,
                Value = "90",
                Operator = "="
            };
            tasksList.Add(task1);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List <TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.AreNotEqual(0, tasksResponse.Count);
            Assert.AreEqual(90, tasksResponse[0].TaskId);
            Assert.AreNotEqual("", tasksResponse[0].TaskName);
            Assert.IsTrue(tasksResponse[0].EvaluatorName != "");
            
        }

        [TestMethod]
        public void GetTasksForSpecificUserId()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.USERID, Constants.TASK_ID, Constants.TASK_NAME, Constants.ASSIGNED_TO, Constants.EVALUATOR_NAME, Constants.EXPIRATION_DATE },
                    Fields = tasksList,
                    AppType = Constants.QUERY_BUILDER,
                    EntityName = "Task"
                }
            };            
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "10",
                Operator = "="
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.STUDENT_DETAILS,
                Value = "10",
                Operator = "=",
                Bitwise = "AND"
            };
            tasksList.Add(task1);
            tasksList.Add(task2);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.AreNotEqual(0, tasksResponse.Count);
            Assert.AreEqual(10, tasksResponse[0].UserId);
        }

        [TestMethod]
        public void GetTasksWithMultipleFields()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, Constants.ASSIGNED_TO, Constants.EVALUATOR_NAME, Constants.EXPIRATION_DATE },
                    Fields = tasksList,
                    AppType = Constants.QUERY_BUILDER,
                    EntityName = "Task"
                }
            };

            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.TASK_ID,
                Value = "19",
                Operator = ">="
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.EVALUATOR_NAME,
                Value = "nn",
                Operator = "contains",
                Bitwise = "AND"
            };
            EmployeeModel task3 = new EmployeeModel
            {
                Name = Constants.DATE_TAKEN,
                Value = "11/21/1991",
                Operator = ">=",
                Bitwise = "AND"
            };
            tasksList.Add(task1);
            tasksList.Add(task2);
            tasksList.Add(task3);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.AreNotEqual(0, tasksResponse.Count);
            Assert.AreNotEqual("", tasksResponse[0].TaskId);
            Assert.IsTrue(tasksResponse[0].TaskId >= 19);
            StringAssert.Matches(tasksResponse[0].EvaluatorName, new Regex(@"(?i)\b(.*?)nn(.*?)\b"));
        }

        [TestMethod]
        public void GetTasksWithOperatorCombinations()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, Constants.ASSIGNED_TO, Constants.EVALUATOR_NAME, Constants.EXPIRATION_DATE },
                    Fields = tasksList,
                    AppType = Constants.QUERY_BUILDER,
                    EntityName = "Task"
                }
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.TASK_ID,
                Value = "119",
                Operator = ">="
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.EVALUATOR_NAME,
                Value = "nn",
                Operator = "contains",
                Bitwise = "AND"
            };
            EmployeeModel task3 = new EmployeeModel
            {
                Name = Constants.DATE_TAKEN,
                Value = "01/21/2000",
                Operator = "<",
                Bitwise = "AND"
            };
            EmployeeModel task4 = new EmployeeModel
            {
                Name = Constants.STATUS,
                Value = "completed",
                Operator = "=",
                Bitwise = "OR"
            };
            EmployeeModel task5 = new EmployeeModel
            {
                Name = Constants.TASK_NAME,
                Value = "e",
                Operator = "does not contain",
                Bitwise = "AND"
            };
            EmployeeModel task6 = new EmployeeModel
            {
                Name = Constants.EVALUATOR_NAME,
                Value = "nn",
                Operator = "contains",
                Bitwise = "AND"
            };
            tasksList.Add(task1);
            tasksList.Add(task2);
            tasksList.Add(task3);
            tasksList.Add(task4);
            tasksList.Add(task5);
            tasksList.Add(task6);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.AreNotEqual(0, tasksResponse.Count);
            Assert.AreNotEqual("", tasksResponse[0].TaskId);
            //StringAssert.Matches(tasksResponse[0].EvaluatorName, new Regex(@"(?i)\b(.*?)nn(.*?)\b"));
        }

        [TestMethod]
        public void GetTasksWithAdditionalColumns()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, Constants.STATUS, Constants.EVALUATOR_NAME, Constants.ASSIGNED_TO, Constants.DATE_TAKEN, Constants.NUMBER_OF_ATTEMPTS, Constants.LOCATION },
                    Fields = tasksList,
                    AppType = Constants.QUERY_BUILDER,
                    EntityName = "Task"
                }
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.STATUS,
                Value = "completed",
                Operator = "="
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.TASK_NAME,
                Value = "Mechanical",
                Operator = "does not contain",
                Bitwise = "AND"
            };
            EmployeeModel task3 = new EmployeeModel
            {
                Name = Constants.EVALUATOR_NAME,
                Value = "en",
                Operator = "contains",
                Bitwise = "AND"
            };
            tasksList.Add(task1);
            tasksList.Add(task2);
            tasksList.Add(task3);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.AreNotEqual(0, tasksResponse.Count);
            Assert.AreNotEqual(null, tasksResponse[0].TaskId);
            Assert.AreEqual("Completed", tasksResponse[0].Status);
            Assert.AreNotEqual(null, tasksResponse[0].NumberofAttempts);
            Assert.AreNotEqual(null, tasksResponse[0].Location);
            StringAssert.Matches(tasksResponse[0].EvaluatorName, new Regex(@"(?i)\b(.*?)en(.*?)\b"));
        }

        [TestMethod]
        public void GetTasksForUnsubscribedCompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 8, // Valid one is 6
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, "ITSLMS", Constants.EVALUATOR_NAME, Constants.ASSIGNED_TO },
                    Fields = tasksList,
                    AppType = Constants.QUERY_BUILDER,
                    EntityName = "Task"
                }
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.STATUS,
                Value = "failed",
                Operator = "="
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.TASK_NAME,
                Value = "e",
                Operator = "does not contain",
                Bitwise = "AND"
            };
            EmployeeModel task3 = new EmployeeModel
            {
                Name = Constants.EVALUATOR_NAME,
                Value = "an",
                Operator = "contains",
                Bitwise = "AND"
            };
            tasksList.Add(task1);
            tasksList.Add(task2);
            tasksList.Add(task3);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            ErrorResponse taskList = taskResponse.Error;
            Assert.AreEqual(403, taskList.Status);
            Assert.AreEqual(14, taskList.Code);
            StringAssert.Contains(taskList.Message, TestConstants.PERMISSION_DENIED);
        }

        [TestMethod]
        public void GetTasksWithInvalidInputs()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, Constants.ASSIGNED_TO, Constants.EVALUATOR_NAME, Constants.EXPIRATION_DATE },
                    Fields = tasksList,
                    AppType = Constants.QUERY_BUILDER,
                    EntityName = "Task"
                }
            };

            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.TASK_ID,
                Value = "LMS",
                Operator = ">="
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.TASK_NAME,
                Value = "1234",
                Operator = "=",
                Bitwise = "AND"
            };
            tasksList.Add(task1);
            tasksList.Add(task2);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            List<TaskModel> tasksResponse = taskResponse.Tasks;
            Assert.AreEqual(0, tasksResponse.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => tasksResponse[0]);
        }

        [TestMethod]
        public void GetTasksWithEmptyColumns()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { },
                    Fields = tasksList,
                    AppType = Constants.QUERY_BUILDER,
                    EntityName = "Task"
                }
            };
            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.TASK_ID,
                Value = "90",
                Operator = "="
            };
            EmployeeModel task2 = new EmployeeModel
            {
                Name = Constants.TASK_ID,
                Value = "900",
                Operator = ">",
                Bitwise = "OR"
            };

            tasksList.Add(task1);
            tasksList.Add(task2);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            ErrorResponse tasksResponse = taskResponse.Error;
            Assert.AreEqual(500, tasksResponse.Status);
            Assert.AreEqual(33, tasksResponse.Code);
            StringAssert.Contains(tasksResponse.Message, TestConstants.SYSTEM_ERROR);
        }
        
        [TestMethod]
        public void RequestWithoutColumnList()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    Fields = tasksList,
                    AppType = Constants.QUERY_BUILDER,
                    EntityName = "Task"
                }
            };

            EmployeeModel task1 = new EmployeeModel
            {
                Name = Constants.TASK_ID,
                Value = "90",
                Operator = "="
            };
            tasksList.Add(task1);

            TaskResponse taskResponse = function.GetTaskQueryBuilder(taskRequest, null);
            ErrorResponse tasksResponse = taskResponse.Error;
            Assert.AreEqual(500, tasksResponse.Status);
            Assert.AreEqual(33, tasksResponse.Code);
            StringAssert.Contains(tasksResponse.Message, TestConstants.SYSTEM_ERROR);
        }
    }
}
