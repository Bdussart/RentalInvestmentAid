using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalInvestmentAid.Core.Announcement.Helper;
using System.Reflection.Metadata;

namespace RentalInvestmentAid.Core.Announcement
{
    public class LeBonCoinWebSiteData : IAnnouncementWebSiteData
    {
        public List<string> GetAnnoucementUrl(List<string> department, int? maxPrice)
        {
            throw new NotImplementedException();
        }

        public AnnouncementInformation? GetAnnouncementInformation(string url)
        {
            AnnouncementInformation? announcementInformation = null;

            try
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

                HtmlNodeCollection htmlNodesMetrageAndCity = document.DocumentNode.SelectNodes("//*[@id=\"grid\"]/article/div[1]/div/div[1]/p/span");
                HtmlNodeCollection htmlNodesPrice = document.DocumentNode.SelectNodes("//*[@id=\"grid\"]/article/div[1]/div/div[1]/div[2]/div/p");
                HtmlNodeCollection htmlNodesRentalType = document.DocumentNode.SelectNodes("//*[@id=\"grid\"]/article/div[1]/div/h1");
                HtmlNodeCollection htmlNodesDescription = document.DocumentNode.SelectNodes("//*[@id=\"grid\"]/article/div[3]/div[2]/div[1]/p");

                announcementInformation = new AnnouncementInformation()
                {
                    RentalType = KeyWordsHelper.GetRentalType(htmlNodesRentalType[0].InnerText.Trim()),
                    Metrage = HtmlWordsHelper.CleanHtml(htmlNodesMetrageAndCity[1].InnerText.Trim().Split("m")[0].Trim()),
                    City = HtmlWordsHelper.CleanHtml(htmlNodesMetrageAndCity[2].InnerText.Trim().Split(" ")[0]),
                    ZipCode = HtmlWordsHelper.CleanHtml(htmlNodesMetrageAndCity[2].InnerText.Trim().Split(" ")[1]),
                    Price = new string(HtmlWordsHelper.CleanHtml(htmlNodesPrice[0].InnerText).Where(char.IsDigit).ToArray()),
                    Description = HtmlWordsHelper.CleanHtml(htmlNodesDescription[0].InnerText.Trim()),
                    UrlWebSite = url
                };

            }
            catch (Exception ex)
            {
                //TODO LOG EX
            }
            return announcementInformation;
        }

        public string GetKeyword()
        {
            return "LeBoncoin";
        }
    }
}
