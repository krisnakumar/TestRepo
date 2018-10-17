using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.RequestHandler;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Helpers;
using System;
using System.Net;

namespace ReportBuilderAPI.Repository
{
    public class AuthenticationRepository
    {
        public APIGatewayProxyResponse Login(APIGatewayProxyRequest request)
        {
            SessionGenerator sessionGenerator = new SessionGenerator();
            UserResponse userResponse;
            try
            {
                var authResponse = sessionGenerator.GenerateAccessToken(RequestReader.GetRequestBody(request));
                if (authResponse != null && authResponse.AuthenticationResult == null)
                {
                    var message = sessionGenerator.CheckChallenge(authResponse.ChallengeName);
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
