using Amazon.Lambda.Core;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;

namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    /// Interface that helps to handle the Authentication
    /// </summary>
    public interface IAuthentication
    {
        UserResponse Login(UserRequest userRequest, ILambdaContext context);
        UserResponse SilentAuth(UserRequest userRequest);
    }
}
