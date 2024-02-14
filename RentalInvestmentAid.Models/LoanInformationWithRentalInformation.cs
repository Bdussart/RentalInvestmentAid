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
    public class LoanInformationWithRentalInformation
    {
        public int DurationInYear { get; set; }
        public RateType Type { get; set; }
        public double Rate { get; set; }
        public double MonthlyCost { get; set; }
        public double TotalCost { get; set; }
        public double InsurranceRate { get; set; }
        public List<RealRentalCost>   RealRentalCosts { get; set; }
    }
}
