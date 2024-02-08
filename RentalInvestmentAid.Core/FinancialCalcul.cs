using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Excel.FinancialFunctions;
using RentalInvestmentAid.Models.Loan;

namespace RentalInvestmentAid.Core
{
    public static class FinancialCalcul
    {
        public static LoanInformation LoanInformation(double rate, int annualy, double loan, double inssuranceRate, LoanType type)
        {
            rate = rate / 100;
            double mensualRate = rate / 12;
            int monthly = annualy * 12;

            var monthlyCost = Financial.Pmt(mensualRate, monthly, loan, 0, 0) * -1;

            double totalInssuranceCost = loan * (inssuranceRate / 100) * annualy;
            return new LoanInformation
            {
                MonthlyCost = monthlyCost,
                MonthlyCostWithInsurrance = monthlyCost + (totalInssuranceCost / monthly), 
                TotalCost = monthlyCost * monthly,
                TotalCostWithInsurrance = (monthlyCost * monthly) + totalInssuranceCost,
                Rate = rate,
                Type = type,
                InsurranceRate = inssuranceRate
            };
        }
    }
}
