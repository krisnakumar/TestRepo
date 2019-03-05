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
        /// <returns></returns>
        public static string GetRole(int companyId)
        {
            return "select Name, Id from role WHERE CompanyId=" + companyId + " and IsShared=1";
        }
    }
}
