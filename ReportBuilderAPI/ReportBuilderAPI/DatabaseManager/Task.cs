
/*
  <copyright file="Task.cs">
        Copyright (c) 2018 All Rights Reserved
  </copyright>
  <author>Shoba Eswar</author>
  <date>10-10-2018</date>
  <summary>
        Repository responsible for all the queries that handles task operations
  </summary>
*/
namespace ReportBuilderAPI.DatabaseManager
{
    /// <summary>
    ///     Class that creates queries for task operations
    /// </summary>
    public class Task
    {
        /// <summary>
        ///     Creates query to get task(s) list under a workbook for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="workbookId"></param>
        /// <returns>Query</returns>
        public static string GetTaskList(int userId, int workbookId)
        {
            return "EXEC [sp_GetTask_Details] " + userId + "," + workbookId;
        }

        /// <summary>
        ///     Creates query to get task attempt(s) details under a workbook for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="workbookId"></param>
        /// <param name="taskId"></param>
        /// <returns>Query</returns>
        public static string GetTaskAttemptsDetails(int userId, int workbookId, int taskId)
        {
            return "EXEC [sp_GetTaskAttempts_Details] " + userId + "," + workbookId + "," + taskId;
        }
    }
}
