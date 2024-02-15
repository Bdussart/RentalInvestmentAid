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
        public List<RentalInformations> FindRentalInformationForAnAnnoucement(List<RentalInformations> rentalInformations, AnnouncementInformation announcementInformation)
        {
            //SameCity name AND zipcode different not handled yet
            return rentalInformations.Where(rent => rent.City.Equals(announcementInformation.City, StringComparison.CurrentCultureIgnoreCase) 
            && announcementInformation.RentalType == rent.RentalTypeOfTheRent).ToList();
        }

        public List<LoanInformation> CalculAllLoan(List<RateInformation> ratesInformation, string amount, string insurranceRate = "0,30")
        {
            List<LoanInformation> loanInformation = new List<LoanInformation>();
            ratesInformation.ForEach(rateInformation =>
            {
                loanInformation.Add(FinancialCalcul.LoanInformation(rateInformation, Double.Parse(amount), Convert.ToDouble(insurranceRate)));
            });
            return loanInformation;
        }

        public List<RealRentalCost> CalculAllRentalPrices(List<RentalInformations> rentalInformation, AnnouncementInformation announcementInformation)
        {
            string metrage = announcementInformation.Metrage;
            List<RealRentalCost> realCost = new List<RealRentalCost>();
            rentalInformation.ForEach(rentalInformation =>
            {
                realCost.Add(new RealRentalCost()
                {
                    PricePerSquareMeter = Convert.ToDouble(rentalInformation.Price),
                    RealPrice = Convert.ToDouble(rentalInformation.Price) * Convert.ToDouble(metrage),
                    Type = rentalInformation.RentalPriceType
                });
            });

            return realCost;
        }


        public RentalResult CheckIfRentable(string price, List<RealRentalCost> realRentalCosts, List<LoanInformation> loansInformation)
        {
            RentalResult result = new RentalResult();
            result.LoanInformationWithRentalInformation = new List<LoanInformationWithRentalInformation>();
            foreach (LoanInformation lLoanInformations in loansInformation)
            {
                LoanInformationWithRentalInformation loanInformationWithRentalInformation = new LoanInformationWithRentalInformation()
                {
                    LoanInformation = lLoanInformations
                };

                    loanInformationWithRentalInformation.RealRentalCosts = new List<RealRentalCost>();
                    foreach (RealRentalCost rentalCost in realRentalCosts)
                    {

                        loanInformationWithRentalInformation.RealRentalCosts.Add(new RealRentalCost()
                        {
                            Type = rentalCost.Type,
                            RealPrice = rentalCost.RealPrice,
                            PricePerSquareMeter = rentalCost.PricePerSquareMeter,
                            IsViable = rentalCost.Rental70Pourcent > loanInformationWithRentalInformation.LoanInformation.MonthlyCost
                        });
                    }
                    result.LoanInformationWithRentalInformation.Add(loanInformationWithRentalInformation);
                
            }

            

            return result;
        }
    }
}
