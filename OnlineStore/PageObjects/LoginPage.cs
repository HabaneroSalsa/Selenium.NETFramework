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

        [FindsBy(How = How.Id, Using = "rememberme")]
        [CacheLookup]
        private IWebElement RememberMeCheckbox { get; set; }

        [FindsBy(How = How.Id, Using = "logo")]
        [CacheLookup]
        private IWebElement SiteLogo { get; set; }

        [FindsBy(How = How.Id, Using = "header_cart")]
        [CacheLookup]
        private IWebElement Cart { get; set; }

        [FindsBy(How = How.Id, Using = "account")]
        [CacheLookup]
        private IWebElement MyAccount { get; set; }

        [FindsBy(How = How.Id, Using = "menu-item-15")]
        [CacheLookup]
        private IWebElement HomeMenu { get; set; }
        
        [FindsBy(How = How.Id, Using = "menu-item-33")]
        [CacheLookup]
        private IWebElement ProductCategoryMenu { get; set; }




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