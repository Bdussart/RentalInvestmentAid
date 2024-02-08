using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.Rental
{
    public class RentalResult
    {
        public double LoanBaseAmout { get; set; }
        public AnnouncementInformation AnnouncementInformation { get; set; }
        public List<LoanInformationWithRentalInformation> LoanInformationWithRentalInformation { get; set; }

    }
}
