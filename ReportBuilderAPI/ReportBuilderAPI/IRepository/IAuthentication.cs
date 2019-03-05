using Amazon.Lambda.APIGatewayEvents;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    /// Interface that handles the Authentication
    /// </summary>
    public interface IAuthentication
    {
        APIGatewayProxyResponse Login(APIGatewayProxyRequest request);
        APIGatewayProxyResponse SilentAuth(APIGatewayProxyRequest request);
    }
}
