using RentalInvestmentAid.Models.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.Rental
{
    public class RentInformation
    {
        public int Id { get; set; }
        public RentalInformations RentalInformations { get; set; }
        public AnnouncementInformation AnnouncementInformation{ get; set; }
        public double RentPrice { get; set; }
        public double Rental70Pourcent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
