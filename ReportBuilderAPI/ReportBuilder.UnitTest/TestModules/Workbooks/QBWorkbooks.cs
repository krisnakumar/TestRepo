using System;
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

namespace ReportBuilder.UnitTest.TestModules.Workbooks
{
    [TestClass]
    public class QBWorkbooks
    {
        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190103.6487    //
        //                                                             //
        /////////////////////////////////////////////////////////////////

        [TestMethod]
        public void GetWorkbooksWithSingleField()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE },
                Fields = wbList
            };

            EmployeeModel wbFilter1 = new EmployeeModel
            {
                Name = Constants.DAYS_TO_COMPLETE,
                Value = "20",
                Operator = "<="
            };
            EmployeeModel wbFilter2 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_CREATED,
                Value = "08/06/1977, 08/08/1975",
                Operator = "BETWEEN",
                Bitwise = "OR"
            };
            EmployeeModel wbFilter3 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_ID,
                Value = "14",
                Operator = "<"
            };
            EmployeeModel wbFilter4 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_CREATED_BY,
                Value = "Jorge Duke Marcie Barber",
                Operator = "="
            };
            //wbList.Add(wbFilter1);
            //wbList.Add(wbFilter2);
            wbList.Add(wbFilter3);

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(wbRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse wbResponse = function.GetWorkbookQuerBuilder(aPIGatewayProxyRequest, null);
            List<WorkbookResponse> workbookList = JsonConvert.DeserializeObject<List<WorkbookResponse>>(wbResponse.Body);
            Assert.AreEqual(200, wbResponse.StatusCode);
            Assert.AreNotEqual(0, workbookList.Count);
            Assert.IsTrue(workbookList[workbookList.Count-1].WorkBookId < 14);
        }
        
        [TestMethod]
        public void GetWorkbooksWithMultipleField()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE },
                Fields = wbList
            };

            EmployeeModel wbFilter1 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_ID,
                Value = "40",
                Operator = ">="
            };
            EmployeeModel wbFilter2 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_CREATED,
                Value = "01/21/2001",
                Operator = "<=",
                Bitwise = "and"
            };
            EmployeeModel wbFilter3 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_NAME,
                Value = "e",
                Operator = "contains",
                Bitwise = "and"
            };
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);
            wbList.Add(wbFilter3);

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(wbRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse wbResponse = function.GetWorkbookQuerBuilder(aPIGatewayProxyRequest, null);
            List<WorkbookResponse> workbookList = JsonConvert.DeserializeObject<List<WorkbookResponse>>(wbResponse.Body);
            Assert.AreEqual(200, wbResponse.StatusCode);
            Assert.AreNotEqual(0, workbookList.Count);
            Assert.AreNotEqual("", workbookList[0].WorkBookId);
            Assert.IsTrue(workbookList[0].WorkBookId >= 40);
            StringAssert.Matches(workbookList[0].WorkbookName, new Regex(@"(?i)\b(.*?)e(.*?)\b"));
        }
        
        [TestMethod]
        public void GetWorkbooksWithAdditionalColumns()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE, Constants.WORKBOOK_ASSIGNED_DATE, Constants.REPETITIONS, Constants.LAST_ATTEMPT_DATE },
                Fields = wbList
            };

            EmployeeModel wbFilter1 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_ID,
                Value = "40",
                Operator = ">="
            };
            EmployeeModel wbFilter2 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_CREATED,
                Value = "01/21/2001",
                Operator = "<=",
                Bitwise = "and"
            };
            EmployeeModel wbFilter3 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_NAME,
                Value = "e",
                Operator = "contains",
                Bitwise = "and"
            };
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);
            wbList.Add(wbFilter3);

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(wbRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse wbResponse = function.GetWorkbookQuerBuilder(aPIGatewayProxyRequest, null);
            List<WorkbookResponse> workbookList = JsonConvert.DeserializeObject<List<WorkbookResponse>>(wbResponse.Body);
            Assert.AreEqual(200, wbResponse.StatusCode);
            Assert.AreNotEqual(0, workbookList.Count);
            Assert.AreNotEqual(null, workbookList[0].WorkBookId);
            Assert.IsTrue(workbookList[0].WorkBookId >= 40);
            Assert.AreNotEqual(null, workbookList[0].WorkbookAssignedDate);
            Assert.AreNotEqual(null, workbookList[0].Repetitions);
            Assert.AreNotEqual(null, workbookList[0].LastAttemptDate);
            StringAssert.Matches(workbookList[0].WorkbookName, new Regex(@"(?i)\b(.*?)e(.*?)\b"));
        }

        [TestMethod]
        public void GetWorkbooksForInvalidCompany()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE },
                Fields = wbList
            };

            EmployeeModel wbFilter1 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_ID,
                Value = "40",
                Operator = ">="
            };
            EmployeeModel wbFilter2 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_CREATED,
                Value = "01/21/2001",
                Operator = "<=",
                Bitwise = "and"
            };
            EmployeeModel wbFilter3 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_NAME,
                Value = "e",
                Operator = "contains",
                Bitwise = "and"
            };
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);
            wbList.Add(wbFilter3);

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "8" } // Valid one is 6
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(wbRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse wbResponse = function.GetWorkbookQuerBuilder(aPIGatewayProxyRequest, null);
            List<WorkbookResponse> workbookList = JsonConvert.DeserializeObject<List<WorkbookResponse>>(wbResponse.Body);
            Assert.AreEqual(200, wbResponse.StatusCode);
            Assert.AreEqual(0, workbookList.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workbookList[0]);
        }

        [TestMethod]
        public void GetWorkbooksForInvalidInputs()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE },
                Fields = wbList
            };

            EmployeeModel wbFilter1 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_ID,
                Value = "40",
                Operator = ">="
            };
            EmployeeModel wbFilter2 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_CREATED,
                Value = "ITS-LMS",
                Operator = "<=",
                Bitwise = "and"
            };
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }
            };
            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(wbRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse wbResponse = function.GetWorkbookQuerBuilder(aPIGatewayProxyRequest, null);
            List<WorkbookResponse> workbookList = JsonConvert.DeserializeObject<List<WorkbookResponse>>(wbResponse.Body);
            Assert.AreEqual(200, wbResponse.StatusCode);
            Assert.AreEqual(0, workbookList.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workbookList[0]);
        }

        [TestMethod]
        public void GetWorkbooksWithOperatorCombinations()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE },
                Fields = wbList
            };

            EmployeeModel wbFilter1 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_ID,
                Value = "40",
                Operator = ">="
            };
            EmployeeModel wbFilter2 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_CREATED,
                Value = "01/21/2001",
                Operator = "<=",
                Bitwise = "and"
            };
            EmployeeModel wbFilter3 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_NAME,
                Value = "e",
                Operator = "contains",
                Bitwise = "and"
            };
            EmployeeModel wbFilter4 = new EmployeeModel
            {
                Name = Constants.DAYS_TO_COMPLETE,
                Value = "20",
                Operator = "<=",
                Bitwise = "or"
            };
            EmployeeModel wbFilter5 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_CREATED,
                Value = "01/21/1991",
                Operator = ">=",
                Bitwise = "and"
            };
            EmployeeModel wbFilter6 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_NAME,
                Value = "e",
                Operator = "contains",
                Bitwise = "and"
            };
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);
            wbList.Add(wbFilter3);
            wbList.Add(wbFilter4);
            wbList.Add(wbFilter5);
            wbList.Add(wbFilter6);

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(wbRequest),
                PathParameters = pathValues
            };

            APIGatewayProxyResponse wbResponse = function.GetWorkbookQuerBuilder(aPIGatewayProxyRequest, null);
            List<WorkbookResponse> workbookList = JsonConvert.DeserializeObject<List<WorkbookResponse>>(wbResponse.Body);
            Assert.AreEqual(200, wbResponse.StatusCode);
            Assert.AreNotEqual(0, workbookList.Count);
            Assert.AreNotEqual(null, workbookList[0].WorkBookId);
            StringAssert.Matches(workbookList[0].WorkbookName, new Regex(@"(?i)\b(.*?)e(.*?)\b"));
        }
        
        [TestMethod]
        public void GetWorkbooksWithEmptyColumnsList()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();

            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                ColumnList = new string[] { },
                Fields = wbList
            };
            
            EmployeeModel wbFilter1 = new EmployeeModel
            {
                Name = Constants.DAYS_TO_COMPLETE,
                Value = "20",
                Operator = "<="
            };
            EmployeeModel wbFilter2 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_CREATED,
                Value = "01/21/1991",
                Operator = ">=",
                Bitwise = "and"
            };
            wbList.Add(wbFilter1);
            wbList.Add(wbFilter2);

            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { "companyId", "6" }
            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                Body = JsonConvert.SerializeObject(wbRequest),
                PathParameters = pathValues
            };
            APIGatewayProxyResponse wbResponse = function.GetWorkbookQuerBuilder(aPIGatewayProxyRequest, null);
            List<WorkbookResponse> workbookList = JsonConvert.DeserializeObject<List<WorkbookResponse>>(wbResponse.Body);
            Assert.AreEqual(200, wbResponse.StatusCode);
            Assert.AreEqual(0, workbookList.Count);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => workbookList[0]);
        }

        // Test for try-catch block
        [TestMethod]
        public void RequestWithoutColumnList()
        {
            //List<EmployeeModel> wbList = new List<EmployeeModel>();
            //Function function = new Function();

            //QueryBuilderRequest wbRequest = new QueryBuilderRequest
            //{
            //    Fields = wbList
            //};
            //EmployeeModel wbFilter1 = new EmployeeModel
            //{
            //    Name = Constants.DAYS_TO_COMPLETE,
            //    Value = "20",
            //    Operator = "<="
            //};
            //wbList.Add(wbFilter1);

            //Dictionary<string, string> pathValues = new Dictionary<string, string>
            //{
            //    { "companyId", "6" }
            //};
            //APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            //{
            //    Body = JsonConvert.SerializeObject(wbRequest),
            //    PathParameters = pathValues
            //};
            //APIGatewayProxyResponse wbResponse = function.GetWorkbookQuerBuilder(aPIGatewayProxyRequest, null);
            //ErrorResponse wbResponses = JsonConvert.DeserializeObject<ErrorResponse>(wbResponse.Body);
            //Assert.AreEqual(500, wbResponse.StatusCode);
            //Assert.IsTrue(wbResponses.Code == 33);
            //StringAssert.Matches(wbResponses.Message, new Regex(@"(?i)\b(.*?)error(.*?)\b"));
        }

    }
}
