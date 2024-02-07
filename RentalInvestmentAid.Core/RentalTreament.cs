using RentalInvestmentAid.Models;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.HouseOrApartement;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace RentalInvestmentAid.Core
{
    public class RentalTreament
    {
        public RentalInformations? FindRentalInformationForAnAnnoucement(List<RentalInformations> rentalInformations, HouseOrApartementInformation houseOrApartementInformation)
        {
            //SameCity name AND zipcode different not handled yet
            return rentalInformations.Find(rent => rent.City.Equals(houseOrApartementInformation.City, StringComparison.CurrentCultureIgnoreCase));
        }

        public List<RealLoanCost> CalculAllLoan(List<BankInformation> bankInformation, string amount, string insurranceRate = "0,30")
        {
            List<RealLoanCost> realRentalCosts = new List<RealLoanCost>();
            bankInformation.ForEach(bankInformation =>
            {
                realRentalCosts.Add(new RealLoanCost()
                {
                    DurationInYear = bankInformation.DurationInYear,
                    Price = amount,
                    LoanInformations = new List<Models.Loan.LoanInformation>()
                    {
                        {FinancialCalcul.LoanInformation(Convert.ToDouble(bankInformation.MaxRate), bankInformation.DurationInYear, Double.Parse(amount), Convert.ToDouble(insurranceRate) ) },
                        {FinancialCalcul.LoanInformation(Convert.ToDouble(bankInformation.MarketRate), bankInformation.DurationInYear, Double.Parse(amount), Convert.ToDouble(insurranceRate) ) },
                        {FinancialCalcul.LoanInformation(Convert.ToDouble(bankInformation.LowerRate), bankInformation.DurationInYear, Double.Parse(amount), Convert.ToDouble(insurranceRate) ) }
                    }
                });

            });
            return realRentalCosts;
        }

        public HouseOrApartementInformation CalculAllRentalPrices(RentalInformations rentalInformation, HouseOrApartementInformation houseOrApartementInformation)
        {

            houseOrApartementInformation.SetRealRentalCost(new List<RealRentalCost>
            {
                { new RealRentalCost(){ PricePerSquareMeter = float.Parse(rentalInformation.LowerPrice, CultureInfo.InvariantCulture.NumberFormat), RealPrice = float.Parse(rentalInformation.LowerPrice, CultureInfo.InvariantCulture.NumberFormat) * Convert.ToDouble(houseOrApartementInformation.Metrage) } },
                { new RealRentalCost(){ PricePerSquareMeter = float.Parse(rentalInformation.MediumPrice,CultureInfo.InvariantCulture.NumberFormat), RealPrice = float.Parse(rentalInformation.MediumPrice, CultureInfo.InvariantCulture.NumberFormat) * Convert.ToDouble(houseOrApartementInformation.Metrage) }},
                { new RealRentalCost(){ PricePerSquareMeter =float.Parse(rentalInformation.HigherPrice,CultureInfo.InvariantCulture.NumberFormat), RealPrice = float.Parse(rentalInformation.HigherPrice, CultureInfo.InvariantCulture.NumberFormat) * Convert.ToDouble(houseOrApartementInformation.Metrage) }}
            });

            return houseOrApartementInformation;
        }
    }
}
