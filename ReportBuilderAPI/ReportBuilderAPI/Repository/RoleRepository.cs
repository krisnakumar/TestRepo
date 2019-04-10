using Amazon.Lambda.Core;
using OnBoardLMS.WebAPI.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.IRepository;
using System;
using System.Linq;


namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Class that manages the role
    /// </summary>
    public class RoleRepository : IRole
    {

        /// <summary>
        /// Get shared roles based on the companyId
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        public RoleResponse GetRoles(RoleRequest roleRequest)
        {
            RoleResponse roleResponse = new RoleResponse();

            try
            {
                using (DBEntity context = new DBEntity())
                {
                    roleResponse.Roles = (from r in context.Role
                                          join rdr in context.Reporting_DashboardRole on r.Id equals rdr.RoleId
                                          join rd in context.Reporting_Dashboard on rdr.DashboardID equals rd.Id
                                          select new RoleModel
                                          {
                                              RoleId = Convert.ToInt32(r.Id),
                                              Role = Convert.ToString(r.Name)
                                          }).ToList();
                    return roleResponse;
                }
            }
            catch (Exception getRolesException)
            {
                LambdaLogger.Log(getRolesException.ToString());
                roleResponse.Error = ResponseBuilder.InternalError();
                return roleResponse;
            }
        }
    }
}
