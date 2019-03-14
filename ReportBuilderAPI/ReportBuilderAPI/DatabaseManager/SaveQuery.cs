using System;

namespace ReportBuilderAPI.Queries
{
    /// <summary>
    /// Class that helps to manage the save queries
    /// </summary>
    public static class SaveQuery
    {
        /// <summary>
        /// Read queryId using query name and userId
        /// </summary>
        /// <param name="queryName"></param>
        /// <param name="userId"></param>
        /// <returns>Query</returns>
        public static string ReadQueryId(string queryName, int userId)
        {
            return "SELECT q.Id FROM dbo.Query q JOIN dbo.UserQuery uq ON uq.queryId=q.Id WHERE q.Name='" + queryName + "' AND uq.UserId=" + userId;
        }

        /// <summary>
        /// Insert userquery to the query table
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="queryName"></param>
        /// <param name="requestBody"></param>
        /// <param name="query"></param>
        /// <returns>Query</returns>
        public static string InsertQuery(Guid guid, string queryName, string requestBody, string query)
        {
            return "INSERT INTO dbo.QUERY(QueryId,Name, QUeryJson, QuerySQL, CreatedDate,LastModified) VALUES('" + guid + "','" + queryName + "','" + requestBody.Replace("'", "''") + "','" + query.Replace("'", "''") + "','" + DateTime.UtcNow.ToString("MM/dd/yyyy") + "','" + DateTime.UtcNow.ToString("MM/dd/yyyy") + "')";
        }

        /// <summary>
        /// Read use role details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Query</returns>
        public static string GetRoleId(int userId)
        {
            return "SELECT roleId FROM dbo.UserRole WHERE  userId=" + userId;
        }

        /// <summary>
        /// Insert user query details in the connection table
        /// </summary>
        /// <param name="queryId"></param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <param name="roleId"></param>
        /// <returns>Query</returns>
        public static string InsertUserQuery(int queryId, int userId, int companyId, int roleId)
        {
            return "INSERT INTO dbo.UserQuery(QueryId, UserId, CompanyId, Role) VALUES (" + queryId + "," + userId + "," + companyId + "," + roleId + ")";
        }

        /// <summary>
        /// Get user queries based on the companyId and UserId
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns>Query</returns>
        public static string GetUserQueries(int companyId, int userId)
        {
            return "SELECT q.*,  (ISNULL(NULLIF(u.LName, '') + ', ', '') + u.Fname)  as employeeName FROM dbo.Query q JOIN dbo.UserQuery uq on uq.QueryId=q.Id JOIN dbo.[User] u on u.Id=uq.UserId WHERE uq.CompanyId=" + companyId + " and uq.UserId=" + userId;
        }

        /// <summary>
        /// Update query name based on the queryId
        /// </summary>
        /// <param name="queryName"></param>
        /// <param name="queryId"></param>
        /// <returns>Query</returns>
        public static string UpdateQueryName(string queryName, string queryId)
        {
            return "UPDATE dbo.Query SET name='" + queryName + "' WHERE QueryId='" + queryId + "'";
        }


        /// <summary>
        /// Read the Id based on the queryId
        /// </summary>
        /// <param name="queryName"></param>
        /// <param name="queryId"></param>
        /// <returns>Query</returns>
        public static string ReadQuery(string queryId)
        {
            return "SELECT ID FROM dbo.Query WHERE QueryId='" + queryId + "'";
        }

        /// <summary>
        /// Delete the query based on the queryId
        /// </summary>
        /// <param name="queryName"></param>
        /// <param name="queryId"></param>
        /// <returns>Query</returns>
        public static string DeleteQuery(string queryId)
        {
            return "DELETE FROM dbo.Query WHERE QueryId='" + queryId + "'";
        }

        /// <summary>
        /// Delete the userquery based on the queryId
        /// </summary>
        /// <param name="queryName"></param>
        /// <param name="queryId"></param>
        /// <returns>Query</returns>
        public static string DeleteUserQuery(int queryId)
        {
            return "DELETE FROM dbo.UserQuery WHERE QueryId='" + queryId + "'";
        }

        /// <summary>
        /// Get the userqueries based on the queryId
        /// </summary>
        /// <param name="queryName"></param>
        /// <param name="queryId"></param>
        /// <returns>Query</returns>
        public static string GetUserQueries(int companyId, string queryId)
        {
            return "SELECT q.*,  (ISNULL(NULLIF(u.LName, '') + ', ', '') + u.Fname)  as employeeName FROM dbo.Query q JOIN dbo.UserQuery uq ON uq.QueryId=q.Id JOIN dbo.[User] u ON u.Id=uq.UserId WHERE uq.CompanyId=" + companyId + " AND q.QueryId='" + queryId
                + "'";
        }

    }
}
