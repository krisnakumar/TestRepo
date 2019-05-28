using Amazon.Lambda.Core;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Resource;
using System;


namespace ReportBuilderAPI.Handlers.ResponseHandler
{
    /// <summary>
    ///     Class responsible for creating Error responses for the API gateway
    /// </summary>
    public class ResponseBuilder
    {
        /// <summary>
        ///     Creates response for internal error
        /// </summary>
        /// <returns>ErrorResponse</returns>
        public static ErrorResponse InternalError()
        {
            try
            {
                ErrorResponse errorResponse = new ErrorResponse
                {
                    Status=500,
                    Code = 33,
                    Message = DataResource.SYSTEM_ERROR
                };              
                return errorResponse;
            }
            catch (Exception systemErrorException)
            {
                LambdaLogger.Log(systemErrorException.ToString());
                return null;
            }
        }

        /// <summary>
        ///     Creates response for bad / invalid requests
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns>ErrorResponse</returns>
        public static ErrorResponse BadRequest(string fieldName)
        {
            try
            {
                ErrorResponse errorResponse = new ErrorResponse
                {
                    Status=400,
                    Code = 1,
                    Message = DataResource.INVALID_INPUT +": "+ fieldName
                };               
                return errorResponse;
            }
            catch (Exception badRequestException)
            {
                LambdaLogger.Log(badRequestException.ToString());
                return null;
            }
        }


        /// <summary>
        ///     Creates response for forbidden resources
        /// </summary>
        /// <returns>ErrorResponse</returns>
        public static ErrorResponse Forbidden()
        {
            try
            {
                ErrorResponse errorResponse = new ErrorResponse
                {
                    Status=403,
                    Code = 14,
                    Message = DataResource.PERMISSION_DENIED
                };              
                return errorResponse;
            }
            catch (Exception forbiddenException)
            {
                LambdaLogger.Log(forbiddenException.ToString());
                return null;
            }
        }


        /// <summary>
        ///     Creates response for unauthorized request
        /// </summary>
        /// <param name="message"></param>
        /// <returns>ErrorResponse</returns>
        public static ErrorResponse UnAuthorized(string message)
        {
            try
            {
                ErrorResponse errorResponse = new ErrorResponse
                {
                    Status=401,
                    Code = 13,
                    Message = message
                };              
                return errorResponse;
            }
            catch (Exception unAuthException)
            {
                LambdaLogger.Log(unAuthException.ToString());
                return null;
            }
        }

      

        /// <summary>
        ///     Creates success response
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="responseBody"></param>
        /// <param name="code"></param>
        /// <returns>APIGatewayProxyResponse</returns>
        public static SuccessResponse SuccessMessage(string Messgae)
        {
            try
            {
                SuccessResponse successResponse = new SuccessResponse
                {            
                    
                    Message = Messgae
                };
                return successResponse;
            }
            catch (Exception exception)
            {
                LambdaLogger.Log(exception.ToString());
                return null;
            }
        }

    }
}
