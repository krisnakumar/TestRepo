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

namespace ReportBuilder.UnitTest.TestModules.SaveQuery
{
    [TestClass]
    public class SaveQuery
    {
        ///////////////////////////////////////////////////////////////////
        ////                                                             //
        ////    All test cases are tested upto build v0.9.190103.6487    //
        ////                                                             //
        ///////////////////////////////////////////////////////////////////

        //[TestMethod]
        //public void SaveANewValidQuery()
        //{
        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        ColumnList = new string[] { Constants.USERID, Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.ASSIGNED_WORKBOOK, Constants.INCOMPLETE_WORKBOOK, Constants.PAST_DUE_WORKBOOK, Constants.COMPLETED_WORKBOOK, Constants.TOTAL_EMPLOYEES },
        //        Fields = employeeList,
        //        EntityName = Constants.EMPLOYEE,
        //        QueryName = "Employees created between",
        //        UserId = 6
        //    };

        //    EmployeeModel employeeModel = new EmployeeModel
        //    {
        //        Name = Constants.USER_CREATED_DATE,
        //        Value = "04/02/1990 and 04/02/2019",
        //        Operator = "Between"
        //    };
        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }

        //    };
        //    employeeList.Add(employeeModel);

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse userResponse = function.SaveQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse saveQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(userResponse.Body);
        //    Assert.AreEqual(200, userResponse.StatusCode);
        //    Assert.AreEqual("Query has been saved successfully!", saveQueryResponse.Message);
        //}

        //[TestMethod]
        //public void SaveQueryWithSpecialCharacters()
        //{
        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.EMAIL, Constants.TOTAL_EMPLOYEES },
        //        Fields = employeeList,
        //        EntityName = Constants.EMPLOYEE,
        //        QueryName = "@b1#its!~chec*k",
        //        UserId = 6
        //    };
        //    EmployeeModel employeeModel = new EmployeeModel
        //    {
        //        Name = Constants.USER_CREATED_DATE,
        //        Value = "04/02/1990 and 04/02/2019",
        //        Operator = "Between"
        //    };
        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };
        //    employeeList.Add(employeeModel);

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse userResponse = function.SaveQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse saveQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(userResponse.Body);
        //    Assert.AreEqual(200, userResponse.StatusCode);
        //    Assert.AreEqual("Query has been saved successfully!", saveQueryResponse.Message);
        //}

        //// FAIL
        //[TestMethod]
        //public void SaveAEmployeeQueryWithoutColumnsList()
        //{
        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        //ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.EMAIL, Constants.TOTAL_EMPLOYEES },
        //        Fields = employeeList,
        //        EntityName = Constants.EMPLOYEE,
        //        QueryName = "Employees created between",
        //        UserId = 6
        //    };
        //    EmployeeModel employeeModel = new EmployeeModel
        //    {
        //        Name = Constants.USER_CREATED_DATE,
        //        Value = "04/02/1990 and 04/02/2019",
        //        Operator = "Between"
        //    };
        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };
        //    employeeList.Add(employeeModel);

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse userResponse = function.SaveQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse saveQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(userResponse.Body);
        //    Assert.AreEqual(200, userResponse.StatusCode);
        //    Assert.AreEqual("Query has been saved successfully!", saveQueryResponse.Message);
        //}

        //[TestMethod]
        //public void SaveAQueryWithoutEntityName()
        //{
        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.EMAIL, Constants.TOTAL_EMPLOYEES },
        //        Fields = employeeList,
        //        QueryName = "Employees created between",
        //        UserId = 6
        //    };
        //    EmployeeModel employeeModel = new EmployeeModel
        //    {
        //        Name = Constants.USER_CREATED_DATE,
        //        Value = "04/02/1990 and 04/02/2019",
        //        Operator = "Between"
        //    };
        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };
        //    employeeList.Add(employeeModel);

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse userResponse = function.SaveQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse saveQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(userResponse.Body);
        //    Assert.AreEqual(400, userResponse.StatusCode);
        //    StringAssert.Contains(saveQueryResponse.Message, "Check your inputs");
        //}

        //[TestMethod]
        //public void SaveQueryWithEmptyName()
        //{
        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.EMAIL, Constants.TOTAL_EMPLOYEES },
        //        Fields = employeeList,
        //        EntityName = Constants.EMPLOYEE,
        //        QueryName = "",
        //        UserId = 6
        //    };
        //    EmployeeModel employeeModel = new EmployeeModel
        //    {
        //        Name = Constants.USER_CREATED_DATE,
        //        Value = "04/02/1990 and 04/02/2019",
        //        Operator = "Between"
        //    };
        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };
        //    employeeList.Add(employeeModel);

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse userResponse = function.SaveQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse saveQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(userResponse.Body);
        //    Assert.AreEqual(400, userResponse.StatusCode);
        //    StringAssert.Contains(saveQueryResponse.Message, "Empty name! Please give a Query name to save");
        //}

        //[TestMethod]
        //public void SaveQueryWithDuplicateName()
        //{
        //    List<EmployeeModel> employeeList = new List<EmployeeModel>();

        //    Function function = new Function();
        //    QueryBuilderRequest employeeRequest = new QueryBuilderRequest
        //    {
        //        ColumnList = new string[] { Constants.EMPLOYEE_NAME, Constants.ROLE, Constants.USERNAME, Constants.ALTERNATE_USERNAME, Constants.EMAIL, Constants.TOTAL_EMPLOYEES },
        //        Fields = employeeList,
        //        EntityName = Constants.EMPLOYEE,
        //        QueryName = "Employees created between",
        //        UserId = 6
        //    };
        //    EmployeeModel employeeModel = new EmployeeModel
        //    {
        //        Name = Constants.USER_CREATED_DATE,
        //        Value = "04/02/1990 and 04/02/2019",
        //        Operator = "Between"
        //    };
        //    Dictionary<string, string> pathValues = new Dictionary<string, string>
        //    {
        //        { "companyId", "6" }
        //    };
        //    employeeList.Add(employeeModel);

        //    APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
        //    {
        //        Body = JsonConvert.SerializeObject(employeeRequest),
        //        PathParameters = pathValues
        //    };

        //    APIGatewayProxyResponse userResponse = function.SaveQuery(aPIGatewayProxyRequest, null);
        //    ErrorResponse saveQueryResponse = JsonConvert.DeserializeObject<ErrorResponse>(userResponse.Body);
        //    Assert.AreEqual(400, userResponse.StatusCode);
        //    StringAssert.Contains(saveQueryResponse.Message, "Query Name already exist! Please select different one!");
        //}
    }
}