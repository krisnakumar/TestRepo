using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using System.Collections.Generic;



namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    ///     Interface that handles Tasks crud operations
    /// </summary>
    public interface ITask
    {
        string CreateTaskQuery(QueryBuilderRequest queryBuilderRequest);
        TaskResponse GetQueryTaskDetails(QueryBuilderRequest queryBuilderRequest);        
        List<TaskModel> ReadTaskDetails(string query, Dictionary<string, string> parameters, QueryBuilderRequest queryBuilderRequest);
    }
}
