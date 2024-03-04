using Microsoft.Extensions.Options;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Helper
{
    public static class SeleniumHelper
    {
        public static void GoAndWaitPageIsReady(IWebDriver driver, string url)
        {
            driver.Navigate().GoToUrl(url);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public static ChromeOptions DefaultChromeOption()
        {
            ChromeOptions options =  new ChromeOptions();
            options.AddArgument("--enable-javascript");
            options.AddArgument("--window-size=500,1080");
            options.AddArgument("incognito");
            options.AddArgument("no-sandbox");

            return options;
        }
    }
}
