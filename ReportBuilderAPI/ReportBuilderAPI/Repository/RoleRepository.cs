using Amazon.Lambda.Core;
using DataInterface.Database;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Queries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Class that manages the role
    /// </summary>
    public class RoleRepository :IRole
    {
        /// <summary>
        /// Get shared roles based on the companyId
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        public RoleResponse GetRoles(RoleRequest roleRequest)
        {
            RoleResponse roleResponse = new RoleResponse();
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<RoleModel> roleList = new List<RoleModel>();
            try
            {
                //Read the roles from DB
                using (SqlDataReader sqldatareader = databaseWrapper.ExecuteReader(Role.GetRole(roleRequest.CompanyId), null))
                {
                    if (sqldatareader != null)
                    {
                        if (sqldatareader.HasRows)
                        {
                            while (sqldatareader.Read())
                            {
                                RoleModel roleModel = new RoleModel
                                {
                                    RoleId = Convert.ToInt32(sqldatareader["Id"]),
                                    Role = Convert.ToString(sqldatareader["Name"])
                                };
                                roleList.Add(roleModel);
                            }
                        }
                        roleResponse.Roles = roleList;
                    }
                    else
                    {
                        roleResponse.Error = ResponseBuilder.InternalError();
                    }
                }
                return roleResponse;
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
