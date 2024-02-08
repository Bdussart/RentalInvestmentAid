using RentalInvestmentAid.Models.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Announcement
{
    public interface IAnnouncementWebSiteData
    {
        public AnnouncementInformation GetAnnouncementInformation(string url);

        public string GetKeyword();
    }
}
