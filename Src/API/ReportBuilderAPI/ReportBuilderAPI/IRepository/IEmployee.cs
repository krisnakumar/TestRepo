using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using System.Collections.Generic;


namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    ///     Interface that handles Employees crud operations
    /// </summary>
    public interface IEmployee
    {
        EmployeeResponse GetEmployeeDetails(QueryBuilderRequest queryBuilderRequest);
        string CreateEmployeeQuery(QueryBuilderRequest employeeRequest);
        string CheckValues(string value, string fieldOperator);
        List<EmployeeQueryModel> ReadEmployeeDetails(string query, Dictionary<string, string> parameters);
    }
}
    