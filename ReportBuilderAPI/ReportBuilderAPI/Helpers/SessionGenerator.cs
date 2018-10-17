using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Lambda.Core;
using ReportBuilder.Models.Request;
using ReportBuilderAPI.Resource;
using ReportBuilderAPI.Utilities;
using System;

namespace ReportBuilderAPI.Helpers
{
    public class SessionGenerator
    {
        /// <summary>
        /// Get token from Cognito using username  and password
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns>AuthFlowResponse</returns>
        public AuthFlowResponse GenerateAccessToken(UserRequest userRequest)
        {
            AuthFlowResponse authResponse;
            try
            {
                AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(DataResource.ACCESS_KEY, DataResource.SECRET_KEY, RegionEndpoint.USWest2);
                CognitoUserPool userPool = new CognitoUserPool(Constants.COGNITO_USERPOOLID, Constants.COGNITO_USERPOOL_CLIENTID, provider);
                CognitoUser user = new CognitoUser(userRequest.UserName, Constants.COGNITO_USERPOOL_CLIENTID, userPool, provider);
                InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
                {
                    Password = userRequest.Password
                };
                authResponse = user.StartWithSrpAuthAsync(authRequest).Result;
                return authResponse;

            }
            catch (Exception sessionGeneratorException)
            {
                LambdaLogger.Log(sessionGeneratorException.ToString());
                return null;
            }
        }


        /// <summary>
        ///  Check AuthenticationResult using ChallengeName 
        /// </summary>
        /// <param name="challengeNameType"></param>
        /// <returns>message</returns>
        public string CheckChallenge(ChallengeNameType challengeNameType)
        {
            string message = string.Empty;
            try
            {
                if (challengeNameType == ChallengeNameType.NEW_PASSWORD_REQUIRED)
                    message = DataResource.ACCOUNT_NOT_HAVING_PERMISSION;
                else
                    message = DataResource.INVALID_CREDENTILAS;
                return message;
            }
            catch (Exception checkChallengeException)
            {
                LambdaLogger.Log(checkChallengeException.ToString());
                return string.Empty;
            }
        }
    }
}
