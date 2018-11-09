// <copyright file="TaskQueries.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
// <date>10-10-2018</date>
// <summary>Queries that handles the task operations</summary>
namespace ReportBuilderAPI.DatabaseManager
{
    /// <summary>
    /// Class that handles the queries for the task operations
    /// </summary>
    public class TaskQueries
    {
        /// <summary>
        /// get task list using userId and workbookid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="workbookId"></param>
        /// <returns></returns>
        public static string GetTaskList(int userId, int workbookId)
        {
            return "EXEC [sp_GetTask_Details] " + userId + "," + workbookId;
        }

        /// <summary>
        /// Get task attempts details using userId and workbookid
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="workbookId"></param>
        /// <returns></returns>
        public static string GetTaskAttemptsDetails(int userId, int workbookId, int taskId)
        {
            return "EXEC [sp_GetTaskAttempts_Details] " + userId + "," + workbookId + "," + taskId;
        }
    }
}
