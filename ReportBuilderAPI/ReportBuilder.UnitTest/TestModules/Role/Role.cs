using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Repository;
using ReportBuilderAPI.Utilities;
using System.Collections.Generic;


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
            RoleRequest roleRequest = new RoleRequest
            {
                Payload = new RoleRequest
                { AppType = Constants.WORKBOOK_DASHBOARD },
                CompanyId = 6
            };
            List<RoleModel> roleList = CreateRoleList();
            Mock<RoleRepository> roleMock = new Mock<RoleRepository>();
            roleMock.Setup(r => r.ReadRole(It.IsAny<string>())).Returns(roleList);
            RoleResponse roleResponse = roleMock.Object.GetRoles(roleRequest);
            Assert.IsTrue(roleResponse.Roles.Count > 0);
            Assert.IsTrue(roleResponse.Error == null);
        }


        [TestMethod]
        public void CheckException()
        {
            RoleRequest roleRequest = new RoleRequest
            {
                Payload = new RoleRequest
                { AppType = Constants.WORKBOOK_DASHBOARD }
            };
            List<RoleModel> roleList = CreateRoleList();
            Mock<RoleRepository> roleMock = new Mock<RoleRepository>();
            roleMock.Setup(r => r.ReadRole(It.IsAny<string>())).Returns(roleList);
            RoleResponse roleResponse = roleMock.Object.GetRoles(roleRequest);
            Assert.IsTrue(roleResponse.Error.Message == "Invalid input :COMPANY_ID");
        }

        [TestMethod]
        public void CheckArgumentExceptionforAppType()
        {
            RoleRequest roleRequest = new RoleRequest { };
            List<RoleModel> roleList = CreateRoleList();
            Mock<RoleRepository> roleMock = new Mock<RoleRepository>();
            roleMock.Setup(r => r.ReadRole(It.IsAny<string>())).Returns(roleList);
            RoleResponse roleResponse = roleMock.Object.GetRoles(roleRequest);
            Assert.IsTrue(roleResponse.Error.Message == "Invalid input :PAYLOAD");
        }

        [TestMethod]
        public void CheckInternalServer()
        {
            RoleRequest roleRequest = new RoleRequest
            {
                Payload = new RoleRequest
                { AppType = Constants.WORKBOOK_DASHBOARD },
                CompanyId = 6
            };
            List<RoleModel> roleList = null;
            Mock<RoleRepository> roleMock = new Mock<RoleRepository>();
            roleMock.Setup(r => r.ReadRole(It.IsAny<string>())).Returns(roleList);
            RoleResponse roleResponse = roleMock.Object.GetRoles(roleRequest);
            Assert.IsTrue(roleResponse.Error.Message == "System Error");
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
