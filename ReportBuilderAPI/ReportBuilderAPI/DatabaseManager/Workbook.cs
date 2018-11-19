// <copyright file="WorkbookQueries.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
// <date>10-10-2018</date>
// <summary>Queries that handles the workbook operations</summary>
namespace ReportBuilderAPI.DatabaseManager
{
    /// <summary>
    ///Queries that handles the workbook operations
    /// </summary>
    public class Workbook
    {
        /// <summary>
        /// Read list of workbook details using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string ReadWorkbookDetails(int userId)
        {
            return "EXEC sp_GetWorkbook_Details " + userId;
        }


        /// <summary>
        /// Read list of workbook details using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string ReadPastDueWorkbookDetails(int userId)
        {
            return "EXEC sp_PastDue_Workbook " + userId;
        }


        /// <summary>
        /// Read list of workbook details using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string ReadInDueWorkbookDetails(int userId)
        {
            return "EXEC sp_InDue_Workbook " + userId;
        }


        /// <summary>
        /// Read list of workbook details using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string CompletedWorkbookDetails(int userId)
        {
            return "EXEC sp_Completed_Workbook " + userId;
        }
    }
}
