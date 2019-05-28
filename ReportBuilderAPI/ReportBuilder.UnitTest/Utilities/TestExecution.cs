using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilderAPI.Handlers.FunctionHandler;
using ReportBuilderAPI.Utilities;
using System.Collections.Generic;

namespace ReportBuilder.UnitTest.Utilities
{
    internal class TestExecution
    {
        internal dynamic ExecuteTests(int companyId, int userId, string[] columns, string appType, List<EmployeeModel> fieldset)
        {
            Function function = new Function();
            QueryBuilderRequest QueryRequest = new QueryBuilderRequest
            {
                CompanyId = companyId,
                UserId = userId,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = columns,
                    Fields = fieldset,
                    AppType = appType
                }
            };
            dynamic testResponse = null;
            switch (appType)
            {
                case Constants.OQ_DASHBOARD:
                case Constants.TRAINING_DASHBOARD:
                    testResponse = function.GetWorkbookQueryBuilder(QueryRequest, null);
                    break;
                case Constants.WORKBOOK_DASHBOARD:
                    testResponse = function.GetWorkbookQueryBuilder(QueryRequest, null);
                    break;
                default:
                    break;
            }
            return testResponse;
        }


        public QueryBuilderRequest CreateQueryRequest(int companyId, int userId, string[] columns, string appType, List<EmployeeModel> fieldset)
        {
            QueryBuilderRequest queryRequest = new QueryBuilderRequest
            {
                CompanyId = companyId,
                UserId = userId,
                Payload = new QueryBuilderRequest
                {
                    ColumnList = columns,
                    Fields = fieldset,
                    AppType = appType
                }
            };
            return queryRequest;
        }


        internal EmployeeModel CreateFields(string name, string value, string oparator, string bitwise)
        {
            EmployeeModel field = new EmployeeModel
            {
                Name = name,
                Value = value,
                Operator = oparator,
                Bitwise = bitwise
            };
            return field;
        }
    }
}
