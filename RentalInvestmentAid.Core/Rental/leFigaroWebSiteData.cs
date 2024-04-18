using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using RentalInvestmentAid.Core.Helper;
using RentalInvestmentAid.Models.Rental;
using RentalInvestmentAid.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalInvestmentAid.Logger;
using RentalInvestmentAid.Caching;
using MathNet.Numerics.LinearAlgebra.Solvers;
using static System.Net.Mime.MediaTypeNames;
using OpenQA.Selenium.Interactions;
using RentalInvestmentAid.Core.Announcement.Helper;
using HtmlAgilityPack;
using RentalInvestmentAid.Models.City;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.ComponentModel;
using Newtonsoft.Json.Linq;

namespace RentalInvestmentAid.Core.Rental
{
    public class LeFigaroWebSiteData : MustInitializeCache, IRentalWebSiteData
    {
        private IBroker _rentalRabbitMQBroker = null;
        public LeFigaroWebSiteData(CachingManager cachingManager, IBroker rentalBroker) : base(cachingManager)
        {
            base._cachingManager = cachingManager;
            _rentalRabbitMQBroker = rentalBroker;
        }
        public void EnQueueUrls(string area, string department, int departmentNumber)
        {
            throw new NotImplementedException();
        }

        public List<RentalInformations> GetApartmentRentalInformation(string url)
        {
            LogHelper.LogInfo($"[{url}]");
            List<RentalInformations> rentalInformations = new List<RentalInformations>();
            try
            {
                HtmlWeb htmlWeb = new HtmlWeb();

                HtmlDocument document = htmlWeb.Load(url);

                HtmlNode nodeInformation = document.DocumentNode.SelectSingleNode("//*[@id=\"loyer-appartement\"]").NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;

                List<HtmlNode> nodes = nodeInformation.ChildNodes.Where(node => node.Name.Equals("div", StringComparison.InvariantCultureIgnoreCase)).ToList();
                //*[@id="jStickySize"]/aside[4]/aside/div[17]
                HtmlNode nodeForZipCodeAndCity = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[2]/div[2]/div[1]/div/main/div/div/div[1]/script/text()");
                //*[@id="wrapper"]/script[2]/text()
                dynamic data = JObject.Parse(nodeForZipCodeAndCity.InnerText);

                JContainer cityBrutInfo = data.about.Last;

                string zipCode = cityBrutInfo["address"]["postalCode"].ToString();
                string city = cityBrutInfo["address"]["addressLocality"].ToString();
                string idFromProvider = url.Split("/").Last().Split("-").Last();
                if (!String.IsNullOrWhiteSpace(city) && !String.IsNullOrWhiteSpace(zipCode))
                {
                    string priceTextLower = HtmlWordsHelper.CleanHtml(nodes[0].InnerText);
                    string priceTextMedium = HtmlWordsHelper.CleanHtml(nodes[1].InnerText);
                    string priceTextHigh = HtmlWordsHelper.CleanHtml(nodes[2].InnerText);

                    string mediumPrice = priceTextMedium.Split(" ").Length > 2 ? priceTextMedium.Split(" ")[2] : String.Empty;
                    string higherPrice = priceTextHigh.Split(" ").Length > 2 ? priceTextHigh.Split(" ")[2] : mediumPrice;
                    string lowerPrice = priceTextLower.Split(" ").Length > 2 ? priceTextLower.Split(" ")[2] : mediumPrice;
                    rentalInformations.Add(new RentalInformations()
                    {
                        CityInfo = new CityInformations
                        {
                            CityName = city,
                            ZipCode = zipCode
                        },
                        Price = lowerPrice,
                        RentalPriceType = RentalPriceType.LowerPrice,
                        RentalTypeOfTheRent = RentalTypeOfTheRent.Apartment,
                        Url = url,
                        IdFromProvider = idFromProvider
                    });
                    rentalInformations.Add(new RentalInformations()
                    {
                        CityInfo = new CityInformations
                        {
                            CityName = city,
                            ZipCode = zipCode
                        },
                        Price = priceTextMedium.Split(" ").Length > 2 ? priceTextMedium.Split(" ")[2] : String.Empty,
                        RentalPriceType = RentalPriceType.MediumPrice,
                        RentalTypeOfTheRent = RentalTypeOfTheRent.Apartment,
                        Url = url,
                        IdFromProvider = idFromProvider
                    });
                    rentalInformations.Add(new RentalInformations()
                    {
                        CityInfo = new CityInformations
                        {
                            CityName = city,
                            ZipCode = zipCode
                        },
                        Price = higherPrice,
                        RentalPriceType = RentalPriceType.HigherPrice,
                        RentalTypeOfTheRent = RentalTypeOfTheRent.Apartment,
                        Url = url,
                        IdFromProvider = idFromProvider
                    });
                }
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

                HtmlNode nodeInformation = document.DocumentNode.SelectSingleNode("//*[@id=\"loyer-maison\"]").NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;

                List<HtmlNode> nodes = nodeInformation.ChildNodes.Where(node => node.Name.Equals("div", StringComparison.InvariantCultureIgnoreCase)).ToList();
                HtmlNode nodeForZipCodeAndCity = document.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div[2]/div[2]/div[1]/div/main/div/div/div[1]/script/text()");
                //*[@id="wrapper"]/script[2]/text()
                dynamic data = JObject.Parse(nodeForZipCodeAndCity.InnerText);

                JContainer cityBrutInfo = data.about.Last;
                
                string zipCode = cityBrutInfo["address"]["postalCode"].ToString();
                string city = cityBrutInfo["address"]["addressLocality"].ToString();
                string idFromProvider = url.Split("/").Last().Split("-").Last();
                if (!String.IsNullOrWhiteSpace(city) && !String.IsNullOrWhiteSpace(zipCode))
                {
                    string priceTextLower = HtmlWordsHelper.CleanHtml(nodes[0].InnerText);
                    string priceTextMedium = HtmlWordsHelper.CleanHtml(nodes[1].InnerText);
                    string priceTextHigh = HtmlWordsHelper.CleanHtml(nodes[2].InnerText);

                    string mediumPrice = priceTextMedium.Split(" ").Length > 2 ? priceTextMedium.Split(" ")[2] : String.Empty;
                    string higherPrice = priceTextHigh.Split(" ").Length > 2 ? priceTextHigh.Split(" ")[2] : mediumPrice;
                    string lowerPrice = priceTextLower.Split(" ").Length > 2 ? priceTextLower.Split(" ")[2] : mediumPrice;
                    rentalInformations.Add(new RentalInformations()
                    {
                        CityInfo = new CityInformations
                        {
                            CityName = city,
                            ZipCode = zipCode
                        },
                        Price = lowerPrice,
                        RentalPriceType = RentalPriceType.LowerPrice,
                        RentalTypeOfTheRent = RentalTypeOfTheRent.House,
                        Url = url,
                        IdFromProvider = idFromProvider
                    });
                    rentalInformations.Add(new RentalInformations()
                    {
                        CityInfo = new CityInformations
                        {
                            CityName = city,
                            ZipCode = zipCode
                        },
                        Price = priceTextMedium.Split(" ").Length > 2 ? priceTextMedium.Split(" ")[2] : String.Empty,
                        RentalPriceType = RentalPriceType.MediumPrice,
                        RentalTypeOfTheRent = RentalTypeOfTheRent.House,
                        Url = url,
                        IdFromProvider = idFromProvider
                    });
                    rentalInformations.Add(new RentalInformations()
                    {
                        CityInfo = new CityInformations
                        {
                            CityName = city,
                            ZipCode = zipCode
                        },
                        Price = higherPrice,
                        RentalPriceType = RentalPriceType.HigherPrice,
                        RentalTypeOfTheRent = RentalTypeOfTheRent.House,
                        Url = url,
                        IdFromProvider = idFromProvider
                    });
                }


            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex);
            }
            return rentalInformations;
        }

        public void SearchByCityNameAndDepartementAndEnqueueUrl(string cityName, int departement)
        {
            LogHelper.LogInfo($" cityName : {cityName}, departement : {departement}");

            string textToWrite = $"{cityName} {departement.ToString("00")}";
            string baseUrl = "https://immobilier.lefigaro.fr/prix-immobilier/auvergne-rhone-alpes/region-84";

            ChromeOptions options = SeleniumHelper.DefaultChromeOption();
            using (IWebDriver driver = new RemoteWebDriver(options))
            {
                SeleniumHelper.GoAndWaitPageIsReady(driver, baseUrl);
                Actions actions = new Actions(driver);
                //if (driver.FindElement(By.ClassName("gJERWf")).Displayed)
                //{
                //    IWebElement element = driver.FindElement(By.ClassName("button__acceptAll"));
                //    actions.ScrollToElement(element);
                //    actions.Perform();
                //    actions.Click(element);
                //    actions.Perform();
                //}

                InteratiorHelper.ImitateHumanTyping(textToWrite, driver.FindElement(By.Name("q")));
                actions.SendKeys(Keys.Enter);
                actions.Perform();

                if (!base._cachingManager.GetRentalInformations().Any(rental => rental.Url.Equals(driver.Url, StringComparison.InvariantCultureIgnoreCase)))
                    _rentalRabbitMQBroker.SendMessage<string>(driver.Url);
            }
        }
    }
}
