﻿using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.HouseOrApartement;
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
                        {FinancialCalcul.LoanInformation(Convert.ToDouble(bankInformation.MaxRate), bankInformation.DurationInYear, Double.Parse(amount), Convert.ToDouble(insurranceRate), LoanType.HigherRate ) },
                        {FinancialCalcul.LoanInformation(Convert.ToDouble(bankInformation.MarketRate), bankInformation.DurationInYear, Double.Parse(amount), Convert.ToDouble(insurranceRate), LoanType.MarketRate  ) },
                        {FinancialCalcul.LoanInformation(Convert.ToDouble(bankInformation.LowerRate), bankInformation.DurationInYear, Double.Parse(amount), Convert.ToDouble(insurranceRate), LoanType.LowerRate) }
                    }
                });

            });
            return realRentalCosts;
        }

        public void CalculAllRentalPrices(RentalInformations rentalInformation, ref HouseOrApartementInformation houseOrApartementInformation)
        {

            houseOrApartementInformation.SetRealRentalCost(new List<RealRentalCost>
            {
                { new RealRentalCost(){ PricePerSquareMeter = float.Parse(rentalInformation.LowerPrice, CultureInfo.InvariantCulture.NumberFormat), RealPrice = float.Parse(rentalInformation.LowerPrice, CultureInfo.InvariantCulture.NumberFormat) * Convert.ToDouble(houseOrApartementInformation.Metrage), Type = RentalType.LowerPrice } },
                { new RealRentalCost(){ PricePerSquareMeter = float.Parse(rentalInformation.MediumPrice,CultureInfo.InvariantCulture.NumberFormat), RealPrice = float.Parse(rentalInformation.MediumPrice, CultureInfo.InvariantCulture.NumberFormat) * Convert.ToDouble(houseOrApartementInformation.Metrage), Type = RentalType.MediumPrice }},
                { new RealRentalCost(){ PricePerSquareMeter =float.Parse(rentalInformation.HigherPrice,CultureInfo.InvariantCulture.NumberFormat), RealPrice = float.Parse(rentalInformation.HigherPrice, CultureInfo.InvariantCulture.NumberFormat) * Convert.ToDouble(houseOrApartementInformation.Metrage), Type = RentalType.HigherPrice }}
            });
        }


        public RentalResult CheckIfRentable(HouseOrApartementInformation houseOrApartementInformation, List<RealLoanCost> realLoanCosts)
        {
            RentalResult result = new RentalResult();
            result.LoanBaseAmout = Convert.ToDouble(houseOrApartementInformation.Price);
            result.LoanInformationWithRentalInformation = new List<LoanInformationWithRentalInformation>();
            result.HouseOrApartementInformation = houseOrApartementInformation; 
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
                    foreach (RealRentalCost rentalCost in houseOrApartementInformation.RentalCost)
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
