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

namespace LocatorsFile_Functional
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
        public const string TableRowElements = "//div[contains(@class,'react-grid-Row')]";
        public const string HeaderRowElements = "//div[contains(@class,'react-grid-HeaderCell-sortable')]";
        public const string ClickableCellElement = "//span[@class = 'text-clickable']";
        public const string AJAXLoader_UI = "//div[@class='loader-show']";
        public const string Profile_DropdownMenu = "//button[@class='topbar__avatar']";
        public const string Logout_Menuoption = "//a[@href='/logout.aspx']";
        public const string ReportButton = "//div/div/div/div/main/div[2]/div/div[1]/div[2]/div/div/nav/div[3]/a";
        public static OpenQA.Selenium.Support.UI.WebDriverWait wait;
        public static string FinalElement = "//div[8]/div/div/div/div/div[2]/div/div/div/div";
        //Employee WB
        public const string ThirdChildElement_AssignedWorkBook = "child::div[3]";
        public const string AssignedWorkBook_CellClick = ThirdChildElement_AssignedWorkBook + "/div/div/span/span";
        public const string SixthChildElement_CompletedWorkBooks = "child::div[6]";
        public const string CompletedWorkBooks_CellClick = SixthChildElement_CompletedWorkBooks + "/div/div/span/span";
        public const string CompletedTask_FirstRow = "//div[2]/div/div/div/div/div/div[2]/div/div/div/div[1]/div[3]/div/div/span/span";
        public const string Repetition_FirstRow = "/html/body/div[3]/div/div[1]/div/div/div[2]/div[2]/div/div/div/div/div[2]/div/div/div/div[1]/div[3]/div/div/span/span";

        //OQ Dashboard Reusable Strings 
        public const string PageHeader = "//div[@class='pageheader']";
        public const string OQDashboard_Button = "//div[6]/a";
        public const string PageDescription = "card__description";
        public const string CompanyTable = "react-grid-Grid";
        public const string OQTable = "//div[8]/div/div/div/div/div[2]/div/div/div";
        public const string FirstChildElement_EmployeeName = "child::div[1]";
        public const string EmployeeName_CellClick = FirstChildElement_EmployeeName + "/div/div/span/span";


        //OQ Dashboard Texts 
        public const string Page_Header_Text_OQ = "Contractor OQ Dashboard";
        public const string OQDashboard_Description_Text = "Contractor Operator Qualifications";
        public const string Company_Header_Text = "Employee View - Demo OQ Contractor";

        //Query Builder Reusable Strings 
        public const string QueryBuilder_Button = "//div/div/div/div/main/div[2]/div/div[1]/div[2]/div/div/nav/div[3]/div/div[7]/a";
        public const string FirstField_Text = "//*[@id='react-select-3--value-item']";
        public const string UserId_HeaderField = "//*[@id='queryResultSet']/div/div/div/div/div/div[1]/div/div/div[3]/div";
        public const string FirstOperatorDropdown = "//tr[1]/td[4]/div/div/span";
        public static string Operator_DropdownIcon = "//tr[1]/td[4]/div/div/span";
        public const string OperatorField_Text = "//*[@id='react-select-4--value-item']";
        public const string SecondDeleteButton = "//tr[2]/td[1]/button[2]";
        public static string QueryBuilderHeader = "//th";
        public static string FirstResultRow = "//*[@id='queryResultSet']/div/div/div/div/div/div[2]/div/div/div/div";
        public static string EntityDropdown = "//div[1]/div/div/span/span";
        public static string RunQuery_Button = "//*[@id='runQueryButton']";
        public static string UserId_ValueField = "//*[@id='id']";
        public static string Result_FirstRowFirstColumn = "//*[@id='queryResultSet']//div[1]/div/div/span/div";
        public static string FourteenthOption = "react-select-3--option-14";
        public static string FifteenthOption = "react-select-3--option-15";
        public static string SixteenthOption = "react-select-3--option-16";
        public static string FirstFieldDropdown = "react-select-3--value";
        public static string ValueField_Text = "//tr[1]/td[5]/div";
        public static string SmartParameter_Dropdown = "//tr[1]/td[5]/div/div/span[2]/span";
        public static string Field_DropdownIcon = "//tr[1]/td[3]/div/div/span/span";
        public static string Continue_RestButton = "//div[3]/div/div[1]/div/div/div[3]/button[1]";
        public static string FirstDeleteButton = "//tr[1]/td[1]/button[2]";
        public static string SmartParameterDropdown = "//tr[1]/td[5]/div/div/span[2]/span";
        public static string ThirdOption = "react-select-3--option-3";
        public static string ThirteenthOption = "react-select-3--option-13";

        //Query Builder Reusbale Texts 
        public const string OperatorDropdown_1 = "Equal To";
        public const string OperatorDropdown_2 = "Not Equal To";
        public const string OperatorDropdown_3 = "Lesser Than or Equal To";
        public const string OperatorDropdown_4 = "Greater Than or Equal To";
        public const string UserId_FieldText = "User Id";
        public const string UserId = "18";

        //Training Dashboard Reusable 
        public const string Page_Header_Text_Training = "Contractor Training Dashboard";
        public const string Training_Description_Text = "Displays contractors' training progress required by role (job function). This report displays current records for the contractor employee's task profile (if a training task is not in the employee task profile it is not counted on this report).";
        public const string Section_Header_Text = "Progress by Role";
        public const string Section_Description_Text = "Complete = Number of contractor companies that have users in a role, who have completed all the training tasks in the role complete.";

        //Training Dashboard Locators 
        public static string Section_Header = "section-info-pageheader";
        public static string Section_Description = "section-info-description";

        public void ReportingModule()
        {
            htmlReporter = new ExtentHtmlReporter(HTML_File_Path)
            {
                //AppendExisting = true
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
        public static void UpArrowKey()
        {
            new Actions(driver).SendKeys(OpenQA.Selenium.Keys.ArrowUp).Perform();
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
            System.Threading.Thread.Sleep(4000);
        }


        //Reusable methods for OQ Dashboard module 
        public void NavigaterOQDashboard()
        {
            MouseHover(ReportButton);
            get(OQDashboard_Button).Click();
        }

        //Reusable methods for Query Builder
        public void NavigateQueryBuilder()
        {
            MouseHover(ReportButton);
            get(QueryBuilder_Button).Click();
        }

        public void NavigateTrainingDashboard()
        {
            driver.Navigate().GoToUrl("https://dev.its-training.com/contractor-management/reports/training-dashboard/");
        } 

        public void GoThroughSmartParameterDropdown()
        {
            driver.FindElement(By.XPath(SmartParameter_Dropdown)).Click();
            DownArrowKey();
            PressEnterKey();
        }
        public void RunEmployeeWithResult()
        {
            Thread.Sleep(1000);
            driver.FindElement(By.Id("id")).SendKeys("18");
            Thread.Sleep(1000);
            driver.FindElement(By.XPath(SecondDeleteButton)).Click();
            driver.FindElement(By.Id("runQueryButton")).Click();
            Thread.Sleep(2000);
        }
        public void SelectWorkbookEntity()
        {
            driver.FindElement(By.XPath(EntityDropdown)).Click();
            DownArrowKey();
            PressEnterKey();
        }
        public void SelectTaskEntity()
        {
            driver.FindElement(By.XPath(EntityDropdown)).Click();
            Thread.Sleep(2000);
            DownArrowKey();
            DownArrowKey();
            PressEnterKey();
        }
        public void SelectUserId_TaskEntity()
        {
            Thread.Sleep(2000);
            get(FirstField_Text).Click();
            DownArrowKey();
            DownArrowKey();
            DownArrowKey();
            DownArrowKey();
            PressEnterKey();
        }
        public void ExpandFirstFieldDropdown()
        {
            driver.FindElement(By.Id(FirstFieldDropdown)).Click();
        }
        public void ExpandMoreOption()
        {
            Thread.Sleep(6000);
            driver.FindElement(By.ClassName("btn-as-text")).Click();
        }

    }
}
    
