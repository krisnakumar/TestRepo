using Amazon.Lambda.Core;
using DataInterface.Database;
using OnBoardLMS.WebAPI.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Class that manages the role
    /// </summary>
    public class RoleRepository : DatabaseWrapper, IRole
    {

        /// <summary>
        /// Get shared roles based on the companyId
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns>Roles</returns>
        public RoleResponse GetRoles(RoleRequest roleRequest)
        {
            RoleResponse roleResponse = new RoleResponse();
            List<RoleModel> roles = new List<RoleModel>();
            string query = string.Empty;
            IDataReader sqlDataReader = null;
            try
            {
                if (string.IsNullOrEmpty(roleRequest.Payload.AppType))
                {
                    throw new ArgumentException(Constants.APP_TYPE);
                }

                if (roleRequest.Payload.AppType == Constants.WORKBOOK_DASHBOARD)
                {
                    query = "EXEC dbo.Roles_GetRoles @companyId=" + roleRequest.CompanyId;
                }
                else
                {
                    query = "SELECT r.Id as Role_Id,r.Name as Role_Name  FROM Role r JOIN Reporting_DashboardRole rdr ON rdr.RoleId=r.Id JOIN Reporting_Dashboard rd on  rdr.DashboardID=rd.Id  WHERE rd.DashboardName='" + roleRequest.Payload.AppType + "'";
                }
                using (sqlDataReader = ExecuteDataReader(query, null))
                {
                    if (sqlDataReader != null)
                    {
                        while (sqlDataReader.Read())
                        {
                            RoleModel roleModel = new RoleModel
                            {
                                Role = Convert.ToString(sqlDataReader["Role_Name"]),
                                RoleId = Convert.ToInt32(sqlDataReader["Role_Id"])
                            };
                            roles.Add(roleModel);
                        }

                        roleResponse.Roles = roles;
                        return roleResponse;
                    }
                    else
                    {
                        roleResponse.Error = ResponseBuilder.InternalError();
                        return roleResponse;
                    }
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
