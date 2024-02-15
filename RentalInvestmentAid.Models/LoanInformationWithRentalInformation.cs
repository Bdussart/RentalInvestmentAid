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
        public LoanInformation LoanInformation { get; set; }
        public List<RentInformation>   RealRentalCosts { get; set; }
    }
}
