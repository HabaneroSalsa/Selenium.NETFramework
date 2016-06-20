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

        [FindsBy(How = How.CssSelector, Using = "#menu-item-72>a")]
        [CacheLookup]
        private IWebElement AllProductMenu { get; set; }

        [FindsBy(How = How.LinkText, Using = "Register")]
        [CacheLookup]
        private IWebElement RegisterLink { get; set; }

        [FindsBy(How = How.LinkText, Using = "Log in")]
        [CacheLookup]
        private IWebElement LoginLink { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".footer_blog>p")]
        [CacheLookup]
        private IWebElement FooterBar { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[1]/a[1]")]
        [CacheLookup]
        private IWebElement Featured1TextLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[1]/a[2]")]
        [CacheLookup]
        private IWebElement Featured1ImgLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[1]/a[3]")]
        [CacheLookup]
        private IWebElement Featured1MoreLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[2]/a[1]")]
        [CacheLookup]
        private IWebElement Featured2TextLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[2]/a[2]")]
        [CacheLookup]
        private IWebElement Featured2ImgLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[2]/a[3]")]
        [CacheLookup]
        private IWebElement Featured2MoreLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[3]/a[1]")]
        [CacheLookup]
        private IWebElement Featured3TextLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[3]/a[2]")]
        [CacheLookup]
        private IWebElement Featured3ImgLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[3]/a[3]")]
        [CacheLookup]
        private IWebElement Featured3MoreLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[4]/a[1]")]
        [CacheLookup]
        private IWebElement Featured4TextLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[4]/a[2]")]
        [CacheLookup]
        private IWebElement Featured4ImgLink { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='footer']/section[2]/ul/li[4]/a[3]")]
        [CacheLookup]
        private IWebElement Featured4MoreLink { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".pinterest>a")]
        [CacheLookup]
        private IWebElement PinterestFooter { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".gplus>a")]
        [CacheLookup]
        private IWebElement GPlusFooter { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".rss>a")]
        [CacheLookup]
        private IWebElement RSSFooter { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#footer_nav>p")]
        [CacheLookup]
        private IWebElement CopyrightFooter { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#menu-item-53>a")]
        [CacheLookup]
        private IWebElement SPHomeFooter { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#menu-item-54>a")]
        [CacheLookup]
        private IWebElement SamplePageFooter { get; set; }

        [FindsBy(How = How.CssSelector, Using = "#menu-item-55>a")]
        [CacheLookup]
        private IWebElement YourAccountFooter { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='wp-admin-bar-my-account']/a")]
        [CacheLookup]
        private IWebElement AuthGreet { get; set; }


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

        public void ClickAllProductMenu()
        {
            AllProductMenu.Click();
        }

        public void GoToShoppingCart()
        {
            driver.Navigate().GoToUrl("http://store.demoqa.com/products-page/checkout/");        
        }

        public string GetAuthGreetText()
        {
            var authGreet = AuthGreet.Text;
            return authGreet;
            
        }

        public bool GetAuthGreetDisplayedStatus()
        {
            bool isGreetingDisplayed = AuthGreet.Displayed;
            return isGreetingDisplayed;
        }

    }
}