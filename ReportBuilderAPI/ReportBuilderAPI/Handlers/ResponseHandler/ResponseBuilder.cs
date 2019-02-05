using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Resource;
using System;
using System.Collections.Generic;
using System.Net;


/*
 <copyright file="ResponseBuilder.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author> Shoba Eswar </author>
 <date>10-10-2018</date>
 <summary>
    Response Builder that builds the responses
 </summary>
*/
namespace ReportBuilderAPI.Handlers.ResponseHandler
{
    /// <summary>
    ///     Class responsible for creating common responses for the API gateway
    /// </summary>
    public class ResponseBuilder
    {
        /// <summary>
        ///     Creates response for internal error
        /// </summary>
        /// <returns>APIGatewayProxyResponse</returns>
        public static APIGatewayProxyResponse InternalError()
        {
            try
            {
                ErrorResponse employeeResponse = new ErrorResponse
                {
                    Code = 33,
                    Message = DataResource.SYSTEM_ERROR
                };
                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Body = JsonConvert.SerializeObject(employeeResponse),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "https://s3-us-west-2.amazonaws.com"  } }
                };
                return response;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return null;
            }
        }

        /// <summary>
        ///     Creates response for bad / invalid requests
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public static APIGatewayProxyResponse BadRequest(string fieldName)
        {
            try
            {
                ErrorResponse errorResponse = new ErrorResponse
                {
                    Code = 1,
                    Message = DataResource.INVALID_INPUT +": "+ fieldName
                };
                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Body = JsonConvert.SerializeObject(errorResponse),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "https://s3-us-west-2.amazonaws.com" } }
                };
                return response;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return null;
            }
        }


        /// <summary>
        ///     Creates response for forbidden resources
        /// </summary>
        /// <returns>APIGatewayProxyResponse</returns>
        public static APIGatewayProxyResponse Forbidden()
        {
            try
            {
                ErrorResponse errorResponse = new ErrorResponse
                {
                    Code = 14,
                    Message = DataResource.PERMISSION_DENIED
                };
                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Body = JsonConvert.SerializeObject(errorResponse),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                };
                return response;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return null;
            }
        }


        /// <summary>
        ///     Creates response for unauthorized request
        /// </summary>
        /// <param name="message"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public static APIGatewayProxyResponse UnAuthorized(string message)
        {
            try
            {
                ErrorResponse employeeResponse = new ErrorResponse
                {
                    Code = 13,
                    Message = message
                };
                var response = new APIGatewayProxyResponse
                {
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Body = JsonConvert.SerializeObject(employeeResponse),
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                };
                return response;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return null;
            }
        }

        /// <summary>
        ///     Creates success response
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="responseBody"></param>
        /// <param name="code"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public static APIGatewayProxyResponse GatewayProxyResponse(int statusCode, string responseBody, int code)
        {
            try
            {
                var response = new APIGatewayProxyResponse
                {
                    StatusCode = statusCode,
                    Body = responseBody,
                    Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
                };
                return response;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return null;
            }
        }
    }
}
