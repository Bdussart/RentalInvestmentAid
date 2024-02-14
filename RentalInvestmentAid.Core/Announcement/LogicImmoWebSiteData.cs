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

namespace RentalInvestmentAid.Core.Announcement
{
    public class LogicImmoWebSiteData : IAnnouncementWebSiteData
    {
        public List<string> GetAnnoucementUrl(List<string> department, int? maxPrice)
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
