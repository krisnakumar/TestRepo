﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Amazon.Lambda.APIGatewayEvents;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilderAPI.Handlers.FunctionHandler;
using ReportBuilder.Models.Request;
using ReportBuilderAPI.Utilities;
using ReportBuilder.Models.Response;

namespace ReportBuilder.UnitTest.TestModules.Tasks
{
    [TestClass]
    public class QBTasks
    {
        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190118.6595    //
        //                                                             //
        /////////////////////////////////////////////////////////////////
        
        [TestMethod]
        public void GetTasksWithSingleField()
        {
            //List<EmployeeModel> tasksList = new List<EmployeeModel>();
            //Function function = new Function();

            //QueryBuilderRequest taskRequest = new QueryBuilderRequest
            //{
            //    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, Constants.ASSIGNED_TO, Constants.EVALUATOR_NAME, Constants.EXPIRATION_DATE },
            //    Fields = tasksList
            //};

            //EmployeeModel task1 = new EmployeeModel
            //{
            //    Name = Constants.TASK_ID,
            //    Value = "90",
            //    Operator = "="
            //};
            //EmployeeModel task2 = new EmployeeModel
            //{
            //    Name = Constants.REPETITIONS_COUNT,
            //    Operator = "!=",
            //    Value = "5"
            //};
            //tasksList.Add(task1);

            //Dictionary<string, string> pathValues = new Dictionary<string, string>
            //{
            //    { "companyId", "6" }
            //};

            //APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            //{
            //    Body = JsonConvert.SerializeObject(taskRequest),
            //    PathParameters = pathValues
            //};

            //APIGatewayProxyResponse taskResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
            //List<TaskResponse> tasksResponse = JsonConvert.DeserializeObject<List<TaskResponse>>(taskResponse.Body);
            //Assert.AreEqual(200, taskResponse.StatusCode);
            //Assert.AreNotEqual(0, tasksResponse.Count);
            //Assert.AreEqual(90, tasksResponse[0].TaskId);
            //Assert.AreNotEqual("", tasksResponse[0].TaskName);
            //StringAssert.Matches(tasksResponse[0].TaskName, new Regex(@"(?i)\broberta579\b"));
            //// Newly
            //Assert.IsTrue(tasksResponse[0].EvaluatorName != "");
        }

        [TestMethod]
        public void GetTasksWithMultipleFields()
        {
            //List<EmployeeModel> tasksList = new List<EmployeeModel>();
            //Function function = new Function();

            //QueryBuilderRequest taskRequest = new QueryBuilderRequest
            //{
            //    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, Constants.ASSIGNED_TO, Constants.EVALUATOR_NAME, Constants.EXPIRATION_DATE },
            //    Fields = tasksList
            //};

            //EmployeeModel task1 = new EmployeeModel
            //{
            //    Name = Constants.TASK_ID,
            //    Value = "19",
            //    Operator = ">="
            //};
            //EmployeeModel task2 = new EmployeeModel
            //{
            //    Name = Constants.EVALUATOR_NAME,
            //    Value = "nn",
            //    Operator = "contains",
            //    Bitwise = "AND"
            //};
            //EmployeeModel task3 = new EmployeeModel
            //{
            //    Name = Constants.DATE_TAKEN,
            //    Value = "11/21/1991",
            //    Operator = ">=",
            //    Bitwise = "AND"
            //};
            //tasksList.Add(task1);
            //tasksList.Add(task2);
            //tasksList.Add(task3);

            //Dictionary<string, string> pathValues = new Dictionary<string, string>
            //{
            //    { "companyId", "6" }
            //};

            //APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            //{
            //    Body = JsonConvert.SerializeObject(taskRequest),
            //    PathParameters = pathValues
            //};

            //APIGatewayProxyResponse taskResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
            //List<TaskResponse> tasksResponse = JsonConvert.DeserializeObject<List<TaskResponse>>(taskResponse.Body);
            //Assert.AreEqual(200, taskResponse.StatusCode);
            //Assert.AreNotEqual(0, tasksResponse.Count);
            //Assert.AreNotEqual("", tasksResponse[0].TaskId);
            //Assert.IsTrue(tasksResponse[0].TaskId >= 19);
            //StringAssert.Matches(tasksResponse[0].EvaluatorName, new Regex(@"(?i)\b(.*?)nn(.*?)\b"));
        }

        [TestMethod]
        public void GetTasksWithOperatorCombinations()
        {
            //List<EmployeeModel> tasksList = new List<EmployeeModel>();
            //Function function = new Function();

            //QueryBuilderRequest taskRequest = new QueryBuilderRequest
            //{
            //    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, Constants.ASSIGNED_TO, Constants.EVALUATOR_NAME, Constants.EXPIRATION_DATE },
            //    Fields = tasksList
            //};

            //EmployeeModel task1 = new EmployeeModel
            //{
            //    Name = Constants.TASK_ID,
            //    Value = "119",
            //    Operator = ">="
            //};
            //EmployeeModel task2 = new EmployeeModel
            //{
            //    Name = Constants.EVALUATOR_NAME,
            //    Value = "nn",
            //    Operator = "contains",
            //    Bitwise = "AND"
            //};
            //EmployeeModel task3 = new EmployeeModel
            //{
            //    Name = Constants.DATE_TAKEN,
            //    Value = "01/21/2000",
            //    Operator = "<",
            //    Bitwise = "AND"
            //};
            //EmployeeModel task4 = new EmployeeModel
            //{
            //    Name = Constants.STATUS,
            //    Value = "completed",
            //    Operator = "=",
            //    Bitwise = "OR"
            //};
            //EmployeeModel task5 = new EmployeeModel
            //{
            //    Name = Constants.TASK_NAME,
            //    Value = "e",
            //    Operator = "does not contain",
            //    Bitwise = "AND"
            //};
            //EmployeeModel task6 = new EmployeeModel
            //{
            //    Name = Constants.EVALUATOR_NAME,
            //    Value = "nn",
            //    Operator = "contains",
            //    Bitwise = "AND"
            //};
            //tasksList.Add(task1);
            //tasksList.Add(task2);
            //tasksList.Add(task3);
            //tasksList.Add(task4);
            //tasksList.Add(task5);
            //tasksList.Add(task6);

            //Dictionary<string, string> pathValues = new Dictionary<string, string>
            //{
            //    { "companyId", "6" }
            //};

            //APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            //{
            //    Body = JsonConvert.SerializeObject(taskRequest),
            //    PathParameters = pathValues
            //};

            //APIGatewayProxyResponse taskResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
            //List<TaskResponse> tasksResponse = JsonConvert.DeserializeObject<List<TaskResponse>>(taskResponse.Body);
            //Assert.AreEqual(200, taskResponse.StatusCode);
            //Assert.AreNotEqual(0, tasksResponse.Count);
            //Assert.AreNotEqual("", tasksResponse[0].TaskId);
            //StringAssert.Matches(tasksResponse[0].EvaluatorName, new Regex(@"(?i)\b(.*?)nn(.*?)\b"));
        }

        [TestMethod]
        public void GetTasksWithAdditionalColumns()
        {
            //List<EmployeeModel> tasksList = new List<EmployeeModel>();
            //Function function = new Function();

            //QueryBuilderRequest taskRequest = new QueryBuilderRequest
            //{
            //    ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, Constants.STATUS, Constants.EVALUATOR_NAME, Constants.ASSIGNED_TO, Constants.DATE_TAKEN, Constants.NUMBER_OF_ATTEMPTS, Constants.LOCATION },
            //    Fields = tasksList
            //};

            //EmployeeModel task1 = new EmployeeModel
            //{
            //    Name = Constants.STATUS,
            //    Value = "completed",
            //    Operator = "="
            //};
            //EmployeeModel task2 = new EmployeeModel
            //{
            //    Name = Constants.TASK_NAME,
            //    Value = "on",
            //    Operator = "does not contain",
            //    Bitwise = "AND"
            //};
            //EmployeeModel task3 = new EmployeeModel
            //{
            //    Name = Constants.EVALUATOR_NAME,
            //    Value = "en",
            //    Operator = "contains",
            //    Bitwise = "AND"
            //};
            //tasksList.Add(task1);
            //tasksList.Add(task2);
            //tasksList.Add(task3);

            //Dictionary<string, string> pathValues = new Dictionary<string, string>
            //{
            //    { "companyId", "6" }
            //};

            //APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            //{
            //    Body = JsonConvert.SerializeObject(taskRequest),
            //    PathParameters = pathValues
            //};

            //APIGatewayProxyResponse taskResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
            //List<TaskResponse> tasksResponse = JsonConvert.DeserializeObject<List<TaskResponse>>(taskResponse.Body);
            //Assert.AreEqual(200, taskResponse.StatusCode);
            //Assert.AreNotEqual(0, tasksResponse.Count);
            //Assert.AreNotEqual(null, tasksResponse[0].TaskId);
            //Assert.AreEqual("Completed", tasksResponse[0].Status);
            //Assert.AreNotEqual(null, tasksResponse[0].NumberofAttempts);
            //Assert.AreNotEqual(null, tasksResponse[0].Location);
            //StringAssert.Matches(tasksResponse[0].EvaluatorName, new Regex(@"(?i)\b(.*?)en(.*?)\b"));
        }

        [TestMethod]
        public void GetTasksForUnsubscribedCompany()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, "ITSLMS", Constants.EVALUATOR_NAME, Constants.ASSIGNED_TO },
                Fields = tasksList
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

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "8" } // Valid one is 6
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(taskRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse taskResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
            List<TaskResponse> tasksResponse = JsonConvert.DeserializeObject<List<TaskResponse>>(taskResponse.Body);
            Assert.AreEqual(200, taskResponse.StatusCode);
            Assert.AreEqual(0, tasksResponse.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => tasksResponse[0]);
        }

        [TestMethod]
        public void GetTasksWithInvalidInputs()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, Constants.ASSIGNED_TO, Constants.EVALUATOR_NAME, Constants.EXPIRATION_DATE },
                Fields = tasksList
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

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(taskRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse taskResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
            List<TaskResponse> tasksResponse = JsonConvert.DeserializeObject<List<TaskResponse>>(taskResponse.Body);
            Assert.AreEqual(200, taskResponse.StatusCode);
            Assert.AreEqual(0, tasksResponse.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => tasksResponse[0]);
        }

        [TestMethod]
        public void GetTasksWithoutColumns()
        {
            List<EmployeeModel> tasksList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest taskRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] {},
                Fields = tasksList
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

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(taskRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse taskResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
            List<TaskResponse> tasksResponse = JsonConvert.DeserializeObject<List<TaskResponse>>(taskResponse.Body);
            Assert.AreEqual(200, taskResponse.StatusCode);
            Assert.AreEqual(0, tasksResponse.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => tasksResponse[0]);
        }

        //// For OQ Dashboard

        //[TestMethod]
        //public void GetAQDetails()
        //{
        //    List<EmployeeModel> tasksList = new List<EmployeeModel>();
        //    Function function = new Function();

        //    QueryBuilderRequest taskRequest = new QueryBuilderRequest
        //    {
        //        ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.ASSIGNED_DATE },
        //        Fields = tasksList
        //    };

        //    EmployeeModel task1 = new EmployeeModel
        //    {
        //        Name = Constants.SUPERVISOR_ID,
        //        Value = "6", // 98
        //        Operator = "="
        //    };
        //    EmployeeModel task2 = new EmployeeModel
        //    {
        //        Name = Constants.PAST_DUE,
        //        Operator = "=",
        //        Value = "30",
        //        Bitwise = "AND"
        //    };
        //    tasksList.Add(task1);
        //    //tasksList.Add(task2);

        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(taskRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse taskResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
        //    List<TaskResponse> tasksResponse = JsonConvert.DeserializeObject<List<TaskResponse>>(taskResponse.Body);
        //    Assert.AreEqual(200, taskResponse.StatusCode);
        //    Assert.AreNotEqual(0, tasksResponse.Count);
        //    Assert.AreEqual(90, tasksResponse[0].TaskId);
        //    Assert.AreNotEqual("", tasksResponse[0].TaskName);
        //    StringAssert.Matches(tasksResponse[0].TaskName, new Regex(@"(?i)\broberta579\b"));
        //}

        //[TestMethod]
        //public void GetExpiredQualifications()
        //{
        //    List<EmployeeModel> tasksList = new List<EmployeeModel>();
        //    Function function = new Function();

        //    QueryBuilderRequest taskRequest = new QueryBuilderRequest
        //    {
        //        ColumnList = new string[] { Constants.TASK_CODE, Constants.TASK_NAME, Constants.EMPLOYEE_NAME, Constants.COURSE_EXPIRATION_DATE },
        //        Fields = tasksList
        //    };

        //    EmployeeModel task1 = new EmployeeModel
        //    {
        //        Name = Constants.SUPERVISOR_ID,
        //        Value = "6", // 98
        //        Operator = "="
        //    };
        //    EmployeeModel task2 = new EmployeeModel
        //    {
        //        Name = Constants.PAST_DUE,
        //        Operator = "=",
        //        Value = "30",
        //        Bitwise = "AND"
        //    };
        //    tasksList.Add(task1);
        //    tasksList.Add(task2);

        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(taskRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse taskResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
        //    List<TaskResponse> tasksResponse = JsonConvert.DeserializeObject<List<TaskResponse>>(taskResponse.Body);
        //    Assert.AreEqual(200, taskResponse.StatusCode);
        //    Assert.AreNotEqual(0, tasksResponse.Count);
        //    Assert.AreEqual(90, tasksResponse[0].TaskId);
        //    Assert.AreNotEqual("", tasksResponse[0].TaskName);
        //    StringAssert.Matches(tasksResponse[0].TaskName, new Regex(@"(?i)\broberta579\b"));
        //}

        //// Conditional test

        //[TestMethod]
        //public void GetTasksForPast30Days()
        //{
        //    List<EmployeeModel> tasksList = new List<EmployeeModel>();
        //    Function function = new Function();

        //    QueryBuilderRequest taskRequest = new QueryBuilderRequest
        //    {
        //        ColumnList = new string[] { Constants.TASK_ID, Constants.TASK_NAME, Constants.ASSIGNED_TO, Constants.EVALUATOR_NAME, Constants.EXPIRATION_DATE, Constants.PAST_DUE,Constants.WORKBOOK_IN_DUE },
        //        Fields = tasksList
        //    };

        //    EmployeeModel task1 = new EmployeeModel
        //    {
        //        Name = Constants.TASK_ID,
        //        Value = "90",
        //        Operator = "="
        //    };
        //    tasksList.Add(task1);

        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(taskRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse taskResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
        //    List<TaskResponse> tasksResponse = JsonConvert.DeserializeObject<List<TaskResponse>>(taskResponse.Body);
        //    Assert.AreEqual(200, taskResponse.StatusCode);
        //    Assert.AreNotEqual(0, tasksResponse.Count);
        //    Assert.AreEqual(90, tasksResponse[0].TaskId);
        //    Assert.AreNotEqual("", tasksResponse[0].TaskName);
        //    StringAssert.Matches(tasksResponse[0].TaskName, new Regex(@"(?i)\broberta579\b"));
        //}

        // Test for try-catch block

        [TestMethod]
        public void RequestWithoutColumnList()
        {
            //List<EmployeeModel> tasksList = new List<EmployeeModel>();
            //Function function = new Function();

            //QueryBuilderRequest taskRequest = new QueryBuilderRequest
            //{
            //    Fields = tasksList
            //};
            //EmployeeModel task1 = new EmployeeModel
            //{
            //    Name = Constants.TASK_ID,
            //    Value = "90",
            //    Operator = "="
            //};
            //tasksList.Add(task1);

            //Dictionary<string, string> pathValues = new Dictionary<string, string>
            //{
            //    { "companyId", "6" }
            //};
            //APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            //{
            //    Body = JsonConvert.SerializeObject(taskRequest),
            //    PathParameters = pathValues
            //};

            //APIGatewayProxyResponse taskResponse = function.GetTaskQuerBuilder(aPIGatewayProxyRequest, null);
            //ErrorResponse tasksResponse = JsonConvert.DeserializeObject<ErrorResponse>(taskResponse.Body);
            //Assert.AreEqual(500, taskResponse.StatusCode);
            //Assert.IsTrue(tasksResponse.Code == 33);
            //StringAssert.Matches(tasksResponse.Message, new Regex(@"(?i)\b(.*?)error(.*?)\b"));
        }
    }
}
