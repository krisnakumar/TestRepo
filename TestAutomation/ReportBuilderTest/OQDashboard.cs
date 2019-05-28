using NUnit.Framework;
using OpenQA.Selenium;
using LocatorsFile;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;

namespace ReportBuilderTest_SmokeTest
{
    [TestFixture]
    class OQDashboard : Locators
    {

        [OneTimeSetUp]
        //Method to extend the reporting module 
        protected void OneTimeSetup()
        {

            htmlReporter = new ExtentHtmlReporter(@"C:\Users\janani_jeeva\Documents\test1.html")
            {
                AppendExisting = true
            };
            htmlReporter.LoadConfig(@"C:\Users\janani_jeeva\source\repos\ReportBuilderTest\ReportBuilderTest\Extent-Config.xml");

            extent = new ExtentReports();

            extent.AttachReporter(htmlReporter);
        }

        [SetUp]
        public void DoSetup()
        {
            Initialize();
            WaitForPresence(PageHeader_Text);

            //Logic to check for empty page after login
            if (IsElementPresent(By.XPath(PageHeader_Text)))
            {
                WaitForPresence(TableRowElements);
                WorkBookRowList = getList(TableRowElements);
            }
        }

        [OneTimeTearDown]
        protected void OneTimeTearDown()
        {
            extent.Flush();

        }
        [TearDown]
        public void DoTearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = TestContext.CurrentContext.Result.StackTrace;
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
                _test.Log(Status.Fail, status + stacktrace + errorMessage);
            Logout();
            driver.Close();
        }
        [Test]
        public void Test001_Positive_Navigate()
        //9165
        //Verify if user is able to navigate to OQ Dashboard page through the Reports module dropdown 
        {
            _test = extent.CreateTest("Checking navigation to OQ Dashboard page page");
            {
                NavigateOQDashboard();
                Assert.IsTrue(get(PageHeader).Text.Equals("Contractor OQ Dashboard"));
                _test.Log(Status.Pass, "Pass");
            }
        }
        [Test]
        public void Test002_Positive_Description()
        //9197
        //Verify if description is available for the page
        {
            _test = extent.CreateTest("Checking presence of description");
            {
                Assert.IsTrue(driver.FindElement(By.ClassName("card__description")).Text.Equals(OQDashboard_Description_Text));
                _test.Log(Status.Pass, "Pass");
            }
        }
        [Test]
        public void Test003_Positive_CompanyTableDisplayed()
        //9204
        //Verify if Company view table is displayed in first level of OQ Dashboard page
        {

        }

    }
}

