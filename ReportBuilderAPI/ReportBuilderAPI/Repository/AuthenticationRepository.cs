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
using ReportBuilderAPI.IRepository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    public class AuthenticationRepository : IAuthentication
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
                //Read username and password from gatewayrequest
                UserRequest userRequest = RequestReader.GetRequestBody(request);
                //Generate Access token for valid user 
                Amazon.Extensions.CognitoAuthentication.AuthFlowResponse authResponse = sessionGenerator.GenerateAccessToken(userRequest);
                //Generate response depends upon the authResponse
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
                        UserName = GetUserName(userRequest.UserName)
                    };
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(userResponse), 0);
                }
                else
                {
                    return ResponseBuilder.BadRequest("Username and Password");
                }
            }
            catch (Exception loginException)
            {
                LambdaLogger.Log(loginException.ToString());
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
                //Get userId based on the username
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
        /// Get user name of the  logged in user.
        /// </summary>
        /// <param name="userName"></param>
        private string GetUserName(string email)
        {
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            string userName = string.Empty;
            try
            {
                //Read employee name using user email
                SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader("SELECT (ISNULL(NULLIF(FName, '') + ' ', '') + Lname)  as employeeName FROM [User] WHERE Email='" + email + "'", new Dictionary<string, string>());
                if (sqlDataReader != null && sqlDataReader.HasRows && sqlDataReader.Read())
                {
                    userName = Convert.ToString(sqlDataReader["employeeName"]);
                }
                return userName;
            }
            catch (Exception getUserNameException)
            {
                LambdaLogger.Log(getUserNameException.ToString());
                return userName;
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
                //Read companyId from the db
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
                //Generate Id token from the refresh token
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
