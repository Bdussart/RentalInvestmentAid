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

namespace RentalInvestmentAid.Core.Rental
{
    public class leFigaroWebSiteData : MustInitializeCache, IRentalWebSiteData
    {
        public leFigaroWebSiteData(CachingManager cachingManager) : base(cachingManager)
        {
            base._cachingManager = cachingManager;
        }
        public void EnQueueUrls(string area, string department, int departmentNumber)
        {
            LogHelper.LogInfo($" Area : {area}, departement : {department}, departmentNumber : {departmentNumber}");
            int iterator = 1;
            string baseUrl = string.Empty;
            string previousUrl = string.Empty;

            ChromeOptions options = SeleniumHelper.DefaultChromeOption();
            //options.AddArguments("--headless");

            bool next = true;
            int tentative = 0;
            do
            {
                baseUrl = $"https://immobilier.lefigaro.fr/prix-immobilier/nothing/ville-{departmentNumber.ToString("00")}{iterator.ToString("000")}";
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
                            if (!base._cachingManager.GetRentalInformations().Any(rental => rental.Url.Equals(driver.Url, StringComparison.InvariantCultureIgnoreCase)))
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

        public List<RentalInformations> GetApartmentRentalInformation(string url)
        {
            throw new NotImplementedException();
        }

        public List<RentalInformations> GetHouseRentalInformation(string url)
        {
            throw new NotImplementedException();
        }

        public void SearchByCityNameAndDepartementAndEnqueueUrl(string cityName, int departement)
        {
            LogHelper.LogInfo($" cityName : {cityName}, departement : {departement}");

            string textToWrite = $"{cityName} {departement.ToString("00")}";
            string baseUrl = "https://immobilier.lefigaro.fr/prix-immobilier/auvergne-rhone-alpes/region-84";

            ChromeOptions options = SeleniumHelper.DefaultChromeOption();
            using (IWebDriver driver = new ChromeDriver(options))
            {
                SeleniumHelper.GoAndWaitPageIsReady(driver, baseUrl);
                Actions actions = new Actions(driver);
                if (driver.FindElement(By.ClassName("gJERWf")).Displayed)
                {
                    IWebElement element = driver.FindElement(By.ClassName("button__acceptAll"));
                    actions.ScrollToElement(element);
                    actions.Perform();
                    actions.Click(element);
                    actions.Perform();
                }

                
            }
        }
    }
}
