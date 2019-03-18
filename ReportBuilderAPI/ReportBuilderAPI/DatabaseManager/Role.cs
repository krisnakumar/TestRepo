namespace ReportBuilderAPI.Queries
{
    /// <summary>
    /// Class that helps to maintain the list of roles
    /// </summary>
    public static class Role
    {
        /// <summary>
        /// Get list of roles based on the companyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>string</returns>
        public static string GetRole(int companyId)
        {
            return "SELECT Name, Id FROM dbo.Role WHERE CompanyId=" + companyId + " AND IsShared=1 AND IsEnabled=1";
        }
    }
}
