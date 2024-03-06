﻿using OpenQA.Selenium.Chrome;
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
using System.Collections.Concurrent;
using OpenQA.Selenium.DevTools.V119.WebAudio;
using System.Text.RegularExpressions;
using OpenQA.Selenium.DevTools.V119.Fetch;
using OpenQA.Selenium.Remote;

namespace RentalInvestmentAid.Core.Announcement
{
    public class IADWebSite : MustInitializeCache, IAnnouncementWebSiteData
    {
        private readonly AnnouncementProvider _announcementProvider = AnnouncementProvider.IAD;
        private string baseUrl { get; set; } = "https://www.iadfrance.fr/";
        public IADWebSite(CachingManager cachingManager) : base(cachingManager)
        {
            base._cachingManager = cachingManager;
        }

        private List<String> FindUrlForEachAnnoncement(string html)
        {
            List<String> urls = new List<String>();
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            HtmlNodeCollection allAnnoncementInThePage = document.DocumentNode.SelectNodes("//*[@id=\"results-list\"]/section/div");

            if (allAnnoncementInThePage != null)
            {
                foreach (HtmlNode node in allAnnoncementInThePage)
                {
                    string announceUid = $"r{node.Attributes["data-gtm"].Value.Split("_")[2]}";

                    if (!_cachingManager.GetAnnouncementInformation().Any(ann => ann.IdFromProvider.Equals(announceUid) && ann.AnnouncementProvider.Equals(_announcementProvider)))
                        urls.Add($"https://www.iadfrance.fr/annonce/{announceUid}");
                }
            }

            return urls;
        }

        public List<string> GetAnnoucementUrl(List<string> departments = null, int? maxPrice = null)
        {
            ConcurrentBag<String> urls = new ConcurrentBag<String>();
            string html = String.Empty;
            ChromeOptions options = SeleniumHelper.DefaultChromeOption();
            Parallel.ForEach(departments, new ParallelOptions { MaxDegreeOfParallelism = 3 }, department =>
            {
                int page = 1;
                bool next = false;

                using (IWebDriver driver = new RemoteWebDriver(options))
                {
                    do
                    {
                        string url = $"{baseUrl}annonces/{department}?page={page}";
                        if (maxPrice.HasValue)
                            url += $"&price_max={maxPrice}";

                        SeleniumHelper.GoAndWaitPageIsReady(driver, url);
                        //Handle cookie banner
                        if (driver.FindElements(By.Id("didomi-notice-disagree-button")).Count > 0)
                            driver.FindElement(By.Id("didomi-notice-disagree-button")).Click();

                        List<string> urlsForAnnouncement = FindUrlForEachAnnoncement(driver.PageSource);

                        page++;
                        if (urlsForAnnouncement.Count > 0)
                        {
                            foreach (string announcementUrl in urlsForAnnouncement)
                                urls.Add(announcementUrl);
                            next = true;
                        }
                        else
                            next = false;

                    } while (next);

                }
            });
            return urls.ToList();
        }

        public AnnouncementInformation? GetAnnouncementInformation(string url)
        {

            //https://www.iadfrance.fr/annonce/r1256029
            AnnouncementInformation? announcementInformation = null;
            ChromeOptions options = SeleniumHelper.DefaultChromeOption();
            try
            {
                string metrage = String.Empty;

                string html = string.Empty;
                string type = string.Empty;
                string announcementIdFromProvider = string.Empty;
                using (IWebDriver driver = new RemoteWebDriver(options))
                {
                    SeleniumHelper.GoAndWaitPageIsReady(driver, url);

                    string[] keywords = driver.Url.Split("/");
                    announcementIdFromProvider = keywords.Last();
                    string[] lightDescription = keywords[4].Split("-");

                    metrage = lightDescription.Last().Split("m")[0];

                    type = string.Join(" ", lightDescription);

                    html = driver.PageSource;
                }

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                HtmlNode priceInfo = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'adPrice')]");

                if (priceInfo != null)
                {
                    string price = priceInfo.InnerText.Trim().Replace(" ", "").Split("€")[0];

                    string zipCodeAndCityToClear = document.DocumentNode.SelectSingleNode("//*[@id=\"advertisement\"]/div[1]/div[1]/div[4]/a").InnerText;

                    int indexOfSeparator = zipCodeAndCityToClear.IndexOf('-') + 1;
                    zipCodeAndCityToClear = zipCodeAndCityToClear.Remove(0, indexOfSeparator).Trim();

                    string[] zipCodeAndCity = zipCodeAndCityToClear.Split("(");

                    string zipCode = zipCodeAndCity[1].Replace("(", "").Replace(")", "").Trim();
                    string city = zipCodeAndCity[0].Trim().Replace("-", " ");


                    string description = document.DocumentNode.SelectSingleNode("//div[contains(@class, 'addescription')]").InnerText.Trim();

                    RentalTypeOfTheRent rentalType = KeyWordsHelper.GetRentalType(type);

                    announcementInformation = new AnnouncementInformation()
                    {
                        CityInformations = new Models.City.CityInformations
                        {
                            CityName = city,
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
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex);
            }
            return announcementInformation;
        }

        public string GetKeyword()
        {
            return "IAD";
        }
    }
}
