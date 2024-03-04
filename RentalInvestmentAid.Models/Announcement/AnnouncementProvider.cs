using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.Announcement
{
    public enum AnnouncementProvider
    {
        [Description("Century21")]
        century21 = 1,
        [Description("Esprit-immo")]
        espritIimmo = 2

    }
}
