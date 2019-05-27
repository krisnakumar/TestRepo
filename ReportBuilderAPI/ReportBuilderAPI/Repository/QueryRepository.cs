using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Helpers;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Logger;
using ReportBuilderAPI.Resource;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;



namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Class that helps to handle the Save queries
    /// </summary>
    public class QueryRepository : DatabaseWrapper, IQuery
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
            int rowAffected = 0;
            string query = string.Empty;
            int IsExist = 0, queryId = 0, userId = 0, roleId = 0;
            QueryResponse queryResponse = new QueryResponse();
            try
            {
                if (queryBuilderRequest.CompanyId == 0) throw new ArgumentException("CompanyId");
                if (queryBuilderRequest.UserId == 0) throw new ArgumentException("UserId");
                if(string.IsNullOrEmpty(queryBuilderRequest.QueryName)) throw new ArgumentException("QueryName");


                userId = queryBuilderRequest.UserId;
                if (userId != 0 && !string.IsNullOrEmpty(queryBuilderRequest.QueryName))
                {
                    //generate the custom query Id for each query
                    Guid guid = Guid.NewGuid();
                    query = ProcessSaveQueryRequest(queryBuilderRequest);
                    IsExist = ExecuteScalar(Queries.SaveQuery.ReadQueryId(queryBuilderRequest.QueryName, userId));
                    if (IsExist == 0)
                    {
                        rowAffected = ExecuteQuery(Queries.SaveQuery.InsertQuery(guid, queryBuilderRequest.QueryName, JsonConvert.SerializeObject(queryBuilderRequest).Replace("'", "''"), query.Replace("'", "''")));
                        if (rowAffected > 0)
                        {
                            queryId = ExecuteScalar(Queries.SaveQuery.ReadQueryId(queryBuilderRequest.QueryName, userId));
                            roleId = ExecuteScalar(Queries.SaveQuery.GetRoleId(userId));
                            rowAffected = ExecuteQuery(Queries.SaveQuery.InsertUserQuery(queryId, userId, queryBuilderRequest.CompanyId, roleId));
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
                    queryResponse.Error = ResponseBuilder.BadRequest(DataResource.USER_ID);
                }
                return queryResponse;
            }
            catch (Exception saveQueryException)
            {
                LambdaLogger.Log(saveQueryException.ToString());
                queryResponse.Error = new ExceptionHandler(saveQueryException).ExceptionResponse();
                return queryResponse;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Get list of user queires
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse GetUserQueries(QueryBuilderRequest queryBuilderRequest)
        {            
            int userId = 0;
            List<QueryModel> queryList = new List<QueryModel>();
            QueryResponse queryResponse = new QueryResponse();
            try
            {
                if (queryBuilderRequest.CompanyId == 0) throw new ArgumentException("CompanyId");
                if (queryBuilderRequest.UserId == 0) throw new ArgumentException("UserId");
                

                userId = queryBuilderRequest.UserId;
                if (userId > 0)
                {
                    using (IDataReader  dataReader = ExecuteReader(Queries.SaveQuery.GetUserQueries(queryBuilderRequest.CompanyId, userId), null))
                    {
                        if (dataReader != null )
                        {
                            while (dataReader.Read())
                            {
                                QueryModel queryModel = new QueryModel
                                {
                                    QueryId = Convert.ToString(dataReader["QueryId"]),
                                    QueryName = Convert.ToString(dataReader["Name"]),
                                    CreatedDate = dataReader["CreatedDate"] != DBNull.Value ? Convert.ToDateTime(dataReader["CreatedDate"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy"),
                                    LastModified = dataReader["LastModified"] != DBNull.Value ? Convert.ToDateTime(dataReader["LastModified"]).ToString("MM/dd/yyyy") : default(DateTime).ToString("MM/dd/yyyy"),
                                    CreatedBy = Convert.ToString(dataReader["employeeName"])
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
                    queryResponse.Error = ResponseBuilder.BadRequest(DataResource.USER_ID);
                    return queryResponse;
                }
            }
            catch (Exception getUserQueriesException)
            {
                LambdaLogger.Log(getUserQueriesException.ToString());
                queryResponse.Error = new ExceptionHandler(getUserQueriesException).ExceptionResponse();
                return queryResponse;
            }
            finally
            {
                CloseConnection();
            }
        }

        /// <summary>
        /// Rename the existing query using queryId
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <returns></returns>
        public QueryResponse RenameQuery(QueryBuilderRequest queryBuilderRequest)
        {            
            int userId = 0, isExist = 0, rowAffected = 0;
            QueryResponse queryResponse = new QueryResponse();
            try
            {
                if (queryBuilderRequest.CompanyId == 0) throw new ArgumentException("CompanyId");
                if (queryBuilderRequest.UserId == 0) throw new ArgumentException("UserId");
                if (string.IsNullOrEmpty(queryBuilderRequest.QueryName)) throw new ArgumentException("QueryName");
                if (string.IsNullOrEmpty(queryBuilderRequest.QueryId)) throw new ArgumentException("QueryId");

                userId = queryBuilderRequest.UserId;
                if (userId > 0 && !string.IsNullOrEmpty(queryBuilderRequest.QueryId) && !string.IsNullOrEmpty(queryBuilderRequest.QueryName))
                {
                    isExist = ExecuteScalar(Queries.SaveQuery.ReadQueryId(queryBuilderRequest.QueryName, userId));
                    if (isExist == 0)
                    {
                        rowAffected = ExecuteQuery(Queries.SaveQuery.UpdateQueryName(queryBuilderRequest.QueryName, queryBuilderRequest.QueryId));
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
                queryResponse.Error = new ExceptionHandler(renameQueryException).ExceptionResponse();
                return queryResponse;
            }
            finally
            {
                CloseConnection();
            }
        }


        /// <summary>
        /// Delete the existing query based on the queryId
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse DeleteQuery(QueryBuilderRequest queryBuilderRequest)
        {            
            int rowAffected = 0, Id = 0;
            QueryResponse queryResponse = new QueryResponse();
            try
            {
                if (string.IsNullOrEmpty(queryBuilderRequest.QueryId)) throw new ArgumentException("QueryId");
                Id = ExecuteScalar(Queries.SaveQuery.ReadQuery(queryBuilderRequest.QueryId));
                if (Id != 0)
                {
                    rowAffected = ExecuteQuery(Queries.SaveQuery.DeleteQuery(queryBuilderRequest.QueryId));
                    if (rowAffected > 0)
                    {
                        rowAffected = ExecuteQuery(Queries.SaveQuery.DeleteUserQuery(Id));
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
                queryResponse.Error = new ExceptionHandler(deleteQueryException).ExceptionResponse();
                return queryResponse;
            }
            finally
            {
                CloseConnection();
            }
        }


        /// <summary>
        /// Get list of user queires
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse GetUserQuery(QueryBuilderRequest queryBuilderRequest)
        {            
            List<QueryModel> queryList = new List<QueryModel>();
            QueryResponse queryResponse = new QueryResponse();
            try
            {
                if (queryBuilderRequest.CompanyId == 0) throw new ArgumentException("CompanyId");
                if (queryBuilderRequest.UserId == 0) throw new ArgumentException("UserId");                
                if (string.IsNullOrEmpty(queryBuilderRequest.QueryId)) throw new ArgumentException("QueryId");

                if (!string.IsNullOrEmpty(queryBuilderRequest.QueryId))
                {
                    using (IDataReader dataReader = ExecuteReader(Queries.SaveQuery.GetUserQueries(queryBuilderRequest.CompanyId, queryBuilderRequest.QueryId), null))
                    {
                        if (dataReader != null )
                        {
                            while (dataReader.Read())
                            {
                                QueryModel queryModel = new QueryModel
                                {
                                    QueryJson = Convert.ToString(dataReader["QueryJson"]),
                                    QueryResult = GetResults(queryBuilderRequest.CompanyId, Convert.ToString(dataReader["QuerySQL"]), Convert.ToString(dataReader["QueryJson"]))
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
                queryResponse.Error = new ExceptionHandler(getUserQueriesException).ExceptionResponse();
                return queryResponse;
            }
            finally
            {
                CloseConnection();
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
                        List<TaskModel> taskList = taskRepository.ReadTaskDetails(query, parameterList, queryBuilderRequest);
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
