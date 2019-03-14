using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Helpers;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Resource;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;


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
        /// <returns>string</returns>
        private string ProcessSaveQueryRequest(QueryBuilderRequest queryBuilderRequest)
        {
            string query = string.Empty;
            try
            {
                //Generate the SQL Query based on the entity
                switch (queryBuilderRequest.EntityName)
                {
                    case Constants.TASK:
                        TaskRepository taskRepository = new TaskRepository();
                        query = taskRepository.CreateTaskQuery(queryBuilderRequest);
                        break;
                    case Constants.WORKBOOK:
                        WorkbookRepository workbookRepository = new WorkbookRepository();
                        query = workbookRepository.CreateWorkbookQuery(queryBuilderRequest);
                        break;
                    case Constants.EMPLOYEE:
                        EmployeeRepository employeeRepository = new EmployeeRepository();
                        query = employeeRepository.CreateEmployeeQuery(queryBuilderRequest);
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
        /// <param name="queryBuilderRequest"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse SaveQuery(QueryBuilderRequest queryBuilderRequest)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            int rowAffected = 0;
            string query = string.Empty;
            int IsExist = 0, queryId = 0, userId = 0, roleId = 0;
            QueryResponse queryResponse = new QueryResponse();
            try
            {
                userId = queryBuilderRequest.UserId;
                if (userId != 0 && !string.IsNullOrEmpty(queryBuilderRequest.QueryName))
                {
                    //generate the custom query Id for each query
                    Guid guid = Guid.NewGuid();
                    query = ProcessSaveQueryRequest(queryBuilderRequest);
                    IsExist = databaseWrapper.ExecuteScalar(Queries.SaveQuery.ReadQueryId(queryBuilderRequest.QueryName, userId));
                    if (IsExist == 0)
                    {
                        rowAffected = databaseWrapper.ExecuteQuery(Queries.SaveQuery.InsertQuery(guid, queryBuilderRequest.QueryName, JsonConvert.SerializeObject(queryBuilderRequest).Replace("'", "''"), query.Replace("'", "''")));
                        if (rowAffected > 0)
                        {
                            queryId = databaseWrapper.ExecuteScalar(Queries.SaveQuery.ReadQueryId(queryBuilderRequest.QueryName, userId));
                            roleId = databaseWrapper.ExecuteScalar(Queries.SaveQuery.GetRoleId(userId));
                            rowAffected = databaseWrapper.ExecuteQuery(Queries.SaveQuery.InsertUserQuery(queryId, userId, queryBuilderRequest.CompanyId, roleId));
                            if (rowAffected > 0)
                            {
                                queryResponse.Message = DataResource.SAVE_QUERY_SUCCESS_MESSAGE;
                            }
                            else
                            {
                                queryResponse.Error = ResponseBuilder.InternalError();
                            }
                        }
                        else
                        {
                            queryResponse.Error = ResponseBuilder.InternalError();
                        }
                    }
                    else
                    {
                        queryResponse.Error = ResponseBuilder.BadRequest(DataResource.RENAME_QUERY_ERROR);
                    }
                }
                else
                {
                    queryResponse.Error = ResponseBuilder.BadRequest("UserId");
                }
                return queryResponse;
            }
            catch (Exception saveQueryException)
            {
                LambdaLogger.Log(saveQueryException.ToString());
                queryResponse.Error = ResponseBuilder.InternalError();
                return queryResponse;
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }

        /// <summary>
        /// Get list of user queires
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse GetUserQueries(QueryBuilderRequest queryBuilderRequest)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            int userId = 0;
            List<QueryModel> queryList = new List<QueryModel>();
            QueryResponse queryResponse = new QueryResponse();
            try
            {
                userId = queryBuilderRequest.UserId;
                if (userId > 0)
                {
                    using (SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(Queries.SaveQuery.GetUserQueries(queryBuilderRequest.CompanyId, userId), new Dictionary<string, string> { }))
                    {
                        if (sqlDataReader != null && sqlDataReader.HasRows)
                        {
                            while (sqlDataReader.Read())
                            {
                                QueryModel queryModel = new QueryModel
                                {
                                    QueryId = Convert.ToString(sqlDataReader["QueryId"]),
                                    QueryName = Convert.ToString(sqlDataReader["Name"]),
                                    CreatedDate = sqlDataReader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(sqlDataReader["CreatedDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy"),
                                    LastModified = sqlDataReader["LastModified"] != DBNull.Value ? Convert.ToDateTime(sqlDataReader["LastModified"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy"),
                                    CreatedBy = Convert.ToString(sqlDataReader["employeeName"])
                                };
                                queryList.Add(queryModel);
                            }
                        }
                        queryResponse.Queries = queryList;
                    }
                    return queryResponse;
                }
                else
                {
                    queryResponse.Error = ResponseBuilder.BadRequest("UserId");
                    return queryResponse;
                }
            }
            catch (Exception getUserQueriesException)
            {
                LambdaLogger.Log(getUserQueriesException.ToString());
                queryResponse.Error = ResponseBuilder.InternalError();
                return queryResponse;
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }

        /// <summary>
        /// Rename the existing query using queryId
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <returns></returns>
        public QueryResponse RenameQuery(QueryBuilderRequest queryBuilderRequest)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            int userId = 0, isExist = 0, rowAffected = 0;
            QueryResponse queryResponse = new QueryResponse();
            try
            {
                userId = queryBuilderRequest.UserId;
                if (userId > 0 && !string.IsNullOrEmpty(queryBuilderRequest.QueryId) && !string.IsNullOrEmpty(queryBuilderRequest.QueryName))
                {
                    isExist = databaseWrapper.ExecuteScalar(Queries.SaveQuery.ReadQueryId(queryBuilderRequest.QueryName, userId));
                    if (isExist == 0)
                    {
                        rowAffected = databaseWrapper.ExecuteQuery(Queries.SaveQuery.UpdateQueryName(queryBuilderRequest.QueryName, queryBuilderRequest.QueryId));
                        if (rowAffected > 0)
                        {
                            queryResponse.Message = DataResource.RENAME_SUCCESS_MESSAGE;
                        }
                        else
                        {
                            queryResponse.Error = ResponseBuilder.InternalError();
                        }
                    }
                    else
                    {
                        queryResponse.Error = ResponseBuilder.BadRequest(DataResource.RENAME_QUERY_ERROR);
                    }
                }
                else
                {
                    queryResponse.Error = ResponseBuilder.BadRequest(DataResource.CHECK_INPUT);
                }
                return queryResponse;
            }
            catch (Exception renameQueryException)
            {
                LambdaLogger.Log(renameQueryException.ToString());
                queryResponse.Error = ResponseBuilder.InternalError();
                return queryResponse;
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        /// Delete the existing query based on the queryId
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse DeleteQuery(QueryBuilderRequest queryBuilderRequest)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            int rowAffected = 0, Id = 0;
            QueryResponse queryResponse = new QueryResponse();
            try
            {
                Id = databaseWrapper.ExecuteScalar(Queries.SaveQuery.ReadQuery(queryBuilderRequest.QueryId));
                if (Id != 0)
                {
                    rowAffected = databaseWrapper.ExecuteQuery(Queries.SaveQuery.DeleteQuery(queryBuilderRequest.QueryId));
                    if (rowAffected > 0)
                    {
                        rowAffected = databaseWrapper.ExecuteQuery(Queries.SaveQuery.DeleteUserQuery(Id));
                        if (rowAffected > 0)
                        {
                            queryResponse.Message = DataResource.DELETE_SUCCESS_MESSAGE;
                        }
                        else
                        {
                            queryResponse.Error = ResponseBuilder.InternalError();
                        }
                    }
                    else
                    {
                        queryResponse.Error = ResponseBuilder.InternalError();
                    }
                }
                else
                {
                    queryResponse.Error = ResponseBuilder.BadRequest(DataResource.CHECK_INPUT);
                }
                return queryResponse;
            }
            catch (Exception deleteQueryException)
            {
                LambdaLogger.Log(deleteQueryException.ToString());
                queryResponse.Error = ResponseBuilder.InternalError();
                return queryResponse;
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        /// Get list of user queires
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse GetUserQuery(QueryBuilderRequest queryBuilderRequest)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<QueryModel> queryList = new List<QueryModel>();
            QueryResponse queryResponse = new QueryResponse();
            try
            {
                if (!string.IsNullOrEmpty(queryBuilderRequest.QueryId))
                {
                    using (SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader(Queries.SaveQuery.GetUserQueries(queryBuilderRequest.CompanyId, queryBuilderRequest.QueryId), new Dictionary<string, string> { }))
                    {
                        if (sqlDataReader != null && sqlDataReader.HasRows)
                        {
                            while (sqlDataReader.Read())
                            {
                                QueryModel queryModel = new QueryModel
                                {
                                    QueryJson = Convert.ToString(sqlDataReader["QueryJson"]),
                                    QueryResult = GetResults(queryBuilderRequest.CompanyId, Convert.ToString(sqlDataReader["QuerySQL"]), Convert.ToString(sqlDataReader["QueryJson"]))
                                };
                                queryList.Add(queryModel);
                            }
                            queryResponse.Queries = queryList;
                        }
                        else
                        {
                            queryResponse.Error = ResponseBuilder.InternalError();
                        }
                    }
                }
                else
                {
                    queryResponse.Error = ResponseBuilder.BadRequest(DataResource.CHECK_INPUT);
                }
                return queryResponse;
            }
            catch (Exception getUserQueriesException)
            {
                LambdaLogger.Log(getUserQueriesException.ToString());
                queryResponse.Error = ResponseBuilder.InternalError();
                return queryResponse;
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
            QueryModel queryModel = new QueryModel();
            try
            {
                //Deserialize the query Json to get the results
                QueryBuilderRequest queryBuilderRequest = JsonConvert.DeserializeObject<QueryBuilderRequest>(queryJson);
                Dictionary<string, string> parameterList = ParameterHelper.Getparameters(queryBuilderRequest);
                switch (queryBuilderRequest.EntityName.ToUpper())
                {
                    case Constants.TASK:
                        TaskRepository taskRepository = new TaskRepository();
                        List<TaskModel> taskList = taskRepository.ReadTaskDetails(query, parameterList);
                        queryModel.Tasks = taskList;
                        break;
                    case Constants.WORKBOOK:
                        WorkbookRepository workbookRepository = new WorkbookRepository();
                        List<WorkbookModel> workbookList = workbookRepository.ReadWorkBookDetails(query, parameterList);
                        queryModel.Workbooks = workbookList;
                        break;
                    case Constants.EMPLOYEE:
                        EmployeeRepository employeeRepository = new EmployeeRepository();
                        List<EmployeeQueryModel> employeeList = employeeRepository.ReadEmployeeDetails(query, parameterList);
                        queryModel.Employee = employeeList;
                        break;
                    default:
                        break;
                }
                return queryModel;
            }
            catch (Exception getResultsexception)
            {
                LambdaLogger.Log(getResultsexception.ToString());
                return queryModel;
            }
        }
    }
}
