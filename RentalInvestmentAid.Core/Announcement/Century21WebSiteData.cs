using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using RentalInvestmentAid.Core.Announcement.Helper;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using static System.Collections.Specialized.BitVector32;
using OpenQA.Selenium.Interactions;
using Microsoft.FSharp.Data.UnitSystems.SI.UnitNames;

namespace RentalInvestmentAid.Core.Announcement
{

    public class Century21WebSiteData : IAnnouncementWebSiteData
    {
        private string baseUrl { get; set; } = "https://www.century21.fr";
        
        private void ImitateHumanTyping(string sentenceToImitate,  IWebElement element)
        {
            foreach (char c in sentenceToImitate)
            {
                Normal normalDist = new Normal(100, 10);
                int randomGaussianValue = (int)normalDist.Sample();
                Thread.Sleep(randomGaussianValue * 15 + 100);
                string s = new StringBuilder().Append(c).ToString();
                element.SendKeys(s);
            }
        }

       private List<String> FindUrlForEachAnnoncement(string html)
        {
            List<String> urls = new List<String>();
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            HtmlNodeCollection allAnnoncementInThePage = document.DocumentNode.SelectNodes("//div[contains(@class, 'js-the-list-of-properties-list-property')]");

            foreach (HtmlNode node in allAnnoncementInThePage)
            {

                HtmlNode divChild = node.ChildNodes.First(child => child.Name.Equals("div", StringComparison.CurrentCultureIgnoreCase));
                string annonceUid = divChild.Attributes["data-uid"].Value;

                urls.Add($"https://www.century21.fr/trouver_logement/detail/{annonceUid}");
            }



            return urls;
        }

        public List<string> GetAnnoucementUrl(List<string> departments, int? maxPrice)
        {
            List<String> urls = new List<String>();
            string html = String.Empty;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--enable-javascript");
            options.AddArgument("--window-size=500,1080");

            using (IWebDriver driver = new ChromeDriver(options))
            {
                foreach (string department in departments)
                {
                    driver.Navigate().GoToUrl(baseUrl);
                    ImitateHumanTyping(department, driver.FindElement(By.Id("q")));
                    driver.FindElement(By.XPath("/html/body/main/article/header/div/div[2]/div/div/div[2]/div/form/div[2]/div")).Click();
                    if (maxPrice.HasValue)
                    {
                        if(!driver.FindElement(By.Id("price_max")).Displayed)
                            driver.FindElement(By.XPath("/html/body/main/section/section/div[1]/div[1]/div[2]")).Click();

                        ImitateHumanTyping(maxPrice.Value.ToString(), driver.FindElement(By.Id("price_max")));
                        driver.FindElement(By.XPath("/html/body/main/section/section/div[2]/div[3]")).Click();
                    }
                    bool nextPage = false;
                    do
                    {
                        nextPage = false;
                        urls.AddRange(FindUrlForEachAnnoncement(driver.PageSource));

                        HtmlDocument document = new HtmlDocument();
                        document.LoadHtml(driver.PageSource);
                        HtmlNodeCollection isOnlyOnePage = document.DocumentNode.SelectNodes("/html/body/main/article/section/section/div[2]/div[4]/section/ul/li");
                        if (isOnlyOnePage.Count < 2) //If lower => only one page
                            nextPage = false;
                        else
                        {
                            HtmlNodeCollection ThereIsANextPage = document.DocumentNode.SelectNodes("/html/body/main/article/section/section/div[2]/div[4]/section/ul/li[last()]/ul/li");
                            if (ThereIsANextPage.Count == 1) //Due to the Century21 logical, The last LI must be the link to the next page
                                nextPage = true;
                        }                        

                        
                        if (nextPage)
                        {
                            IWebElement element = driver.FindElement(By.XPath("/html/body/main/article/section/section/div[2]/div[4]/section/ul/li[last()]/ul/li/a"));
                            IWebElement footer = driver.FindElement(By.XPath("/html/body/footer"));

                            Thread.Sleep(TimeSpan.FromSeconds(2));
                            Actions actions = new Actions(driver);
                            actions.ScrollToElement(footer); //Need to be a bit lower than the next page button, there is a popup for a bot ... :(
                            actions.Perform();
                            Thread.Sleep(TimeSpan.FromSeconds(2));
                            actions.Click(element);
                            actions.Perform();
                        }
                    }
                    while (nextPage);


                }
                driver.Close();
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

                HtmlNodeCollection nodesInformationWhitoutPrice = document.DocumentNode.SelectNodes("/html/body/main/article/div/section/div[1]/div[1]/h1");

                HtmlNodeCollection nodesInformationWithPrice = document.DocumentNode.SelectNodes("/html/body/main/article/div/section/div[1]/div[2]/div[1]/p[1]");
                HtmlNodeCollection nodesWithDescription = document.DocumentNode.SelectNodes("/html/body/main/article/div/div[2]/section[1]/div");
                List<HtmlNode> spanInfos = nodesInformationWhitoutPrice.First().ChildNodes.Where(child => child.Name.Equals("span", StringComparison.CurrentCultureIgnoreCase)).ToList();

                string[] locationInformations = spanInfos[2].InnerText.Trim().Split("-");

                RentalTypeOfTheRent rentalType = KeyWordsHelper.GetRentalType(spanInfos[0].InnerText.Trim());
                string metrage = string.Empty;

                if (rentalType == RentalTypeOfTheRent.Land || rentalType == RentalTypeOfTheRent.Parking || rentalType == RentalTypeOfTheRent.Other)
                    metrage = HtmlWordsHelper.CleanHtml(spanInfos[1].InnerText.Trim().Split("m")[0].Trim()).Replace(",", ".");
                else
                    metrage = HtmlWordsHelper.CleanHtml(spanInfos[1].InnerText.Trim().Split("-")[1].Split("m")[0].Trim()).Replace(",", ".");

                announcementInformation = new AnnouncementInformation()
                {
                    RentalType = rentalType,
                    Metrage = metrage,
                    City = HtmlWordsHelper.CleanHtml(locationInformations[0].Trim()),
                    ZipCode = HtmlWordsHelper.CleanHtml(locationInformations[1].Trim()),
                    Price = new string(HtmlWordsHelper.CleanHtml(nodesInformationWithPrice[0].InnerText).Where(char.IsDigit).ToArray()),
                    Description = HtmlWordsHelper.CleanHtml(nodesWithDescription[0].InnerText.Trim()),
                    UrlWebSite = url
                };
            }
            catch (Exception ex) { 
            //TODO: LOG INFO
            }

            return announcementInformation;
        }

        public string GetKeyword()
        {
            return "Century21";
        }
    }
}
