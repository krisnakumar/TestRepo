using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using LocatorsFile_Functional;
using AventStack.ExtentReports;

namespace ReportBuilder_FunctionalTest
{
    [TestFixture]
    public class TrainingDashboard_FunctionalTest : Locators
    {
        private int TotalWorkbooksAssigned, randomIndex = 0;
        new IList<IWebElement> WorkBookRowList;
        IList<IWebElement> WorkBookHeaderList;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            ReportingModule(); //Reports be generated for all test methods
        }

        [SetUp]
        public void DoSetup()
        {
            Initialize(); //Open browsert and navigate to the given URL 
            /*WaitForPresence(PageHeader_Text); //Wait until elements are loaded 
            if (IsElementPresent(By.XPath(PageHeader_Text)))
            {
                WaitForPresence(TableRowElements);
                WorkBookRowList = getList(TableRowElements);
            }
            else
            {
                return;
            }*/
            NavigateTrainingDashboard();
        }

        [TearDown]
        protected void OneTimeTearDown()
        {
            extent.Flush(); //Flush data in reports 
            driver.Close(); //Close the browser 
        }

        [TearDown]
        public void DoTearDown()
        {
            FailedReport(); //Reporting module for failed cases 
        }

        //11074
        [Test]
        public void Test001__12008_Positive_Navigation_URL()
        {
            _test = extent.CreateTest("Verify if user is able to navigate to Contractor Training Dashboard");
            Assert.AreEqual(get(PageHeader).Text, Page_Header_Text_Training, "ERROR: User is NOT navigated to Training page");
            _test.Log(Status.Pass, "Pass");
        }

        [Test]
        public void Test004_12010_Positive_PageDescription()
        {
            _test = extent.CreateTest("Verify if the page has proper description");
            Assert.AreEqual(driver.FindElement(By.ClassName(PageDescription)).Text, Training_Description_Text, "ERROR: Proper description is NOT present in OQ Dashboard page");
            _test.Log(Status.Pass, "Pass");
        }

        [Test]
        public void Test005_12012_Positive_ProgressRole()
        {
            _test = extent.CreateTest("Verify if progress by role table is present in the Contractor Training Dashboard");
            Assert.AreEqual(driver.FindElement(By.ClassName(Section_Header)).Text, Section_Description_Text, "ERROR: Progress by Role table is not displayed");
            _test.Log(Status.Pass, "Pass");
        }

        [Test]
        public void Test006_12014_Positive_ProgressByRole_Description()
        {
            _test = extent.CreateTest("Verify if proper description is available for the Progress by role table ");
            Assert.AreEqual(driver.FindElement(By.ClassName(Section_Description)).Text, Section_Description_Text, "ERROR: Progress by Role table is not displayed");
            _test.Log(Status.Pass, "Pass");
        }

        [Test]
        public void Test007_12015_Positive_ColumnHeaders()
        {
            _test = extent.CreateTest("Verify if required column headers are available in the Progress by role table");
            Dictionary<int, string> HashMap = new Dictionary<int, string>() { { 0, "Role" }, { 1, "Incomplete Companies" }, { 2, "Completed Companies" } };
            WorkBookHeaderList = getList(HeaderRowElements);
            foreach (var index in HashMap)
            {
                Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Proper column headers are NOT present in Level 1");
                _test.Log(Status.Pass, "Pass");
            }
        }

        //11309
        [Test]
        public void Test008_12004_Positive_OpenCompanyWidget()
        {
            _test = extent.CreateTest("Verify if user is able to open Company View widget");
            WaitForPresence(ClickableCellElement);
            do
            {
                randomIndex = Random(WorkBookRowList.Count, 1);
            } while (WorkBookRowList[randomIndex].FindElement(By.XPath(EmployeeName_CellClick)).GetAttribute("class").Length == 0);
            WorkBookRowList[randomIndex].FindElement(By.XPath(EmployeeName_CellClick)).Click();
            Thread.Sleep(2000);
            driver.SwitchTo().ActiveElement();
            Dictionary<int, string> HashMap = new Dictionary<int, string>() { { 7, "Employee" }, { 8, "Role" }, { 9, "Assigned Workbooks" }, { 10, "Workbooks Due" }, { 11, "Past Due Workbooks" }, { 12, "Completed Workbooks" }, { 13, "Total Employees" } };
            WorkBookHeaderList = getList(HeaderRowElements);
            foreach (var index in HashMap)
                Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Appropriate columns are NOT present in Employee widget");
            _test.Log(Status.Pass, "Pass");

        }

        //11177
        [Test]
        public void Test009_11885_Positive_DefaultMoreOptions()
        {
            _test = extent.CreateTest("Verify if 'More Options' is present by default in the Filter option");
            WaitForPresence(PageHeader);
            Assert.IsTrue(get("//div[5]/div[1]/button").Text.Equals("More Options"));
            _test.Log(Status.Pass, "Pass");
        }

        [Test]
        public void Test010_11886_Positive_ExpandMoreOptions()
        {
            _test = extent.CreateTest("Verify if on clicking the 'more options' link expands the filter section");
            //if(driver.FindElement(By.XPath("//div[1]/div[3]/button")).isEnabled())
            {
                _test.Log(Status.Pass, "Pass");
            }
            //else
            {
                _test.Log(Status.Fail, "Fail");
            }
        }

        [Test]
        public void Test011_11887_Positive_ClickLesserOptions()
        {
            _test = extent.CreateTest("Verify if user is able to close the filter option panel by clicking 'Less options' link");
            ExpandMoreOption();
            Thread.Sleep(2000);
            Assert.IsTrue(get("//div[5]/div[1]/button").Text.Equals("Less Options"));
            _test.Log(Status.Pass, "Pass");
        }

        [Test]
        public void Test012_11888_Positive_FilterOptionsPresent()
        {
            _test = extent.CreateTest("Verify if filter option 'Role' and 'Company' are displayed in the filter options");
            ExpandMoreOption();
            List<string> FilterLabel = new List<string>();
            for (int count = 1; count <= 2; count++)
            {
                FilterLabel.Add(driver.FindElement(By.XPath("//div[5]/div[2]/div[" + count + "]/div[1]")).Text);
            }
            List<string> ExpectedFilterLabel = new List<string> { "Role:", "Company:" };
            Assert.AreEqual(FilterLabel, ExpectedFilterLabel);
            _test.Log(Status.Pass, "Pass");
        }

        [Test]
        public void Test013_11889_Positive_OpenFilterWidget()
        {
            _test = extent.CreateTest("Verify if widget titled 'Role' and 'Company' is expanded on clicking the appropriate change buttons ");
            ExpandMoreOption();
            List<string> ModalHeaderList = new List<string>();
            for (int count = 1; count <= 2; count++)
            {
                get("//div[" + count + "]/div[3]/button").Click();
                ModalHeaderList.Add(get("//div/div[1]/h5").Text);
                get("//div[2]/div/div[1]/div/div/div[1]/button").Click();
                Thread.Sleep(2000);
            }
            List<string> ExpectedModalHeaderList = new List<string> { "Roles", "Companies" };
            Assert.AreEqual(ModalHeaderList, ExpectedModalHeaderList);
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test014_11890_Positive_PresetFilterOptions()
        {
            _test = extent.CreateTest("Verify if preset filter options are displayed on opening the Role or Company filter widget");
            ExpandMoreOption();
            List<string> ModalHeaderList = new List<string>();
            for (int count = 1; count <= 2; count++)
            {
                get("//div[" + count + "]/div[3]/button").Click();
                if (driver.FindElement(By.Id("4fb251c9-8d2e-b883-2059-babd0c29be8a-option-0")).Displayed)
                {
                    _test.Log(Status.Pass, "Pass");
                }
                else
                {
                    _test.Log(Status.Fail, "Fail");
                }
            }

        }
    }
}
