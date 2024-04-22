using RentalInvestmentAid.Models;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.City;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models.Rental;

namespace RentalInvestmentAid.Database
{
    public interface IDatabaseFactory
    {

        /// <summary>
        /// TODO : BULK INSERT !!!!!!
        /// </summary>

        public List<RentalInformations> GetRentalsInformations();
        public List<RentabilityResult> GetRentabilitiesResults();
        public List<AnnouncementInformation> GetAnnouncementsInformations();
        public List<RateInformation> GetRatesInformations();
        public List<LoanInformation> GetLoansInformations();
        public List<RentInformation> GetRentsInformations();

        public List<CityInformations> GetCities();
        public List<CityInformations> GetCitiesWithNoRent();
        public RentalInformations InsertRentalInformation(RentalInformations rental);
        public AnnouncementInformation InsertAnnouncementInformation(AnnouncementInformation announcementInformation);
        public RateInformation InsertRateInformation(RateInformation announcementInformation);
        public LoanInformation InsertLoanInformation(LoanInformation loanInformation);
        public RentInformation InsertRentInformation(RentInformation rentInformation);
        public CityInformations InsertCity(CityInformations city);

        public void UpdateRentabilityInformation(int announcementId);
    }
}
