using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;

namespace ReportBuilderAPI.IRepository
{
    /// <summary>
    /// Interface that helps to get the list of roles
    /// </summary>
    public interface IRole
    {
        RoleResponse GetRoles(RoleRequest roleRequest);
    }
}
