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
        public RateType Type { get; set; }
        public double Rate { get; set; }
        public double MonthlyCost { get; set; }
        public double MonthlyCostWithInsurrance { get; set; }
        public double TotalCost { get; set; }
        public double TotalCostWithInsurrance { get; set; }
        public double InsurranceRate { get;set; }
    }
}
