using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;

namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    /// Interface that helps to handle the user queries
    /// </summary>
    public interface IQuery
    {
        QueryResponse SaveQuery(QueryBuilderRequest queryBuilderRequest);
        QueryResponse GetUserQueries(QueryBuilderRequest queryBuilderRequest);

        QueryResponse RenameQuery(QueryBuilderRequest queryBuilderRequest);

        QueryResponse DeleteQuery(QueryBuilderRequest queryBuilderRequest);

        QueryResponse GetUserQuery(QueryBuilderRequest queryBuilderRequest);

        QueryModel GetResults(int companyId, string query, string queryJson);
    }
}
