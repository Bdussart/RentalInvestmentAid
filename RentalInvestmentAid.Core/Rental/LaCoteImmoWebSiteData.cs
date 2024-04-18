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
using OpenQA.Selenium.Remote;
using RentalInvestmentAid.Models.City;
using System.Runtime;
using OpenQA.Selenium.Interactions;
using RentalInvestmentAid.Core.Announcement.Helper;

namespace RentalInvestmentAid.Core.Rental
{

    /// <summary>
    /// DOWN .... est passé sur Seloger.com => protegé contre le WebScrapping
    /// </summary>

    public class LaCoteImmoWebSiteData : MustInitializeCache, IRentalWebSiteData
    {
        public LaCoteImmoWebSiteData(CachingManager cachingManager) : base(cachingManager)
        {
            base._cachingManager = cachingManager;
        }

        public List<RentalInformations> GetApartmentRentalInformation(string url)
        {
            throw new NotImplementedException();
        }

        public List<RentalInformations> GetHouseRentalInformation(string url)
        {

            throw new NotImplementedException();
        }


        public void EnQueueUrls(string area, string department, int departmentNumber)
        {
            throw new NotImplementedException();

        }

        public void SearchByCityNameAndDepartementAndEnqueueUrl(string cityName, int departement)
        {
            throw new NotImplementedException();
        }
    }
}