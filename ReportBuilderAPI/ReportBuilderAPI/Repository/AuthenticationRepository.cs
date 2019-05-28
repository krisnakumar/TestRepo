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
                userResponse = sessionGenerator.ProcessRefreshToken(userRequest);
                return userResponse;                
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
