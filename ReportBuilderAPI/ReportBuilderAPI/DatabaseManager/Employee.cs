
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
    public static class Employee
    {
        
        /// <summary>
        ///     Creates query to get userId details using email 
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Query</returns>
        public static string GetUserId(string email)
        {
            return "SELECT Id FROM dbo.[USER] WHERE Email='" + email + "' AND IsEnabled=1";
        }

        /// <summary>
        ///     Creates query to get companyId details using email 
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Query</returns>
        public static string GetCompanyId(string email)
        {
            return "SELECT uc.companyId FROM dbo.[USER] u JOIN dbo.UserCompany uc on uc.UserId=u.Id WHERE Email='" + email + "'  AND u.IsEnabled=1 AND uc.IsEnabled=1";
        }
    }
}
