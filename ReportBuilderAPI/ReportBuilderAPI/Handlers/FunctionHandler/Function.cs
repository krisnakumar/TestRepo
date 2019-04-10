using Amazon.Lambda.Core;
using DataInterface.Database;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Helpers;
using ReportBuilderAPI.Repository;
using ReportBuilderAPI.Utilities;
using System;
using System.Linq;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace ReportBuilderAPI.Handlers.FunctionHandler
{
    /// <summary>
    ///     This class contains list of lambda fucntions
    /// </summary>
    public class Function
    {
        public Function()
        {
            DatabaseWrapper._connectionString = Environment.GetEnvironmentVariable("ConnectionString").ToString();
        }
        /// <summary>
        ///  Function that helps to validate the user using user name and password
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>UserResponse</returns>
        public UserResponse Login(UserRequest userRequest, ILambdaContext context = null)
        {
            AuthenticationRepository authenticationRepository = new AuthenticationRepository();
            UserResponse userResponse = new UserResponse();
            try
            {
                userRequest.CognitoPoolId = Environment.GetEnvironmentVariable("CognitoPoolId").ToString();
                userRequest.CognitoClientId = Environment.GetEnvironmentVariable("CognitoClientId").ToString();
                return authenticationRepository.Login(userRequest, context);

            }
            catch (Exception loginException)
            {
                LambdaLogger.Log(loginException.ToString());
                userResponse.Error = ResponseBuilder.InternalError();
                return userResponse;
            }
        }
        /// <summary>
        ///   This function generates new Identity token using refresh token
        /// </summary>
        /// <param name="userRequest"></param>
        /// <param name="context"></param>
        /// <returns>UserResponse</returns>
        public UserResponse SilentAuth(UserRequest userRequest, ILambdaContext context = null)
        {
            AuthenticationRepository authenticationRepository = new AuthenticationRepository();
            UserResponse userResponse = new UserResponse();
            try
            {
                userRequest.CognitoClientId = Environment.GetEnvironmentVariable("CognitoClientId").ToString();
                //userRequest.ClientSecret = Environment.GetEnvironmentVariable("ClientSecret").ToString();
                return authenticationRepository.SilentAuth(userRequest);
            }
            catch (Exception silentAuthException)
            {
                LambdaLogger.Log(silentAuthException.ToString());
                userResponse.Error = ResponseBuilder.InternalError();
                return userResponse;
            }
        }



        /// <summary>
        /// [QueryBuilder] Function to get list of employee(s) filtered with some condition(s)
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <param name="context"></param>
        /// <returns>EmployeeResponse</returns>
        public EmployeeResponse GetEmployeesQueryBuilder(QueryBuilderRequest queryBuilderRequest, ILambdaContext context = null)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository();
            EmployeeResponse employeeResponse = new EmployeeResponse();
            Authorizer authorizer = new Authorizer();
            try
            {
                //Validating the student details  
                string studentDetails = queryBuilderRequest.Payload.Fields.Where(x => x.Name.ToUpper() == Constants.STUDENT_DETAILS).Select(x => x.Value).FirstOrDefault();

                if (authorizer.ValidateUser(queryBuilderRequest.UserId, queryBuilderRequest.CompanyId, queryBuilderRequest.Payload.AppType, studentDetails))
                {
                    return employeeRepository.GetEmployeeDetails(queryBuilderRequest);
                }
                else
                {
                    employeeResponse.Error = ResponseBuilder.Forbidden();
                    return employeeResponse;
                }
            }
            catch (Exception getEmployeesException)
            {
                LambdaLogger.Log(getEmployeesException.ToString());
                employeeResponse.Error = ResponseBuilder.InternalError();
                return employeeResponse;
            }
        }
        /// <summary>
        ///  [QueryBuilder] Function to get list of workbook(s) filtered with some condition(s)
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <param name="context"></param>
        /// <returns>WorkbookResponse</returns>
        public WorkbookResponse GetWorkbookQueryBuilder(QueryBuilderRequest queryBuilderRequest, ILambdaContext context)
        {
            WorkbookRepository workbookRepository = new WorkbookRepository();
            WorkbookResponse workbookResponse = new WorkbookResponse();
            Authorizer authorizer = new Authorizer();
            try
            {
                //Validating the student details  
                string studentDetails = queryBuilderRequest.Payload.Fields.Where(x => x.Name.ToUpper() == Constants.STUDENT_DETAILS).Select(x => x.Value).FirstOrDefault();

                if (authorizer.ValidateUser(queryBuilderRequest.UserId, queryBuilderRequest.CompanyId, queryBuilderRequest.Payload.AppType, studentDetails))
                {
                    return workbookRepository.GetWorkbookDetails(queryBuilderRequest);
                }
                else
                {
                    workbookResponse.Error = ResponseBuilder.Forbidden();
                    return workbookResponse;
                }
            }
            catch (Exception getWorkbookQueryBuilderException)
            {
                LambdaLogger.Log(getWorkbookQueryBuilderException.ToString());
                workbookResponse.Error = ResponseBuilder.InternalError();
                return workbookResponse;
            }
        }

        /// <summary>
        ///   [QueryBuilder] Function to get list of task(s) filtered with some condition(s)
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <param name="context"></param>
        /// <returns>TaskResponse</returns>
        public TaskResponse GetTaskQueryBuilder(QueryBuilderRequest queryBuilderRequest, ILambdaContext context)
        {
            TaskRepository taskRepository = new TaskRepository();
            TaskResponse taskResponse = new TaskResponse();
            Authorizer authorizer = new Authorizer();
            try
            {
                //Validating the student details  
                string studentDetails = queryBuilderRequest.Payload.Fields.Where(x => x.Name.ToUpper() == Constants.STUDENT_DETAILS).Select(x => x.Value).FirstOrDefault();

                if (authorizer.ValidateUser(queryBuilderRequest.UserId, queryBuilderRequest.CompanyId, queryBuilderRequest.Payload.AppType, studentDetails))
                {
                    return taskRepository.GetQueryTaskDetails(queryBuilderRequest);
                }
                else
                {
                    taskResponse.Error = ResponseBuilder.Forbidden();
                    return taskResponse;
                }
            }
            catch (Exception getTaskQueryBuilderException)
            {
                LambdaLogger.Log(getTaskQueryBuilderException.ToString());
                taskResponse.Error = ResponseBuilder.InternalError();
                return taskResponse;
            }
        }


        /// <summary>
        ///  save's the userquery using userId and companyId 
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <param name="context"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse SaveQuery(QueryBuilderRequest queryBuilderRequest, ILambdaContext context = null)
        {
            QueryRepository queryRepository = new QueryRepository();
            QueryResponse queryResponse = new QueryResponse();
            Authorizer authorizer = new Authorizer();
            try
            {
                if (authorizer.ValidateUser(queryBuilderRequest.UserId, queryBuilderRequest.CompanyId))
                {
                    return queryRepository.SaveQuery(queryBuilderRequest);
                }
                else
                {
                    queryResponse.Error = ResponseBuilder.Forbidden();
                    return queryResponse;
                }
            }
            catch (Exception saveQueryException)
            {
                LambdaLogger.Log(saveQueryException.ToString());
                queryResponse.Error = ResponseBuilder.InternalError();
                return queryResponse;
            }
        }


        /// <summary>
        ///  Get list of queries based on the userId
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <param name="context"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse GetQueries(QueryBuilderRequest queryBuilderRequest, ILambdaContext context = null)
        {
            QueryRepository queryRepository = new QueryRepository();
            QueryResponse queryResponse = new QueryResponse();
            Authorizer authorizer = new Authorizer();
            try
            {
                if (authorizer.ValidateUser(queryBuilderRequest.UserId, queryBuilderRequest.CompanyId))
                {
                    return queryRepository.GetUserQueries(queryBuilderRequest);
                }
                else
                {
                    queryResponse.Error = ResponseBuilder.Forbidden();
                    return queryResponse;
                }
            }
            catch (Exception getQueriesException)
            {
                LambdaLogger.Log(getQueriesException.ToString());
                queryResponse.Error = ResponseBuilder.InternalError();
                return queryResponse;
            }
        }

        /// <summary>
        ///   Rename the existing query based on the query Id and UserId
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <param name="context"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse RenameQuery(QueryBuilderRequest queryBuilderRequest, ILambdaContext context = null)
        {
            QueryRepository queryRepository = new QueryRepository();
            QueryResponse queryResponse = new QueryResponse();
            Authorizer authorizer = new Authorizer();
            try
            {
                if (authorizer.ValidateUser(queryBuilderRequest.UserId, queryBuilderRequest.CompanyId))
                {
                    return queryRepository.RenameQuery(queryBuilderRequest);
                }
                else
                {
                    queryResponse.Error = ResponseBuilder.Forbidden();
                    return queryResponse;
                }
            }
            catch (Exception renameQueryException)
            {
                LambdaLogger.Log(renameQueryException.ToString());
                queryResponse.Error = ResponseBuilder.InternalError();
                return queryResponse;
            }
        }

        /// <summary>
        ///  Delete the existing query based on the query Id and UserId
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <param name="context"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse DeleteQuery(QueryBuilderRequest queryBuilderRequest, ILambdaContext context = null)
        {
            QueryRepository queryRepository = new QueryRepository();
            QueryResponse queryResponse = new QueryResponse();
            Authorizer authorizer = new Authorizer();
            try
            {
                if (authorizer.ValidateUser(queryBuilderRequest.UserId, queryBuilderRequest.CompanyId))
                {
                    return queryRepository.DeleteQuery(queryBuilderRequest);
                }
                else
                {
                    queryResponse.Error = ResponseBuilder.Forbidden();
                    return queryResponse;
                }
            }
            catch (Exception deleteQueryException)
            {
                LambdaLogger.Log(deleteQueryException.ToString());
                queryResponse.Error = ResponseBuilder.InternalError();
                return queryResponse;
            }
        }


        /// <summary>
        /// Get specific query details and results based on the queryId
        /// </summary>
        /// <param name="queryBuilderRequest"></param>
        /// <param name="context"></param>
        /// <returns>QueryResponse</returns>
        public QueryResponse GetQuery(QueryBuilderRequest queryBuilderRequest, ILambdaContext context = null)
        {
            QueryRepository queryRepository = new QueryRepository();
            QueryResponse queryResponse = new QueryResponse();
            Authorizer authorizer = new Authorizer();
            try
            {
                if (authorizer.ValidateUser(queryBuilderRequest.UserId, queryBuilderRequest.CompanyId))
                {
                    return queryRepository.GetUserQuery(queryBuilderRequest);
                }
                else
                {
                    queryResponse.Error = ResponseBuilder.Forbidden();
                    return queryResponse;
                }
            }
            catch (Exception getQueryException)
            {
                LambdaLogger.Log(getQueryException.ToString());
                queryResponse.Error = ResponseBuilder.InternalError();
                return queryResponse;
            }
        }

        /// <summary>
        /// Get the list of roles based on the company
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <param name="context"></param>
        /// <returns>RoleResponse</returns>
        public RoleResponse GetRoles(RoleRequest roleRequest, ILambdaContext context = null)
        {
            RoleRepository roleRepository = new RoleRepository();
            RoleResponse roleResponse = new RoleResponse();
            Authorizer authorizer = new Authorizer();
            try
            {
                if (authorizer.ValidateUser(roleRequest.UserId, roleRequest.CompanyId))
                {
                    return roleRepository.GetRoles(roleRequest);
                }
                else
                {
                    roleResponse.Error = ResponseBuilder.Forbidden();
                    return roleResponse;
                }
            }
            catch (Exception getRolesException)
            {
                LambdaLogger.Log(getRolesException.ToString());
                roleResponse.Error = ResponseBuilder.InternalError();
                return roleResponse;
            }
        }

        /// <summary>
        ///  Get the list of companies based on the userId
        /// </summary>
        /// <param name="companyRequest"></param>
        /// <param name="context"></param>
        /// <returns>CompanyResponse</returns>
        public CompanyResponse GetCompany(CompanyRequest companyRequest, ILambdaContext context = null)
        {
            CompanyRepository companyRepository = new CompanyRepository();
            CompanyResponse companyResponse = new CompanyResponse();
            try
            {
                return companyRepository.GetCompany(companyRequest);
            }
            catch (Exception getCompanyException)
            {
                LambdaLogger.Log(getCompanyException.ToString());
                companyResponse.Error = ResponseBuilder.InternalError();
                return companyResponse;
            }
        }
    }
}

