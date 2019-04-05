using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using LocatorsFile;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;

namespace ReportBuilderTest_SmokeTest
{
    [TestFixture]
    public class EmployeeWorkbook : Locators
    {
        private int TotalWorkbooksAssigned, randomIndex = 0;
        protected ExtentHtmlReporter htmlReporter;
        protected ExtentReports extent;
        protected ExtentTest _test;
        new IList<IWebElement> WorkBookRowList;
        IList<IWebElement> WorkBookHeaderList;

        [OneTimeSetUp]
        //Method to extend the reporting module 
        protected void OneTimeSetup()
        {
            htmlReporter = new ExtentHtmlReporter(HTML_File_Path)
            {
                AppendExisting = true
            };
            htmlReporter.LoadConfig(Extent_File_Path);
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
        [TearDown]
        protected void OneTimeTearDown()
        {
            extent.Flush();
            //Logout();
            driver.Close();
        }
        [TearDown]
        public void DoTearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = TestContext.CurrentContext.Result.StackTrace;
            var errorMessage = TestContext.CurrentContext.Result.Message;

            if (status == TestStatus.Failed)
                _test.Log(Status.Fail, status + stacktrace + errorMessage);
        }
        [Test]
        public void Test001_Positive_NavigateToEmployeeWorkbook()
        {
            _test = extent.CreateTest("Checking if user is taken to Employee Workbook page");
            driver.Navigate().GoToUrl(url);
            WaitForPresence(ClickableCellElement);
            Assert.IsTrue(get(PageHeader).Text.Equals("Workbook Dashboard"), "ERROR: User is not navigated to Employee Workbook page");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test002_Positive_OpenWidgets()
        {
            _test = extent.CreateTest("Checking if appropriate widgets are opened the hyperlinks");
            Dictionary<string, string> HashMap = new Dictionary<string, string>() { { EmployeeName_CellClick, "My Employees" }, { AssignedWorkBook_CellClick, "Assigned Workbook" }, { PastDueWorkBooks_CellClick, "Past Due WorkBooks" }, { CompletedWorkBooks_CellClick, "Workbook Completed" } };
            foreach (var index in HashMap)
            {
                WorkBookRowList[0].FindElement(By.XPath(index.Key)).Click();
                driver.SwitchTo().ActiveElement();
                Assert.IsTrue(get(ModalTitle_Text).Text.Contains(index.Value), "ERROR: Appropriate widgets are not opened on clicking the hyperlinks");
                _test.Log(Status.Pass, "Pass");
                WaitForInvisible(AJAXLoader_UI);
                Thread.Sleep(5000);
                get(ModalClose_Button).Click();
                driver.SwitchTo().DefaultContent();
                Thread.Sleep(5000);
            }
        }

    [Test]
        public void Test003_Positive_ZeroNotHyperlinked()
        {
            _test = extent.CreateTest("Verify if 0 count is not hyperlinked and does not open any widget");
            foreach (IWebElement tempElement in WorkBookRowList)
            {
                if (WorkBookRowList[randomIndex].FindElement(By.XPath(TotalEmployees_CellClick)).GetAttribute("class").Length == 0)
                {
                    Assert.AreEqual(WorkBookRowList[randomIndex].FindElement(By.XPath(EmployeeName_CellClick)).GetAttribute("class").Length, 0, "ERROR: 0 is also hyperlinked");
                    _test.Log(Status.Pass, "Pass");
                }
                else
                    continue;
            }
        }
        [Test]
        public void Test004_Positive_OpenMyEmployee()
        {
            _test = extent.CreateTest("Checking if same widgets are opened on clicking employee name and employee count links");

            WaitForPresence(ClickableCellElement);
            do
            {
                randomIndex = Random(WorkBookRowList.Count, 1);
            } while (WorkBookRowList[randomIndex].FindElement(By.XPath(EmployeeName_CellClick)).GetAttribute("class").Length == 0);

            WorkBookRowList[randomIndex].FindElement(By.XPath(EmployeeName_CellClick)).Click();
            driver.SwitchTo().ActiveElement();
            var ModalTitle_when_EmployeeName_clicked = get(ModalTitle_Text).Text;
            get(ModalClose_Button).Click();
            driver.SwitchTo().DefaultContent();
            WorkBookRowList[randomIndex].FindElement(By.XPath(TotalEmployees_CellClick)).Click();
            Thread.Sleep(2000);
            driver.SwitchTo().ActiveElement();
            var ModalTitle_when_TotalEmployees_clicked = get(ModalTitle_Text).Text;
            Assert.AreEqual(ModalTitle_when_EmployeeName_clicked, ModalTitle_when_TotalEmployees_clicked, "ERROR: Same modal popup is not displayed");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test005_Positive_SumOfAssignedWB()
        {
            _test = extent.CreateTest("Check if sum of the assigned workbook count is equal to the total");
            WaitForPresence(ClickableCellElement);
            for (int index = 0; index < WorkBookRowList.Count - 1; index++)
            {
                TotalWorkbooksAssigned += Convert.ToInt32(WorkBookRowList[index].FindElement(By.XPath(ThirdChildElement_AssignedWorkBook)).GetAttribute("value"));
            }
            var Total = driver.FindElement(By.XPath(AssignedTotal)).GetAttribute("value");
            Assert.AreEqual(Convert.ToInt32(Total), Convert.ToInt32(WorkBookRowList[WorkBookRowList.Count - 1].FindElement(By.XPath(ThirdChildElement_AssignedWorkBook)).GetAttribute("value")), " ERROR: Sum of Assigned Workbooks of all employees are NOT equal to the Table 'Total' value");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test006_Positive_SumOfWBDue()
        {
            _test = extent.CreateTest("Check if sum of Workbook due count is equal to the total");
            WaitForPresence(ClickableCellElement);
            for (int index = 0; index < WorkBookRowList.Count - 1; index++)
            {
                TotalWorkbooksAssigned += Convert.ToInt32(WorkBookRowList[index].FindElement(By.XPath(FourthChildElement_WorkBooksDue)).GetAttribute("value"));
            }
            var Total = get((WorkbookDueTotal)).GetAttribute("value");
            Assert.AreEqual(Convert.ToInt32(Total), Convert.ToInt32(WorkBookRowList[WorkBookRowList.Count - 1].FindElement(By.XPath(FourthChildElement_WorkBooksDue)).GetAttribute("value")), " ERROR: Sum of Workbook due of all employees are NOT equal to the Table 'Total' value");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test007_Positive_SumOfPastDueWB()
        //Verify if sum of the count for all Past Due Workbook is equal to the total
        {
            _test = extent.CreateTest("Check if sum of Past Due counts is equal to the total");
            for (int index = 0; index < WorkBookRowList.Count - 1; index++)
            {
                TotalWorkbooksAssigned += Convert.ToInt32(WorkBookRowList[index].FindElement(By.XPath(FifthChildElement_PastDueWorkBooks)).GetAttribute("value"));
            }
            var Total = driver.FindElement(By.XPath(PastDueTotal)).GetAttribute("value");
            Assert.AreEqual(Convert.ToInt32(Total), Convert.ToInt32(WorkBookRowList[WorkBookRowList.Count - 1].FindElement(By.XPath(FifthChildElement_PastDueWorkBooks)).GetAttribute("value")), " ERROR: Sum of Past Due Workbooks of all employees are NOT equal to the Table 'Total' value");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test008_Positive_SumOfCompletedWB()
        //Verify if sum of the count for Completed Workbook is equal to the total
        {
            _test = extent.CreateTest("Check if sum of Completed workbooks counts is equal to the total");
            WaitForPresence(ClickableCellElement);
            for (int index = 0; index < WorkBookRowList.Count - 1; index++)
            {
                WaitForPresence(ClickableCellElement);
                TotalWorkbooksAssigned += Convert.ToInt32(WorkBookRowList[index].FindElement(By.XPath(SixthChildElement_CompletedWorkBooks)).GetAttribute("value"));
            }
            var Total = driver.FindElement(By.XPath(completedtotal)).GetAttribute("value");
            Assert.AreEqual(Convert.ToInt32(Total), Convert.ToInt32(WorkBookRowList[WorkBookRowList.Count - 1].FindElement(By.XPath(SixthChildElement_CompletedWorkBooks)).GetAttribute("value")), " ERROR: Sum of Completed Workbooks of all employees are NOT equal to the Table 'Total' value");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test009_Positive_SumOfTotalEmployee()
        //Verify if sum of the count for number of employees is equal to the total
        {
            _test = extent.CreateTest("Check if sum of number of employees counts is equal to the total");
            WaitForPresence(ClickableCellElement);
            for (int index = 0; index < WorkBookRowList.Count - 1; index++)
            {
                WaitForPresence(ClickableCellElement);
                TotalWorkbooksAssigned += Convert.ToInt32(WorkBookRowList[index].FindElement(By.XPath(SeventhChildElement_TotalEmployees)).GetAttribute("value"));
            }
            var Total = driver.FindElement(By.XPath(TotalEmployee)).GetAttribute("value");
            Assert.AreEqual(Convert.ToInt32(Total), Convert.ToInt32(WorkBookRowList[WorkBookRowList.Count - 1].FindElement(By.XPath(SeventhChildElement_TotalEmployees)).GetAttribute("value")), " ERROR: Sum of number of employees count is NOT equal to the Table 'Total' value");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test010_Positive_EmployeeWorkbookColumnHeaders()
        {
            _test = extent.CreateTest("Checking if proper columns are present in Employee Workbook");
            Dictionary<int, string> HashMap = new Dictionary<int, string>() { { 0, "Employee" }, { 1, "Role" }, { 2, "Assigned Workbooks" }, { 3, "Workbooks Due" }, { 4, "Past Due Workbooks" }, { 5, "Completed Workbooks" }, { 6, "Total Employees" } };
            WorkBookHeaderList = getList(HeaderRowElements);
            foreach (var index in HashMap)
            {
                Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Proper column headers are NOT present in Level 1");
                _test.Log(Status.Pass, "Pass");
            }
        }
        [Test]
        public void Test011_Positive_EmployeeWidgetColumnHeaders()
        {
            _test = extent.CreateTest("Checking if proper columns are present in Employee widget");
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
        [Test]
        public void Test012_Positive_AssignedWorkBookColumnHeaders()
        { 
            _test = extent.CreateTest("Checking if proper columns are present in Employee widget");
            WaitForPresence(ClickableCellElement);
            {
                do
                {
                    randomIndex = Random(WorkBookRowList.Count, 1);
                } while (WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).GetAttribute("class").Length == 0);
                WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).Click();
                Thread.Sleep(2000);
                driver.SwitchTo().ActiveElement();
                Dictionary<int, String> HashMap = new Dictionary<int, string>() { { 7, "Employee" }, { 8, "Workbook" }, { 9, "Completed / Total Tasks" }, { 10, "Percentage Completed" }, { 11, "Due Date" } };
                WorkBookHeaderList = getList(HeaderRowElements);
                foreach (var index in HashMap)
                {
                    Console.WriteLine(WorkBookHeaderList[index.Key].Text);
                    Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Appropriate columns are NOT present in Assigned Workbook Widget");
                    _test.Log(Status.Pass, "Pass");
                }
            }
        }
        [Test]
        public void Test013_Positive_WorkBookDueColumnHeaders()
        {
            _test = extent.CreateTest("Checking if proper columns are present in Employee widget");
            WaitForPresence(ClickableCellElement);
            {
                do
                {
                    randomIndex = Random(WorkBookRowList.Count, 1);
                } while (WorkBookRowList[randomIndex].FindElement(By.XPath(WorkBooksDue_CellClick)).GetAttribute("class").Length == 0);

                WorkBookRowList[randomIndex].FindElement(By.XPath(WorkBooksDue_CellClick)).Click();
                Thread.Sleep(2000);
                driver.SwitchTo().ActiveElement();
                Dictionary<int, String> HashMap = new Dictionary<int, string>() { { 7, "Employee" }, { 8, "Role" }, { 9, "Workbook Name" }, { 10, "Percentage Completed" }, { 11, "Due Date" } };
                WorkBookHeaderList = getList(HeaderRowElements);
                foreach (var index in HashMap)
                {
                    Console.WriteLine(WorkBookHeaderList[index.Key].Text);
                    Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value),"ERROR: Proper columns are NOT displated in Workbook Due widget");
                    _test.Log(Status.Pass, "Pass");
                }
            }
        }
        [Test]
        public void Test014_Positive_PastDueWorkbookColumnHeaders()
        { 
            _test = extent.CreateTest("Checking if proper columns are present in Employee widget");
            WaitForPresence(ClickableCellElement);
            {
                do
                {
                    randomIndex = Random(WorkBookRowList.Count, 1);
                } while (WorkBookRowList[randomIndex].FindElement(By.XPath(PastDueWorkBooks_CellClick)).GetAttribute("class").Length == 0);
                WorkBookRowList[randomIndex].FindElement(By.XPath(PastDueWorkBooks_CellClick)).Click();
                Thread.Sleep(2000);
                driver.SwitchTo().ActiveElement();
                Dictionary<int, String> HashMap = new Dictionary<int, string>() { { 7, "Employee" }, { 8, "Role" }, { 9, "Workbook" }, { 10, "Percentage Completed" }, { 11, "Due Date" } };
                WorkBookHeaderList = getList(HeaderRowElements);
                foreach (var index in HashMap)
                {
                    Console.WriteLine(WorkBookHeaderList[index.Key].Text);
                    Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value),"ERROR: Proper columns are NOT displayed in Past Due Workbook widgets");
                    _test.Log(Status.Pass, "Pass");
                }
            }
        }
        [Test]
        public void Test015_Positive_CompletedWorkbookColumnHeaders()
        {
            _test = extent.CreateTest("Checking if proper columns are present in Employee widget");
            WaitForPresence(ClickableCellElement);
            {
                do
                {
                    randomIndex = Random(WorkBookRowList.Count, 1);
                } while (WorkBookRowList[randomIndex].FindElement(By.XPath(CompletedWorkBooks_CellClick)).GetAttribute("class").Length == 0);
                WorkBookRowList[randomIndex].FindElement(By.XPath(CompletedWorkBooks_CellClick)).Click();
                Thread.Sleep(2000);
                driver.SwitchTo().ActiveElement();
                Dictionary<int, String> HashMap = new Dictionary<int, string>() { { 7, "Employee" }, { 8, "Role" }, { 9, "Workbook" }, { 10, "Completion Date" } };
                WorkBookHeaderList = getList(HeaderRowElements);
                foreach (var index in HashMap)
                {
                    Console.WriteLine(WorkBookHeaderList[index.Key].Text);
                    Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value),"ERROR: Proper columns are NOT displayed in Completed Workbook widget");
                    _test.Log(Status.Pass, "Pass");
                }
            }
        }
        [Test]
        public void Test016_Positive_TotalEmployeesColumnHeaders()
        {
            _test = extent.CreateTest("Checking if proper columns are present in Employee widget opened by clicking the total employees ");
            WaitForPresence(ClickableCellElement);
            {
                do
                {
                    randomIndex = Random(WorkBookRowList.Count, 1);
                } while (WorkBookRowList[randomIndex].FindElement(By.XPath(TotalEmployees_CellClick)).GetAttribute("class").Length == 0);
                WorkBookRowList[randomIndex].FindElement(By.XPath(TotalEmployees_CellClick)).Click();
                Thread.Sleep(2000);
                driver.SwitchTo().ActiveElement();
                Dictionary<int, String> HashMap = new Dictionary<int, string>() { { 7, "Employee" }, { 8, "Role" }, { 9, "Assigned Workbooks" }, { 10, "Workbooks Due" }, { 11, "Past Due Workbooks" }, { 12, "Completed Workbooks" }, { 13, "Total Employees" } };
                WorkBookHeaderList = getList(HeaderRowElements);
                foreach (var index in HashMap)
                {
                    Console.WriteLine(WorkBookHeaderList[index.Key].Text);
                    Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Proper column headers are NOT displayed in Employee widget opened by clicking Total employee count");
                    _test.Log(Status.Pass, "Pass");
                }
            }
        }
        [Test]
        public void Test017_Positive_SupervisorNameInEmployeeWidget()
        {
            _test = extent.CreateTest("Checking for the presence of appripriate supervisor name in the employee widget");
            WaitForPresence(ClickableCellElement);
            do
            {
                randomIndex = Random(WorkBookRowList.Count, 1);
            } while (WorkBookRowList[randomIndex].FindElement(By.XPath(EmployeeName_CellClick)).GetAttribute("class").Length == 0);
            var Employee_Name = WorkBookRowList[randomIndex].FindElement(By.XPath(EmployeeName_CellClick)).Text;
            WorkBookRowList[randomIndex].FindElement(By.XPath(EmployeeName_CellClick)).Click();
            driver.SwitchTo().ActiveElement();
            Assert.IsTrue(get(ModalTitle_Text).Text.Contains(Employee_Name), "ERROR: Supervisor name is NOT displayed in Employee Widget");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test018_Positive_SortingLevel1()
        {
            _test = extent.CreateTest("Checking if sorting is working in Level 1 widgets");
            WaitForPresence(ClickableCellElement);
            IList<IWebElement> EmployeeName_InTable = getList(FinalElement+"/div");
            List<string> sortedElement_Text = new List<string>();
            for (int i = 1; i < EmployeeName_InTable.Count; i++)
            {
                sortedElement_Text.Add(driver.FindElement(By.XPath(FinalElement+"/div[" + i + "]/div[1]")).Text);
            }
            sortedElement_Text.Sort();
            get("(//div[@class='react-grid-HeaderCell-sortable'])[1]").Click();
            List<string> ElementsAfterSorting_Text = new List<string>();
            for (int i = 1; i < EmployeeName_InTable.Count; i++)
            {
                ElementsAfterSorting_Text.Add(driver.FindElement(By.XPath(FinalElement+"/div[" + i + "]/div[1]")).Text);     
            }
            Assert.AreEqual(sortedElement_Text,ElementsAfterSorting_Text);
        }
        [Test]
        public void Test019_Positive_SortingAssignedWorkbook()
        {
            _test = extent.CreateTest("Checking if sorting is working Assigned Workbook widget");
            WaitForPresence(ClickableCellElement);

            IList<IWebElement> EmployeeName_InTable = getList(FinalElement + "/div");
            List<string> sortedElement_Text = new List<string>();
            for (int i = 1; i < EmployeeName_InTable.Count; i++)
            {
                sortedElement_Text.Add(driver.FindElement(By.XPath(FinalElement + "/div[" + i + "]/div[1]")).Text);
            }
            sortedElement_Text.Sort();
            get("(//div[@class='react-grid-HeaderCell-sortable'])[1]").Click();
            List<string> ElementsAfterSorting_Text = new List<string>();
            for (int i = 1; i < EmployeeName_InTable.Count; i++)
            {
                ElementsAfterSorting_Text.Add(driver.FindElement(By.XPath(FinalElement + "/div[" + i + "]/div[1]")).Text);
            }
            Assert.AreEqual(sortedElement_Text, ElementsAfterSorting_Text);
        }

        [Test]
        public void Test20_Positive_OpenSameTotalWidget()
        {
            _test = extent.CreateTest("Checking if same widgets are opening on clickig Total/completed and completed percentage links");
            WaitForPresence(ClickableCellElement);
            do
            {
              randomIndex = Random(WorkBookRowList.Count, 1);
            } while (WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).GetAttribute("class").Length == 0);
            WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).Click();
            Thread.Sleep(2000);
            driver.SwitchTo().ActiveElement();
            get(TotalCompleted_Assigned).Click();
            Thread.Sleep(2000);
            var ModalTitel_When_CompletedTotal_Clicked = get("(//h5[@class='modal-title'])[2]").Text;
            get(ModalClose_Button_Level2).Click();
            get(TotalPercentage_Assigned).Click();
            Thread.Sleep(2000);
            var ModalTitel_When_Percentage_Clicked = get("(//h5[@class='modal-title'])[2]").Text;
            Assert.AreEqual(ModalTitel_When_CompletedTotal_Clicked, ModalTitel_When_Percentage_Clicked, "ERROR: Same modal popup is NOT displayed");
            _test.Log(Status.Pass, "Pass");
        }
        [Test]
        public void Test21_Positive_TotalTaskCompletedPercentageColumns()
        {
            _test = extent.CreateTest("Checking if proper columns are present in Total tasks and completed percentage widget");
            WaitForPresence(ClickableCellElement);
            do
            {
                randomIndex = Random(WorkBookRowList.Count, 1);
            }
            while (WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).GetAttribute("class").Length == 0);
            WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).Click();
            Thread.Sleep(2000);
            driver.SwitchTo().ActiveElement();
            get(TotalCompleted_Assigned).Click();
            Thread.Sleep(2000);
            Dictionary<int, string> HashMap = new Dictionary<int, string>() { { 12, "Task Code" }, { 13, "Task Name" }, { 14, "Completed / Total Repetitions" }, { 15, "Incomplete Repetitions" }, { 16, "Percentage Completed" } };
            WorkBookHeaderList = getList(HeaderRowElements);
            foreach (var index in HashMap)
            {
                Console.WriteLine(WorkBookHeaderList[index.Key].Text);
                Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Proper column headers are NOT present in Level 1");
                _test.Log(Status.Pass, "Pass");
            }
        }
        [Test]
        public void Test22_Positive_WorkbookRepetitionColumns()
        {
            _test = extent.CreateTest("Checking if proper columns are present in workbook repetition widget");
            WaitForPresence(ClickableCellElement);
            do
            {
                randomIndex = Random(WorkBookRowList.Count, 1);
            }
            while (WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).GetAttribute("class").Length == 0);
            WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).Click();
            Thread.Sleep(2000);
            driver.SwitchTo().ActiveElement();
            get(TotalCompleted_Assigned).Click();
            Thread.Sleep(2000);
            get(ThirdColumn_FirstRow_Click + "[3]").Click();
            Dictionary<int, string> HashMap = new Dictionary<int, string>() { { 17, "Attempt(s)" }, { 18, "Complete/Incomplete" }, { 19, "Last Attempted Date" }, { 20, "Location" }, { 21, "Submitted By" }, { 22, "Comments" } };
            WorkBookHeaderList = getList(HeaderRowElements);
            foreach (var index in HashMap)
            {
                Console.WriteLine(WorkBookHeaderList[index.Key].Text);
                Assert.IsTrue(WorkBookHeaderList[index.Key].Text.Equals(index.Value), "ERROR: Proper column headers are NOT present in Level 1");
                _test.Log(Status.Pass, "Pass");
            }
        }
        [Test]
        public void Test23_Positive_TotalTaskDescription()
        //Verify if proper details are displayed in Total Tasks and completed percentage widget  
        {
            _test = extent.CreateTest("Checking if proper details are displayed in Total Tasks and completed percentage widget");
            WaitForPresence(ClickableCellElement);

            do
            {
                randomIndex = Random(WorkBookRowList.Count, 1);
            } while (WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).GetAttribute("class").Length == 0);
            WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).Click();
            Thread.Sleep(2000);
            driver.SwitchTo().ActiveElement();
            var EmployeeName_AssignedWB = driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div/div/div[2]/div/div/div/div/div/div[2]/div/div/div/div/div[1]")).Text;
            Console.WriteLine(EmployeeName_AssignedWB);
            var WBName_AssignedWB = driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div/div/div[2]/div/div/div/div/div/div[2]/div/div/div/div[1]/div[2]/div/div/span/span")).Text;
            ///html/body/div[2]/div/div[1]/div/div/div[2]/div/div/div/div/div/div[2]/div/div/div/div[1]/div[2]/div/div/span/span
            Console.WriteLine(WBName_AssignedWB);

            //click on the first element under Total/Completed header
            driver.FindElement(By.XPath(TotalCompleted_Assigned)).Click();
            Thread.Sleep(2000);
            var TotalTask_Description = driver.FindElement(By.XPath("/html/body/div[3]/div/div[1]/div/div/div[2]/div[1]")).Text;
            Console.WriteLine(TotalTask_Description);
            Assert.IsTrue(TotalTask_Description.Contains(EmployeeName_AssignedWB));
            Assert.IsTrue(TotalTask_Description.Contains(WBName_AssignedWB));

        }
        [Test]
        public void Test24_Positive_WorkBookRepetitionDescription()
        //Verify if proper details are displayed in Total Tasks and completed percentage widget  
        {
            _test = extent.CreateTest("Checking if proper details are displayed in Total Tasks and completed percentage widget");
            WaitForPresence(ClickableCellElement);

            do
            {
                randomIndex = Random(WorkBookRowList.Count, 1);
            } while (WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).GetAttribute("class").Length == 0);
            WorkBookRowList[randomIndex].FindElement(By.XPath(AssignedWorkBook_CellClick)).Click();
            Thread.Sleep(2000);
            driver.SwitchTo().ActiveElement();
            var EmployeeName_AssignedWB = driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div/div/div[2]/div/div/div/div/div/div[2]/div/div/div/div/div[1]")).Text;
            Console.WriteLine(EmployeeName_AssignedWB);
            var WBName_AssignedWB = driver.FindElement(By.XPath("/html/body/div[2]/div/div[1]/div/div/div[2]/div/div/div/div/div/div[2]/div/div/div/div[1]/div[2]/div/div/span/span")).Text;
            ///html/body/div[2]/div/div[1]/div/div/div[2]/div/div/div/div/div/div[2]/div/div/div/div[1]/div[2]/div/div/span/span
            //Console.WriteLine(WBName_AssignedWB);
            driver.FindElement(By.XPath(TotalCompleted_Assigned)).Click();
            Thread.Sleep(2000);
            var TaskCode = driver.FindElement(By.XPath("/html/body/div[3]/div/div[1]/div/div/div[2]/div[2]/div/div/div/div/div[2]/div/div/div/div[1]/div[1]")).Text;
            //Console.WriteLine(TaskCode);
            var TaskName = driver.FindElement(By.XPath("/html/body/div[3]/div/div[1]/div/div/div[2]/div[2]/div/div/div/div/div[2]/div/div/div/div[1]/div[2]/div/div/span/span")).Text;
            //Console.WriteLine(TaskName);
            driver.FindElement(By.XPath("/html/body/div[3]/div/div[1]/div/div/div[2]/div[2]/div/div/div/div/div[2]/div/div/div/div[1]/div[3]/div/div/span/span")).Click();
            var WorkbookRepetition_Description = driver.FindElement(By.XPath("(//div[@class='grid-description'])[2]")).Text;
            Console.WriteLine(WorkbookRepetition_Description); 
            Assert.IsTrue(WorkbookRepetition_Description.Contains(EmployeeName_AssignedWB));
            Assert.IsTrue(WorkbookRepetition_Description.Contains(WBName_AssignedWB));
            Assert.IsTrue(WorkbookRepetition_Description.Contains(TaskCode));
            Assert.IsTrue(WorkbookRepetition_Description.Contains(TaskName));
        }
    }
}



