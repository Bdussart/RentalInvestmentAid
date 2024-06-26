﻿using HtmlAgilityPack;
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
using RentalInvestmentAid.Core.Helper;
using RentalInvestmentAid.Caching;
using OpenQA.Selenium.Remote;
using System.Collections.Concurrent;
using OpenQA.Selenium.Interactions;
using System.Reflection.Emit;

namespace RentalInvestmentAid.Core.Announcement
{
    public class LeBonCoinWebSiteData :  IAnnouncementWebSiteData
    {
        private readonly AnnouncementProvider _announcementProvider = AnnouncementProvider.LeBonCoin;
        private AnnouncementTreatment _announcementTreatment = null;


        public LeBonCoinWebSiteData(AnnouncementTreatment announcementTreatment)
        {
            _announcementTreatment = announcementTreatment;
        }
        private string baseUrl { get; set; } = "https://www.leboncoin.fr";

        private IWebDriver UsingGoogleToGoTo()
        {
            ChromeOptions options = SeleniumHelper.DefaultChromeOption();
            IWebDriver webDriver = new ChromeDriver(options);
            SeleniumHelper.GoAndWaitPageIsReady(webDriver, "https://www.google.fr/");
            //handle cookie
            // 

            Actions actions = new Actions(webDriver);
            if (webDriver.FindElement(By.Id("W0wltc")).Displayed)
            {
                IWebElement element = webDriver.FindElement(By.Id("W0wltc"));
                actions.ScrollToElement(element);
                actions.Perform();
                actions.Click(element);
                actions.Perform();
            }
            InteratiorHelper.ImitateHumanTyping(GetKeyword(), webDriver.FindElement(By.Id("APjFqb")));
            actions.SendKeys(Keys.Enter);
            actions.Perform();

            HtmlDocument document = new HtmlDocument();

            document.LoadHtml(webDriver.PageSource);

            //List<HtmlNode> nodes = document.DocumentNode.Descendants("a").ToList();

            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes($"//a[contains(@href, '{baseUrl}')]");


            webDriver.FindElements(By.XPath(nodes.First().XPath)).First().Click();
            ///
            return webDriver;
        }

        public List<string> GetAnnoucementUrl(List<string> departments = null, int? maxPrice = null)
        {
            ConcurrentBag<String> urls = new ConcurrentBag<String>();
            string html = String.Empty;
            using (IWebDriver driver = UsingGoogleToGoTo())
            {

            }

            return urls.ToList();
        }
        public AnnouncementInformation? GetAnnouncementInformation(string url)
        {
            AnnouncementInformation? announcementInformation = null;

            try
            {
                HtmlWeb htmlWeb = new HtmlWeb();
                string html = string.Empty;

                ChromeOptions options = SeleniumHelper.DefaultChromeOption();
                using (IWebDriver driver = new RemoteWebDriver(SeleniumHelper.SeleniumUrl,options))
                {

                    SeleniumHelper.GoAndWaitPageIsReady(driver, url);
                    html = driver.PageSource;
                }

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(html);

                HtmlNode nodeMetrageAndCity = document.DocumentNode.SelectSingleNode("//*[@id=\"grid\"]/article/div[1]/div/div[1]/p");
                if (nodeMetrageAndCity != null)
                {
                    HtmlNodeCollection htmlNodesMetrageAndCity = nodeMetrageAndCity.ChildNodes;
                    HtmlNodeCollection htmlNodesPrice = document.DocumentNode.SelectNodes("//*[@id=\"grid\"]/article/div[1]/div/div[1]/div[2]/div/p");
                    HtmlNodeCollection htmlNodesRentalType = document.DocumentNode.SelectNodes("//*[@id=\"grid\"]/article/div[1]/div/h1");
                    HtmlNodeCollection htmlNodesDescription = document.DocumentNode.SelectNodes("//*[@id=\"grid\"]/article/div[3]/div[2]/div[1]/p");

                    string zipCode = HtmlWordsHelper.CleanHtml(htmlNodesMetrageAndCity[2].InnerText.Trim().Split(" ")[1]);

                    string announcementIdFromProvider = url.Split("/").Last();
                    announcementInformation = new AnnouncementInformation()
                    {
                        CityInformations = new Models.City.CityInformations
                        {
                            CityName = CityHelper.CleanCityName(HtmlWordsHelper.CleanHtml(htmlNodesMetrageAndCity[2].InnerText.Trim().Split(" ")[0])),
                            ZipCode = zipCode,
                            Departement = zipCode.Substring(0, 2),
                        },
                        RentalType = KeyWordsHelper.GetRentalType(htmlNodesRentalType[0].InnerText.Trim()),
                        Metrage = HtmlWordsHelper.CleanHtml(htmlNodesMetrageAndCity[1].InnerText.Trim().Split("m")[0].Trim()),
                        Price = new string(HtmlWordsHelper.CleanHtml(htmlNodesPrice[0].InnerText).Where(char.IsDigit).ToArray()),
                        Description = HtmlWordsHelper.CleanHtml(htmlNodesDescription[0].InnerText.Trim()),
                        UrlWebSite = url,
                        AnnouncementProvider = _announcementProvider,
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
            return "LeBoncoin";
        }

        public void EnQueueAnnoucementUrl(List<string> departments = null, int? maxPrice = null)
        {
            throw new NotImplementedException();
        }
    }
}
