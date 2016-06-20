using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Globalization;

namespace OnlineStore.PageObjects
{
    class CheckoutPage
    {
        private IWebDriver driver;

        [FindsBy(How = How.XPath, Using = ".//*[@id='header_cart']/a/em[1]")]
        private IWebElement CartItemsTotal { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/span/span")]
        private IWebElement DisplayedSubTotal { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[2]/td[5]/span/span")]
        private IWebElement Line1Total { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[3]/td[5]/span/span")]
        private IWebElement Line2Total { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[4]/td[5]/span/span")]
        private IWebElement Line3Total { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[5]/td[5]/span/span")]
        private IWebElement Line4Total { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[6]/td[5]/span/span")]
        private IWebElement Line5Total { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[2]/td[3]/form/input[1]")]
        private IWebElement Line1Quantity { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[3]/td[3]/form/input[1]")]
        private IWebElement Line2Quantity { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[4]/td[3]/form/input[1]")]
        private IWebElement Line3Quantity { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[5]/td[3]/form/input[1]")]
        private IWebElement Line4Quantity { get; set; }

        [FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[6]/td[3]/form/input[1]")]
        private IWebElement Line5Quantity { get; set; }

        //[FindsBy(How = How.XPath, Using = ".//*[@id='checkout_page_container']/div[1]/table/tbody/tr")]
        private IWebElement CheckoutTableCount { get; set; }

        public CheckoutPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        public int GetCartItemsTotal()
        {
            int cartItemsTotal = 0;
            Int32.TryParse(CartItemsTotal.Text, out cartItemsTotal);
           
            return cartItemsTotal;
        }

        public decimal GetDisplayedSubTotal()
        {
            decimal displayedSubTotal = decimal.Parse(DisplayedSubTotal.Text, NumberStyles.Currency);
            return displayedSubTotal;
        }

        public void GetLine1Total()
        {
            Line1Total.GetAttribute("text");
        }

        public int CheckoutTableRowCount()
        {
            int checkoutRowCount = driver.FindElements(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr")).Count;
            return checkoutRowCount;
        }
       

        public decimal GetCalculatedSubTotal()
        {
            decimal calculatedSubTotal = decimal.Parse(Line1Total.Text, NumberStyles.Currency) +
                                         decimal.Parse(Line2Total.Text, NumberStyles.Currency) +
                                         decimal.Parse(Line3Total.Text, NumberStyles.Currency) +
                                         decimal.Parse(Line4Total.Text, NumberStyles.Currency) +
                                         decimal.Parse(Line5Total.Text, NumberStyles.Currency);
            return calculatedSubTotal;
        }

        public int GetCalculatedItemTotal()
        {
            int calculatedItemTotal = int.Parse(Line1Quantity.GetAttribute("value")) +
                                      int.Parse(Line2Quantity.GetAttribute("value")) +
                                      int.Parse(Line3Quantity.GetAttribute("value")) +
                                      int.Parse(Line4Quantity.GetAttribute("value")) +
                                      int.Parse(Line5Quantity.GetAttribute("value"));
            return calculatedItemTotal;
        } 

        public decimal CalculateLineTotal(int i)
        {
            decimal CalculatedLineValue =
            decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[" + i + "]/td[4]/span")).Text), NumberStyles.Currency) *
            Int32.Parse(driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[" + i + "]/td[3]/form/input[1]")).GetAttribute("value"));
            return CalculatedLineValue;
        }

        public decimal GetDisplayedLineTotal(int i)
        {
            decimal DisplayedLineTotal =
            decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[" + i + "]/td[5]/span/span")).Text), NumberStyles.Currency);
            return DisplayedLineTotal;
        }

        public string GenerateLineCalcText(int i, decimal calculatedLinePrice)
        {
            string CalcText="";
            CalcText = "calculatedLinePrice (" + decimal.Parse((driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[" + i +
                "]/td[4]/span")).Text), NumberStyles.Currency) + ") * " + Int32.Parse(driver.FindElement(By.XPath(".//*[@id='checkout_page_container']/div[1]/table/tbody/tr[" + i +
                "]/td[3]/form/input[1]")).GetAttribute("value")) + " is " + calculatedLinePrice;
            return CalcText;
        }
    }
}
