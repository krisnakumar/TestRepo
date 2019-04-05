using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using LocatorsFile_Functional;
using AventStack.ExtentReports;

[TestFixture]
public class WorkbookDashboard_FunctionalTest : Locators
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
    //10645
    [Test]
    public void Test001_11657_Positive_EmployeeNameFormat_WBDashboard()
    {
        _test = extent.CreateTest("Verify if Employee name is 2 words only in Workbook Dashboard page");
        randomIndex = Random(WorkBookRowList.Count, 1);
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
    public void Test002_11694_Positive_WorkbookNameToWorkbook_AssignedWB()
    {
        _test = extent.CreateTest("Verify if Workbook name is changed to 'Workbook' in assigned workbook");
        randomIndex = Random(WorkBookRowList.Count, 1);
        do
        {
            randomIndex = Random(WorkBookRowList.Count, 1);
        } while (WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).GetAttribute("class").Length == 0);
        WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).Click();
        string Word_NotToBePresent = get("//div[text()='Workbook']").Text;
        Assert.IsTrue(!Word_NotToBePresent.Contains("Workbook Name"));
        _test.Log(Status.Pass, "Pass");
    }
    [Test]
    public void Test003_11715_Positive_WorkbookNameToWorkbook_CompletedWB()
    {
        _test = extent.CreateTest("Verify if Workbook name is changed to 'Workbook' in Completed workbook");
        randomIndex = Random(WorkBookRowList.Count, 1);
        do
        {
            randomIndex = Random(WorkBookRowList.Count, 1);
        } while (WorkBookRowList[randomIndex].FindElement(By.XPath(CompletedWorkBooks_CellClick)).GetAttribute("class").Length == 0);
        WorkBookRowList[randomIndex].FindElement(By.XPath(CompletedWorkBooks_CellClick)).Click();
        string Word_NotToBePresent = get("//div[text()='Workbook']").Text;
        Assert.IsTrue(!Word_NotToBePresent.Contains("Workbook Name"));
        _test.Log(Status.Pass, "Pass");
    }
    [Test]
    public void Test004_xxx_Positive_EvaluatorToSubmittedBy_WBRepetition()
    {
        _test = extent.CreateTest("xys");
        randomIndex = Random(WorkBookRowList.Count, 1);
        do
        {
            randomIndex = Random(WorkBookRowList.Count, 1);
        } while (WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).GetAttribute("class").Length == 0);
        WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).Click();
        Thread.Sleep(2000);
        get(CompletedTask_FirstRow).Click();
        Thread.Sleep(3000);
        get(Repetition_FirstRow).Click();
        string Word_NotToBePresent = get("//div[text()='Submitted By']").Text;
        Assert.IsTrue(!Word_NotToBePresent.Contains("Evaluator"));
        _test.Log(Status.Pass, "Pass");
    }
    [Test]
    public void Test005_xxx_Positive_EmployeeReportsToWorkbookDashboard()
    {
        _test = extent.CreateTest("xys");
        string Header_Text = get(PageHeader_Text).Text;

        MouseHover(ReportButton);
        string MenuDropdown_Text = get("//div[3]/div/div[5]/a").Text;

        Assert.Multiple(() =>
        {
            Assert.IsTrue(Header_Text.Equals("Workbook Dashboard") && !Header_Text.Equals("Employee Reports"));
            Assert.IsTrue(MenuDropdown_Text.Equals("Workbook Dashboard") && !MenuDropdown_Text.Equals("Employee Reports"));
        }
        );
    }
    [Test]
    public void Test006_xxx_Positive_CompletionDate_Format()
    {
        _test = extent.CreateTest("xys");
        randomIndex = Random(WorkBookRowList.Count, 1);
        do
        {
            randomIndex = Random(WorkBookRowList.Count, 1);
        } while (WorkBookRowList[randomIndex].FindElement(By.XPath(CompletedWorkBooks_CellClick)).GetAttribute("class").Length == 0);
        WorkBookRowList[randomIndex].FindElement(By.XPath(CompletedWorkBooks_CellClick)).Click();
        string Completion_Date = get("//div[2]/div/div/div/div[1]/div[4]/div/div/span/span").Text;
        Assert.IsTrue(!Completion_Date.Contains(":"));
    }
    [Test]
    public void Test007_xxx_Positive_DueDate_Format()
    {
        _test = extent.CreateTest("xys");
        randomIndex = Random(WorkBookRowList.Count, 1);
        do
        {
            randomIndex = Random(WorkBookRowList.Count, 1);
        } while (WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).GetAttribute("class").Length == 0);
        WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).Click();
        Thread.Sleep(2000);
        string Completion_Date = get("//div[2]/div/div/div/div[1]/div[4]/div/div/span/span").Text;
        Assert.IsTrue(!Completion_Date.Contains(":"));
    }
}
    
