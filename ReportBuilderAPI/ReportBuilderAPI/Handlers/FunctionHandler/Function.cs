using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ReportBuilderAPI.Handlers.RequestHandler;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Repository;
using System;


/*
 <copyright file="Function.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>10-10-2018</date>
 <summary>
    - Repository to handle Lambda operations.
    - Entry point for APIGateway requests
 </summary>
*/

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace ReportBuilderAPI.Handlers.FunctionHandler
{
    /// <summary>
    ///     This class contains list of lambda fucntions
    /// </summary>
    public class Function
    {

        /// <summary>
        ///     Function that takes user credentials as request and responsible for login the user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse Login(APIGatewayProxyRequest request, ILambdaContext context)
        {
            AuthenticationRepository authenticationRepository = new AuthenticationRepository();
            try
            {
                return authenticationRepository.Login(request);
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return ResponseBuilder.InternalError();
            }
        }
        
        /// <summary>
        ///     This function generaes new token once previous token is expired
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse SilentAuth(APIGatewayProxyRequest request, ILambdaContext context)
        {
            AuthenticationRepository authenticationRepository = new AuthenticationRepository();
            try
            {
                return authenticationRepository.SilentAuth(request);
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     [ReportBuilder] Function to get list of employee(s) under a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetEmployees(APIGatewayProxyRequest request, ILambdaContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository();
            try
            {
                return employeeRepository. GetEmployeeList(RequestReader.GetUserId(request), RequestReader.ReadQueryString(request));
            }
            catch (Exception getEmployees)
            {
                LambdaLogger.Log(getEmployees.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     [ReportBuilder] Function to get list of assigned workbook(s) for a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetWorkbookDetails(APIGatewayProxyRequest request, ILambdaContext context)
        {
            WorkbookRepository workbookRepository = new WorkbookRepository();
            try
            {
                return workbookRepository.GetWorkbookDetails(RequestReader.GetUserId(request));
            }
            catch (Exception getEmployees)
            {
                LambdaLogger.Log(getEmployees.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     [ReportBuilder] Function to get list of past due workbook(s) for a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetPastDueWorkbookDetails(APIGatewayProxyRequest request, ILambdaContext context)
        {
            WorkbookRepository workbookRepository = new WorkbookRepository();
            try
            {
                return workbookRepository.GetPastDueWorkbooks(RequestReader.GetUserId(request));
            }
            catch (Exception getEmployees)
            {
                LambdaLogger.Log(getEmployees.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     [ReportBuilder] Function to get list of coming due workbook(s) for a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetInDueWorkbookDetails(APIGatewayProxyRequest request, ILambdaContext context)
        {
            WorkbookRepository workbookRepository = new WorkbookRepository();
            try
            {
                return workbookRepository.GetInDueWorkbooks(RequestReader.GetUserId(request));
            }
            catch (Exception getEmployees)
            {
                LambdaLogger.Log(getEmployees.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     [ReportBuilder] Function to get list of completed workbook(s) for a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetCompletedWorkbookDetails(APIGatewayProxyRequest request, ILambdaContext context)
        {
            WorkbookRepository workbookRepository = new WorkbookRepository();
            try
            {
                return workbookRepository.GetCompletedWorkbooks(RequestReader.GetUserId(request));
            }
            catch (Exception getEmployees)
            {
                LambdaLogger.Log(getEmployees.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     [ReportBuilder] Function to get list of task(s) under a workbook for a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetTaskList(APIGatewayProxyRequest request, ILambdaContext context)
        {
            TaskRepository taskRepository = new TaskRepository();
            try
            {
                return taskRepository.GetTaskDetails(RequestReader.GetUserId(request), RequestReader.GetWorkbookId(request));
            }
            catch (Exception getEmployees)
            {
                LambdaLogger.Log(getEmployees.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     [ReportBuilder] Function to get attempt(s) made over a task under a workbook for a user
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetTaskAttemptsDetails(APIGatewayProxyRequest request, ILambdaContext context)
        {
            TaskRepository taskRepository = new TaskRepository();
            try
            {
                return taskRepository.GetTaskAttemptsDetails(RequestReader.GetUserId(request), RequestReader.GetWorkbookId(request), RequestReader.GetTaskId(request));
            }
            catch (Exception getEmployees)
            {
                LambdaLogger.Log(getEmployees.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     [QueryBuilder] Function to get list of employee(s) filtered with some condition(s)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetEmployeesQuerBuilder(APIGatewayProxyRequest request, ILambdaContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository();
            try
            {
                return employeeRepository.GetEmployeeDetails(request.Body, RequestReader.GetCompanyId(request));
            }
            catch (Exception getEmployees)
            {
                LambdaLogger.Log(getEmployees.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     [QueryBuilder] Function to get list of workbook(s) filtered with some condition(s)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetWorkbookQuerBuilder(APIGatewayProxyRequest request, ILambdaContext context)
        {
            WorkbookRepository workbookRepository = new WorkbookRepository();
            try
            {
                return workbookRepository.GetWorkbookDetails(request.Body, RequestReader.GetCompanyId(request));
            }
            catch (Exception getEmployees)
            {
                LambdaLogger.Log(getEmployees.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     [QueryBuilder] Function to get list of task(s) filtered with some condition(s)
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetTaskQuerBuilder(APIGatewayProxyRequest request, ILambdaContext context)
        {
            TaskRepository taskRepository = new TaskRepository();
            try
            {
                return taskRepository.GetQueryTaskDetails(request.Body, RequestReader.GetCompanyId(request));
            }
            catch (Exception getEmployees)
            {
                LambdaLogger.Log(getEmployees.ToString());
                return ResponseBuilder.InternalError();
            }
        }


        /// <summary>
        ///    Save Query 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse SaveQuery(APIGatewayProxyRequest request, ILambdaContext context)
        {
            QueryRepository queryRepository = new QueryRepository();
            try
            {
                return queryRepository.SaveQuery(request.Body, RequestReader.GetCompanyId(request));
            }
            catch (Exception saveQueryException)
            {
                LambdaLogger.Log(saveQueryException.ToString());
                return ResponseBuilder.InternalError();
            }
        }


        /// <summary>
        ///     Get list of queries based on the userId
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse GetQuery(APIGatewayProxyRequest request, ILambdaContext context)
        {
            QueryRepository queryRepository = new QueryRepository();
            try
            {
                return queryRepository.GetUserQueries(request.Body, RequestReader.GetCompanyId(request));
            }
            catch (Exception getQueryException)
            {
                LambdaLogger.Log(getQueryException.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     Rename the existing query based on the query Id and UserId
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse RenameQuery(APIGatewayProxyRequest request, ILambdaContext context)
        {
            QueryRepository queryRepository = new QueryRepository();
            try
            {
                return queryRepository.RenameQuery(request.Body, RequestReader.GetCompanyId(request));
            }
            catch (Exception renameQueryException)
            {
                LambdaLogger.Log(renameQueryException.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     Delete the existing query based on the query Id and UserId
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse DeleteQuery(APIGatewayProxyRequest request, ILambdaContext context)
        {
            QueryRepository queryRepository = new QueryRepository();
            try
            {
                return queryRepository.DeleteQuery(request.Body, RequestReader.GetCompanyId(request));
            }
            catch (Exception renameQueryException)
            {
                LambdaLogger.Log(renameQueryException.ToString());
                return ResponseBuilder.InternalError();
            }
        }
    }
}
