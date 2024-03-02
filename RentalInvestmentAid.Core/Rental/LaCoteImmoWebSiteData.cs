using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.DevTools;
using RentalInvestmentAid.Core.Helper;
using RentalInvestmentAid.Logger;
using RentalInvestmentAid.Queue;
using RentalInvestmentAid.Caching;

namespace RentalInvestmentAid.Core.Rental
{
    public class LaCoteImmoWebSiteData : MustInitializeCache, IRentalWebSiteData
    {
        public LaCoteImmoWebSiteData(CachingManager cachingManager) : base(cachingManager)
        {
            base._cachingManager = cachingManager;
        }

        //NOTE : Voici comment est fait le site lacoteImmo  :
        //https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/alby-sur-cheran/740002.htm
        //https://www.lacoteimmo.com/prix-de-l-immo/location/{région}/{département}/{n'importe quoi}/{département en chiffre}"74" +  {quatre digit incrémental}
        public List<RentalInformations> GetApartmentRentalInformation(string url)
        {
            LogHelper.LogInfo($"[{url}]");
            List<RentalInformations> rentalInformations = new List<RentalInformations>();
            try
            {
                HtmlWeb htmlWeb = new HtmlWeb();

                HtmlDocument document = htmlWeb.Load(url);

                HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/table[1]/tbody[1]/tr/td");

                HtmlNodeCollection nodeForZipCode = document.DocumentNode.SelectNodes("//*[@id=\"wrapper\"]/script[2]/text()");

                string zipCode = new string(nodeForZipCode[0].InnerText.Split("\n")[2].Where(char.IsDigit).ToArray());

                string city = System.Net.WebUtility.HtmlDecode(nodes[0].InnerText);
                string idFromProvider = url.Split("/").Last().Split(".").First();
                rentalInformations.Add(new RentalInformations()
                {
                    City = city.Replace("-", " ").Trim(),
                    Price = nodes[1].InnerText.Split(" ")[0],
                    RentalPriceType = RentalPriceType.LowerPrice,
                    ZipCode = zipCode,
                    RentalTypeOfTheRent = RentalTypeOfTheRent.Apartment,
                    Url = url,
                    IdFromProvider = idFromProvider
                });

                rentalInformations.Add(new RentalInformations()
                {
                    City = city.Replace("-", " ").Trim(),
                    Price = nodes[2].InnerText.Split(" ")[0],
                    RentalPriceType = RentalPriceType.MediumPrice,
                    ZipCode = zipCode,
                    RentalTypeOfTheRent = RentalTypeOfTheRent.Apartment,
                    Url = url,
                    IdFromProvider = idFromProvider
                });

                rentalInformations.Add(new RentalInformations()
                {
                    City = city.Replace("-", " ").Trim(),
                    Price = nodes[3].InnerText.Split(" ")[0],
                    RentalPriceType = RentalPriceType.HigherPrice,
                    ZipCode = zipCode,
                    RentalTypeOfTheRent = RentalTypeOfTheRent.Apartment,
                    Url = url,
                    IdFromProvider = idFromProvider
                });
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex);
            }
            return rentalInformations;
        }

        public List<RentalInformations> GetHouseRentalInformation(string url)
        {
            LogHelper.LogInfo($"[{url}]");
            List<RentalInformations> rentalInformations = new List<RentalInformations>();
            try
            {
                HtmlWeb htmlWeb = new HtmlWeb();

                HtmlDocument document = htmlWeb.Load(url);

                HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html[1]/body[1]/div[2]/div[2]/div[2]/div[2]/div[1]/table[1]/tbody[1]/tr/td");

                HtmlNodeCollection nodeForZipCode = document.DocumentNode.SelectNodes("//*[@id=\"wrapper\"]/script[2]/text()");
                HtmlNodeCollection nodeForCity = document.DocumentNode.SelectNodes("/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/table[1]/tbody[1]/tr/td");
                //*[@id="wrapper"]/script[2]/text()


                string zipCode = new string(nodeForZipCode[0].InnerText.Split("\n")[2].Where(char.IsDigit).ToArray());
                string city = System.Net.WebUtility.HtmlDecode(nodeForCity[0].InnerText);
                string idFromProvider = url.Split("/").Last().Split(".").First();
                rentalInformations.Add(new RentalInformations()
                {
                    City = city.Replace("-", " ").Trim(),
                    Price = nodes[0].InnerText.Split(" ")[0],
                    RentalPriceType = RentalPriceType.LowerPrice,
                    ZipCode = zipCode,
                    RentalTypeOfTheRent = RentalTypeOfTheRent.House,
                    Url = url,
                    IdFromProvider = idFromProvider
                });
                rentalInformations.Add(new RentalInformations()
                {
                    City = city.Replace("-", " ").Trim(),
                    Price = nodes[1].InnerText.Split(" ")[0],
                    RentalPriceType = RentalPriceType.MediumPrice,
                    ZipCode = zipCode,
                    RentalTypeOfTheRent = RentalTypeOfTheRent.House,
                    Url = url,
                    IdFromProvider = idFromProvider
                });
                rentalInformations.Add(new RentalInformations()
                {
                    City = city.Replace("-", " ").Trim(),
                    Price = nodes[2].InnerText.Split(" ")[0],
                    RentalPriceType = RentalPriceType.HigherPrice,
                    ZipCode = zipCode,
                    RentalTypeOfTheRent = RentalTypeOfTheRent.House,
                    Url = url,
                    IdFromProvider = idFromProvider
                });

            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex);
            }
            return rentalInformations;
        }

        public void EnQueueUrls(string area, string department, int departmentNumber)
        {
            LogHelper.LogInfo($" Area : {area}, departement : {department}, departmentNumber : {departmentNumber}");
            int iterator = 1;
            string baseUrl = string.Empty;
            string previousUrl = string.Empty;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--enable-javascript");
            options.AddArguments("--headless");
            options.AddArgument("incognito");
            options.AddArgument("no-sandbox");

            bool next = true;
            int tentative = 0;
            do
            {
                baseUrl = $"https://www.lacoteimmo.com/prix-de-l-immo/location/{area}/{department}/nothing/{departmentNumber}{iterator.ToString("0000")}.htm";
                LogHelper.LogInfo($"Get information for : {baseUrl}");
                try
                {
                    using (IWebDriver driver = new ChromeDriver(options)) // why open a new driver in the loop ? => Because this website is heavy for the memory and the processor, i don't want to shutdown the website server and my computer :) 
                    {
                        SeleniumHelper.GoAndWaitPageIsReady(driver, baseUrl);
                        LogHelper.LogInfo($"Get information for : {driver.Url}");
                        if ((baseUrl.Equals(driver.Url, StringComparison.CurrentCultureIgnoreCase)) ||
                            (previousUrl.Equals(driver.Url, StringComparison.CurrentCultureIgnoreCase)) ||
                            driver.Url == string.Empty)
                            next = false;
                        else
                        {
                            if(!base._cachingManager.GetRentalInformations().Any(rental => rental.Url.Equals(driver.Url, StringComparison.InvariantCultureIgnoreCase)))
                                RentalQueue.SendMessage(driver.Url);

                            previousUrl = driver.Url;
                            tentative = 0;
                        }
                        iterator++;
                    }

                }
                catch (Exception ex)
                {
                    Logger.LogHelper.LogException(ex);
                    if (tentative == 3)
                        next = false;
                    tentative++;
                }
            } while (next);

        }
    }
}