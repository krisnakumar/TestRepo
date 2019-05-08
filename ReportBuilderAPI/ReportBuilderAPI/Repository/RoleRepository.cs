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
                if (roleRequest.Payload.AppType == Constants.WORKBOOK_DASHBOARD)
                {
                    query = "EXEC dbo.Roles_GetRoles @companyId=" + roleRequest.CompanyId;
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
                else
                {
                    roleResponse.Roles = GetDashboardRoles(roleRequest.Payload.AppType);
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


        /// <summary>
        /// Read dashboard roles from database
        /// </summary>
        /// <param name="appType"></param>
        public List<RoleModel> GetDashboardRoles(string appType)
        {
            List<RoleModel> roleList = new List<RoleModel>();
            try
            {
                using (DBEntity context = new DBEntity())
                {
                    roleList = (from r in context.Role
                                join rdr in context.Reporting_DashboardRole on r.Id equals rdr.RoleId
                                join rd in context.Reporting_Dashboard on rdr.DashboardID equals rd.Id
                                where rd.DashboardName == appType
                                select new RoleModel
                                {
                                    RoleId = Convert.ToInt32(r.Id),
                                    Role = Convert.ToString(r.Name)
                                }).ToList();
                    return roleList;
                }
            }
            catch (Exception getDashboardRolesException)
            {
                LambdaLogger.Log(getDashboardRolesException.ToString());
                return roleList;
            }
        }
    }
}
