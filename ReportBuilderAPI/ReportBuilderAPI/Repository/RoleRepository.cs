using Amazon.Lambda.Core;
using DataInterface.Database;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Logger;
using ReportBuilderAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Data;


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
            try
            {
                if (roleRequest.Payload == null && string.IsNullOrEmpty(roleRequest.Payload.AppType))
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
                var roleList = ReadRole(query);
                if (roleList != null)
                {
                    roleResponse.Roles = roleList;
                    return roleResponse;
                }
                else
                {
                    roleResponse.Error = ResponseBuilder.InternalError();
                    return roleResponse;
                }

            }
            catch (Exception getRolesException)
            {
                LambdaLogger.Log(getRolesException.ToString());
                roleResponse.Error = new ExceptionHandler(getRolesException).ExceptionResponse();
                return roleResponse;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        private List<RoleModel> ReadRole(string query)
        {
            List<RoleModel> roleList = new List<RoleModel>();
            IDataReader dataReader = null;
            try
            {
                using (dataReader = ExecuteDataReader(query, null))
                {
                    if (dataReader != null)
                    {
                        while (dataReader.Read())
                        {
                            RoleModel roleModel = new RoleModel
                            {
                                Role = Convert.ToString(dataReader["Role_Name"]),
                                RoleId = Convert.ToInt32(dataReader["Role_Id"])
                            };
                            roleList.Add(roleModel);
                        }
                        return roleList;
                    }
                }
                return roleList;
            }
            catch (Exception roleException)
            {
                LambdaLogger.Log(roleException.ToString());
                return null;
            }
        }
    }
}
