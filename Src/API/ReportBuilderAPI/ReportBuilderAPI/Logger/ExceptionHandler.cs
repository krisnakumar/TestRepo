using Amazon.Lambda.Core;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Resource;
using ReportBuilderAPI.Utilities;
using System;
using System.Net;
using System.Resources;

namespace ReportBuilderAPI.Logger
{
    public class ExceptionHandler
    {
        private Exception _exception { get; set; }
        public ExceptionHandler(Exception exception)
        {
            _exception = exception;
        }

        public ErrorResponse ExceptionResponse()
        {
            ErrorResponse errorResponse = new ErrorResponse();
            try
            {
                ResourceManager _resourceManager = new ResourceManager(typeof(DataResource));
                if (_exception is ArgumentException)
                {
                    errorResponse = new ErrorResponse
                    {
                        Status = (int)HttpStatusCode.BadRequest,
                        Code = 33,
                        Message = DataResource.INVALID_INPUT + _exception.Message
                    };
                }
                else if (_exception is System.Data.SqlClient.SqlException)
                {
                    errorResponse = new ErrorResponse
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Code = 33,
                        Message = _resourceManager.GetString(Constants.SQLException)
                    };
                }
                else
                {
                    errorResponse = new ErrorResponse
                    {
                        Status = (int)HttpStatusCode.InternalServerError,
                        Code = 33,
                        Message = _resourceManager.GetString(Constants.DEFAULT_MESSAGE)
                    };
                }
                return errorResponse;
            }
            catch (Exception _ex)
            {
                LambdaLogger.Log(_ex.ToString());
                return errorResponse;
            }
        }
    }
}
