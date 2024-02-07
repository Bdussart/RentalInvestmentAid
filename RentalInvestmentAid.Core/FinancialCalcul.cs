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
        public static LoanInformation LoanInformation(double rate, int annualy, double loan, double inssuranceRate)
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
                TotalCostWithInsurrance = (monthlyCost * monthly) + totalInssuranceCost

            };
        }
        public static void test()
        {
            var mensualRate = (0.0446 / 12);
            var monthly = 25 * 12;
            var loan = 200000;
            var value =  Financial.Pmt(mensualRate, monthly, loan, 0, 0) * -1;

            double mensualite = (loan * mensualRate * Math.Pow(1 + mensualRate, monthly)) / (Math.Pow(1 + mensualRate, monthly) - 1);

            var total = value * monthly;
        }

    }
}
