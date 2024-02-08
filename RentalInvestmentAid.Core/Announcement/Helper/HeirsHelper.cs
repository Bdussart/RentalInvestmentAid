using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Announcement.Helper
{
    public static class HeirsHelper
    {
        public static IAnnouncementWebSiteData? FindTheRightHeir(string url, List<IAnnouncementWebSiteData> datas)
        {
            return datas.Find(child => {
                return url.Contains(child.GetKeyword(), StringComparison.InvariantCultureIgnoreCase);
                }
            );
        }
    }
}
