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
using ReportBuilder.UnitTest.TestModules.Utilities;
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
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE },
                    Fields = wbList,
                    EntityName = "Workbook",
                    AppType = Constants.QUERY_BUILDER
                }
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
                Value = "01/01/1787 and 04/03/2019", //"08 /06/1977, 08/08/1975",
                Operator = "BETWEEN"
            };
            EmployeeModel wbFilter3 = new EmployeeModel
            {
                Name = Constants.WORKBOOK_ID,
                Value = "14",
                Operator = "<"
            };
            EmployeeModel wbFilter4 = new EmployeeModel
            {
                Name = Constants.SUPERVISOR_ID,
                Value = "6",
                Operator = "!="
            };
            wbList.Add(wbFilter3);
            //wbList.Add(wbFilter4);

            WorkbookResponse wbResponse = function.GetWorkbookQueryBuilder(wbRequest, null);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.AreNotEqual(0, workbookList.Count);
            Assert.IsTrue(workbookList[workbookList.Count - 1].WorkBookId < 14);
        }

        [TestMethod]
        public void GetWorkbooksForSpecificUserId()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                CompanyId = 6, //1, //6,
                UserId = 6, //335216, //6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] {Constants.USERID, Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE },
                    Fields = wbList,
                    EntityName = "Workbook",
                    AppType = Constants.QUERY_BUILDER
                }
            };
            EmployeeModel wbFilter1 = new EmployeeModel
            {
                Name = Constants.USERID,
                Value = "10",
                Operator = ">="
            };
            EmployeeModel wbFilter2 = new EmployeeModel
            {
                Name = Constants.STUDENT_DETAILS,
                Value = "10",
                Operator = "=",
                Bitwise = "AND"
            };
            wbList.Add(wbFilter1);
            //wbList.Add(wbFilter2);

            WorkbookResponse wbResponse = function.GetWorkbookQueryBuilder(wbRequest, null);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.AreNotEqual(0, workbookList.Count);
            Assert.IsTrue(workbookList[workbookList.Count - 1].UserId >= 10);
        }

        [TestMethod]
        public void GetWorkbooksWithMultipleField()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE },
                    Fields = wbList,
                    EntityName = "Workbook",
                    AppType = Constants.QUERY_BUILDER
                }
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

            WorkbookResponse wbResponse = function.GetWorkbookQueryBuilder(wbRequest, null);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.AreNotEqual(0, workbookList.Count);
            Assert.AreNotEqual("", workbookList[0].WorkBookId);
            Assert.IsTrue(workbookList[0].WorkBookId >= 40);
            StringAssert.Matches(workbookList[0].WorkBookName, new Regex(@"(?i)\b(.*?)e(.*?)\b"));
        }

        [TestMethod]
        public void GetWorkbooksWithAdditionalColumns()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE, Constants.WORKBOOK_ASSIGNED_DATE, Constants.REPETITIONS, Constants.LAST_ATTEMPT_DATE },
                    Fields = wbList,
                    EntityName = "Workbook",
                    AppType = Constants.QUERY_BUILDER
                }
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

            WorkbookResponse wbResponse = function.GetWorkbookQueryBuilder(wbRequest, null);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.AreNotEqual(0, workbookList.Count);
            Assert.AreNotEqual(null, workbookList[0].WorkBookId);
            Assert.IsTrue(workbookList[0].WorkBookId >= 40);
            Assert.AreNotEqual(null, workbookList[0].WorkbookAssignedDate);
            Assert.AreNotEqual(null, workbookList[0].Repetitions);
            Assert.AreNotEqual(null, workbookList[0].LastAttemptDate);
            StringAssert.Matches(workbookList[0].WorkBookName, new Regex(@"(?i)\b(.*?)e(.*?)\b"));
        }

        [TestMethod]
        public void GetWorkbooksForInvalidCompany()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                CompanyId = 8, // Valid one is 6
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE },
                    Fields = wbList,
                    EntityName = "Workbook",
                    AppType = Constants.QUERY_BUILDER
                }
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

            WorkbookResponse wbResponse = function.GetWorkbookQueryBuilder(wbRequest, null);
            ErrorResponse workbookList = wbResponse.Error;
            Assert.AreEqual(403, workbookList.Status);
            Assert.AreEqual(14, workbookList.Code);
            StringAssert.Contains(workbookList.Message, TestConstants.PERMISSION_DENIED);
        }

        [TestMethod]
        public void GetWorkbooksForInvalidInputs()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE },
                    Fields = wbList,
                    EntityName = "Workbook",
                    AppType = Constants.QUERY_BUILDER
                }
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

            WorkbookResponse wbResponse = function.GetWorkbookQueryBuilder(wbRequest, null);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
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
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { Constants.WORKBOOK_ID, Constants.WORKBOOK_NAME, Constants.DESCRIPTION, Constants.WORKBOOK_CREATED_BY, Constants.DAYS_TO_COMPLETE },
                    Fields = wbList,
                    EntityName = "Workbook",
                    AppType = Constants.QUERY_BUILDER
                }
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
            
            WorkbookResponse wbResponse = function.GetWorkbookQueryBuilder(wbRequest, null);
            List<WorkbookModel> workbookList = wbResponse.Workbooks;
            Assert.AreNotEqual(0, workbookList.Count);
            Assert.AreNotEqual(null, workbookList[0].WorkBookId);
            StringAssert.Matches(workbookList[0].WorkBookName, new Regex(@"(?i)\b(.*?)e(.*?)\b"));
        }

        [TestMethod]
        public void GetWorkbooksWithEmptyColumnsList()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = new string[] { },
                    Fields = wbList,
                    EntityName = "Workbook",
                    AppType = Constants.QUERY_BUILDER
                }
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

            WorkbookResponse wbResponse = function.GetWorkbookQueryBuilder(wbRequest, null);
            ErrorResponse workbookResponse = wbResponse.Error;
            Assert.AreEqual(500, workbookResponse.Status);
            Assert.AreEqual(33, workbookResponse.Code);
            StringAssert.Contains(workbookResponse.Message, TestConstants.SYSTEM_ERROR);
        }

        [TestMethod]
        public void RequestWithoutColumnList()
        {
            List<EmployeeModel> wbList = new List<EmployeeModel>();
            Function function = new Function();
            QueryBuilderRequest wbRequest = new QueryBuilderRequest
            {
                CompanyId = 6,
                UserId = 6,
                Payload = new QueryBuilderRequest
                {
                    Fields = wbList,
                    EntityName = "Workbook",
                    AppType = Constants.QUERY_BUILDER
                }
            };

            EmployeeModel wbFilter1 = new EmployeeModel
            {
                Name = Constants.DAYS_TO_COMPLETE,
                Value = "20",
                Operator = "<="
            };
            wbList.Add(wbFilter1);

            WorkbookResponse wbResponse = function.GetWorkbookQueryBuilder(wbRequest, null);
            ErrorResponse workbookResponse = wbResponse.Error;
            Assert.AreEqual(500, workbookResponse.Status);
            Assert.AreEqual(33, workbookResponse.Code);
            StringAssert.Contains(workbookResponse.Message, TestConstants.SYSTEM_ERROR);
        }

    }
}
