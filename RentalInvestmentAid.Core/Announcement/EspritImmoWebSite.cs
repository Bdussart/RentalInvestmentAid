using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium;
using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Models.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalInvestmentAid.Core.Helper;
using RentalInvestmentAid.Core.Announcement.Helper;
using Microsoft.Extensions.Options;
using HtmlAgilityPack;
using RentalInvestmentAid.Models.Rental;
using OpenQA.Selenium.Remote;

namespace RentalInvestmentAid.Core.Announcement
{
    public class EspritImmoWebSite : MustInitializeCache, IAnnouncementWebSiteData
    {
        private readonly AnnouncementProvider _announcementProvider = AnnouncementProvider.espritIimmo;
        private string baseUrl { get; set; } = "https://www.esprit-immo.fr/";
        public EspritImmoWebSite(CachingManager cachingManager) : base(cachingManager)
        {
            base._cachingManager = cachingManager;
        }

        private List<String> FindUrlForEachAnnoncement(string html)
        {
            List<String> urls = new List<String>();
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            HtmlNodeCollection allAnnoncementInThePage = document.DocumentNode.SelectNodes("//*[@id=\"listing_bien\"]/div");

            foreach (HtmlNode node in allAnnoncementInThePage)
            {
                HtmlNode nodeUrl = node.SelectNodes("div/div/div/div[3]/a").First();
                string url = nodeUrl.Attributes["href"].Value.Replace("../", baseUrl).Split(";")[0].Replace("&amp", string.Empty);
                urls.Add(url);

            }

            return urls;
        }


        private void SetSearchInformation(IWebDriver driver, int? maxPrice)
        {
            try
            {
                SeleniumHelper.GoAndWaitPageIsReady(driver, baseUrl);

                //Handle cookie banner
                driver.FindElement(By.XPath("//*[@id=\"cookie-banner\"]/div/div/a[1]")).Click();
                if (maxPrice.HasValue)
                {
                    InteratiorHelper.ImitateHumanTyping(maxPrice.Value.ToString(), driver.FindElement(By.Id("C_30_MAX")));
                }
                driver.FindElement(By.CssSelector("input[type=\"submit\"]")).Click();
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex);
                throw;
            }
        }

        public List<string> GetAnnoucementUrl(List<string> departments = null, int? maxPrice = null)
        {
            List<String> urls = new List<String>();
            string html = String.Empty;
            ChromeOptions options = SeleniumHelper.DefaultChromeOption();

            using (IWebDriver driver = new RemoteWebDriver(SeleniumHelper.SeleniumUrl, options))
            {
                SetSearchInformation(driver, maxPrice);
                //urls.AddRange(FindUrlForEachAnnoncement(driver.PageSource));

                while (driver.FindElement(By.Id("next_page_listing")).Displayed)
                {
                    IWebElement element = driver.FindElement(By.Id("next_page_listing"));
                    Actions actions = new Actions(driver);
                    actions.ScrollToElement(element);
                    actions.Perform();
                    actions.Click(element);
                    actions.Perform();
                }

                urls.AddRange(FindUrlForEachAnnoncement(driver.PageSource));
            }
            return urls;
        }

        public AnnouncementInformation? GetAnnouncementInformation(string url)
        {
            AnnouncementInformation? announcementInformation = null;
            try
            {
                HtmlWeb htmlWeb = new HtmlWeb();
                HtmlDocument document = htmlWeb.Load(url);

                string type = document.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[7]/div[1]/div[1]/div[1]/div[2]/div[8]/div[2]/div[1]/ul[1]/li[1]/div[1]/div[2]/b[1]").InnerText;
                string zipCode = document.DocumentNode.SelectSingleNode("html[1]/body[1]/div[7]/div[1]/div[1]/div[1]/div[2]/div[8]/div[3]/div[1]/ul[1]/li[1]/div[1]/div[2]/b[1]").InnerText;
                string city = document.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[7]/div[1]/div[1]/div[1]/div[2]/div[8]/div[3]/div[1]/ul[1]/li[2]/div[1]/div[2]/b[1]").InnerText;

                string price = document.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[7]/div[1]/div[1]/div[1]/div[2]/div[8]/div[4]/div[1]/ul[1]/li[1]/div[1]/div[2]/b[1]").InnerText.Trim().Split(" ")[0];
             
                string metrage = document.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[7]/div[1]/div[1]/div[1]/div[2]/div[8]/div[6]/div[1]/ul[1]/li[1]/div[1]/div[2]/b[1]").InnerText.Trim().Split(" ")[0];
                string description = document.DocumentNode.SelectSingleNode("/html/body/div[7]/div/div[1]/div/div[2]/div[2]").InnerText;

                string announcementIdFromProvider = url.Split("/").Last().Split("=")[1];
                RentalTypeOfTheRent rentalType = KeyWordsHelper.GetRentalType(type);

                announcementInformation = new AnnouncementInformation()
                {
                    CityInformations = new Models.City.CityInformations
                    {
                        CityName = CityHelper.CleanCityName(city),
                        ZipCode = zipCode,
                        Departement = zipCode.Substring(0, 2),
                    },
                    RentalType = rentalType,
                    AnnouncementProvider = _announcementProvider,
                    Metrage = metrage,
                    Price = price,
                    Description = description,
                    UrlWebSite = url,
                    IdFromProvider = announcementIdFromProvider
                };
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex);
            }

            return announcementInformation;
        }

        public string GetKeyword()
        {
            return "esprit-immo";
        }

        public void EnQueueAnnoucementUrl(List<string> departments = null, int? maxPrice = null)
        {
            throw new NotImplementedException();
        }
    }
}
