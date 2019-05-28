using Amazon.Extensions.CognitoAuthentication;
using Amazon.Lambda.Core;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Helpers;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Logger;
using ReportBuilderAPI.Resource;
using System;


namespace ReportBuilderAPI.Repository
{
    /// <summary>
    ///     Class that authenticates the user(s), login the user(s) into the app and handle the session(s)
    /// </summary>
    public class AuthenticationRepository : IAuthentication
    {              
        /// <summary>
        ///  API to handle the silent Auth using refresh token
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns>UserResponse</returns>
        public UserResponse SilentAuth(UserRequest userRequest)
        {
            SessionGenerator sessionGenerator = new SessionGenerator();
            UserResponse userResponse = new UserResponse();
            try
            {
                if (string.IsNullOrEmpty(userRequest.CognitoClientId)) throw new ArgumentException("CognitoClientId");
                
                if (string.IsNullOrEmpty(userRequest.Payload.RefreshToken)) throw new ArgumentException("RefreshToken");

                if (string.IsNullOrEmpty(userRequest.ClientSecret)) throw new ArgumentException("ClientSecret");

                if (string.IsNullOrEmpty(userRequest.UserName)) throw new ArgumentException("UserName");
                

                //Generate Id token from the refresh token
                AuthFlowResponse authResponse = sessionGenerator.ProcessRefreshToken(userRequest);
                
                //Check if user having any challenges 
                if (authResponse != null && authResponse.AuthenticationResult == null)
                {
                    string message = sessionGenerator.CheckChallenge(authResponse.ChallengeName);
                    userResponse.Error = ResponseBuilder.UnAuthorized(message);
                    return userResponse;
                }
                //Create user response for valid token
                else if (authResponse != null && authResponse.AuthenticationResult != null)
                {
                    userResponse = new UserResponse
                    {
                        AccessToken = authResponse.AuthenticationResult.AccessToken,
                        IdentityToken = authResponse.AuthenticationResult.IdToken
                    };
                    return userResponse;
                }
                //Send bad request if user name is invalid
                else
                {
                    userResponse.Error = ResponseBuilder.BadRequest(DataResource.USERNAME);
                    return userResponse;
                }
            }
            catch (Exception silentAuthException)
            {
                LambdaLogger.Log(silentAuthException.ToString());
                userResponse.Error = new ExceptionHandler(silentAuthException).ExceptionResponse();
                return userResponse;
            }
        }        
    }
}
