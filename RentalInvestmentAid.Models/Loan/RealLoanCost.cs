using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.Loan
{
    public class RealLoanCost
    {
        public int DurationInYear { get; set; }
        public string Price { get; set; } = string.Empty;
        public List<LoanInformation> LoanInformations { get; set; } = new List<LoanInformation>();
    }
}
