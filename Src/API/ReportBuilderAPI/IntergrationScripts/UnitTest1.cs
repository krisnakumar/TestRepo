using DataInterface.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ReportBuilder.Models.Request;
using ReportBuilder.Models.Response;
using ReportBuilderAPI.Repository;
using ReportBuilderAPI.Utilities;
using System.Collections.Generic;
using System.Data;

namespace IntergrationScripts
{
    [TestClass]
    public class UnitTest1
    {       
            private Mock<RoleRepository> mockEmployeeRepository = new Mock<RoleRepository>();


            private const string Column1 = "Role_Name";
            private const string Column2 = "Role_Id";
            private const string ExpectedValue1 = "Shoba";
            private const string ExpectedValue2 = "1";

            [TestMethod]
            public void TestMethod1()
            {
                Mock<IDataReader> dataReader = new Moq.Mock<IDataReader>();
                DatabaseWrapper._connectionString = string.Empty;

                dataReader.Setup(x => x[Column1]).Returns(ExpectedValue1);
                dataReader.Setup(x => x[Column2]).Returns(ExpectedValue2);

                dataReader.SetupSequence(m => m.Read())
                    .Returns(true)
                    .Returns(false);
                RoleRequest roleRequest = new RoleRequest
                {
                    UserId = 1,
                    CompanyId = 6,
                    Payload = new RoleRequest
                    {
                        AppType = Constants.WORKBOOK_DASHBOARD
                    }
                };

                RoleModel roleModel = new RoleModel
                {
                    Role = "Shoba",
                    RoleId = 1
                };
                List<RoleModel> roleModels = new List<RoleModel>
            {
                roleModel
            };

                mockEmployeeRepository.Setup(m => m.ExecuteDataReader(It.IsAny<string>(), null)).Returns(dataReader.Object);
                mockEmployeeRepository.Setup(m => m.CloseConnection());
                mockEmployeeRepository.CallBase = true;

                ReportBuilder.Models.Response.RoleResponse ActualResponse = mockEmployeeRepository.Object.GetRoles(roleRequest);
                Assert.AreEqual(null, ActualResponse.Error);
                Assert.AreEqual(roleModels, ActualResponse.Roles);
            }
        }
    }
