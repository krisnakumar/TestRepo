using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReportBuilder.Models.Models;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
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
            Assert.IsTrue(companyResponse.Companies.Count > 0);
            Assert.IsTrue(companyResponse.Error==null);            
        }


        [TestMethod]
        public void CheckException()
        {
            CompanyRequest companyRequest = new CompanyRequest { };       
            Mock<CompanyRepository> companyMock = new Mock<CompanyRepository>();
            CompanyResponse companyResponse = companyMock.Object.GetCompany(companyRequest);            
            Assert.IsTrue(companyResponse.Error != null);
            Assert.IsTrue(companyResponse.Error.Message == "Invalid input :UserId");
        }


        [TestMethod]
        public void CheckInternalServer()
        {
            CompanyRequest companyRequest = new CompanyRequest { UserId = 10 };
            List<CompanyModels> companyList = null; 
            Mock<CompanyRepository> companyMock = new Mock<CompanyRepository>();
            companyMock.Setup(r => r.ReadCompanies(It.IsAny<string>())).Returns(companyList);
            CompanyResponse companyResponse = companyMock.Object.GetCompany(companyRequest);
            Assert.IsTrue(companyResponse.Error.Message == "System Error");
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
                CompanyId = 2,
                CompanyName = "OQ"
            };
            companyList.Add(companyModel);
            return companyList;
        }
    }
}
