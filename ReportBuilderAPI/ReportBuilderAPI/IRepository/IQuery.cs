using Amazon.Lambda.APIGatewayEvents;
using ReportBuilder.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    /// Interface that handles the Save queries
    /// </summary>
    public interface IQuery
    {
        APIGatewayProxyResponse SaveQuery(string requestBody, int companyId);
        APIGatewayProxyResponse GetUserQueries(string requestBody, int companyId);

        APIGatewayProxyResponse RenameQuery(string requestBody, int companyId);

        APIGatewayProxyResponse DeleteQuery(string requestBody, int companyId);

        APIGatewayProxyResponse GetUserQuery(string requestBody, int companyId);

        QueryModel GetResults(int companyId, string query, string queryJson);
    }
}
