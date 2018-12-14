using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.DatabaseManager;
using ReportBuilderAPI.Handlers.RequestHandler;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Helpers;
using System;
using System.Net;

/* 
 <copyright file="AuthenticationRepository.cs">
    Copyright (c) 2018 All Rights Reserved
 </copyright>
 <author>Shoba Eswar</author>
 <date>10-10-2018</date>
 <summary> 
    Repository that helps to authenticate the user(s), login the user(s) into the app 
    and handle the session(s).
 </summary>
*/
namespace ReportBuilderAPI.Repository
{
    /// <summary>
    ///     Class that authenticates the user(s), login the user(s) into the app and handle the session(s)
    /// </summary>
    public class AuthenticationRepository
    {
        /// <summary>
        ///     Login API to create session in cognito for valid user(s)
        /// </summary>
        /// <param name="request"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse Login(APIGatewayProxyRequest request)
        {
            SessionGenerator sessionGenerator = new SessionGenerator();
            UserResponse userResponse;
            try
            {
                UserRequest userRequest = RequestReader.GetRequestBody(request);
                Amazon.Extensions.CognitoAuthentication.AuthFlowResponse authResponse = sessionGenerator.GenerateAccessToken(userRequest);
                if (authResponse != null && authResponse.AuthenticationResult == null)
                {
                    string message = sessionGenerator.CheckChallenge(authResponse.ChallengeName);
                    return ResponseBuilder.UnAuthorized(message);
                }
                else if (authResponse != null && authResponse.AuthenticationResult != null)
                {
                    userResponse = new UserResponse
                    {
                        AccessToken = authResponse.AuthenticationResult.AccessToken,
                        IdentityToken = authResponse.AuthenticationResult.IdToken,
                        RefreshToken = authResponse.AuthenticationResult.RefreshToken,
                        UserId = Convert.ToInt32(GetUserId(userRequest.UserName)),
                        CompanyId = Convert.ToInt32(GetCompanyId(userRequest.UserName)),
                    };
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(userResponse), 0);
                }
                else
                {
                    return ResponseBuilder.BadRequest("Username and Password");
                }
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return ResponseBuilder.InternalError();
            }
        }

        /// <summary>
        ///     Get companyId from database
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>companyId</returns>
        private int GetUserId(string userName)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            int userId = 0;
            try
            {
                userId = databaseWrapper.ExecuteScalar(Employee.GetUserId(userName));
                return userId;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return userId;
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        ///     Get companyId from database
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>companyId</returns>
        private int GetCompanyId(string userName)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            int companyId = 0;
            try
            {
                companyId = databaseWrapper.ExecuteScalar(Employee.GetCompanyId(userName));
                return companyId;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return companyId;
            }
            finally
            {
                databaseWrapper.CloseConnection();
            }
        }


        /// <summary>
        ///     API to handle the silent Auth using refresh token
        /// </summary>
        /// <param name="request"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public APIGatewayProxyResponse SilentAuth(APIGatewayProxyRequest request)
        {

            SessionGenerator sessionGenerator = new SessionGenerator();
            UserResponse userResponse;
            try
            {
                Amazon.Extensions.CognitoAuthentication.AuthFlowResponse authResponse = sessionGenerator.ProcessRefreshToken(RequestReader.GetRequestBody(request));
                if (authResponse != null && authResponse.AuthenticationResult == null)
                {
                    string message = sessionGenerator.CheckChallenge(authResponse.ChallengeName);
                    return ResponseBuilder.UnAuthorized(message);
                }
                else if (authResponse != null && authResponse.AuthenticationResult != null)
                {
                    userResponse = new UserResponse
                    {
                        AccessToken = authResponse.AuthenticationResult.AccessToken,
                        IdentityToken = authResponse.AuthenticationResult.IdToken
                    };
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(userResponse), 0);
                }
                else
                {
                    return ResponseBuilder.BadRequest("Username and Password");
                }
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return ResponseBuilder.InternalError();
            }
        }

    }
}
