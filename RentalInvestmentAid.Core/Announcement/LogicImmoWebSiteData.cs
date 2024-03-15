using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalInvestmentAid.Caching;

namespace RentalInvestmentAid.Core.Announcement
{
    public class LogicImmoWebSiteData : MustInitializeCache, IAnnouncementWebSiteData
    {
        public LogicImmoWebSiteData(CachingManager cachingManager) : base(cachingManager)
        {
            base._cachingManager = cachingManager;
        }

        public void EnQueueAnnoucementUrl(List<string> departments = null, int? maxPrice = null)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAnnoucementUrl(List<string> department = null, int? maxPrice = null)
        {
            throw new NotImplementedException();
        }

        public AnnouncementInformation? GetAnnouncementInformation(string url)
        {
            throw new NotImplementedException();
        }

        public string GetKeyword()
        {
            return "LogicImmo";
        }
    }
}
