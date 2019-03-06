using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using ReportBuilder.Models.Request;
using ReportBuilderAPI.Resource;
using ReportBuilderAPI.Utilities;
using System;


/*
 <copyright file = "SessionGenerator.cs" >
 Copyright(c) 2018 All Rights Reserved
 </copyright>
 <author> Shoba Eswar </author>
 <date>01-11-2018</date>
 <summary>
    Repository that helps to generate token for the cognito
 </summary>
*/
namespace ReportBuilderAPI.Helpers
{
    /// <summary>
    ///     Class that handles the token for the cognito
    /// </summary>
    public class SessionGenerator
    {
        /// <summary>
        ///     Get token from Cognito using username  and password
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
        ///     Get refreshed token from Cognito using for the current user
        /// </summary>
        /// <param name="userRequest"></param>
        /// <returns>AuthFlowResponse</returns>
        public AuthFlowResponse ProcessRefreshToken(UserRequest userRequest)
        {
            AuthFlowResponse authResponse;
            try
            {
                AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(new AnonymousAWSCredentials(), RegionEndpoint.USWest2);
                CognitoUserPool userPool = new CognitoUserPool("XXX_XXX", Constants.COGNITO_USERPOOL_CLIENTID, provider);
                CognitoUser user = new CognitoUser(userRequest.UserName, Constants.COGNITO_USERPOOL_CLIENTID, userPool, provider) { SessionTokens = new CognitoUserSession(null, null, userRequest.RefreshToken, DateTime.Now, DateTime.Now.AddDays(10)) };


                InitiateRefreshTokenAuthRequest authRequest = new InitiateRefreshTokenAuthRequest()
                {
                    AuthFlowType = AuthFlowType.REFRESH_TOKEN
                };
                authResponse = user.StartWithRefreshTokenAuthAsync(authRequest).Result;
                return authResponse;
            }
            catch (Exception sessionGeneratorException)
            {
                LambdaLogger.Log(sessionGeneratorException.ToString());
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
                return string.Empty;
            }
        }
    }
}
