using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace OnlineStore.PageObjects
{
    class AllProductPage
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = ".//*[@id='default_products_page_container']/div[3]/div[2]/h2/a")]
        [CacheLookup]
        private IWebElement LinkiPhone5 { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='default_products_page_container']/div[3]/div[2]/form/div[2]/div[1]/span/input")]
        [CacheLookup]
        private IWebElement ButtoniPhone5 { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='default_products_page_container']/div[4]/div[2]/h2/a")]
        [CacheLookup]
        private IWebElement LinkMagicMouse { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='default_products_page_container']/div[4]/div[2]/form/div[2]/div[1]/span/input")]
        [CacheLookup]
        private IWebElement ButtonMagicMouse { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='default_products_page_container']/div[5]/div[2]/h2/a")]
        [CacheLookup]
        private IWebElement LinkiPodNanoBlue { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='default_products_page_container']/div[5]/div[2]/form/div[2]/div[1]/span/input")]
        [CacheLookup]
        private IWebElement ButtoniPodNanoBlue { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='meta']/ul/li[1]/a")]
        [CacheLookup]
        private IWebElement LinkMetaSiteAdmin { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='meta']/ul/li[2]/a")]
        [CacheLookup]
        private IWebElement LinkMetaLogOut { get; set; }

        [FindsBy(How = How.CssSelector, Using = ".wpsc_buy_button")]
        private IWebElement AddToCartButton { get; set; }

        public AllProductPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public void ClickLinkiPhone5()
        {
            LinkiPhone5.Click();
        }

        public void ClickButtoniPhone5()
        {
            ButtoniPhone5.Click();
        }

        public void ClickLinkMagicMouse()
        {
            LinkMagicMouse.Click();
        }

        public void ClickButtonMagicMouse()
        {
            ButtonMagicMouse.Click();
        }

        public void ClickLinkiPodNanoBlue()
        {
            LinkiPodNanoBlue.Click();
        }
     
         public void ClickButtoniPodNanoBlue()
        {
            ButtoniPodNanoBlue.Click();
        }   
    
        public void ClickLinkMetaSiteAdmin()
        {
            LinkMetaSiteAdmin.Click();
        }

        public void ClickLinkMetaLogOut()
        {
            LinkMetaLogOut.Click();
        }    
        
        public void ClickAddToCart()
        {
            AddToCartButton.Click();
        }
    }
}
