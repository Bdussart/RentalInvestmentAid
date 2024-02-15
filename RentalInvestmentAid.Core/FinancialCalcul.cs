using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Excel.FinancialFunctions;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models.Rate;

namespace RentalInvestmentAid.Core
{
    public static class FinancialCalcul
    {
        public static LoanInformation LoanInformation(RateInformation rateInformation, double loan, double inssuranceRate)
        {
            double mensualRate = (Convert.ToDouble(rateInformation.Rate) / 100 )/ 12;
            int monthly = rateInformation.DurationInYear * 12;

            var monthlyCost = Financial.Pmt(mensualRate, monthly, loan, 0, 0) * -1;

            double totalInssuranceCost = loan * (inssuranceRate / 100) * rateInformation.DurationInYear;
            return new LoanInformation
            {
                MonthlyCost = monthlyCost,
                MonthlyCostWithInsurrance = monthlyCost + (totalInssuranceCost / monthly), 
                TotalCost = monthlyCost * monthly,
                TotalCostWithInsurrance = (monthlyCost * monthly) + totalInssuranceCost,
                InsurranceRate = inssuranceRate,
                RateInformation = rateInformation,
            };
        }
    }
}
