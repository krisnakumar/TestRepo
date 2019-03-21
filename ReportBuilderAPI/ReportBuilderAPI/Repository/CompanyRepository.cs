using Amazon.Lambda.Core;
using OnBoardLMS.WebAPI.Models;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ReportBuilderAPI.Repository
{
    public class CompanyRepository
    {
        /// <summary>
        /// Get shared companies based on the userId
        /// </summary>
        /// <param name="roleRequest"></param>
        /// <returns></returns>
        public CompanyResponse GetCompany(CompanyRequest companyRequest)
        {
            CompanyResponse companyResponse = new CompanyResponse();
            try
            {
                using (DBEntity context = new DBEntity())
                {
                    companyResponse.Companies = (from c in context.Company
                                                 join uc in context.UserCompany on c.Id equals uc.CompanyId
                                                 where uc.Status == 1 && uc.IsEnabled && uc.UserId == companyRequest.UserId
                                                 select new CompanyModels
                                                 {
                                                     CompanyId = Convert.ToInt32(c.Id),
                                                     CompanyName = Convert.ToString(c.Name)
                                                 }).ToList();
                    return companyResponse;
                }
            }
            catch (Exception getRolesException)
            {
                LambdaLogger.Log(getRolesException.ToString());
                companyResponse.Error = ResponseBuilder.InternalError();
                return companyResponse;
            }
        }
    }
}
