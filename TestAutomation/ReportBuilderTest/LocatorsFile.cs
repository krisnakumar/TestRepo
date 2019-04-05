using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using SeleniumExtras.WaitHelpers;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;

namespace LocatorsFile
{
    public class Locators
    {
        public IList<IWebElement> WorkBookRowList;
        public static IWebDriver driver;
        public ExtentHtmlReporter htmlReporter;
        public ExtentReports extent;
        public ExtentTest _test;

        //Local Paths
        public static string HTML_File_Path = @"C:\Users\janani_jeeva\Documents\test1.html";
        public static string Extent_File_Path = @"C:\Users\janani_jeeva\source\repos\ReportBuilderTest\ReportBuilderTest\Extent-Config.xml";

        public static string url, Username, Password = null;
        public const string Login_Username_TextBox = "//input[@id='LoginControl_UserName']"; 
        public const string Login_Password_TextBox = "//input[@id='LoginControl_Password']";
        public const string Login_Submit_Button = "//*[@id='LoginControl_LoginButton']";
        public const string PageHeader_Text = "//div[contains(@class,'pageheader')]";
        public const string Element_FrontPage = "//*[@id='form1']/div[3]/div[5]/img";
        public const string TableRowElements = "//div[contains(@class,'react-grid-Row')]";
        public const string HeaderRowElements = "//div[contains(@class,'react-grid-HeaderCell-sortable')]";
        public const string ClickableCellElement = "//span[@class = 'text-clickable']";
        public const string FinalElement = "//div/div/div/div/main/div[2]/div/div[2]/div/div/div/div/div/div[7]/div/div/div/div/div[2]/div/div/div";
        public const string Assigned_Total = FinalElement + "div[10]/div[3]/div/div/span/span";
        public const string FirstChildElement_EmployeeName = "child::div[1]";
        public const string SecondChildElement_Role = "child::div[2]";
        public const string ThirdChildElement_AssignedWorkBook = "child::div[3]";
        public const string FourthChildElement_WorkBooksDue = "child::div[4]";
        public const string FifthChildElement_PastDueWorkBooks = "child::div[5]";
        public const string SixthChildElement_CompletedWorkBooks = "child::div[6]";
        public const string SeventhChildElement_TotalEmployees = "child::div[7]";
        public const string EmployeeName_CellClick = FirstChildElement_EmployeeName + "/div/div/span/span";
        public const string AssignedWorkBook_CellClick = ThirdChildElement_AssignedWorkBook + "/div/div/span/span";
        public const string WorkBooksDue_CellClick = FourthChildElement_WorkBooksDue + "/div/div/span/span";
        public const string PastDueWorkBooks_CellClick = FifthChildElement_PastDueWorkBooks + "/div/div/span/span";
        public const string CompletedWorkBooks_CellClick = SixthChildElement_CompletedWorkBooks + "/div/div/span/span";
        public const string ModalTitle_Text = "//h5[@class='modal-title']";
        public const string ModalClose_Button = "//button[@class='close']";
        public const string TotalEmployees_CellClick = SeventhChildElement_TotalEmployees + "/div/div/span/span";
        public const string AJAXLoader_UI = "//div[@class='loader-show']";
        public const string Profile_DropdownMenu = "//button[@class='topbar__avatar']";
        public const string Logout_Menuoption = "//a[@href='/logout.aspx']";
        public const string ReportButton = "//div/div/div/div/main/div[2]/div/div[1]/div[2]/div/div/nav/div[3]/a";
        public const string QueryBuilder_Button = "//div/div/div/div/main/div[2]/div/div[1]/div[2]/div/div/nav/div[3]/div/div[7]/a";
        public const string PageHeader = "//div[@class='pageheader']";
        public const string QueryBuilder_Description = "//div/div/div/div/main/div[2]/div/div[2]/div/div/div/div/div/div[1]/div";
        public const string OQDashboard_Button = "//div/div/div/div/main/div[2]/div/div[1]/div[2]/div/div/nav/div[3]/div/div[7]/a";
        public const string QueryBuilder_Description_Text = "Choose an entity from the available list. Create a query with the attributes available for the corresponding entity. Run the query to see corresponding search result.";
        public const string OQDashboard_Description_Text = "Contractor Operator Qualifications";
        public static OpenQA.Selenium.Support.UI.WebDriverWait wait;
        public static string AssignedTotal = "//div/div/div/div/main/div[2]/div/div[2]/div/div/div/div/div/div[7]/div/div/div/div/div[2]/div/div/div/div[10]/div[3]";
        public static string WorkbookDueTotal = "//div/div/div/div/main/div[2]/div/div[2]/div/div/div/div/div/div[7]/div/div/div/div/div[2]/div/div/div/div[10]/div[4]";
        public static string PastDueTotal = "//div/div/div/div/main/div[2]/div/div[2]/div/div/div/div/div/div[7]/div/div/div/div/div[2]/div/div/div/div[10]/div[5]";
        public static string completedtotal = "//div/div/div/div/main/div[2]/div/div[2]/div/div/div/div/div/div[7]/div/div/div/div/div[2]/div/div/div/div[10]/div[6]";
        public static string TotalEmployee = "//div/div/div/div/main/div[2]/div/div[2]/div/div/div/div/div/div[7]/div/div/div/div/div[2]/div/div/div/div[10]/div[7]";
        public static string Employee_Table = "//div[@class='text-left react-grid-Cell']";
        public static string Row_Select = "//div[contains(@class='react-grid-Row react-grid-Row')]";
        public static string TotalCompleted_Assigned = "/html/body/div[2]/div/div[1]/div/div/div[2]/div/div/div/div/div/div[2]/div/div/div/div[1]/div[3]/div/div";
        public static string TotalPercentage_Assigned = "/html/body/div[2]/div/div[1]/div/div/div[2]/div/div/div/div/div/div[2]/div/div/div/div[1]/div[4]/div/div";
        public const string ModalClose_Button_Level2 = "(//button[@class='close'])[2]";
        public static string ThirdColumn_FirstRow_Click = "(//div[2]/div/div/div/div[1]/div[3])";

        //QueryBuilder Reusable Strings 
        public static string FirstDeleteButton = "//tr[1]/td[1]/button[2]";
        public static string SecondDeleteButton = "//tr[2]/td[1]/button[2]";
        public static string FirstResultRow = "//*[@id='queryResultSet']/div/div/div/div/div/div[2]/div/div/div/div";
        public static string EntityDropdown = "//div[1]/div/div/span/span";
        public static string GuidanceMessagePanel = "//*[@id='queryResultSet']/div/div/div/div/div/div[2]/div";
        public static string NoResultsPanel = "no-records-found-result-set";
        public static string ExpextedMessageNoResult = "Sorry, no records";
        public static string ExpectedMessageGuidance = "Please customize and run your query in the above window. The search result(s) will be displayed here.";
        public static string QueryBuilderHeader = "//th";
        public static string ThirteenthOption = "react-select-3--option-13";
        public static string FourteenthOption = "react-select-3--option-14";
        public static string ThirdOption = "react-select-3--option-3";
        public static string SecondOption = "react-select-3--option-2";
        public static string SmartParameterDropdown = "//tr[1]/td[5]/div/div/span[2]/span";
        public static string ValueField_Text = "//tr[1]/td[5]/div";
        public static string ExportButton = "//div[2]/div/div[5]/span/button";
        public static string AddColumnButton = "//div[2]/div/div[10]/button";
        public static string OKButton_ColumnOptions = "//div[12]/button[1]";
        public static string FirstFieldDropdown = "react-select-3--value";
        public static string ColumnOptionsButton = "//div[4]/button/span[2]";
        public static string EightColumnHeader = "//div[1]/div/div/div[8]/div";
        public const string ResetQuery_btn = "//div[3]/button";
        public const string Continue_btn = "//div[3]/div/div[1]/div/div/div[3]/button[1]";

        public void ReportingModule()
        {
            htmlReporter = new ExtentHtmlReporter(HTML_File_Path)
            {
                AppendExisting = true
            };
            htmlReporter.LoadConfig(Extent_File_Path);
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }
        public void FailedReport()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = TestContext.CurrentContext.Result.StackTrace;
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
                _test.Log(Status.Fail, status + stacktrace + errorMessage);
        }
        public static void DownArrowKey()
        {
            new Actions(driver).SendKeys(OpenQA.Selenium.Keys.ArrowDown).Perform();
        }
        public static void PressEnterKey()
        {
            new Actions(driver).SendKeys(OpenQA.Selenium.Keys.Enter).Perform();
        }
        public static IWebElement get(String element)
      
        {
            return driver.FindElement(By.XPath(element));
        }

        public static IList<IWebElement> getList(String element)
        {
            return driver.FindElements(By.XPath(element));
        }

        public static void OpenChromeBrowser()
        {
            var service = ChromeDriverService.CreateDefaultService(@"C:\Users\janani_jeeva", "chromedriver.exe");

            ChromeOptions chromeOptions = new ChromeOptions();
            driver = new ChromeDriver(service, chromeOptions);
            wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(30));
            ChooseSettings("QA");
            driver.Manage().Window.Maximize();
        }

        public static void ChooseSettings(string env)
        {
            env = env.ToLower();
            if (env == "qa")
            {
                url = "https://dev.its-training.com/";
                Username = "qatester";
                Password = "QATesterDev#";
            }
            else if (env == "prod")
            {
                url = "http://its-report-builder.s3-website-us-west-2.amazonaws.com/";
                Username = "devtester@its-training.com";
                Password = "Demo@2017";
            }
        }
        public static void Login()
        {

            driver.Navigate().GoToUrl(url);
            get(Login_Username_TextBox).Clear();
            get(Login_Username_TextBox).SendKeys(Username);
            get(Login_Password_TextBox).Clear();
            get(Login_Password_TextBox).SendKeys(Password);
            Thread.Sleep(2000);
            WaitForClickable(Login_Submit_Button);
            get(Login_Submit_Button).Click();
        }

        public void Initialize()
        {
            //Login into LMS
            OpenChromeBrowser();
            ChooseSettings("QA");
            driver.Navigate().GoToUrl(url);
            Login();
        }

        public bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException e)
            {
                return false;
            }
        }

        public int Random(int maxLimit, int minLimit)
        {
            Random randomObject = new Random();
            int index = randomObject.Next(minLimit, maxLimit);
            return index;
        }

        public static void Logout()
        {
            get(Profile_DropdownMenu).Click();
            WaitForClickable(Logout_Menuoption);
            get(Logout_Menuoption).Click();
        }

        public static void WaitForPresence(String element)
        {
            wait.Until(ExpectedConditions.ElementExists(By.XPath(element)));
        }

        public static void WaitForInvisible(String element)
        {
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.XPath(element)));
        }

        public static void WaitForVisible(String element)
        {
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(element)));
        }

        public static void WaitForClickable(String element)
        {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath(element)));
        }
        //Menu XPath is the XPath of menu for which you have to perform a hover operation

        public void MouseHover(String ReportButton)
        {

            OpenQA.Selenium.Support.UI.WebDriverWait wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
            var element = wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(ReportButton)));
            Actions action = new Actions(driver);
            action.MoveToElement(element).Perform();
            //Waiting for the menu to be displayed    
            Thread.Sleep(4000);
        }

        public void NavigateQueryBuilder()
        {
            driver.Navigate().GoToUrl("https://dev.its-training.com/reports/query-builder/");
        }

        public void NavigateOQDashboard()
        {
            MouseHover(ReportButton);
            get(OQDashboard_Button).Click();
        }

       public void SelectTaskEntity()
        {
            driver.FindElement(By.XPath(EntityDropdown)).Click();
            Thread.Sleep(2000);
            DownArrowKey();
            DownArrowKey();
            PressEnterKey();
        }

        public void SelectWorkbookEntity()
        {
            driver.FindElement(By.XPath(EntityDropdown)).Click();
            DownArrowKey();
            PressEnterKey();
        }

        public void PlaceholderForDateEmployee()
        {
        Thread.Sleep(1000);
        driver.FindElement(By.Id(FirstFieldDropdown)).Click();
        driver.FindElement(By.Id(ThirdOption)).Click();
        }

        public void RunEmployeeWithResult()
        {
            Thread.Sleep(1000);
            driver.FindElement(By.Id("id")).SendKeys("335216");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath(SecondDeleteButton)).Click();
            driver.FindElement(By.Id("runQueryButton")).Click();
            Thread.Sleep(2000);
        }

        public void OpenColumnOptions()
        {
            Thread.Sleep(1000);
            get(ColumnOptionsButton).Click();
        }

        public void ExpandFirstFieldDropdown()
        {
            driver.FindElement(By.Id(FirstFieldDropdown)).Click();
        }

        public void GoThroughSmartParameterDropdown()
        {
            driver.FindElement(By.XPath(SmartParameterDropdown)).Click();
            DownArrowKey();
            PressEnterKey();
        }
        }
    }
