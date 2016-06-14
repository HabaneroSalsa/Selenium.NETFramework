using NUnit.Framework;
using OnlineStore.PageObjects;
using OnlineStore.TestDataAccess;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RelevantCodes.ExtentReports;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Threading;
using OnlineStore.TextLogging;
using System.Globalization;

namespace OnlineStore.TestCases
{
    class ShoppingTest
    {   
        
        static System.Collections.Specialized.NameValueCollection appSettings = ConfigurationManager.AppSettings;
        // Static log filename
        string LogFile = System.IO.Path.Combine(appSettings["LogDirectory"] == null ? Environment.CurrentDirectory : appSettings["LogDirectory"],
            appSettings["LogPrefix"] == null ? "NUnitTest" : appSettings["LogPrefix"] + ".log");
        // Dynamic log filename
        //string LogFile = System.IO.Path.Combine(appSettings["LogDirectory"] == null ? Environment.CurrentDirectory : appSettings["LogDirectory"],
        //    appSettings["LogPrefix"] == null ? "NUnitTest" : appSettings["LogPrefix"] + string.Format("_{0:yyyyMMddHHmmss}.log", DateTime.Now));
        string LogFolder = System.IO.Path.Combine(appSettings["LogDirectory"] == null ? Environment.CurrentDirectory : appSettings["LogDirectory"])+"\\";

        // Logging definition start
        public void LogQAData(string str)
        {
            LogQAData(str, null);
        }

        public void LogQAData(string str, params object[] para)
        {
            string str2 = string.Format("[{0:yyyy-MM-dd HH:mm:ss.ffff}] ", DateTime.Now) + str;
            Console.WriteLine(str2, para);
            if (para != null)
            {
                Debug.WriteLine(str2, para);
                try
                {
                    System.IO.File.AppendAllText(LogFile, string.Format(str2, para) + Environment.NewLine);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
            else
            {
                Debug.WriteLine(str2);
                try
                {
                    System.IO.File.AppendAllText(LogFile, str2 + Environment.NewLine);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
        // Logging definition end


        [Test]
        public void AddItemsToCart()
        {
            string assertText = " ";
        // Report engine setup
            string HTMLLogFile = LogFile  + ".html";
            var extent = new ExtentReports(HTMLLogFile, false, DisplayOrder.OldestFirst);
            extent.AddSystemInfo("Selenium Version", "2.53.0");
            extent.AddSystemInfo("NUnit Version", "3.2.0");
            extent.AddSystemInfo("Dapper Version", "1.4.2");
            extent.AddSystemInfo("Environment", "Local");
            extent.AddSystemInfo("Browser", "Chrome");

            var testSuite = extent.StartTest("Shopping Cart Test Suite", "<b>"  + string.Format("[{0:yyyy-MM-dd HH:mm:ss.ffff}] ", DateTime.Now) + "Suite Objective:</b><br/>Log into the system with credentials pulled from Excel.<br/>Add 5 items to the shopping cart where the item names, URLs and locators are pulled from Excel.<br/>Validate pageloads and interactions.");
            string currentURL = "";

            testSuite.AssignCategory("Functional", "Regression", "Training");
            testSuite.AssignAuthor("Rick Johnson");
            testSuite.Log(LogStatus.Info, "Log file location:<br/>" + HTMLLogFile);

            LogQAData("Log file location: {0}",HTMLLogFile);

            IWebDriver driver = new ChromeDriver();// FirefoxDriver();
            driver.Url = driver.Url = ConfigurationManager.AppSettings["URL"];
            
            // Validate page load : caseLoadHomePage START
            var homePage = new HomePage(driver);
            var caseLoadHomePage = extent.StartTest("Load Home Page");
            caseLoadHomePage.Log(LogStatus.Info, "Home Page load load validation");
            currentURL = driver.Url;
            assertText = "ASSERT: Expected URL: " + ConfigurationManager.AppSettings["URL"] + " <br /> Actual URL: " + currentURL;
                if (ConfigurationManager.AppSettings["URL"] == currentURL)
                {
                    caseLoadHomePage.Log(LogStatus.Pass, assertText);
                }
                else
                {
                    caseLoadHomePage.Log(LogStatus.Fail, assertText);
                }
            Assert.AreEqual(ConfigurationManager.AppSettings["URL"], currentURL);
            // Validate page load : caseLoadHomePage END


            // Authenticate successful : caseSuccessAuth START
            var caseSuccessAuth = extent.StartTest("Authenticate Successfully");
            homePage.ClickMyAccount();
            var loginPage = new LoginPage(driver);
            loginPage.LoginToApplication("LoginTest");
            bool isGreetingDisplayed = false;

            // Login has occurred, poll for Howdy, in .//*[@id='wp-admin-bar-my-account']/a
            do
            {
                LogQAData("Before display check {0}", isGreetingDisplayed);
                caseSuccessAuth.Log(LogStatus.Info, "Checking for greeting to confirm login");
                Thread.Sleep(500);
                try
                {
                    isGreetingDisplayed = driver.FindElement(By.XPath(".//*[@id='wp-admin-bar-my-account']/a")).Displayed;
                }
                catch
                {
                    LogQAData("After display check {0}", isGreetingDisplayed);
                    caseSuccessAuth.Log(LogStatus.Info, "Waiting for login to process");
                }
            } while (!isGreetingDisplayed);

            string authGreet = driver.FindElement(By.XPath(".//*[@id='wp-admin-bar-my-account']/a")).Text;
            
            if (authGreet.Contains("Howdy,"))
            {
                caseSuccessAuth.Log(LogStatus.Info, "Current URL = " + driver.Url);
                caseSuccessAuth.Log(LogStatus.Info, "authGreet = " + authGreet);
                caseSuccessAuth.Log(LogStatus.Pass, "Login has succeeded");
            }
            else
            {
                caseSuccessAuth.Log(LogStatus.Info, "Current URL = " + driver.Url);
                caseSuccessAuth.Log(LogStatus.Info, "authGreet = " + authGreet);
                caseSuccessAuth.Log(LogStatus.Fail, "Login has failed");
            }
            LogQAData("Current URL = {0}",driver.Url);
         
            //Wait for post-login screen refresh to complete
            do
                {
                    currentURL = driver.Url;
                }
            while (driver.Url != "http://store.demoqa.com/products-page/your-account/?login=1");

            testSuite
                .AppendChild(caseLoadHomePage)
                .AppendChild(caseSuccessAuth);
            // Authenticate successful : caseSuccessAuth END


            // Validate shopping cart population : caseShoppingCart START

            // Load test data from Excel
            var shoppingData = ExcelDataAccess.GetTestCaseData();

            // Iterate through data, execute process and test for each item in the list of test data
            for (int i = 0; i < shoppingData.Count; i++)
            {                
                var caseShoppingCart = extent.StartTest("Add cart item #" + i);
                caseShoppingCart.Log(LogStatus.Info, "Record " + i + " of " + (shoppingData.Count - 1) + ":<br/> ItemName: " + shoppingData[i].ItemName
                    + "<br/> URL: " + shoppingData[i].ItemURL + "<br/> Locator: " + shoppingData[i].ItemAddToCartLocator);
                LogQAData("Record {0} of {1}: \n ItemName: {2}\n URL: {3} \n Locator: {4}",
                    i, shoppingData.Count-1, shoppingData[i].ItemName, shoppingData[i].ItemURL, shoppingData[i].ItemAddToCartLocator);
                driver.Navigate().GoToUrl(shoppingData[i].ItemURL);
                // Navigate and validate
                currentURL = driver.Url;
                assertText = "ASSERT:<br/ >Expected URL: " + shoppingData[i].ItemURL + " <br /> Actual URL: " + currentURL;
                if (shoppingData[i].ItemURL == currentURL)
                {
                    caseShoppingCart.Log(LogStatus.Pass, assertText);
                }
                else
                {
                    caseShoppingCart.Log(LogStatus.Fail, assertText);
                }

                LogQAData(assertText);
                Assert.AreEqual(shoppingData[i].ItemURL, currentURL);

                // Report on item addition success
                driver.FindElement(By.XPath(shoppingData[i].ItemAddToCartLocator)).Click();
                caseShoppingCart.Log(LogStatus.Pass, "Added " + shoppingData[i].ItemName + " to shopping cart by clicking the element with XPath: " + shoppingData[i].ItemAddToCartLocator);
                LogQAData("Added {0} to shopping cart.", shoppingData[i].ItemName);
                // Capture, store, add report data for screen shot after adding the current item to the cart
                string sslogfile = LogFolder + string.Format("{0:yyyyMMddHHmmss}.png", DateTime.Now);
                Thread.Sleep(1200);
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(sslogfile, ImageFormat.Png);
                caseShoppingCart.Log(LogStatus.Info, "Add item to cart screenshot:" + testSuite.AddScreenCapture(sslogfile));
                   

                testSuite.AppendChild(caseShoppingCart);
            }
            // Validate shopping cart population : caseShoppingCart END


            // Validate math calculations in shopping cart : caseValidateMath START
            var caseValidateMath = extent.StartTest("Validate math");
            caseValidateMath.Log(LogStatus.Info, "Field calulation validations");

            driver.Navigate().GoToUrl("http://store.demoqa.com/products-page/checkout/");
            driver.FindElement(By.XPath(".//*[@id='header_cart']/a/span[1]")).Click();


            // Validate displayed item cart total matches calculated sum of item quantities
            int displayedItemTotal = Int32.Parse(driver.FindElement(By.XPath(".//*[@id='header_cart']/a/em[1]")).Text);
            caseValidateMath.Log(LogStatus.Info, "totalItems is " + displayedItemTotal);
            int calculatedItemTotal =
                Int32.Parse(driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[2]/td[3]/form/input[1]")).GetAttribute("value")) +
                Int32.Parse(driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[3]/td[3]/form/input[1]")).GetAttribute("value")) +
                Int32.Parse(driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[4]/td[3]/form/input[1]")).GetAttribute("value")) +
                Int32.Parse(driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[5]/td[3]/form/input[1]")).GetAttribute("value")) +
                Int32.Parse(driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[6]/td[3]/form/input[1]")).GetAttribute("value"));
            caseValidateMath.Log(LogStatus.Info, "calculatedItemTotal is " + calculatedItemTotal);
            // Assert and log item counts: calculated = displayed
            assertText = "ASSERT:<br/>Expected total item quantity in shopping cart (" + displayedItemTotal + ") = calculated item quantity (" + calculatedItemTotal + ")";
            if (displayedItemTotal == calculatedItemTotal)
            {
                caseValidateMath.Log(LogStatus.Pass, assertText);
            }
            else
            {
                caseValidateMath.Log(LogStatus.Fail, assertText);
            }
            Assert.AreEqual(displayedItemTotal, calculatedItemTotal);
            // Validate SubTotal matches sum of line item totals
            decimal displayedSubTotal = decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/span/span")).Text), NumberStyles.Currency);
            caseValidateMath.Log(LogStatus.Info, "subTotal is " + displayedSubTotal);
            decimal calculatedSubTotal = 
                decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[2]/td[5]/span/span")).Text), NumberStyles.Currency) +
                decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[3]/td[5]/span/span")).Text), NumberStyles.Currency) +
                decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[4]/td[5]/span/span")).Text), NumberStyles.Currency) +
                decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[5]/td[5]/span/span")).Text), NumberStyles.Currency) +
                decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[6]/td[5]/span/span")).Text), NumberStyles.Currency);
            caseValidateMath.Log(LogStatus.Info, "calculatedSubTotal is " + calculatedSubTotal);
            // Assert and log subtotals: calculated = displayed
            assertText = "ASSERT:<br/>Expected total item quantity in shopping cart (" + displayedSubTotal + ") = calculated item quantity (" + calculatedSubTotal + ")";
            if (displayedSubTotal == calculatedSubTotal)
            {
                caseValidateMath.Log(LogStatus.Pass, assertText);
            }
            else
            {
                caseValidateMath.Log(LogStatus.Fail, assertText);
            }
            Assert.AreEqual(displayedSubTotal, calculatedSubTotal);

            // Validate line items Quantity x Price match line item totals for all lines
            int checkoutRowCount = driver.FindElements(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr")).Count;
            int checkoutRowCountIndex;
            for (checkoutRowCountIndex = 2 ; checkoutRowCountIndex <= checkoutRowCount ; checkoutRowCountIndex++)
            {
            decimal calculatedLinePrice =
                decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[" + checkoutRowCountIndex + "]/td[4]/span")).Text), NumberStyles.Currency) *
                 Int32.Parse(driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[" + checkoutRowCountIndex + "]/td[3]/form/input[1]")).GetAttribute("value"));
            decimal displayedLinePrice = decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[" + checkoutRowCountIndex + "]/td[5]/span/span")).Text), NumberStyles.Currency);
            caseValidateMath.Log(LogStatus.Info, "calculatedLinePrice (" + decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[" + checkoutRowCountIndex + "]/td[4]/span")).Text), NumberStyles.Currency) +
                ") * " + Int32.Parse(driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[" + checkoutRowCountIndex + "]/td[3]/form/input[1]")).GetAttribute("value")) +
                " is " + calculatedLinePrice);
              // Assert Line 
            assertText = "ASSERT:<br/>Expected line item total for Line " + (checkoutRowCountIndex-1) + " displayed line price (" + displayedLinePrice + ") = calculated line price (" + calculatedLinePrice + ")";
            if (displayedLinePrice == calculatedLinePrice)
            {
                caseValidateMath.Log(LogStatus.Pass, assertText);
            }
            else
            {
                caseValidateMath.Log(LogStatus.Fail, assertText);
            }
            Assert.AreEqual(displayedLinePrice, calculatedLinePrice);
        }
            // Validate math calculations in shopping cart : caseValidateMath END


            // Test closure : testClosure START
            var testClosure = extent.StartTest("Ending Validation");
            string sslogfileEnd = LogFolder + string.Format("{0:yyyyMMddHHmmss}.png", DateTime.Now);
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(sslogfileEnd, ImageFormat.Png);
            testClosure.Log(LogStatus.Skip, "Add item to cart screenshot:" + testSuite.AddScreenCapture(sslogfileEnd));
            // Test closure : testClosure END

            testSuite
                    .AppendChild(caseValidateMath)
                    .AppendChild(testClosure);
                    
            extent.EndTest(testSuite);    
            extent.Flush();
            extent.Close();
        }
    }
     
}