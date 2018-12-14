
/*
  <copyright file="Workbook.cs">
    Copyright (c) 2018 All Rights Reserved
  </copyright>
  <author>Shoba Eswar</author>
  <date>10-10-2018</date>
  <summary>
    Repository responsible for all the queries that handles the workbook operations
  </summary>
 */
namespace ReportBuilderAPI.DatabaseManager
{
    /// <summary>
    ///      Class that creates queries for workbook operations
    /// </summary>
    public class Workbook
    {
        /// <summary>
        ///     Creates query to read list of assigned workbook(s) details for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Query</returns>
        public static string ReadWorkbookDetails(int userId)
        {
            return "EXEC sp_GetWorkbook_Details " + userId;
        }


        /// <summary>
        ///     Creates query to read list of past due workbook(s) details for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Query</returns>
        public static string ReadPastDueWorkbookDetails(int userId)
        {
            return "EXEC sp_PastDue_Workbook " + userId;
        }


        /// <summary>
        ///     Creates query to read list of coming due workbook(s) details for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Query</returns>
        public static string ReadInDueWorkbookDetails(int userId)
        {
            return "EXEC sp_InDue_Workbook " + userId;
        }


        /// <summary>
        ///     Creates query to read list of completed workbook(s) details for a user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Query</returns>
        public static string CompletedWorkbookDetails(int userId)
        {
            return "EXEC sp_Completed_Workbook " + userId;
        }
    }
}
