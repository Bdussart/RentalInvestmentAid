using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Rental;
using RentalInvestmentAid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Caching;
using OpenQA.Selenium.DevTools.V119.SystemInfo;
using RentalInvestmentAid.Core.Announcement;
using RentalInvestmentAid.Logger;
using RentalInvestmentAid.Core.Rental;
using RentalInvestmentAid.Models.City;
using OpenQA.Selenium.DevTools.V119.Fetch;

namespace RentalInvestmentAid.Core
{
    public class RentalTreament : MustInitializeCache
    {
        private IDatabaseFactory _databaseFactory;
        private IRentalWebSiteData _rentalWebSiteData;
        public RentalTreament(CachingManager cachingManager, IDatabaseFactory databaseFactory, IRentalWebSiteData rentalWebSiteData) : base(cachingManager)
        {
            base._cachingManager = cachingManager;
            _databaseFactory = databaseFactory;
            _rentalWebSiteData = rentalWebSiteData;
        }
        private List<RentalInformations> FindRentalInformationForAnAnnoucement(AnnouncementInformation announcementInformation)
        {
            return _cachingManager.GetRentalInformations().Where(rent => rent.CityInfo.Id == announcementInformation.CityInformations.Id
                && announcementInformation.RentalType == rent.RentalTypeOfTheRent).ToList();
        }

        private List<LoanInformation> CalculAllLoan(string amount, string insurranceRate = "0,30")
        {
            List<LoanInformation> loanInformation = new List<LoanInformation>();
            _cachingManager.GetRatesInformation().ForEach(rateInformation =>
            {
                loanInformation.Add(FinancialCalcul.LoanInformation(rateInformation, Double.Parse(amount), Convert.ToDouble(insurranceRate)));
            });
            return loanInformation;
        }

        private List<RentInformation> CalculAllRentalPrices(List<RentalInformations> rentalInformation, AnnouncementInformation announcementInformation)
        {
            string metrage = announcementInformation.Metrage;
            List<RentInformation> realCost = new List<RentInformation>();
            rentalInformation.ForEach(rentalInformation =>
            {
                double price = Convert.ToDouble(rentalInformation.Price) * Convert.ToDouble(metrage.Replace(".", ","));
                realCost.Add(new RentInformation()
                {
                    AnnouncementInformation = announcementInformation,
                    RentalInformations = rentalInformation,
                    RentPrice = price,
                    Rental70Pourcent = price * 0.70
                });
            });

            return realCost;
        }

        public void UpdateCitiesRentInformations()
        {
            foreach (CityInformations city in _databaseFactory.GetCitiesWithNoRent())
            {
                LogHelper.LogInfo($"****** Search City {city.CityName} rental information *****");
                _rentalWebSiteData.SearchByCityNameAndDepartementAndEnqueueUrl(city.CityName, int.Parse(city.Departement));
            }
        }

        public bool CheckDataRentabilityForAnnouncement(AnnouncementInformation announcement)
        {
            bool rentabilityChecked = false;
            List<RentalInformations> currentsRentalInformation = FindRentalInformationForAnAnnoucement(announcement);

            LogHelper.LogInfo("****** Find the right rental information Check if not null *****");
            if (currentsRentalInformation.Count == 0) { 
                LogHelper.LogInfo($"{announcement} - Don't find rental information -----");
            }
            else
            {
                List<LoanInformation> loansInformation = CalculAllLoan(announcement.Price);

                foreach (LoanInformation loan in loansInformation)
                {
                    loan.AnnouncementInformation = announcement;
                    _databaseFactory.InsertLoanInformation(loan);
                }

                _cachingManager.ForceCacheUpdateLoans();
                List<RentInformation> realRentalCosts = CalculAllRentalPrices(currentsRentalInformation, announcement);
                foreach (RentInformation rent in realRentalCosts)
                {
                    rent.AnnouncementInformation = announcement;
                    _databaseFactory.InsertRentInformation(rent);
                }
                _cachingManager.ForceCacheUpdateRents();

                rentabilityChecked = true;
            }

            return rentabilityChecked;
        }

        public bool isRentable(AnnouncementInformation announcement)
        {
            bool isRentable = false;

            List<LoanInformation> loansInformation = _cachingManager.GetLoansByAnnoncementId(announcement.Id);
            List<RentInformation> realRentalCosts = _cachingManager.GetRentsByAnnouncementId(announcement.Id);
            if(loansInformation.Any() && realRentalCosts.Any())
            {
                for (int i = 0; i < realRentalCosts.Count && !isRentable; ++i)
                { 
                    isRentable = loansInformation.Exists(loan => realRentalCosts[i].Rental70Pourcent > loan.MonthlyCostWithInsurrance);
                }
            }

            return isRentable;
        }

        public List<LoanInformation> GetLoansForAnnouncementId(int announcementId)
        {
            return _cachingManager.GetLoans().Where(loan => loan.AnnouncementInformation.Id == announcementId).ToList();
        }

        public List<RentInformation> GetRentsForAnnouncementId(int announcementId)
        {
            return _cachingManager.GetRents().Where(rent => rent.AnnouncementInformation.Id == announcementId).ToList();
        }

    }
}
