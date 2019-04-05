using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using LocatorsFile;
using AventStack.ExtentReports;

namespace ReportBuilderTest_SmokeTest
{
    [TestFixture]
    public class QueryBuilder : Locators
    {
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
            WaitForPresence(Element_FrontPage); //Wait until elements are loaded 
                         // if (IsElementPresent(By.XPath(Element_FrontPage)))
                         // {
                          // WaitForPresence(Element_FrontPage);
                          // WorkBookRowList = getList(Element_FrontPage);
                        //  }
                         // else
                         //{
                         //  return;
                          //}
            Thread.Sleep(2000);
            NavigateQueryBuilder();
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

        [Test]
        public void Test001_Positive_NavigateToQueryBuilder()
        {
            _test = extent.CreateTest("Checking navigation to Query Builder page");
            Assert.AreEqual(get(PageHeader).Text, "Query Builder");
            _test.Log(Status.Pass, "Pass");
        }

        [Test]
        public void Test002_Positive_EmployeeEntityResult()
        {
            _test = extent.CreateTest("Verify if result is populated on running a valid query for Employee entity");
            WaitForPresence(QueryBuilderHeader);
            RunEmployeeWithResult();
            if (driver.FindElement(By.XPath(FirstResultRow)).Displayed)
                _test.Log(Status.Pass, "Pass");
            else
                _test.Log(Status.Fail, "Fail");
        }

        [Test]
        public void Test003_Positive_WorkbookEntityResult()
        {
            _test = extent.CreateTest("Verify if result is populated on running a valid query for Workbook entity ");
            Thread.Sleep(1000);
            SelectWorkbookEntity();
            driver.FindElement(By.XPath(FirstDeleteButton)).Click();
            driver.FindElement(By.Id("workbookId")).SendKeys("12");
            driver.FindElement(By.Id("runQueryButton")).Click();
            Thread.Sleep(2000);
            if (driver.FindElement(By.XPath(FirstResultRow)).Displayed)
                _test.Log(Status.Pass, "Pass");
            else
                _test.Log(Status.Fail, "Fail");
        }

        [Test]
        public void Test004_Positive_TaskEntityResult()
        {
            _test = extent.CreateTest("Verify if result is populated on running a valid query for Task entity ");
            Thread.Sleep(2000);
            SelectTaskEntity();
            driver.FindElement(By.XPath(FirstDeleteButton)).Click();
            Thread.Sleep(2000);
            driver.FindElement(By.Id("taskId")).SendKeys("29546");
            driver.FindElement(By.Id("runQueryButton")).Click();
            Thread.Sleep(2000);
            if (driver.FindElement(By.XPath(FirstResultRow)).Displayed)
                _test.Log(Status.Pass, "Pass");
            else
                _test.Log(Status.Fail, "Fail");
        }

        [Test]
        public void Test005_Positive_QueryColumnHeaders()
        {
            _test = extent.CreateTest("Verify if proper column headers are present in the query builder panel");
            Dictionary<int, string> HashMap = new Dictionary<int, string>() { { 0, "Add/Delete" }, { 1, "And/Or" }, { 2, "Field" }, { 3, "Operator" }, { 4, "Value" } };
            WorkBookHeaderList = getList(QueryBuilderHeader);
            foreach (var index in HashMap)
            {
                Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Proper column headers are NOT present in Level 1");
                _test.Log(Status.Pass, "Pass");
            }
        }

        [Test]
        public void Test006_Positive_QueryResultHeadersEmployees()
        {
            _test = extent.CreateTest("Verify if proper column headers are present in the query results panel");
            Dictionary<int, string> HashMap = new Dictionary<int, string>() { { 0, "Employee Name" }, { 1, "Role" }, { 2, "User Id" }, { 3, "Username" }, { 4, "Email" }, { 5, "Alternative Name" }, { 6, "Total Employees" } };
            WorkBookHeaderList = getList(HeaderRowElements);
            foreach (var index in HashMap)
            {
                Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Proper column headers are NOT present when Employee Entity is selected");
                _test.Log(Status.Pass, "Pass");
            }
        }

        [Test]
        public void Test007_Positive_QueryResultHeadersWorkbooks()
        {
            _test = extent.CreateTest("Verify if proper column headers are present in the query results panel when workbook entity is selected");
            SelectWorkbookEntity();
            Dictionary<int, string> HashMap = new Dictionary<int, string>() { { 0, "Workbook ID" }, { 1, "Workbooks" }, { 2, "Description" }, { 3, "Created By" }, { 4, "Days to Complete" } };
            WorkBookHeaderList = getList(HeaderRowElements);
            foreach (var index in HashMap)
            {
                Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Proper column headers are NOT present when Workbook Entity is selected");
                _test.Log(Status.Pass, "Pass");
            }
        }

        [Test]
        public void Test008_Positive_QueryResultHeadersTasks()
        {
            _test = extent.CreateTest("Verify if proper column headers are present in the query results panel when task entity is selected");
            Thread.Sleep(2000);
            SelectTaskEntity();
            Dictionary<int, string> HashMap = new Dictionary<int, string>() { { 0, "Task Id" }, { 1, "Task Name" }, { 2, "Assigned To" }, { 3, "Evaluator Name" }, { 4, "Expiration Date" } };
            WorkBookHeaderList = getList(HeaderRowElements);
            foreach (var index in HashMap)
            {
                Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Proper column headers are NOT present when Tasks Entity is selected");
                _test.Log(Status.Pass, "Pass");
            }
        }

        [Test]
        public void Test009_Postive_GuidanceMessage()
        {
            _test = extent.CreateTest("Verify if proper column headers are present in the query results panel");
            var ResultMessage = driver.FindElement(By.XPath(GuidanceMessagePanel)).Text;
            Assert.AreEqual(ResultMessage, ExpectedMessageGuidance, "ERROR: Proper guidance message is NOT displayed");
        }

        [Test]
        public void Test010_Positive_SorryNoResults()
        {
            _test = extent.CreateTest("Verify if Sorry no results message is displayed for valid query with no results");
            driver.FindElement(By.Id("id")).SendKeys("0");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath(SecondDeleteButton)).Click();
            driver.FindElement(By.Id("runQueryButton")).Click();
            Thread.Sleep(1000);
            var NoResultMessage = driver.FindElement(By.ClassName(NoResultsPanel)).Text;
            Assert.AreEqual(NoResultMessage, ExpextedMessageNoResult, "ERROR: Proper guidance message is NOT displayed");
        }
        [Test]
        public void Test011_Positive_ValidationMessageEmptyValue()
        {
            _test = extent.CreateTest("Verify if validation message is displayed when query is run while value field is empty");
            Thread.Sleep(2000);
            driver.FindElement(By.Id("runQueryButton")).Click();
            string Validation_Msg = driver.FindElement(By.ClassName("help-block")).Text;
            Assert.AreEqual(Validation_Msg, "This field is required", "ERROR: Proper validation message is NOT displayed");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test012_Positive_ResetWarningMessage()
        {
            _test = extent.CreateTest("Verify if result is populated on running a valid query for Employee entity ");
            driver.FindElement(By.Id("id")).SendKeys("18");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath(SecondDeleteButton)).Click();
            driver.FindElement(By.Id("runQueryButton")).Click();
            Thread.Sleep(2000);
            Assert.IsTrue(get(FirstResultRow).Displayed);
            get(ResetQuery_btn).Click();
            Thread.Sleep(1000);
            string WarningMessage = driver.FindElement(By.ClassName("modal-body")).Text;
            Assert.AreEqual("Your query and result(s) will be lost. Do you wish to proceed?", WarningMessage, "ERROR: Proper alert message is NOT displayed on reseting");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test013_Positive_EntityWarningMessage()
        {
            _test = extent.CreateTest("Verify if warning message is displayed when entity is changed ");
            RunEmployeeWithResult();
            driver.FindElement(By.XPath(EntityDropdown)).Click();
            DownArrowKey();
            PressEnterKey();
            Thread.Sleep(1000);
            string WarningMessage = driver.FindElement(By.ClassName("modal-body")).Text;
            Assert.AreEqual("Your query and result(s) will be lost. Do you wish to proceed?", WarningMessage, "ERROR: Proper alert message is NOT displayed on changing the entity");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test014_Positive_ResetQuery()
        {
            _test = extent.CreateTest("Verify if user is able to reset the changes ");
            RunEmployeeWithResult();
            driver.FindElement(By.XPath(ResetQuery_btn)).Click();
            driver.SwitchTo().ActiveElement();
            get(Continue_btn).Click();
            Thread.Sleep(2000);
            Assert.Multiple(() =>
            {
                Assert.IsTrue(get(GuidanceMessagePanel).Displayed);
                Assert.IsTrue(driver.FindElement(By.Id("id")).GetAttribute("value").Length == 0);
                Assert.IsTrue(get(SecondDeleteButton).Displayed);
            });
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test015_Positive_SmartParametersUsername()
        {
            _test = extent.CreateTest("Verify if smart parameters are available for Username field");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath(FirstDeleteButton)).Click();
            driver.FindElement(By.XPath(SmartParameterDropdown)).Click();
            List<string> SmartParameter = new List<string>();
            for (int count = 1; count <= 3; count++)
            {
                GoThroughSmartParameterDropdown();
                SmartParameter.Add(driver.FindElement(By.XPath(ValueField_Text)).Text);
            }
            List<string> UsernameList = new List<string> { "@Me", "@Me and Direct Subordinates", "@Direct Subordinates" };
            Assert.AreEqual(SmartParameter, UsernameList);
            _test.Log(Status.Pass, "Pass");
        }

        [Test]
        public void Test016_Positive_SmartParameterRole()
        {
            _test = extent.CreateTest("Verify if smart parameters are available for Role field");
            Thread.Sleep(2000);
            ExpandFirstFieldDropdown();
            driver.FindElement(By.Id(ThirteenthOption)).Click();
            List<string> SmartParameter = new List<string>();
            for (int count = 1; count <= 7; count++)
            {
                GoThroughSmartParameterDropdown();
                SmartParameter.Add(driver.FindElement(By.XPath(ValueField_Text)).Text);
            }
            List<string> RolesList = new List<string> { "Contractor", "Evaluator", "Section Manager", "Student", "Supervisor", "System Admin", "Admin" };
            Assert.AreEqual(SmartParameter, RolesList);
            _test.Log(Status.Pass, "Pass");
        }

        [Test]
        public void Test017_Positive_SmartParameterBoolean()
        {
            _test = extent.CreateTest("Verify if smart parameters are available for Booleans ");
            Thread.Sleep(2000);
            ExpandFirstFieldDropdown();
            driver.FindElement(By.Id(FourteenthOption)).Click();
            List<string> SmartParameter = new List<string>();

            for (int i = 1; i <= 2; i++)
            {
                GoThroughSmartParameterDropdown();
                SmartParameter.Add(driver.FindElement(By.XPath(ValueField_Text)).Text);
            }
            List<string> RolesList = new List<string> { "No", "Yes" };
            Assert.AreEqual(SmartParameter, RolesList);
            _test.Log(Status.Pass, "Pass");
        }

        [Test]
        public void Test018_Positive_SmartParameterDate()
        {
            _test = extent.CreateTest("Verify if smart parameters are available for date fields ");
            Thread.Sleep(2000);
            ExpandFirstFieldDropdown();
            driver.FindElement(By.Id(ThirdOption)).Click();
            List<string> SmartParameter = new List<string>();

            for (int i = 1; i <= 8; i++)
            {
                GoThroughSmartParameterDropdown();
                SmartParameter.Add(driver.FindElement(By.XPath(ValueField_Text)).Text);
            }
            List<string> RolesList = new List<string> { "Yesterday", "This Week", "Last Week", "This Month", "Last Month", "This Year", "Last Year", "Today" };
            Assert.AreEqual(SmartParameter, RolesList);
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test019_Positive_DatePlaceholderEmployees()
        {
            _test = extent.CreateTest("Verify the presence of date format as placeholder when date field is selected - Employees entity");
            PlaceholderForDateEmployee();
            string PlaceholderText = driver.FindElement(By.ClassName("Select-placeholder")).Text;
            Assert.AreEqual("MM/DD/YYYY", PlaceholderText, "ERROR: Date format is not displayed in the placeholder text");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test020_Positive_DatePlaceholderWorkbook()
        {
            _test = extent.CreateTest("Verify the presence of date format as placeholder when date field is selected - Workbook entity");
            Thread.Sleep(1000);
            SelectWorkbookEntity();
            PlaceholderForDateEmployee();
            string PlaceholderText = driver.FindElement(By.ClassName("Select-placeholder")).Text;
            Assert.AreEqual("MM/DD/YYYY", PlaceholderText, "ERROR: Date format is not displayed in the placeholder text");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test021_Positive_DatePlaceholderTask()
        {
            _test = extent.CreateTest("Verify the presence of date format as placeholder when date field is selected - task entity");
            Thread.Sleep(1000);
            SelectTaskEntity();
            ExpandFirstFieldDropdown();
            driver.FindElement(By.Id(SecondOption)).Click();
            string PlaceholderText = driver.FindElement(By.ClassName("Select-placeholder")).Text;
            Assert.AreEqual("MM/DD/YYYY", PlaceholderText, "ERROR: Date format is not displayed in the placeholder text");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test022_Positive_ExportButton()
        {
            _test = extent.CreateTest("Check for the presence of Export button after result are populated");
            Thread.Sleep(1000);
            RunEmployeeWithResult();
            if (driver.FindElement(By.XPath(ExportButton)).Displayed)
                _test.Log(Status.Pass, "Pass");
            else
                _test.Log(Status.Fail, "Fail");
        }

        [Test]
        public void Test023_Positive_ColumnOptionsDisplay()
        {
            _test = extent.CreateTest("Verify if column options panel is expanded on clicking 'Column Options' button");
            OpenColumnOptions();
            if (driver.FindElement(By.ClassName("slide-pane__content")).Displayed)
                _test.Log(Status.Pass, "Pass");
            else
                _test.Log(Status.Fail, "Fail");
        }

        [Test]
        public void Test024_Positive_AddColumns()
        {
            _test = extent.CreateTest("Verify if added column is displayed in result set");
            OpenColumnOptions();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath(AddColumnButton)).Click();
            driver.FindElement(By.XPath(OKButton_ColumnOptions)).Click();
            driver.FindElement(By.XPath(OKButton_ColumnOptions)).Click();
            string AddedColumn = driver.FindElement(By.XPath(EightColumnHeader)).Text;
            Dictionary<int, string> HashMap = new Dictionary<int, string>() { { 7, AddedColumn } };
            WorkBookHeaderList = getList(HeaderRowElements);
            foreach (var index in HashMap)
            {
                Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value));
            }
            _test.Log(Status.Pass, "Pass");
        }
    }
}

