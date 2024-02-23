using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models.Rate;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models
{
    public class RentabilityResult
    {
        public int AnnouncementId { get; set; }
        public List<int> LoanIds { get; set; } = new List<int>();
        public List<int> RentsIds { get; set; } = new List<int>();
        public AnnouncementInformation AnnouncementInformation { get; set; }
        public List<LoanInformation> Loans { get; set; }
        public List<RentInformation> Rents { get; set; }
    }
}
