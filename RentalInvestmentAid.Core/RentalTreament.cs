using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models;

namespace RentalInvestmentAid.Core
{
    public class RentalTreament
    {
        public RentalInformations? FindRentalInformationForAnAnnoucement(List<RentalInformations> rentalInformations, AnnouncementInformation announcementInformation)
        {
            //SameCity name AND zipcode different not handled yet
            return rentalInformations.Find(rent => rent.City.Equals(announcementInformation.City, StringComparison.CurrentCultureIgnoreCase));
        }

        public List<RealLoanCost> CalculAllLoan(List<RateInformation> bankInformation, string amount, string insurranceRate = "0,30")
        {
            List<RealLoanCost> realRentalCosts = new List<RealLoanCost>();
            bankInformation.ForEach(bankInformation =>
            {
                realRentalCosts.Add(new RealLoanCost()
                {
                    DurationInYear = bankInformation.DurationInYear,
                    Price = amount,
                    LoanInformations = new List<LoanInformation>()
                    {
                        {FinancialCalcul.LoanInformation(Convert.ToDouble(bankInformation.Rate), bankInformation.DurationInYear, Double.Parse(amount), Convert.ToDouble(insurranceRate), bankInformation.RateType ) },
                    }
                });

            });
            return realRentalCosts;
        }

        public void CalculAllRentalPrices(RentalInformations rentalInformation, ref AnnouncementInformation announcementInformation)
        {

            announcementInformation.SetRealRentalCost(new List<RealRentalCost>
            {
                { new RealRentalCost(){ PricePerSquareMeter = float.Parse(rentalInformation.LowerPrice, CultureInfo.InvariantCulture.NumberFormat), RealPrice = float.Parse(rentalInformation.LowerPrice, CultureInfo.InvariantCulture.NumberFormat) * Convert.ToDouble(announcementInformation.Metrage), Type = RentalType.LowerPrice } },
                { new RealRentalCost(){ PricePerSquareMeter = float.Parse(rentalInformation.MediumPrice,CultureInfo.InvariantCulture.NumberFormat), RealPrice = float.Parse(rentalInformation.MediumPrice, CultureInfo.InvariantCulture.NumberFormat) * Convert.ToDouble(announcementInformation.Metrage), Type = RentalType.MediumPrice }},
                { new RealRentalCost(){ PricePerSquareMeter =float.Parse(rentalInformation.HigherPrice,CultureInfo.InvariantCulture.NumberFormat), RealPrice = float.Parse(rentalInformation.HigherPrice, CultureInfo.InvariantCulture.NumberFormat) * Convert.ToDouble(announcementInformation.Metrage), Type = RentalType.HigherPrice }}
            });
        }


        public RentalResult CheckIfRentable(AnnouncementInformation announcementInformation, List<RealLoanCost> realLoanCosts)
        {
            RentalResult result = new RentalResult();
            result.LoanBaseAmout = Convert.ToDouble(announcementInformation.Price);
            result.LoanInformationWithRentalInformation = new List<LoanInformationWithRentalInformation>();
            result.AnnouncementInformation = announcementInformation; 
            foreach (RealLoanCost realLoanCost in realLoanCosts)
            {
                foreach (LoanInformation loanInformation in realLoanCost.LoanInformations)
                {
                    LoanInformationWithRentalInformation loanInformationWithRentalInformation = new LoanInformationWithRentalInformation()
                    {
                        DurationInYear = realLoanCost.DurationInYear,
                        Rate = loanInformation.Rate,
                        Type = loanInformation.Type,
                        InsurranceRate = loanInformation.InsurranceRate,
                        MonthlyCost = loanInformation.MonthlyCost,
                        TotalCost = loanInformation.TotalCost
                    };

                    loanInformationWithRentalInformation.RealRentalCosts = new List<RealRentalCost>();
                    foreach (RealRentalCost rentalCost in announcementInformation.RentalCost)
                    {

                        loanInformationWithRentalInformation.RealRentalCosts.Add(new RealRentalCost()
                        {
                            Type = rentalCost.Type,
                            RealPrice = rentalCost.RealPrice,
                            PricePerSquareMeter = rentalCost.PricePerSquareMeter,
                            IsViable = rentalCost.Rental70Pourcent > loanInformationWithRentalInformation.MonthlyCost
                        });
                    }
                    result.LoanInformationWithRentalInformation.Add(loanInformationWithRentalInformation);
                }
            }

            

            return result;
        }
    }
}
