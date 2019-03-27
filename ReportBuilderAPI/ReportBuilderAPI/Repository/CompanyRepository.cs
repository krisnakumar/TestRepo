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
    /// <summary>
    /// Class that manages the Companies
    /// </summary>
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
                    companyResponse.Companies = (from uc in context.UserCompany
                                                 join cc in context.CompanyClient on uc.CompanyId equals cc.OwnerCompany
                                                 join c in context.Company on cc.ClientCompany equals c.Id
                                                 where uc.IsDefault && uc.IsEnabled && uc.Status == 1 && cc.IsEnabled && c.IsEnabled && uc.UserId == companyRequest.UserId
                                                 select new CompanyModels
                                                 {
                                                     CompanyId = Convert.ToInt32(c.Id),
                                                     CompanyName = Convert.ToString(c.Name)
                                                 }).ToList();
                    return companyResponse;
                }
            }
            catch (Exception getCompanyException)
            {
                LambdaLogger.Log(getCompanyException.ToString());
                companyResponse.Error = ResponseBuilder.InternalError();
                return companyResponse;
            }
        }
    }
}
