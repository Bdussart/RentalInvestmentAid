using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Rate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.Loan
{
    public class LoanInformation
    {
        public int Id { get; set; }
        public AnnouncementInformation AnnouncementInformation {  get; set; }
        public RateInformation RateInformation { get; set; }
        public double MonthlyCost { get; set; }
        public double MonthlyCostWithInsurrance { get; set; }
        public double TotalCost { get; set; }
        public double TotalCostWithInsurrance { get; set; }
        public double InsurranceRate { get;set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
