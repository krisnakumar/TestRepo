using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Queries;
using ReportBuilderAPI.Resource;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;

namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Class that helps to handle the Save queries
    /// </summary>
    public class QueryRepository : IQuery
    {

        /// <summary>
        /// Generate the SQL Query based on the entity
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <param name="companyId"></param>
        /// <returns>string</returns>
        private string ProcessSaveQueryRequest(QueryBuilderRequest queryBuilderRequest, int companyId)
        {
            string query = string.Empty;
            try
            {
                //Generate the SQL Query based on the entity
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
        ///  Save the user query based on the user and company Id
        /// </summary>
        /// <param name="requestBody"></param>
        /// <param name="companyId"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse SaveQuery(string requestBody, int companyId)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            int rowAffected = 0;
            string query = string.Empty;
            int IsExist = 0, queryId = 0, userId = 0, roleId = 0;
            try
            {
                QueryBuilderRequest queryBuilderRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(requestBody);
                userId = queryBuilderRequest.UserId;
                if (userId != 0 && !string.IsNullOrEmpty(queryBuilderRequest.QueryName))
                {
                    //generate the custom query Id for each query
                    Guid guid = Guid.NewGuid();
                    query = ProcessSaveQueryRequest(queryBuilderRequest, companyId);
                    IsExist = databaseWrapper.ExecuteScalar(Queries.SaveQuery.ReadQueryId(queryBuilderRequest.QueryName, userId));
                    if (IsExist == 0)
                    {
                        rowAffected = databaseWrapper.ExecuteQuery(Queries.SaveQuery.InsertQuery(guid, queryBuilderRequest.QueryName, requestBody.Replace("'", "''"), query.Replace("'", "''")));
                        if (rowAffected > 0)
                        {
                            queryId = databaseWrapper.ExecuteScalar(Queries.SaveQuery.ReadQueryId(queryBuilderRequest.QueryName, userId));
                            roleId = databaseWrapper.ExecuteScalar(Queries.SaveQuery.GetRoleId(userId));
                            rowAffected = databaseWrapper.ExecuteQuery(Queries.SaveQuery.InsertUserQuery(queryId, userId, companyId, roleId));
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
                userId = queryBuilderRequest.UserId;
                if (userId > 0)
                {
                    SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(Queries.SaveQuery.GetUserQueries(companyId, userId), new Dictionary<string, string> { });
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
                else
                {
                    return ResponseBuilder.BadRequest("UserId");
                }
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
                userId = queryBuilderRequest.UserId;
                if (userId > 0 && !string.IsNullOrEmpty(queryBuilderRequest.QueryId) && !string.IsNullOrEmpty(queryBuilderRequest.QueryName))
                {
                    isExist = databaseWrapper.ExecuteScalar(Queries.SaveQuery.ReadQueryId(queryBuilderRequest.QueryName, userId));
                    if (isExist == 0)
                    {
                        rowAffected = databaseWrapper.ExecuteQuery(Queries.SaveQuery.UpdateQueryName(queryBuilderRequest.QueryName, queryBuilderRequest.QueryId));
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
                else
                {
                    return ResponseBuilder.BadRequest(DataResource.CHECK_INPUT);
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
                Id = databaseWrapper.ExecuteScalar(Queries.SaveQuery.ReadQuery(queryBuilderRequest.QueryId));
                if (Id != 0)
                {
                    rowAffected = databaseWrapper.ExecuteQuery(Queries.SaveQuery.DeleteQuery(queryBuilderRequest.QueryId));
                    if (rowAffected > 0)
                    {
                        rowAffected = databaseWrapper.ExecuteQuery(Queries.SaveQuery.DeleteUserQuery(Id));
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
                    return ResponseBuilder.BadRequest(DataResource.CHECK_INPUT);
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
                if (!string.IsNullOrEmpty(queryBuilderRequest.QueryId))
                {
                    SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(Queries.SaveQuery.GetUserQueries(companyId, queryBuilderRequest.QueryId), new Dictionary<string, string> { });
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
                    else
                    {
                        return ResponseBuilder.InternalError();
                    }
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(queryList), 0);
                }
                else
                {
                    return ResponseBuilder.BadRequest(DataResource.CHECK_INPUT);
                }
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
        /// Get query results based on the entity
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="query"></param>
        /// <param name="queryJson"></param>
        /// <returns>QueryModel</returns>
        public QueryModel GetResults(int companyId, string query, string queryJson)
        {
            TaskRepository taskRepository = new TaskRepository();
            QueryModel queryModel = new QueryModel();
            try
            {
                //Deserialize the query Json to get the results
                QueryBuilderRequest queryBuilderRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(queryJson);
                Dictionary<string, string> parameterList = taskRepository.Getparameters(queryBuilderRequest, companyId);
                switch (queryBuilderRequest.EntityName.ToUpper())
                {
                    case Constants.TASK:
                        List<TaskResponse> taskList = taskRepository.ReadTaskDetails(query, parameterList);
                        queryModel.Tasks = taskList;
                        break;
                    case Constants.WORKBOOK:
                        WorkbookRepository workbookRepository = new WorkbookRepository();
                        List<WorkbookResponse> workbookList = workbookRepository.ReadWorkBookDetails(query, parameterList);
                        queryModel.Workbooks = workbookList;
                        break;
                    case Constants.EMPLOYEE:
                        EmployeeRepository employeeRepository = new EmployeeRepository();
                        List<EmployeeResponse> employeeList = employeeRepository.ReadEmployeeDetails(query, parameterList);
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
