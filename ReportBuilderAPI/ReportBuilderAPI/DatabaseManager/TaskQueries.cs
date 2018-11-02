namespace ReportBuilderAPI.DatabaseManager
{
    public class TaskQueries
    {
        public static string GetTaskList(int userId, int workbookId)
        {
            return "EXEC [sp_GetTask_Details] " + userId + "," + workbookId;
        }

        public static string GetTaskAttemptsDetails(int userId, int workbookId)
        {
            return "EXEC [sp_GetTask_Details] " + userId + "," + workbookId;
        }
    }
}
