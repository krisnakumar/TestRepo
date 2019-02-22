using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Resource;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;

namespace ReportBuilderAPI.Repository
{
    public class QueryRepository
    {

        /// <summary>
        /// Generate the SQL Query based on the request
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="companyId"></param>
        public string ProcessSaveQueryRequest(QueryBuilderRequest queryBuilderRequest, int companyId)
        {
            string query = string.Empty;
            try
            {
                switch (queryBuilderRequest.EntityName)
                {
                    case Constants.TASK:
                        TaskRepository taskRepository = new TaskRepository();
                        query = taskRepository.CreateTaskQuery(queryBuilderRequest, companyId);
                        break;
                    case Constants.WORKBOOK:
                        WorkbookRepository workbookRepository = new WorkbookRepository();
                        query = workbookRepository.CreateWorkbookQuery(queryBuilderRequest, companyId);
                        break;
                    case Constants.EMPLOYEE:
                        EmployeeRepository employeeRepository = new EmployeeRepository();
                        query = employeeRepository.CreateEmployeeQuery(queryBuilderRequest, companyId);
                        break;
                    default:
                        break;
                }
                return query;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return string.Empty;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public APIGatewayProxyResponse SaveQuery(string requestBody, int companyId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            int rowAffected = 0;
            string query = string.Empty;
            int IsExist = 0, queryId = 0, userId = 0, roleId = 0;
            try
            {
                QueryBuilderRequest queryBuilderRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(requestBody);
                userId = Convert.ToInt32(queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.USERID).Select(x => x.Value).FirstOrDefault());
                if (userId != 0)
                {
                    //generate the custom query Id for each query
                    Guid guid = Guid.NewGuid();
                    query = ProcessSaveQueryRequest(queryBuilderRequest, companyId);
                    IsExist = databaseWrapper.ExecuteScalar("SELECT q.Id FROM Query q JOIN UserQuery uq on uq.queryId=q.Id where q.Name='" + queryBuilderRequest.QueryName + "'");
                    if (IsExist == 0)
                    {
                        rowAffected = databaseWrapper.ExecuteQuery("INSERT INTO QUERY(QueryId,Name, QUeryJson, QuerySQL, CreatedDate,LastModified) VALUES('" + guid + "','" + queryBuilderRequest.QueryName + "','" + requestBody.Replace("'", "''") + "','" + query.Replace("'", "''") + "','" + DateTime.UtcNow.ToString("MM/dd/yyyy") + "','" + DateTime.UtcNow.ToString("MM/dd/yyyy") + "')");
                        if (rowAffected > 0)
                        {
                            queryId = databaseWrapper.ExecuteScalar("SELECT Id FROM Query  where Name='" + queryBuilderRequest.QueryName + "'");

                            roleId = databaseWrapper.ExecuteScalar("select roleId from UserRole WHERE  userId=" + userId);
                            rowAffected = databaseWrapper.ExecuteQuery("INSERT INTO UserQuery(QueryId, UserId, CompanyId, Role) VALUES (" + queryId + "," + userId + "," + companyId + "," + roleId + ")");
                            if (rowAffected > 0)
                            {
                                return ResponseBuilder.SuccessMessage(DataResource.SAVE_QUERY_SUCCESS_MESSAGE);
                            }
                            else
                            {
                                return ResponseBuilder.InternalError();
                            }
                        }
                        else
                        {
                            return ResponseBuilder.InternalError();
                        }
                    }
                    else
                    {
                        return ResponseBuilder.BadRequest(DataResource.RENAME_QUERY_ERROR);
                    }
                }
                else
                {
                    return ResponseBuilder.BadRequest("UserId");
                }
            }
            catch (Exception saveQueryException)
            {
                LambdaLogger.Log(saveQueryException.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }

        /// <summary>
        /// Get list of user queires
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="companyId"></param>
        public APIGatewayProxyResponse GetUserQueries(string requestBody, int companyId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            int userId = 0;
            List<QueryResponse> queryList = new List<QueryResponse>();
            try
            {
                QueryBuilderRequest queryBuilderRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(requestBody);
                userId = Convert.ToInt32(queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.USERID).Select(x => x.Value).FirstOrDefault());
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader("SELECT q.*,  (ISNULL(NULLIF(u.LName, '') + ', ', '') + u.Fname)  as employeeName FROM Query q JOIN UserQuery uq on uq.QueryId=q.Id JOIN dbo.[User] u on u.Id=uq.UserId WHERE uq.CompanyId=" + companyId + " and uq.UserId=" + userId, new Dictionary<string, string> { });
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        QueryResponse queryResponse = new QueryResponse
                        {
                            QueryId = Convert.ToString(sqlDataReader["QueryId"]),
                            QueryName = Convert.ToString(sqlDataReader["Name"]),
                            CreatedDate = sqlDataReader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(sqlDataReader["CreatedDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy"),
                            LastModified = sqlDataReader["LastModified"] != DBNull.Value ? Convert.ToDateTime(sqlDataReader["LastModified"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy"),
                            CreatedBy = Convert.ToString(sqlDataReader["employeeName"])
                        };
                        queryList.Add(queryResponse);
                    }
                }
                return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(queryList), 0);
            }
            catch (Exception getUserQueriesException)
            {
                LambdaLogger.Log(getUserQueriesException.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse RenameQuery(string requestBody, int companyId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            int userId = 0, isExist = 0, rowAffected = 0;
            try
            {
                QueryBuilderRequest queryBuilderRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(requestBody);
                userId = Convert.ToInt32(queryBuilderRequest.Fields.Where(x => x.Name.ToUpper() == Constants.USERID).Select(x => x.Value).FirstOrDefault());
                isExist = databaseWrapper.ExecuteScalar("SELECT q.Id FROM Query q JOIN UserQuery uq on uq.queryId=q.Id where q.Name='" + queryBuilderRequest.QueryName + "' and uq.UserId=" + userId);
                if (isExist == 0)
                {
                    rowAffected = databaseWrapper.ExecuteQuery("UPDATE Query SET name='" + queryBuilderRequest.QueryName + "' WHERE QueryId='" + queryBuilderRequest.QueryId + "'");
                    if (rowAffected > 0)
                    {
                        return ResponseBuilder.SuccessMessage(DataResource.RENAME_SUCCESS_MESSAGE);
                    }
                    else
                    {
                        return ResponseBuilder.InternalError();
                    }
                }
                else
                {
                    return ResponseBuilder.BadRequest(DataResource.RENAME_QUERY_ERROR);
                }
            }
            catch (Exception renameQueryException)
            {
                LambdaLogger.Log(renameQueryException.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse DeleteQuery(string requestBody, int companyId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            int rowAffected = 0, Id = 0;
            try
            {
                QueryBuilderRequest queryBuilderRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(requestBody);
                Id = databaseWrapper.ExecuteScalar("SELECT ID FROM Query WHERE QueryId='" + queryBuilderRequest.QueryId + "'");
                if (Id != 0)
                {
                    rowAffected = databaseWrapper.ExecuteQuery("DELETE FROM Query WHERE QueryId='" + queryBuilderRequest.QueryId + "'");
                    if (rowAffected > 0)
                    {
                        rowAffected = databaseWrapper.ExecuteQuery("DELETE FROM UserQuery WHERE QueryId='" + Id + "'");
                        if (rowAffected > 0)
                        {
                            return ResponseBuilder.SuccessMessage(DataResource.DELETE_SUCCESS_MESSAGE);
                        }
                        else
                        {
                            return ResponseBuilder.InternalError();
                        }
                    }
                    else
                    {
                        return ResponseBuilder.InternalError();
                    }
                }
                else
                {
                    return ResponseBuilder.BadRequest(DataResource.RENAME_QUERY_ERROR);
                }
            }
            catch (Exception renameQueryException)
            {
                LambdaLogger.Log(renameQueryException.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        /// Get list of user queires
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="companyId"></param>
        public APIGatewayProxyResponse GetUserQuery(string requestBody, int companyId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<QueryResponse> queryList = new List<QueryResponse>();

            try
            {
                QueryBuilderRequest queryBuilderRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(requestBody);
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader("SELECT q.*,  (ISNULL(NULLIF(u.LName, '') + ', ', '') + u.Fname)  as employeeName FROM Query q JOIN UserQuery uq on uq.QueryId=q.Id JOIN dbo.[User] u on u.Id=uq.UserId WHERE uq.CompanyId=" + companyId + " and q.QueryId='" + queryBuilderRequest.QueryId + "'", new Dictionary<string, string> { });
                if (sqlDataReader != null && sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        QueryResponse queryResponse = new QueryResponse
                        {
                            QueryJson = Convert.ToString(sqlDataReader["QueryJson"]),
                            QueryModel = GetResults(companyId, Convert.ToString(sqlDataReader["QuerySQL"]), Convert.ToString(sqlDataReader["QueryJson"]))
                        };
                        queryList.Add(queryResponse);
                    }
                }
                return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(queryList), 0);
            }
            catch (Exception getUserQueriesException)
            {
                LambdaLogger.Log(getUserQueriesException.ToString());
                return ResponseBuilder.InternalError();
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="query"></param>
        /// <param name="queryJson"></param>
        /// <returns></returns>
        public QueryModel GetResults(int companyId, string query, string queryJson)
        {
            TaskRepository taskRepository = new TaskRepository();
            QueryModel queryModel = new QueryModel();
            try
            {
                QueryBuilderRequest queryBuilderRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(queryJson);
                Dictionary<string, string> parameterList = taskRepository.Getparameters(queryBuilderRequest, companyId);
                switch (queryBuilderRequest.EntityName.ToUpper())
                {
                    case Constants.TASK:
                        List<TaskResponse> taskList = new List<TaskResponse>();
                        taskList = taskRepository.ReadTaskDetails(query, parameterList);
                        queryModel.Tasks = taskList;
                        break;
                    case Constants.WORKBOOK:
                        WorkbookRepository workbookRepository = new WorkbookRepository();
                        List<WorkbookResponse> workbookList = new List<WorkbookResponse>();
                        workbookList = workbookRepository.ReadWorkBookDetails(query, parameterList);
                        queryModel.Workbooks = workbookList;
                        break;
                    case Constants.EMPLOYEE:
                        EmployeeRepository employeeRepository = new EmployeeRepository();
                        List<EmployeeResponse> employeeList = new List<EmployeeResponse>();
                        employeeList = employeeRepository.ReadEmployeeDetails(query, parameterList);
                        queryModel.Employee = employeeList;
                        break;
                    default:
                        break;
                }
                return queryModel;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return queryModel;
            }
        }
    }
}
