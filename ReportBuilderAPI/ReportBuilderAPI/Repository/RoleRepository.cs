using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using DataInterface.Database;
using Newtonsoft.Json;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.Queries;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;

namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Class that manages the role
    /// </summary>
    public class RoleRepository
    {
        /// <summary>
        /// Get shared roles based on the companyId
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns>Roles</returns>
        public APIGatewayProxyResponse GetRoles(int companyId)
        {
            RoleResponse roleResponse = new RoleResponse();
            DatabaseWrapper databaseWrapper = new DatabaseWrapper();
            List<RoleModel> roleList = new List<RoleModel>();
            try
            {
                //Read the roles from DB
                SqlDataReader sqldatareader = databaseWrapper.ExecuteReader(Role.GetRole(companyId), new Dictionary<string, string>());
                if (sqldatareader != null && sqldatareader.HasRows)
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
                    roleResponse.Roles = roleList;
                    return ResponseBuilder.GatewayProxyResponse((int)HttpStatusCode.OK, JsonConvert.SerializeObject(roleResponse), 0);
                }
                else
                {
                    return ResponseBuilder.InternalError();
                }
            }
            catch (Exception getRolesException)
            {
                LambdaLogger.Log(getRolesException.ToString());
                return ResponseBuilder.InternalError();
            }
        }
    }
}
