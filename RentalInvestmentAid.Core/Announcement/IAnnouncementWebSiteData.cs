﻿using RentalInvestmentAid.Models.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Announcement
{
    public interface IAnnouncementWebSiteData 
    {        
        public AnnouncementInformation? GetAnnouncementInformation(string url);

        public List<String> GetAnnoucementUrl(List<string> departments = null, int? maxPrice = null);

        public string GetKeyword();

        public void EnQueueAnnoucementUrl(List<string> departments = null, int? maxPrice = null);
    }
}
