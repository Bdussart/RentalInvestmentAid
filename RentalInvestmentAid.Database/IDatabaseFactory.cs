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
        public AnnouncementInformation? GetAnnouncementsInformationsByProviderId(int providerId, string announcmentProviderId);
        public List<RateInformation> GetRatesInformations();
        public List<LoanInformation> GetLoansInformations();
        public List<RentInformation> GetRentsInformations();
        public List<CityInformations> GetCities();
        public List<CityInformations> GetCitiesWithNoRent();
        public List<DepartmentToSearchData> GetDepartmentToSearchDatas();
        public RentalInformations InsertRentalInformation(RentalInformations rental);
        public AnnouncementInformation InsertAnnouncementInformation(AnnouncementInformation announcementInformation);
        public RateInformation InsertRateInformation(RateInformation announcementInformation);
        public LoanInformation InsertLoanInformation(LoanInformation loanInformation);
        public RentInformation InsertRentInformation(RentInformation rentInformation);
        public CityInformations InsertCity(CityInformations city);

        public DepartmentToSearchData InsertDepartment(DepartmentToSearchData departmentToSearchData);

        public void DeleteDepartment(int departmentId);

        public void UpdateRentabilityInformation(int announcementId, bool isRentable);
        public void InsertInformationProvidedByGeminiForAnAnnouncement(int announcementId, string geminiInformation);

        public Task DeleteAnnouncementInformation(int announcementId);
        public String GetMiscPerKey(string key);
    }
}
