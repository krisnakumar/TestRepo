using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using LocatorsFile_Functional;
using AventStack.ExtentReports;

[TestFixture]
public class ContractorOQDashboard_FunctionalTest : Locators
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
        NavigaterOQDashboard();
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
    public void Test001__9165_Positive_NavigateToOQDashboard()
    {
        _test = extent.CreateTest("Checking navigation to OQ Dashboard page");
        Assert.AreEqual(get(PageHeader).Text, Page_Header_Text_OQ, "ERROR: User is NOT navigated to OQ Dashboard page");
        _test.Log(Status.Pass, "Pass");
    }

    [Test]
    public void Test002_9196_Positive_Description_In_OQDashbaord()
    {
        _test = extent.CreateTest("Checking for the presence of description in the page");
        Assert.AreEqual(driver.FindElement(By.ClassName(PageDescription)).Text, OQDashboard_Description_Text, "ERROR: Proper description is NOT present in OQ Dashboard page");
        _test.Log(Status.Pass, "Pass");
    }
    //[Test] //Planned
    //public void Test003_9198_Positive_Logout()
    //{
    //_test = extent.CreateTest("Verify if user is able to logout from OQ Dashboard module ");
    //Logout();
    //Assert.IsTrue()

    [Test]
    public void Test003_9204_Positive_Table()
    {
        _test = extent.CreateTest("Verify if OQ View table is displayed to the user ");
        Assert.IsTrue(driver.FindElement(By.ClassName(CompanyTable)).Displayed, "ERROR: Table is NOT Present in OQ Dashboard");
        _test.Log(Status.Pass, "Pass");
    }
    [Test]
    public void Test004_9206_Positive_Table_Headers()
    {
        _test = extent.CreateTest("Verify if OQ View table is displayed to the user ");
        Dictionary<int, string> HashMap = new Dictionary<int, string>() { { 0, "Company" }, { 1, "Assigned Qualifications" }, { 2, "Qualifications" }, { 3, "Disqualifications" }, { 4, "Expired Qualifications (30 Days)" }, { 5, "Expires in 30 Days" }, { 6, "Total Employees" } };
        WorkBookHeaderList = getList(HeaderRowElements);
        foreach (var index in HashMap)
        {
            Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Proper column headers are NOT present in Level 1");
            _test.Log(Status.Pass, "Pass");
        }
    }
    [Test]
    public void Test005_9209_Positive_SummationEqualsTotal()
    {
        _test = extent.CreateTest("Verify if sum of column value equals to the total");
    }
    [Test]
    public void Test006_9210_Positive_OpenSameWidgets()
    {
        _test = extent.CreateTest("Verify if same column is opened while expanding the company and total employees link");
        WaitForPresence(ClickableCellElement);
        // do
        //{
        //    randomIndex = Random(WorkBookRowList.Count, 1);
        // } while (WorkBookRowList[randomIndex].FindElement(By.XPath("//div[1]/div[1]/div/div/span/span")).GetAttribute("class").Length == 0);

        get("//div[1]/div[1]/div/div/span/span").Click();
        driver.SwitchTo().ActiveElement();
        var ModalTitle_when_EmployeeName_clicked = driver.FindElement(By.ClassName("modal-title")).Text;
        get("//div[1]/button").Click();
        driver.SwitchTo().DefaultContent();
        Thread.Sleep(2000);
        get("///div[1]/div[7]/div/div/span/span").Click();
        Thread.Sleep(2000);
        driver.SwitchTo().ActiveElement();
        var ModalTitle_when_TotalEmployees_clicked = driver.FindElement(By.ClassName("modal-title")).Text;
        Assert.AreEqual(ModalTitle_when_EmployeeName_clicked, ModalTitle_when_TotalEmployees_clicked, "ERROR: Same modal popup is not displayed");
        _test.Log(Status.Pass, "Pass");
    }
    [Test]
    public void Test007_9212_Positive_CompanyLInk()
    {
        _test = extent.CreateTest("Checking if appropriate widgets are opened the hyperlinks");
        Thread.Sleep(2000);
        Dictionary<string, string> HashMap = new Dictionary<string, string>() { { EmployeeName_CellClick, "My Employees" } };
        //"//*[@id='root']/div/div/div/div/main/div[2]/div/div[2]/div/div/div/div/div/div[8]/div/div/div/div/div[2]/div/div/div/div[1]/div[1]/div/div/span/span"
        foreach (var index in HashMap)
        {
            WorkBookRowList[0].FindElement(By.XPath(index.Key)).Click();
            driver.SwitchTo().ActiveElement();
            Assert.IsTrue(driver.FindElement(By.ClassName("modal-title")).Text.Contains(index.Value), "ERROR: Appropriate widgets are not opened on clicking the hyperlinks");
            _test.Log(Status.Pass, "Pass");
            WaitForInvisible(AJAXLoader_UI);
            Thread.Sleep(5000);
            //get(ModalClose_Button).Click();
            driver.SwitchTo().DefaultContent();
            _test.Log(Status.Pass, "Pass");
        }
    }
    [Test]
    public void Test008_9209_SummationEqualsToTotal()
    {
        _test = extent.CreateTest("Check if sum of Past Due counts is equal to the total");
        Thread.Sleep(2000);
        driver.SwitchTo().ActiveElement();
        for (int index = 0; index < WorkBookRowList.Count - 1; index++)
        {
            TotalWorkbooksAssigned += Convert.ToInt32(WorkBookRowList[index].FindElement(By.XPath("//div[8]/div/div/div/div/div[2]/div/div/div/div/div[2]")).GetAttribute("value"));
        }
        var Total = driver.FindElement(By.XPath("//div[8]/div/div/div/div/div[2]/div/div/div/div[2]/div[2]")).GetAttribute("value");
        Assert.AreEqual(Convert.ToInt32(Total), Convert.ToInt32(WorkBookRowList[WorkBookRowList.Count - 1].FindElement(By.XPath("//div[8]/div/div/div/div/div[2]/div/div/div/div[2]/div[2]")).GetAttribute("value")), " ERROR: Sum of Past Due Workbooks of all employees are NOT equal to the Table 'Total' value");
        _test.Log(Status.Pass, "Pass");
    }


    //10645 - Enhancements 
    [Test]
    public void Test009_11660_Positive_EmployeeNameFormat_OQDashboard()
    {
        _test = extent.CreateTest("Verify if Employee name is 2 words only in Contractor OQ page");
        Thread.Sleep(2000);
        get("//div[8]/div/div/div/div/div[2]/div/div/div/div[1]/div[2]/div/div/span/span").Click();
        string Employee_Name = WorkBookRowList[randomIndex].FindElement(By.XPath(EmployeeName_CellClick)).Text;
        string LastName = Employee_Name.Substring(0, Employee_Name.IndexOf(","));
        Employee_Name = Employee_Name.Substring(Employee_Name.LastIndexOf(","));
        string FirstName = Employee_Name.Substring(2);
        Assert.Multiple(() =>
        {
            Assert.IsTrue(!LastName.Contains(" "), "ERROR: Two names are there in Lastname");
            Assert.IsTrue(!FirstName.Contains(" "), "ERROR: Two names are there in Firstname");
        }
        );
        _test.Log(Status.Pass, "Pass");
    }
    [Test]
    public void Test010_xxx_Positive_OQDashboardToContractorOQDashboard()
    {
        _test = extent.CreateTest("xys");
        string Header_Text = get(PageHeader_Text).Text;
        MouseHover(ReportButton);
        string MenuDropdown_Text = get("//div[3]/div/div[6]/a").Text;
        Assert.Multiple(() =>
        {
            Assert.IsTrue(Header_Text.Equals("Contractor OQ Dashboard") && !Header_Text.Equals("OQ Dashboard"));
            Assert.IsTrue(MenuDropdown_Text.Equals("Contractor OQ Dashboard") && !MenuDropdown_Text.Equals("OQ Dashboard"));
        }
        );
        _test.Log(Status.Pass, "Pass");
    }
    [Test]
    public void Test011_xxx_Positive_CompanyToDemoOQContractor()
    {
        _test = extent.CreateTest("xys");
        Thread.Sleep(2000);
        string Company_Name = get("//div[1]/div[1]/div/div/span/span").Text;
        Assert.IsTrue(Company_Name.Equals("Demo OQ Contractor") && !Company_Name.Equals("Company"));
    }
}



