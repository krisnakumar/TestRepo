using Amazon.Lambda.APIGatewayEvents;

namespace ReportBuilderAPI.IRepository
{
    public interface ITask
    {
        APIGatewayProxyResponse GetTaskDetails(int userId, int workbookId);

        APIGatewayProxyResponse GetTaskAttemptsDetails(int userId, int workbookId, int taskId);
    }
}
