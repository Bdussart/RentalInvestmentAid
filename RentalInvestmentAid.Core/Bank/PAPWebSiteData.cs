using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Bank
{
    public class PAPWebSiteData : IBankWebSiteData
    {
        public List<BankInformation> GetRatesInformations(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            string html = string.Empty;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--enable-javascript");
            options.AddArgument("--window-size=500,1080");
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(url);

                html = driver.PageSource;

                driver.Close();
            }

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html/body/div[1]/div[1]/div[1]/table/tbody/tr");

            List<BankInformation> bankInformation = new List<BankInformation>();

            foreach (HtmlNode node in nodes)
            {
                List<HtmlNode> childs = node.ChildNodes.Where(child => child.Name.Equals("td", StringComparison.InvariantCultureIgnoreCase)).ToList();
                bankInformation.Add(new BankInformation
                {
                    DurationInYear = int.Parse(childs[0].InnerText),
                    LowerRate = childs[4].InnerText.Replace("%", "").Trim(),
                    MarketRate = childs[3].InnerText.Replace("%", "").Trim(),
                    MaxRate = childs[2].InnerText.Replace("%", "").Trim()
                });
            }
            return bankInformation;
        }
    }
}
