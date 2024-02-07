using RentalInvestmentAid.Models.Loan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models
{
    public class RealLoanCost
    {
        public int DurationInYear { get; set; }
        public string Price { get; set; } = String.Empty;
        public List<LoanInformation> LoanInformations { get; set; } = new List<LoanInformation>();
    }
}
