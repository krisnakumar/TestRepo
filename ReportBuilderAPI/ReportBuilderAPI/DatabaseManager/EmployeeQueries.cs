

// <copyright file="EmployeeQueries.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
// <date>10-10-2018</date>
// <summary>Queries that handles the employee operations</summary>
namespace ReportBuilderAPI.DatabaseManager
{
    /// <summary>
    /// Class that handles the employee operations
    /// </summary>
    public class EmployeeQueries
    {
        /// <summary>
        /// Read list of employee details using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string ReadEmployeeDetails(int userId)
        {
            return "EXEC GetEmployee " + userId;
        }

        /// <summary>
        /// Read list of employee details using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetWorkBookDetails(int userId, int completedWorkBooks, int workbookInDue, int pastWorkbook)
        {
            return "EXEC sp_GetWorkBook " + userId + "," + completedWorkBooks + "," + workbookInDue + "," + pastWorkbook;
        }


        /// <summary>
        /// Get company Id details using email 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static string GetCompanyId(string email)
        {
            return "SELECT Id FROM [USER] WHERE Email='" + email + "'";
        }
    }
}
