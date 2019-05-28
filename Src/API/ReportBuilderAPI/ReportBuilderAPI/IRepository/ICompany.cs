using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using System.Collections.Generic;

namespace ReportBuilderAPI.IRepository
{
    public interface ICompany
    {
        CompanyResponse GetCompany(CompanyRequest companyRequest);
        List<CompanyModels> ReadCompanies(string query);
    }
}
