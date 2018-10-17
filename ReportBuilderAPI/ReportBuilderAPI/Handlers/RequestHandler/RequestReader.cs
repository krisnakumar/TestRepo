using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilderAPI.Utilities;
using System;


// <copyright file="RequestReader.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author></author>
// <date>10-10-2018</date>
// <summary>Read values from Gatewayrequest</summary>
namespace ReportBuilderAPI.Handlers.RequestHandler
{
    /// <summary>
    /// Read requested values from the gatewayrequest
    /// </summary>
    public class RequestReader
    {
        /// <summary>
        /// Read the path parameters from the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static int GetPathValue(APIGatewayProxyRequest request)
        {
            int userId = 0;
            try
            {
                if (request.PathParameters != null && request.PathParameters.ContainsKey("userId"))
                    userId = Convert.ToInt32(request.PathParameters["userId"]);
                return userId;
            }
            catch (Exception getPathValueException)
            {
                LambdaLogger.Log(getPathValueException.ToString());
                return userId;
            }
        }


        /// <summary>
        /// Read the path parameters from the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static QueryStringModel ReadQueryString(APIGatewayProxyRequest request)
        {
            QueryStringModel queryStringModel = new QueryStringModel();
            try
            {
                if (request.QueryStringParameters != null)
                {
                    foreach (var parameters in request.QueryStringParameters)
                    {
                        switch (parameters.Key.ToUpper())
                        {
                            case Constants.CompletedWorkBooks:
                                queryStringModel.CompletedWorkBooks = Convert.ToInt32((!string.IsNullOrEmpty(Convert.ToString(parameters.Value))) ? Convert.ToString(parameters.Value) : "0");
                                break;
                            case Constants.WorkBookInDue:
                                queryStringModel.WorkBookInDue = Convert.ToInt32((!string.IsNullOrEmpty(Convert.ToString(parameters.Value))) ? Convert.ToString(parameters.Value) : "0");
                                break;
                            case Constants.PastDueWorkBook:
                                queryStringModel.PastDueWorkBook = Convert.ToInt32((!string.IsNullOrEmpty(Convert.ToString(parameters.Value))) ? Convert.ToString(parameters.Value) : "0");
                                break;
                        }
                    }
                    return queryStringModel;
                }
                return null;
            }
            catch (Exception getPathValueException)
            {
                LambdaLogger.Log(getPathValueException.ToString());
                return null;
            }
        }

        /// <summary>
        /// Read the request body from the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static UserRequest GetRequestBody(APIGatewayProxyRequest request)
        {
            try
            {
                var userDetails = JsonConvert.DeserializeObject<UserRequest>(request?.Body);
                return userDetails;
            }
            catch (Exception getPathValueException)
            {
                LambdaLogger.Log(getPathValueException.ToString());
                return null;
            }
        }
    }
}
