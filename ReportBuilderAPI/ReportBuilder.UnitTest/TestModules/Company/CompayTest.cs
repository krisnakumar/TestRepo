using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Handlers.FunctionHandler;
using ReportBuilderAPI.Repository;
using System.Collections.Generic;

namespace ReportBuilder.UnitTest.TestModules.Company
{
    [TestClass]
    public class CompayTest
    {

        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190103.6487    //
        //                                                             //
        /////////////////////////////////////////////////////////////////

        [TestMethod]
        public void GetCompanyDetails()
        {
            CompanyRequest companyRequest = new CompanyRequest { UserId=10};
            var companyList = CreateCompanyList();
            Mock<CompanyRepository> companyMock = new Mock<CompanyRepository>();
            companyMock.Setup(r => r.ReadCompanies(It.IsAny<string>())).Returns(companyList);
            CompanyResponse companyResponse = companyMock.Object.GetCompany(companyRequest);
            //Assert.Is

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CompanyModels> CreateCompanyList()
        {
            List<CompanyModels> companyList = new List<CompanyModels>();
            CompanyModels companyModels = new CompanyModels
            {
                CompanyId=1,
                CompanyName="ITS"
            };
            companyList.Add(companyModels);
            CompanyModels companyModel = new CompanyModels
            {
                CompanyId = 1,
                CompanyName = "OQ"
            };
            companyList.Add(companyModel);
            return companyList;
        }
    }
}
