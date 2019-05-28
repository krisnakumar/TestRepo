using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Repository;
using System.Collections.Generic;
using ReportBuilderAPI.Utilities;

namespace ReportBuilder.UnitTest.TestModules.Company
{
    [TestClass]
    public class RoleTest
    {

        /////////////////////////////////////////////////////////////////
        //                                                             //
        //    All test cases are tested upto build v0.9.190103.6487    //
        //                                                             //
        /////////////////////////////////////////////////////////////////

        [TestMethod]
        public void GetWorkbookRoleDetails()
        {
            RoleRequest roleRequest = new RoleRequest { AppType = Constants.WORKBOOK_DASHBOARD, CompanyId=6 };
            var companyList = CreateRoleList();
            Mock<RoleRepository> roleMock = new Mock<RoleRepository>();
            roleMock.Setup(r => r.ReadRole(It.IsAny<string>())).Returns(companyList);
            RoleResponse roleResponse = roleMock.Object.GetRoles(roleRequest);
            Assert.IsTrue(roleResponse.Roles.Count > 0);
            Assert.IsTrue(roleResponse.Error == null);
        }


        [TestMethod]
        public void CheckException()
        {
            RoleRequest roleRequest = new RoleRequest { AppType = Constants.WORKBOOK_DASHBOARD};
            var companyList = CreateRoleList();
            Mock<RoleRepository> roleMock = new Mock<RoleRepository>();
            roleMock.Setup(r => r.ReadRole(It.IsAny<string>())).Returns(companyList);
            RoleResponse roleResponse = roleMock.Object.GetRoles(roleRequest);
            Assert.IsTrue(roleResponse.Roles.Count > 0);
            Assert.IsTrue(roleResponse.Error.Message == "Invalid input :CompanyId");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<RoleModel> CreateRoleList()
        {
            List<RoleModel> roleList = new List<RoleModel>();
            RoleModel roleModel = new RoleModel
            {
                RoleId = 1,
                Role = "Supervisor"
            };
            roleList.Add(roleModel);
            RoleModel roleModels = new RoleModel
            {
                RoleId = 2,
                Role = "Employee"
            };
            roleList.Add(roleModels);
            return roleList;
        }
    }
}
