
/*
 <copyright file="Employee.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>10-10-2018</date>
 <summary>
    Repository responsible for all the queries that handles employee operations
 </summary>
*/
namespace ReportBuilderAPI.DatabaseManager
{
    /// <summary>
    ///     Class that creates queries for the employee operations
    /// </summary>
    public class Employee
    {
        /// <summary>
        ///     Creates query to read list of employee(s) details using userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Query</returns>
        public static string ReadEmployeeDetails(int userId)
        {
            return "EXEC GetEmployee " + userId;
        }

        /// <summary>
        ///     Creates query to read list of workbook(s) details
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="completedWorkBooks"></param>
        /// <param name="workbookInDue"></param>
        /// <param name="pastWorkbook"></param>
        /// <returns>Query</returns>
        public static string GetWorkBookDetails(int userId, int completedWorkBooks, int workbookInDue, int pastWorkbook)
        {
            return "EXEC sp_GetWorkBook " + userId + "," + completedWorkBooks + "," + workbookInDue + "," + pastWorkbook;
        }


        /// <summary>
        ///     Creates query to get userId details using email 
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Query</returns>
        public static string GetUserId(string email)
        {
            return "SELECT Id FROM [USER] WHERE Email='" + email + "'";
        }

        /// <summary>
        ///     Creates query to get companyId details using email 
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Query</returns>
        public static string GetCompanyId(string email)
        {
            return "SELECT uc.companyId FROM [USER] u JOIN UserCompany uc on uc.UserId=u.Id WHERE Email='" + email + "'";
        }
    }
}
