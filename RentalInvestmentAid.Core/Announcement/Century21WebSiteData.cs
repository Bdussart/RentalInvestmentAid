using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using RentalInvestmentAid.Core.Announcement.Helper;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Rental;
using OpenQA.Selenium.Interactions;
using RentalInvestmentAid.Core.Helper;
using System.Collections.Concurrent;
using OpenQA.Selenium.Remote;
using RentalInvestmentAid.Queue;

namespace RentalInvestmentAid.Core.Announcement
{

    public class Century21WebSiteData : IAnnouncementWebSiteData
    {
        private readonly AnnouncementProvider _announcementProvider = AnnouncementProvider.century21;
        private AnnouncementTreatment _announcementTreatment = null;
        private IBroker _announcementBroker = null;

        public Century21WebSiteData(AnnouncementTreatment announcementTreatment, IBroker annoucementBroker)
        {
            _announcementTreatment = announcementTreatment;
            _announcementBroker = annoucementBroker;
        }

        private string baseUrl { get; set; } = "https://www.century21.fr";


        private List<String> FindUrlForEachAnnoncement(string html)
        {
            List<String> urls = new List<String>();
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);

            HtmlNodeCollection allAnnoncementInThePage = document.DocumentNode.SelectNodes("//div[contains(@class, 'js-the-list-of-properties-list-property')]");
            if (allAnnoncementInThePage is null)
                allAnnoncementInThePage = document.DocumentNode.SelectNodes("//div[contains(@class, 'js-the-list-of-properties-related-property')]");

            if (allAnnoncementInThePage is not null)
            {
                foreach (HtmlNode node in allAnnoncementInThePage)
                {

                    HtmlNode divChild = node.ChildNodes.First(child => child.Name.Equals("div", StringComparison.CurrentCultureIgnoreCase));
                    string annonceUid = divChild.Attributes["data-uid"].Value;

                    if (!_announcementTreatment.ExistAnnouncementByProviderAndProviderId(annonceUid, _announcementProvider))
                        urls.Add($"https://www.century21.fr/trouver_logement/detail/{annonceUid}");
                }
            }

            return urls;
        }

        private void SetSearchInformation(IWebDriver driver, int? maxPrice, string department)
        {
            try
            {
                SeleniumHelper.GoAndWaitPageIsReady(driver, baseUrl);
                InteratiorHelper.ImitateHumanTyping(department, driver.FindElement(By.Id("q")));
               driver.FindElement(By.XPath("/html/body/main/article/header/div/div[2]/div/div/div[2]/div/form/div[2]/div")).Click();
                SeleniumHelper.WaitPageIsReady(driver);
                if (maxPrice.HasValue)
                {
                    if (!driver.FindElement(By.Id("price_max")).Displayed)
                        driver.FindElement(By.XPath("/html/body/main/section/section/div[1]/div[1]/div[2]")).Click();

                    InteratiorHelper.ImitateHumanTyping(maxPrice.Value.ToString(), driver.FindElement(By.Id("price_max")));
                    driver.FindElement(By.XPath("/html/body/main/section/section/div[2]/div[3]")).Click();
                }
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex, objectInfo: $"department : {department} maxPrice : {maxPrice}");
            }
        }

        private bool ThereIsANextPage(string html)
        {
            bool nextPage = false;

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            HtmlNodeCollection isOnlyOnePage = document.DocumentNode.SelectNodes("/html/body/main/article/section/section/div[2]/div[4]/section/ul/li");
            if (isOnlyOnePage is null || isOnlyOnePage.Count < 2) //If lower => only one page
                nextPage = false;
            else
            {
                HtmlNodeCollection ThereIsANextPage = document.DocumentNode.SelectNodes("/html/body/main/article/section/section/div[2]/div[4]/section/ul/li[last()]/ul/li");
                if (ThereIsANextPage.Count == 1) //Due to the Century21 logical, The last LI must be the link to the next page
                    nextPage = true;
            }

            return nextPage;
        }

        private List<string> GetAnnouncementForADepartement(string department, int? maxPrice = null)
        {
            List<string> urls = new List<string>();
            try
            {
                ChromeOptions options = SeleniumHelper.DefaultChromeOption();
                using (IWebDriver driver = new RemoteWebDriver(options))
                {
                    SetSearchInformation(driver, maxPrice, department);
                    //urls.AddRange(FindUrlForEachAnnoncement(driver.PageSource));
                    bool nextPage = false;
                    do
                    {
                        SeleniumHelper.WaitPageIsReady(driver);
                        foreach (string url in FindUrlForEachAnnoncement(driver.PageSource))
                            urls.Add(url);
                        nextPage = ThereIsANextPage(driver.PageSource);

                        if (nextPage)
                        {
                            IWebElement element = driver.FindElement(By.XPath("/html/body/main/article/section/section/div[2]/div[4]/section/ul/li[last()]/ul/li/a"));
                            IWebElement footer = driver.FindElement(By.XPath("/html/body/footer"));

                            Actions actions = new Actions(driver);
                            actions.ScrollToElement(footer); //Need to be a bit lower than the next page button, there is a popup for a bot ... :(
                            actions.Perform();
                            actions.Click(element);
                            actions.Perform();
                        }
                    }
                    while (nextPage);
                }
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex, objectInfo: $"department : {department} maxPrice : {maxPrice}");
            }
            return urls;
        }

        public List<string> GetAnnoucementUrl(List<string> departments = null, int? maxPrice = null)
        {
            Logger.LogHelper.LogInfo($"departements to fetch data {departments.Count}");
            ConcurrentBag<String> urls = new ConcurrentBag<String>();

            foreach (string department in departments)
            {
                Logger.LogHelper.LogInfo($"Start fetching information for {department}");
                GetAnnouncementForADepartement(department, maxPrice).ForEach(url => urls.Add(url));
                Logger.LogHelper.LogInfo($"Done fetching information for {department}");
            }
            return urls.ToList();
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

                var announcementIdFromProvider = url.Split("/").Last();
                string zipCode = HtmlWordsHelper.CleanHtml(locationInformations[1].Trim());
                announcementInformation = new AnnouncementInformation()
                {
                    CityInformations = new Models.City.CityInformations
                    {
                        CityName = HtmlWordsHelper.CleanHtml(locationInformations[0].Replace("-", " ").Trim()),
                        ZipCode = zipCode,
                        Departement = zipCode.Substring(0, 2),
                    },
                    RentalType = rentalType,
                    AnnouncementProvider = _announcementProvider,
                    Metrage = metrage,
                    Price = new string(HtmlWordsHelper.CleanHtml(nodesInformationWithPrice[0].InnerText).Where(char.IsDigit).ToArray()),
                    Description = HtmlWordsHelper.CleanHtml(nodesWithDescription[0].InnerText.Trim()),
                    UrlWebSite = url,
                    IdFromProvider = announcementIdFromProvider
                };
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex, objectInfo: url);
            }

            return announcementInformation;
        }

        public string GetKeyword()
        {
            return "Century21";
        }

        public void EnQueueAnnoucementUrl(List<string> departments = null, int? maxPrice = null)
        {
            Logger.LogHelper.LogInfo($"departements to fetch data {departments.Count}");
            foreach (string department in departments)
            {
                Logger.LogHelper.LogInfo($"Start fetching information for {department}");
                GetAnnouncementForADepartement(department, maxPrice).ForEach(url => _announcementBroker.SendMessage(url));
                Logger.LogHelper.LogInfo($"Done fetching information for {department}");
            }
        }
    }
}
