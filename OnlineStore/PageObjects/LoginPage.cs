using OnlineStore.TestDataAccess;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace OnlineStore.PageObjects
{
    public class LoginPage
    {
        private IWebDriver driver;

        [FindsBy(How = How.Id, Using = "log")]
        [CacheLookup]
        private IWebElement UserName { get; set; }

        [CacheLookup]
        [FindsBy(How = How.Id, Using = "pwd")]
        private IWebElement Password { get; set; }

        [FindsBy(How = How.Id, Using = "login")]
        [CacheLookup]
        private IWebElement Submit { get; set; }

        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public void LoginToApplication(string testName)
        {
            var userData = ExcelDataAccess.GetTestUserData(testName);
            UserName.SendKeys(userData.Username);
            Password.SendKeys(userData.Password);
            Submit.Submit();
        }
    }
}