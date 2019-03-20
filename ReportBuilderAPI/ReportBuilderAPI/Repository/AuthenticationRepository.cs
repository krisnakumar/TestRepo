using Amazon.Extensions.CognitoAuthentication;
using Amazon.Lambda.Core;
using DataInterface.Database;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.DatabaseManager;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Helpers;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Resource;
using System;
using System.Data.SqlClient;


namespace ReportBuilderAPI.Repository
{
    /// <summary>
    ///     Class that authenticates the user(s), login the user(s) into the app and handle the session(s)
    /// </summary>
    public class AuthenticationRepository : IAuthentication
    {
        /// <summary>
        /// Login API to create session in cognito for valid user(s)
        /// </summary>
        /// <param name="userRequest"></param>
        /// <param name="context"></param>
        /// <returns>UserResponse</returns>
        public UserResponse Login(UserRequest userRequest, ILambdaContext context)
        {
            SessionGenerator sessionGenerator = new SessionGenerator();
            UserResponse userResponse = new UserResponse();
            try
            {
                //Generate Access token for valid user 
                AuthFlowResponse authResponse = sessionGenerator.GenerateAccessToken(userRequest);
                //Creates response for the depends upon the Challenge Name
                if (authResponse != null && authResponse.AuthenticationResult == null)
                {
                    string message = sessionGenerator.CheckChallenge(authResponse.ChallengeName);
                    userResponse.Error = ResponseBuilder.UnAuthorized(message);
                    return userResponse;
                }
                //Creates success response for the valid username and password
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
                    return userResponse;
                }
                //Create the Error Response for the invalid user
                else
                {
                    userResponse.Error = ResponseBuilder.BadRequest(DataResource.USERNAME_PASSWORD);
                    return userResponse;
                }
            }
            catch (Exception loginException)
            {
                LambdaLogger.Log(loginException.ToString());
                //Send error response if any invalid error occured
                userResponse.Error = ResponseBuilder.InternalError();
                return userResponse;
            }
        }


        #region Temp method to get the username, userId and companyId (It will be retrieved from LMS as JWT token in future)
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
                using (SqlDataReader sqlDataReader = databaseWrapper.ExecuteReader("SELECT Full_Name_Format1  as employeeName FROM dbo.[UserDetails] WHERE Email='" + email + "'", null))
                {
                    if (sqlDataReader != null && sqlDataReader.HasRows && sqlDataReader.Read())
                    {
                        userName = Convert.ToString(sqlDataReader["employeeName"]);
                    }
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

        #endregion

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
                userResponse.Error = ResponseBuilder.InternalError();
                return userResponse;
            }
        }
    }
}
