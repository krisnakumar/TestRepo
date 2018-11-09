﻿using Amazon.Lambda.APIGatewayEvents;
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
        /// <returns>userId</returns>
        public static int GetUserId(APIGatewayProxyRequest request)
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
        /// <returns>workbookId</returns>
        public static int GetWorkbookId(APIGatewayProxyRequest request)
        {
            int workbookId = 0;
            try
            {
                if (request.PathParameters != null && request.PathParameters.ContainsKey("workbookId"))
                    workbookId = Convert.ToInt32(request.PathParameters["userId"]);
                return workbookId;
            }
            catch (Exception getPathValueException)
            {
                LambdaLogger.Log(getPathValueException.ToString());
                return workbookId;
            }
        }


        /// <summary>
        /// Read the path parameters from the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>taskId</returns>
        public static int GetTaskId(APIGatewayProxyRequest request)
        {
            int taskId = 0;
            try
            {
                if (request.PathParameters != null && request.PathParameters.ContainsKey("taskId"))
                    taskId = Convert.ToInt32(request.PathParameters["taskId"]);
                return taskId;
            }
            catch (Exception getPathValueException)
            {
                LambdaLogger.Log(getPathValueException.ToString());
                return taskId;
            }
        }


        /// <summary>
        /// Read the path parameters from the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>QueryStringModel</returns>
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
                            case Constants.PARAM:
                                queryStringModel.Param = Convert.ToString(parameters.Value);                                
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

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="param"></param>
        //public static void GetRequiredValue(string param)
        //{
        //    List<string> queries = new List<string>();
        //    string regex = string.Empty;
        //    string sqlQuery = string.Empty;
        //    QueryStringModel queryStringModel = new QueryStringModel();
        //    try
        //    {
        //        regex = @"\((\w+)(\W+)(\w+)\) (and|or)";
        //        var matchedString= Regex.Matches(param, regex);
        //        regex = @"\((\w+)(\W+)(\w+)\)$";
        //        var matchedFinalString = Regex.Matches(param, regex);

        //        foreach (dynamic query in matchedString)
        //        {
        //            var key= query.Groups[1].ToString();
        //            var value = query.Groups[3].ToString();
        //            var oper = query.Groups[4].ToString();

        //            switch (query.Groups[1].ToString())
        //            {
        //                case Constants.CompletedWorkBooks:
        //                    queryStringModel.CompletedWorkBook = Convert.ToString(value);
        //                    break;
        //                case Constants.WorkBookInDue:
        //                    queryStringModel.WorkBookInDue = Convert.ToString(value);
        //                        break;
        //                case Constants.PastDueWorkBook:
        //                    queryStringModel.PastDueWorkBook = Convert.ToInt32((!string.IsNullOrEmpty(Convert.ToString(parameters.Value))) ? Convert.ToString(parameters.Value) : "0");
        //                    break;
        //                case Constants.PARAM:
        //                    queryStringModel.Param = Convert.ToString(parameters.Value);
        //                    GetRequiredValue(Convert.ToString(parameters.Value));
        //                    break;
        //            }
        //            sqlQuery = 
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        LambdaLogger.Log(exception.ToString());
        //    }
        //}

        /// <summary>
        /// Read the request body from the request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>UserRequest</returns>
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
