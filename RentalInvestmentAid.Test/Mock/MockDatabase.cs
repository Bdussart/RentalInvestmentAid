using RentalInvestmentAid.Database;
using RentalInvestmentAid.Models;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.City;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Test.Mock
{
    internal class MockDatabase : IDatabaseFactory
    {
        public List<AnnouncementInformation> GetAnnouncementsInformations()
        {
            throw new NotImplementedException();
        }

        public List<CityInformations> GetCities()
        {
            throw new NotImplementedException();
        }

        public List<LoanInformation> GetLoansInformations()
        {
            throw new NotImplementedException();
        }

        public List<RateInformation> GetRatesInformations()
        {
            throw new NotImplementedException();
        }

        public List<RentabilityResult> GetRentabilitiesResults()
        {
            throw new NotImplementedException();
        }

        public List<RentalInformations> GetRentalsInformations()
        {
            throw new NotImplementedException();
        }

        public List<RentInformation> GetRentsInformations()
        {
            throw new NotImplementedException();
        }

        public AnnouncementInformation InsertAnnouncementInformation(AnnouncementInformation announcementInformation)
        {
            throw new NotImplementedException();
        }

        public CityInformations InsertCity(CityInformations city)
        {
            throw new NotImplementedException();
        }

        public LoanInformation InsertLoanInformation(LoanInformation loanInformation)
        {
            throw new NotImplementedException();
        }

        public RateInformation InsertRateInformation(RateInformation announcementInformation)
        {
            throw new NotImplementedException();
        }

        public RentalInformations InsertRentalInformation(RentalInformations rental)
        {
            throw new NotImplementedException();
        }

        public RentInformation InsertRentInformation(RentInformation rentInformation)
        {
            throw new NotImplementedException();
        }

        public void UpdateRentabilityInformation(int announcementId)
        {
            throw new NotImplementedException();
        }
    }
}
