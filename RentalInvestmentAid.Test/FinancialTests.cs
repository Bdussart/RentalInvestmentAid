using RentalInvestmentAid.Core;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models.Rate;

namespace RentalInvestmentAid.Test
{
    [TestClass]
    public class FinancialTests
    {
        [TestMethod]
        public void NormalUseCase()
        {
            //ref : https://www.meilleurtaux.com/credit-immobilier/simulation-de-pret-immobilier/calcul-des-mensualites.html

            double rate = 3.85;
            int annualy = 15;
            double loan = 200000;
            double insurranceRate = 0.34;
            RateType type = RateType.HighRate;

            string expectedMensualyCost = "1521,05";
            string expectedTotalCost = "273789,7";

            LoanInformation info =  FinancialCalcul.LoanInformation(rate, annualy, loan, insurranceRate, type);

            Assert.AreEqual(rate, info.Rate);
            Assert.AreEqual(type, info.Type);
            Assert.AreEqual(insurranceRate, info.InsurranceRate);

            Assert.AreEqual(expectedMensualyCost, info.MonthlyCostWithInsurrance.ToString("#.##"));
            Assert.AreEqual(expectedTotalCost, info.TotalCostWithInsurrance.ToString("#.##"));
        }
    }
}