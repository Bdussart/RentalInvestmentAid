using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using RentalInvestmentAid.Core.Helper;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Rate;
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
        public List<RateInformation> GetRatesInformations(string url)
        {
            List<RateInformation> bankInformation = new List<RateInformation>();
            try
            {
                HtmlWeb htmlWeb = new HtmlWeb();
                string html = string.Empty;

                ChromeOptions options = SeleniumHelper.DefaultChromeOption();
                using (IWebDriver driver = new RemoteWebDriver(options))
                {
                    SeleniumHelper.GoAndWaitPageIsReady(driver, url);
                    html = driver.PageSource;
                }

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html/body/div[1]/div[1]/div[1]/table/tbody/tr");

                foreach (HtmlNode node in nodes)
                {
                    List<HtmlNode> childs = node.ChildNodes.Where(child => child.Name.Equals("td", StringComparison.InvariantCultureIgnoreCase)).ToList();
                    bankInformation.Add(new RateInformation
                    {
                        DurationInYear = int.Parse(childs[0].InnerText),
                        Rate = childs[4].InnerText.Replace("%", "").Trim(),
                        RateType = RateType.LowRate
                    });

                    bankInformation.Add(new RateInformation
                    {
                        DurationInYear = int.Parse(childs[0].InnerText),
                        Rate = childs[3].InnerText.Replace("%", "").Trim(),
                        RateType = RateType.MediumRate
                    });

                    bankInformation.Add(new RateInformation
                    {
                        DurationInYear = int.Parse(childs[0].InnerText),
                        Rate = childs[2].InnerText.Replace("%", "").Trim(),
                        RateType = RateType.HighRate
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex);
            };
            return bankInformation;
        }
    }
}
