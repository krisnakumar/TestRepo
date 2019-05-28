using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Resource;
using System;


namespace ReportBuilderAPI.Helpers
{
    /// <summary>
    ///     Class that handles the token for the cognito
    /// </summary>
    public class SessionGenerator
    {
       

        /// <summary>
        ///     Get refreshed token from Cognito using for the current user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns>AuthFlowResponse</returns>
        public UserResponse ProcessRefreshToken(UserRequest userRequest)
        {
            AuthFlowResponse authResponse;
            UserResponse userResponse = new UserResponse();
            
            try
            {
               
                AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), RegionEndpoint.USWest2);
                CognitoUserPool userPool = new CognitoUserPool("XXX_XXX", userRequest.CognitoClientId, provider);
                CognitoUser user = new CognitoUser(userRequest.UserName, userRequest.CognitoClientId, userPool, provider, userRequest.ClientSecret) { SessionTokens = new CognitoUserSession(null, null, userRequest.Payload.RefreshToken, DateTime.Now, DateTime.Now.AddDays(10)) };
                    
                InitiateRefreshTokenAuthRequest authRequest = new InitiateRefreshTokenAuthRequest()
                {
                    AuthFlowType = AuthFlowType.REFRESH_TOKEN
                };
                authResponse = user.StartWithRefreshTokenAuthAsync(authRequest).Result;                
                
                //Check if user having any challenges 
                if (authResponse != null && authResponse.AuthenticationResult == null)
                {
                    string message = CheckChallenge(authResponse.ChallengeName);
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
            catch (Exception processRefreshTokenException)
            {
                LambdaLogger.Log(processRefreshTokenException.ToString());
                return null;
            }
        }


        /// <summary>
        ///     Check AuthenticationResult using ChallengeNameType 
        /// </summary>
        /// <param name="challengeNameType"></param>
        /// <returns>message</returns>
        public string CheckChallenge(ChallengeNameType challengeNameType)
        {
            string message = string.Empty;
            try
            {
                if (challengeNameType == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                {
                    message = DataResource.ACCOUNT_NOT_HAVING_PERMISSION;
                }
                else
                {
                    message = DataResource.INVALID_CREDENTILAS;
                }

                return message;
            }
            catch (Exception checkChallengeException)
            {
                LambdaLogger.Log(checkChallengeException.ToString());
                return message;
            }
        }
    }
}
