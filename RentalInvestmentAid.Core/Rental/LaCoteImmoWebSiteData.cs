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

namespace RentalInvestmentAid.Core.Rental
{
    public class LaCoteImmoWebSiteData : IRentalWebSiteData
    {

        //NOTE : Voici comment est fait le site lacoteImmo  :
        //https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/alby-sur-cheran/740002.htm
        //https://www.lacoteimmo.com/prix-de-l-immo/location/{région}/{département}/{n'importe quoi}/{département en chiffre}"74" +  {quatre digit incrémental}
        public List<RentalInformations> GetApartmentRentalInformation(string url)
        {
            List<RentalInformations> rentalInformations = new List<RentalInformations>();
            try { 
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument document = htmlWeb.Load(url);

            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/table[1]/tbody[1]/tr/td");

            HtmlNodeCollection nodeForZipCode = document.DocumentNode.SelectNodes("//*[@id=\"wrapper\"]/script[2]/text()");

            string zipCode = new string(nodeForZipCode[0].InnerText.Split("\n")[2].Where(char.IsDigit).ToArray());

            rentalInformations.Add(new RentalInformations()
            {
                City = nodes[0].InnerText,
                Price = nodes[1].InnerText.Split(" ")[0],
                RentalPriceType = RentalPriceType.LowerPrice,
                ZipCode = zipCode,
                RentalTypeOfTheRent = RentalTypeOfTheRent.Apartment,
                Url = url
            });

            rentalInformations.Add(new RentalInformations()
            {
                City = nodes[0].InnerText,
                Price = nodes[2].InnerText.Split(" ")[0],
                RentalPriceType = RentalPriceType.MediumPrice,
                ZipCode = zipCode,
                RentalTypeOfTheRent = RentalTypeOfTheRent.Apartment,
                Url = url
            });

            rentalInformations.Add(new RentalInformations()
            {
                City = nodes[0].InnerText,
                Price = nodes[3].InnerText.Split(" ")[0],
                RentalPriceType = RentalPriceType.HigherPrice,
                ZipCode = zipCode,
                RentalTypeOfTheRent = RentalTypeOfTheRent.Apartment,
                Url = url
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
            rentalInformations.Add(new RentalInformations()
            {
                City = nodeForCity[0].InnerText,
                Price = nodes[0].InnerText.Split(" ")[0],
                RentalPriceType = RentalPriceType.LowerPrice,
                ZipCode = zipCode,
                RentalTypeOfTheRent = RentalTypeOfTheRent.House,
                Url = url
            });
            rentalInformations.Add(new RentalInformations()
            {
                City = nodeForCity[0].InnerText,
                Price = nodes[1].InnerText.Split(" ")[0],
                RentalPriceType = RentalPriceType.MediumPrice,
                ZipCode = zipCode,
                RentalTypeOfTheRent = RentalTypeOfTheRent.House,
                Url = url
            });
            rentalInformations.Add(new RentalInformations()
            {
                City = nodeForCity[0].InnerText,
                Price = nodes[2].InnerText.Split(" ")[0],
                RentalPriceType = RentalPriceType.HigherPrice,
                ZipCode = zipCode,
                RentalTypeOfTheRent = RentalTypeOfTheRent.House,
                Url = url
            });

            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex);
            }
            return rentalInformations;
        }

        public List<string> GetUrlForRentalInformation(string area, string department, int departmentNumber)
        {
            List<string> urls = new List<string>();
            int iterator = 1;
            string baseUrl = string.Empty;
            string previousUrl = string.Empty;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--enable-javascript"); 
            options.AddArguments("--headless");
            //options.AddArgument("--window-size=500,1080");
            options.AddArgument("incognito");
            options.AddArgument("no-sandbox");


            bool next = true;
            do
            {
                using (IWebDriver driver = new ChromeDriver(options)) // why open a new driver in the loop ? => Because this website is heavy for the memory and the processor, i don't want to shutdown the website server and my computer :) 
                {
                    baseUrl = $"https://www.lacoteimmo.com/prix-de-l-immo/location/{area}/{department}/nothing/{departmentNumber}{iterator.ToString("0000")}.htm";
                    try                    
                    {
                        SeleniumHelper.GoAndWaitPageIsReady(driver, baseUrl);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogHelper.LogException(ex);
                    }

                    if ((baseUrl.Equals(driver.Url, StringComparison.CurrentCultureIgnoreCase)) || 
                        (previousUrl.Equals(driver.Url, StringComparison.CurrentCultureIgnoreCase)) ||
                        driver.Url == string.Empty)
                        next = false;
                    else
                    {
                        urls.Add(driver.Url);
                        previousUrl = driver.Url;
                    }
                    iterator++;
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            } while (next);


            return urls;
        }
    }
}
