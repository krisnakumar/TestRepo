using Amazon.Lambda.Core;
using DataInterface.Database;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.ResponseHandler;
using ReportBuilderAPI.IRepository;
using ReportBuilderAPI.Logger;
using System;
using System.Collections.Generic;
using System.Data;
namespace ReportBuilderAPI.Repository
{
    /// <summary>
    /// Class that manages the Companies
    /// </summary>
    public class CompanyRepository : DatabaseWrapper, ICompany
    {
        /// <summary>
        /// Get shared companies based on the userId
        /// </summary>
        /// <param name="companyRequest"></param>
        /// <returns>CompanyResponse</returns>
        public CompanyResponse GetCompany(CompanyRequest companyRequest)
        {
            CompanyResponse companyResponse = new CompanyResponse();
            List<CompanyModels> companyList = new List<CompanyModels>();
            string query = string.Empty;
            try
            {
                if (companyRequest.UserId == 0)
                {
                    throw new ArgumentException("UserId");
                }

                query = "SELECT c.Id [company_id], c.Name [company_name] FROM dbo.UserCompany uc JOIN dbo.CompanyClient cc ON uc.CompanyId=cc.OwnerCompany JOIN dbo.Company c ON c.Id = cc.ClientCompany WHERE uc.IsDefault=1 	AND uc.IsEnabled=1	AND uc.UserId=" + companyRequest.UserId + " 	AND cc.IsEnabled=1	and c.IsEnabled=1	AND cc.ClientCompany!=uc.CompanyId ";
                companyList = ReadCompanies(query);
                if (companyList != null)
                {
                    companyResponse.Companies = companyList;
                }
                else
                {
                    companyResponse.Error = ResponseBuilder.InternalError();
                    return companyResponse;
                }
                return companyResponse;
            }
            catch (Exception getCompanyException)
            {
                LambdaLogger.Log(getCompanyException.ToString());
                companyResponse.Error = new ExceptionHandler(getCompanyException).ExceptionResponse();
                return companyResponse;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public virtual List<CompanyModels> ReadCompanies(string query)
        {
            List<CompanyModels> companyList = new List<CompanyModels>();
            IDataReader iDataReader = null;
            try
            {
                using (iDataReader = ExecuteDataReader(query, null))
                {
                    if (iDataReader != null)
                    {
                        while (iDataReader.Read())
                        {
                            CompanyModels roleModel = new CompanyModels
                            {
                                CompanyId = Convert.ToInt32(iDataReader["company_id"]),
                                CompanyName = Convert.ToString(iDataReader["company_name"])
                            };
                            companyList.Add(roleModel);
                        }

                    }
                }
                return companyList;
            }
            catch (Exception companyException)
            {
                LambdaLogger.Log(companyException.ToString());
                return null;
            }
        }
    }
}
