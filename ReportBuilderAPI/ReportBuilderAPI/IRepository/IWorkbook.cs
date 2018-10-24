using Amazon.Lambda.APIGatewayEvents;

namespace ReportBuilderAPI.IRepository
{
    public interface IWorkbook
    {
        APIGatewayProxyResponse GetWorkbookDetails(int userId);
    }
}
