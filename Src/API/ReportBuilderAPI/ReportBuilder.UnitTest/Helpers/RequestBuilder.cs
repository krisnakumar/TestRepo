using System;
using System.Collections.Generic;
using System.Text;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;

namespace ReportBuilder.UnitTest.Helpers
{
    public class RequestBuilder
    {

        public static APIGatewayProxyRequest PathParamsRequest(string key, string value)
        {
            Dictionary<string, string> pathValues = new Dictionary<string, string>
            {
                { key, value }

            };

            APIGatewayProxyRequest aPIGatewayProxyRequest = new APIGatewayProxyRequest
            {
                PathParameters = pathValues
            };

            return aPIGatewayProxyRequest;
        }


    }
}
