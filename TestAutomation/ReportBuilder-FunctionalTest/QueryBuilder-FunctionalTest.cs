using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using LocatorsFile_Functional;
using AventStack.ExtentReports;

[TestFixture]
public class QueryBuilder_FunctionalTest : Locators
{
    public int TotalWorkbooksAssigned, randomIndex = 0;
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
        WaitForPresence(PageHeader_Text); //Wait until elements are loaded 
        if (IsElementPresent(By.XPath(PageHeader_Text)))
        {
            WaitForPresence(TableRowElements);
            WorkBookRowList = getList(TableRowElements);
        }
        else
        {
            return;
        }
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
    //10573
    [Test]
    public void Test001__10582_Positive_UserIdDisplayed()
    {
        _test = extent.CreateTest("Verify if User Id is displayed as Field dropdown option and as a header in the results table");
        Assert.Multiple(() =>
        {
            Assert.IsTrue(get(FirstField_Text).Text.Equals(UserId_FieldText));
            Assert.IsTrue(get(UserId_HeaderField).Text.Equals(UserId_FieldText));
        });
        _test.Log(Status.Pass, "Pass");
    }

    [Test]
    public void Test002_10583_Positive_UserIdOperators()
    {
        _test = extent.CreateTest("Verify if Operators are displayed appropriately for User Id field");
        Thread.Sleep(2000);
        driver.FindElement(By.XPath(FirstOperatorDropdown)).Click();
        List<string> Operator_Option = new List<string>();
        for (int i = 1; i <= 4; i++)
        {
            GoThroughSmartParameterDropdown();
            Operator_Option.Add(driver.FindElement(By.XPath(OperatorField_Text)).Text);
        }
        List<string> OperatorList = new List<string> { OperatorDropdown_1, OperatorDropdown_2, OperatorDropdown_3, OperatorDropdown_4 };
        Assert.AreEqual(Operator_Option, OperatorList);
        _test.Log(Status.Pass, "Pass");
    }

    [Test]
    public void Test003_10584_Positive_ResultForUserId()
    {
        _test = extent.CreateTest("Verify if proper results are displayed when User Id query is run ");
        WaitForPresence(QueryBuilderHeader);
        RunEmployeeWithResult();
        WaitForPresence(FirstResultRow);
        if (driver.FindElement(By.XPath(FirstResultRow)).Displayed)
            _test.Log(Status.Pass, "Pass");
        else
            _test.Log(Status.Fail, "Fail");
    }

    [Test]
    public void Test004_10585_Positive_ResultForWorkbook()
    {
        _test = extent.CreateTest("Verify if workbook assigned to the employee is displayed on using User Id field when workbook entity is selected");
        Thread.Sleep(2000);
        SelectWorkbookEntity();
        WaitForPresence(HeaderRowElements);
        driver.FindElement(By.XPath(SecondDeleteButton)).Click();
        get(FirstField_Text).Click();
        UpArrowKey();
        PressEnterKey();
        get(UserId_ValueField).SendKeys(UserId);
        get(RunQuery_Button).Click();
        Thread.Sleep(2000);
        Assert.AreEqual(get(Result_FirstRowFirstColumn).Text, UserId, "ERROR: Workbook details for the User id is NOT displayed");
    }
    [Test]
    public void Test005_10586_Positive_ResultForTask()
    {
        _test = extent.CreateTest("Verify if User Id field in Tasks entity gives results related to the tasks assigned to the User Id");
        Thread.Sleep(2000);
        SelectTaskEntity();
        WaitForPresence(HeaderRowElements);
        get(SecondDeleteButton).Click();
        SelectUserId_TaskEntity();
        get(UserId_ValueField).SendKeys(UserId);
        get(RunQuery_Button).Click();
        WaitForPresence(FirstResultRow);
        Assert.IsTrue(get(FirstResultRow).Displayed, "ERROR: Tasks details for the User Id is NOT displayed");
    }
    [Test]
    public void Test006_10587_Positive_BooleanOperatorForHasPhoto()
    {
        _test = extent.CreateTest("Verify if 'Has Photo' dropdown has a boolen operator");
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
        Assert.AreEqual(SmartParameter, RolesList, "ERROR: 'Has Photo' does NOT have boolean operators");
        _test.Log(Status.Pass, "Pass");
    }
    [Test]
    public void Test007_10588_Positive_BooleanOperatorForHasQRCode()
    {
        _test = extent.CreateTest("Verify if 'Has QR Code' dropdown has a boolen operator  ");
        Thread.Sleep(2000);
        ExpandFirstFieldDropdown();
        driver.FindElement(By.Id(FifteenthOption)).Click();
        List<string> SmartParameter = new List<string>();

        for (int i = 1; i <= 2; i++)
        {
            GoThroughSmartParameterDropdown();
            SmartParameter.Add(driver.FindElement(By.XPath(ValueField_Text)).Text);
        }
        List<string> RolesList = new List<string> { "No", "Yes" };
        Assert.AreEqual(SmartParameter, RolesList, "ERROR: 'Has QR Code' does NOT have boolean operators");
        _test.Log(Status.Pass, "Pass");
    }
    [Test]
    public void Test008_10589_Positive_BooleanOperatorForHasDepartment()
    {
        _test = extent.CreateTest("Verify if 'Has Department' dropdown has a boolen operator");
        Thread.Sleep(2000);
        ExpandFirstFieldDropdown();
        driver.FindElement(By.Id(SixteenthOption)).Click();
        List<string> SmartParameter = new List<string>();

        for (int i = 1; i <= 2; i++)
        {
            GoThroughSmartParameterDropdown();
            SmartParameter.Add(driver.FindElement(By.XPath(ValueField_Text)).Text);
        }
        List<string> RolesList = new List<string> { "No", "Yes" };
        Assert.AreEqual(SmartParameter, RolesList, "ERROR: 'Has Department' does NOT have boolean operators");
        _test.Log(Status.Pass, "Pass");
    }
    [Test]
    public void Test009_10590_Positive_ResultsForHasFields()
    {
        _test = extent.CreateTest("Verify if proper results are displayed for 'has' fields ");
        Thread.Sleep(2000);
        ExpandFirstFieldDropdown();
        driver.FindElement(By.Id(FourteenthOption)).Click();
        driver.FindElement(By.XPath(SmartParameter_Dropdown)).Click();
        PressEnterKey();
        get(SecondDeleteButton).Click();
        get(RunQuery_Button).Click();
        WaitForPresence(FirstResultRow);
        Assert.IsTrue(get(FirstResultRow).Displayed, "ERROR: Results are not displayed for Has field and 'Yes' smart parameter");
    }
    [Test]
    public void Test010_10591_Positive_ResultsForHasFields_NoSmartParameter()
    {
        _test = extent.CreateTest("Verify if proper results are displayed when query is run for 'has not' field");
        Thread.Sleep(2000);
        ExpandFirstFieldDropdown();
        driver.FindElement(By.Id(FourteenthOption)).Click();
        driver.FindElement(By.XPath(SmartParameter_Dropdown)).Click();
        DownArrowKey();
        PressEnterKey();
        get(SecondDeleteButton).Click();
        get(RunQuery_Button).Click();
        WaitForPresence(FirstResultRow);
        Assert.IsTrue(get(FirstResultRow).Displayed, "ERROR: Results are not displayed for Has Field and 'No' smart parameter");
    }
    [Test]
    public void Test011_10592_Positive_HasNotInWorkbookOrTask()
    {
        _test = extent.CreateTest("Verify if 'has' fields are not displayed in Workbook or Tasks entity");
        Thread.Sleep(2000);
        SelectWorkbookEntity();
        List<string> Field_Options_Workbook = new List<string>();
        for (int i = 1; i <= 17; i++)
        {
            driver.FindElement(By.XPath(Field_DropdownIcon)).Click();
            DownArrowKey();
            PressEnterKey();
            Field_Options_Workbook.Add(driver.FindElement(By.XPath(FirstField_Text)).Text);
        }
        Thread.Sleep(2000);
        SelectTaskEntity();
        get(Continue_RestButton).Click();
        Thread.Sleep(1000);
        List<string> Field_Options_Task = new List<string>();
        for (int i = 1; i <= 13; i++)
        {
            driver.FindElement(By.XPath(Field_DropdownIcon)).Click();
            DownArrowKey();
            PressEnterKey();
            Field_Options_Task.Add(driver.FindElement(By.XPath(FirstField_Text)).Text);
        }
        Assert.Multiple(() =>
        {
            Assert.IsTrue(!Field_Options_Workbook.Contains("Has Photo") && !Field_Options_Workbook.Contains("Has QR Code") && !Field_Options_Workbook.Contains("Has Department"));
            Assert.IsTrue(!Field_Options_Task.Contains("Has Photo") && !Field_Options_Task.Contains("Has QR Code") && !Field_Options_Task.Contains("Has Department"));
        });
        _test.Log(Status.Pass, "Pass");
    }
    //10645 - Enhancements
    [Test]
    public void Test013__11662_Positive_EmployeeNameFormat_QueryBuilder()
    {
        _test = extent.CreateTest("Verify if Employee name is 2 words only in Query Builder page");
        RunEmployeeWithResult();
        string Employee_Name = get("//div[1]/div/div/span/div").Text;
        string LastName = Employee_Name.Substring(0, Employee_Name.IndexOf(","));
        Employee_Name = Employee_Name.Substring(Employee_Name.LastIndexOf(","));
        string FirstName = Employee_Name.Substring(2);
        Assert.Multiple(() =>
        {
            Assert.IsTrue(!LastName.Contains(" "), "ERROR: Two names are there in Lastname");
            Assert.IsTrue(!FirstName.Contains(" "), "ERROR: Two names are there in Firstname");
        }
        );
            }
    
        [Test]
        public void Test014_11716_Positive_WorkbookNameToWorkbook_QueryBuilder()
    {
        _test = extent.CreateTest("Verify if Employee name is 2 words only in Query Builder page");
        Thread.Sleep(2000);
        SelectWorkbookEntity();
        string Word_NotToBePresent = get("//div[text()='Workbook']").Text;
        Assert.IsTrue(!Word_NotToBePresent.Contains("Workbook Name"));
        _test.Log(Status.Pass, "Pass");
    }

    [Test]
    public void Test015_xxx_Positive_EvaluatorNameFormat_QB()
    {
        _test = extent.CreateTest("xys");
        Thread.Sleep(2000);
        SelectTaskEntity();
        WaitForPresence(HeaderRowElements);
        get(SecondDeleteButton).Click();
        SelectUserId_TaskEntity();
        get(UserId_ValueField).SendKeys(UserId);
        get(RunQuery_Button).Click();
        Thread.Sleep(2000);
        string Evaluator_Name = get("//div[4]/div/div/span/div").Text;
        string LastName = Evaluator_Name.Substring(0, Evaluator_Name.IndexOf(","));
        Evaluator_Name = Evaluator_Name.Substring(Evaluator_Name.LastIndexOf(","));
        string FirstName = Evaluator_Name.Substring(2);
        Assert.Multiple(() =>
        {
            Assert.IsTrue(!LastName.Contains(" "), "ERROR: Two names are there in Lastname");
            Assert.IsTrue(!FirstName.Contains(" "), "ERROR: Two names are there in Firstname");
        }
        );
        _test.Log(Status.Pass, "Pass");
    }
    [Test]
    public void Test016_xxx_Positive_ExpirationDate_Format()
    {
        _test = extent.CreateTest("xys");
        Thread.Sleep(1000);
        SelectTaskEntity();
        Thread.Sleep(2000);
        get(SecondDeleteButton).Click();
        SelectUserId_TaskEntity();
        get(UserId_ValueField).SendKeys(UserId);
        get(RunQuery_Button).Click();
        Thread.Sleep(2000);
        string Completion_Date = get("//div[5]/div/div/span/div").Text;
        Assert.IsTrue(!Completion_Date.Contains(":"));
        _test.Log(Status.Pass, "Pass");
    }

    //9890
    [Test]
    public void Test017_11221_Positive_SmartParametersDisplayed()
    {
        _test = extent.CreateTest("xys");
        Thread.Sleep(2000);
        get("//*[@id='userName']");
        if(get("//td[5]/div/div/span[2]/span").Displayed)
        {
            _test.Log(Status.Pass, "Pass");
        }
        else
        {
            _test.Log(Status.Fail, "Fail");
        }
    }
    [Test]
    public void Test018_11225_Positive_SmartParametersDropdown_Username()
    {
        _test = extent.CreateTest("Verify if appropriate options are displayed as smart parameters for Username field");
        Thread.Sleep(3000);
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
    public void Test019_11467_Positive_SmartParametersDropdown_DateField()
    {
        _test = extent.CreateTest("Verifying smart parameters for Date fields");
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
    public void Test020_11468_Positive_SmartParametersDropdown_Boolean()
    {
        _test = extent.CreateTest("Verifying the smart parameters for Boolean fields");
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
    public void Test021_11469_Positive_SmartParametersDropdown_Role()
    {
        _test = extent.CreateTest("Verifying the smart parameters for Role field");
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
    public void Test022_11471_Positive_SmartParameter_RunQuery()
    {
        _test = extent.CreateTest("Verifying whether results are displayed when query is run using smart parameters");
        get(FirstDeleteButton).Click();
        get("//[*id='UserName']").Click();
        PressEnterKey();
        //Assert.Display
    }
}
    


