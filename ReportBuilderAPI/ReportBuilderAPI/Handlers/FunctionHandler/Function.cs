using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ReportBuilderAPI.Handlers.RequestHandler;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Repository;
using System;



// <copyright file="EmployeeQueries.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Shoba Eswar</author>
// <date>10-10-2018</date>
// <summary>Handles Lambda operations</summary>
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace ReportBuilderAPI.Handlers.FunctionHandler
{
    /// <summary>
    /// Handles list of lambda fucntions
    /// </summary>
    public class Function
    {

        /// <summary>
        /// Login Function to generate token
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
        /// Refresh Token once the token is expired
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
        /// Get list of employees using userId
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
        /// Get list of employees using userId
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
        /// Get list of past due workbooks
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
        /// Get list of past due workbooks
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
        /// Get list of past due workbooks
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
        /// Get list of past due workbooks
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
        /// Get list of past due workbooks
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
        /// Get list of employees using userId
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

    }
}
