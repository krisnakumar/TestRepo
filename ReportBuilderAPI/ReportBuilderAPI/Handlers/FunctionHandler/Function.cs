using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using ReportBuilderAPI.Handlers.RequestHandler;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Repository;
using System;
using System.Net;



// <copyright file="EmployeeQueries.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
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
                return null;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return null;
            }
        }
        /// <summary>
        /// Get list of employees using userId
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public APIGatewayProxyResponse GetEmployees(APIGatewayProxyRequest request, ILambdaContext context)
        {
            EmployeeRepository employeeRepository = new EmployeeRepository();
            try
            {
                return employeeRepository.GetEmployeeList(RequestReader.GetPathValue(request));                
            }
            catch (Exception getEmployees)
            {
                LambdaLogger.Log(getEmployees.ToString());
                return ResponseBuilder.InternalError();
            }
        }
    }
}
