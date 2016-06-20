using NUnit.Framework;
using OnlineStore.PageObjects;
using OnlineStore.TestDataAccess;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RelevantCodes.ExtentReports;
using System;
using System.Configuration;
using System.Drawing.Imaging;
using System.Threading;
using OnlineStore.TextLogging;

namespace OnlineStore.TestCases
{
    class ShoppingTest
    {
        #region Setup
        static System.Collections.Specialized.NameValueCollection appSettings = ConfigurationManager.AppSettings;
        // Static log filename
        string LogFile = System.IO.Path.Combine(appSettings["LogDirectory"] == null ? Environment.CurrentDirectory : appSettings["LogDirectory"],
            appSettings["LogPrefix"] == null ? "NUnitTest" : appSettings["LogPrefix"] + ".log");
        // Dynamic log filename
        //string LogFile = System.IO.Path.Combine(appSettings["LogDirectory"] == null ? Environment.CurrentDirectory : appSettings["LogDirectory"],
        //    appSettings["LogPrefix"] == null ? "NUnitTest" : appSettings["LogPrefix"] + string.Format("_{0:yyyyMMddHHmmss}.log", DateTime.Now));
        string LogFolder = System.IO.Path.Combine(appSettings["LogDirectory"] == null ? Environment.CurrentDirectory : appSettings["LogDirectory"])+"\\";
        #endregion
        [Test]
        public void AddItemsToCart()
        {
            #region Initialization
            string assertText = " ";

            // Text Logging setup
            string HTMLLogFile = LogFile + ".html";
        
            QALog.QATextLog("Log file location: {0}", HTMLLogFile);
            
            // Report engine setup            
            var extent = new ExtentReports(HTMLLogFile, false, DisplayOrder.NewestFirst);
                extent.AddSystemInfo("Selenium Version", "2.53.0")
                      .AddSystemInfo("NUnit Version", "3.2.0")
                      .AddSystemInfo("Dapper Version", "1.4.2")
                      .AddSystemInfo("Environment", "Local")
                      .AddSystemInfo("Browser", "Chrome");
            var testSuite = extent.StartTest("Shopping Cart Test Suite", "<b>" + string.Format("[{0:yyyy-MM-dd HH:mm:ss.ffff}] ", DateTime.Now) +
                "Suite Objective:</b><br/>Log into the system with credentials pulled from Excel.<br/>Add 5 items to the shopping cart where the item names and " +
                "URLs are pulled from Excel.<br/>Validate pageloads, interactions, and math calculations.");
                testSuite.AssignCategory("Functional", "Regression", "Training")
                         .AssignAuthor("Rick Johnson")
                         .Log(LogStatus.Info, "Log file location:<br/>" + HTMLLogFile);
            
            IWebDriver driver = new ChromeDriver();// FirefoxDriver();
            driver.Url = driver.Url = ConfigurationManager.AppSettings["URL"];
            string currentURL = "";
            #endregion
            #region LoadHomePageTestCase
            // Validate page load : caseLoadHomePage START
            var homePage = new HomePage(driver);
            var caseLoadHomePage = extent.StartTest("Load Home Page");
            caseLoadHomePage.Log(LogStatus.Info, "Home Page load load validation");
            currentURL = driver.Url;
            assertText = "<b>ASSERT: </b><br />Expected URL: " + ConfigurationManager.AppSettings["URL"] + " <br /> Actual URL: " + currentURL;
                if (ConfigurationManager.AppSettings["URL"] == currentURL)
                {
                    caseLoadHomePage.Log(LogStatus.Pass, assertText);
                }
                else
                {
                    caseLoadHomePage.Log(LogStatus.Fail, assertText);
                }
            Assert.AreEqual(ConfigurationManager.AppSettings["URL"], currentURL);
            QALog.QATextLog(assertText.Replace("<br />", "").Replace("<b>", "").Replace("</b>", ""));

            // Validate page load : caseLoadHomePage END
            #endregion
            #region SuccesfulAuthentication
            // Authenticate successful : caseSuccessAuth START
            var caseSuccessAuth = extent.StartTest("Authenticate Successfully");
            homePage.ClickMyAccount();
            var loginPage = new LoginPage(driver);
            loginPage.LoginToApplication("LoginTest");
            bool isGreetingDisplayed = false;

            // Login has occurred, poll for Howdy, in .//*[@id='wp-admin-bar-my-account']/a from loginPage.GetAuthGreetDisplayedStatus()
            do
            {
                string LogText = "Checking for greeting to confirm login: before display check " + isGreetingDisplayed.ToString();
                QALog.QATextLog(LogText);
                caseSuccessAuth.Log(LogStatus.Info, LogText);
                Thread.Sleep(500);
                try
                {
                    isGreetingDisplayed = loginPage.GetAuthGreetDisplayedStatus();
                }
                catch
                {
                    LogText = "Waiting for login to process: after display check " + isGreetingDisplayed.ToString();
                    QALog.QATextLog(LogText);
                    caseSuccessAuth.Log(LogStatus.Info, LogText);
                }
            } while (!isGreetingDisplayed);
            
            string authGreet = loginPage.GetAuthGreetText();            
            
            if (authGreet.Contains("Howdy,"))
            {
                caseSuccessAuth.Log(LogStatus.Info, "Current URL = " + driver.Url);
                caseSuccessAuth.Log(LogStatus.Info, "authGreet = " + authGreet);
                caseSuccessAuth.Log(LogStatus.Pass, "Login has succeeded");
                QALog.QATextLog("Login has succeeded");
            }
            else
            {
                caseSuccessAuth.Log(LogStatus.Info, "Current URL = " + driver.Url);
                caseSuccessAuth.Log(LogStatus.Info, "authGreet = " + authGreet);
                caseSuccessAuth.Log(LogStatus.Fail, "Login has failed");
                QALog.QATextLog("Login has failed");
            }
            QALog.QATextLog("Current URL = {0}", driver.Url);
         
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
            #endregion
            #region ShoppingCartTestCase
            // Validate shopping cart population : caseShoppingCart START
            loginPage.ClickAllProductMenu();
            var allProductPage = new AllProductPage(driver);
            // (Test to click specific abstracted element) allProductPage.ClickButtoniPhone5();

            // Validate shopping cart population : caseShoppingCart START
            // Load test data from Excel
            var shoppingCartData = ExcelDataAccess.GetTestCaseData();

            // Iterate through data, execute process and test for each item in the list of test data
            for (int i = 0; i < shoppingCartData.Count; i++)
            {                
                // Create auto-enumerated Child Test Case Section for reporting
                var caseShoppingCart = extent.StartTest("Add cart item #" + i);
                // Create a report entry Info record with a snapshot of the current iteration's data
                caseShoppingCart.Log(LogStatus.Info, "<i>Record " + i + " of " + (shoppingCartData.Count - 1) + ":</i><br/> ItemName: " + shoppingCartData[i].ItemName
                    + "<br/> URL: " + shoppingCartData[i].ItemURL);
                // Log the current record data snapshot to text logger 
                QALog.QATextLog("Record {0} of {1}: ItemName: {2} URL: {3}", i, shoppingCartData.Count - 1, shoppingCartData[i].ItemName, shoppingCartData[i].ItemURL);
                // Navigate to the URL for this iteration and validate the URL change
                driver.Navigate().GoToUrl(shoppingCartData[i].ItemURL);              
                currentURL = driver.Url;
                assertText = "<b>ASSERT: </b><br />Expected URL: " + shoppingCartData[i].ItemURL + " <br /> Actual URL: " + currentURL;
                if (shoppingCartData[i].ItemURL == currentURL)
                {
                    caseShoppingCart.Log(LogStatus.Pass, assertText);
                }
                else
                {
                    caseShoppingCart.Log(LogStatus.Fail, assertText);
                }                  
                // Press the Add To Cart button
                allProductPage.ClickAddToCart();
                // Report on item addition success
                caseShoppingCart.Log(LogStatus.Pass, "Added " + shoppingCartData[i].ItemName + " to shopping cart by clicking the element with CSS Locator: \".wpsc_buy_button\"");
                QALog.QATextLog("Added {0} to shopping cart by clicking the element with CSS Locator: \".wpsc_buy_button\"", shoppingCartData[i].ItemName);                               
                //Assert
                Assert.AreEqual(shoppingCartData[i].ItemURL, currentURL);
                // Log to text log
                QALog.QATextLog(assertText.Replace("<br />", "").Replace("<b>", "").Replace("</b>", ""));
                // Capture and store screen shot after adding the current item to the cart, add a report entry
                Thread.Sleep(1200); // Optional wait, allow time for item name to populate the popup
                string sslogfile = LogFolder + string.Format("{0:yyyyMMddHHmmss}.png", DateTime.Now);
                ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(sslogfile, ImageFormat.Png);
                caseShoppingCart.Log(LogStatus.Info, "Add item to cart screenshot:" + testSuite.AddScreenCapture(sslogfile));
                  
                // Append the iteration as a child case for the report
                testSuite.AppendChild(caseShoppingCart);
            }
            // Validate shopping cart population : caseShoppingCart END
            #endregion
            #region ValidateMath
            // Validate math calculations in shopping cart : caseValidateMath START
            var caseValidateMath = extent.StartTest("Validate math");
            caseValidateMath.Log(LogStatus.Info, "Field calulation validations");
            loginPage.GoToShoppingCart();
            var checkoutPage = new CheckoutPage(driver);            
            // Validate displayed item cart total matches calculated sum of item quantities
            int displayedItemTotal = checkoutPage.GetCartItemsTotal();
            caseValidateMath.Log(LogStatus.Info, "totalItems is " + displayedItemTotal);
            int calculatedItemTotal = checkoutPage.GetCalculatedItemTotal();
            caseValidateMath.Log(LogStatus.Info, "calculatedItemTotal is " + calculatedItemTotal);
            // Assert and log item counts: calculated = displayed
            assertText = "<b>ASSERT: </b><br />Expected total item quantity in shopping cart (" + displayedItemTotal + ") = calculated item quantity (" + calculatedItemTotal + ")";
            if (displayedItemTotal == calculatedItemTotal)
            {
                caseValidateMath.Log(LogStatus.Pass, assertText);
            }
            else
            {
                caseValidateMath.Log(LogStatus.Fail, assertText);
            }
            Assert.AreEqual(displayedItemTotal, calculatedItemTotal);
            QALog.QATextLog(assertText.Replace("<br />", "").Replace("<b>", "").Replace("</b>", ""));
            // Validate SubTotal matches sum of line item totals
        
            decimal displayedSubTotal = checkoutPage.GetDisplayedSubTotal();

            caseValidateMath.Log(LogStatus.Info, "subTotal is " + displayedSubTotal);
            decimal calculatedSubTotal = checkoutPage.GetCalculatedSubTotal();

            caseValidateMath.Log(LogStatus.Info, "calculatedSubTotal is " + calculatedSubTotal);
            // Assert and log subtotals: calculated = displayed
            assertText = "<b>ASSERT: </b><br />Expected shopping cart subtotal (" + displayedSubTotal + ") = calculated shopping cart subtotal (" + calculatedSubTotal + ")";
            if (displayedSubTotal == calculatedSubTotal)
            {
                caseValidateMath.Log(LogStatus.Pass, assertText);
            }
            else
            {
                caseValidateMath.Log(LogStatus.Fail, assertText);
            }
            Assert.AreEqual(displayedSubTotal, calculatedSubTotal);
            QALog.QATextLog(assertText.Replace("<br />", "").Replace("<b>", "").Replace("</b>", ""));

            // Validate line items Quantity x Price match line item totals for all lines
            int checkoutRowCount = checkoutPage.CheckoutTableRowCount();
            int checkoutRowCountIndex;
            for (checkoutRowCountIndex = 2 ; checkoutRowCountIndex <= checkoutRowCount ; checkoutRowCountIndex++)
            {
                // Get Calculated Line Price Total for current location by multiplying Displayed Line Item Price * Displayed Line Quantity
                var checkoutLine = new CheckoutPage(driver);
                decimal calculatedLinePrice = checkoutLine.CalculateLineTotal(checkoutRowCountIndex);
                // Get Displayed Line Price Total for current location
                decimal displayedLinePrice = checkoutLine.GetDisplayedLineTotal(checkoutRowCountIndex);
                // Log calculated values to the report for current location
                string lineCalcText = checkoutLine.GenerateLineCalcText(checkoutRowCountIndex, calculatedLinePrice);
                caseValidateMath.Log(LogStatus.Info, lineCalcText);
                // Generate Assert Line 
                assertText = "<b>ASSERT: </b><br />Expected line item total for Line " + (checkoutRowCountIndex-1) + " displayed line price (" + displayedLinePrice + 
                    ") = calculated line price (" + calculatedLinePrice + ")";
                if (displayedLinePrice == calculatedLinePrice)
                {
                    caseValidateMath.Log(LogStatus.Pass, assertText);
                }
                else
                {
                    caseValidateMath.Log(LogStatus.Fail, assertText);
                }
                Assert.AreEqual(displayedLinePrice, calculatedLinePrice);
                QALog.QATextLog(assertText.Replace("<br />", "").Replace("<b>", "").Replace("</b>", ""));
        }
            // Validate math calculations in shopping cart : caseValidateMath END
            #endregion
            #region CloseTest
            // Test closure : testClosure START
            var testClosure = extent.StartTest("Ending Validation");
            string sslogfileEnd = LogFolder + string.Format("{0:yyyyMMddHHmmss}.png", DateTime.Now);
            ((ITakesScreenshot)driver).GetScreenshot().SaveAsFile(sslogfileEnd, ImageFormat.Png);
            testClosure.Log(LogStatus.Info, "Final cart screenshot:" + testSuite.AddScreenCapture(sslogfileEnd));
            testClosure.Log(LogStatus.Pass, "Test complete");
            // Test closure : testClosure END

            testSuite
                    .AppendChild(caseValidateMath)
                    .AppendChild(testClosure);
                    
            extent.EndTest(testSuite);    
            extent.Flush();
            extent.Close();
            #endregion
        }
    }    
}