using NUnit.Framework;
using OnlineStore.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.PageObjects;
using System.Configuration;

namespace OnlineStore.TestCases
{

    class LogInTest
    {
        [Test]
        public void Test()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Url = driver.Url = ConfigurationManager.AppSettings["URL"]; ;

            var homePage = new HomePage(driver);
            homePage.ClickMyAccount();

            var loginPage = new LoginPage(driver);
            loginPage.LoginToApplication("LoginTest");

            // driver.Close();
        }


    }
}