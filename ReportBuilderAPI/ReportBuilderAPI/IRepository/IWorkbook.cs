using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using System.Collections.Generic;


namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    ///     Interface that handles Workbooks crud operations
    /// </summary>
    public interface IWorkbook
    {
        WorkbookResponse GetWorkbookDetails(QueryBuilderRequest queryBuilderRequest);
        string CreateWorkbookQuery(QueryBuilderRequest queryBuilderRequest);
        List<WorkbookModel> ReadWorkBookDetails(string query, Dictionary<string, string> parameters);
    }
}
