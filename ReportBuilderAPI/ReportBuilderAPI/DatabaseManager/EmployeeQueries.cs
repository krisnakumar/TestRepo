

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
    }
}
