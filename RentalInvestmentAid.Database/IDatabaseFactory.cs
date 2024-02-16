using RentalInvestmentAid.Models;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models.Rental;

namespace RentalInvestmentAid.Database
{
    public interface IDatabaseFactory
    {

        /// <summary>
        /// TODO : BULK INSERT !!!!!!
        /// </summary>
        public List<RentalInformations> RentalInformations { get; }

        public void InsertRentalInformation(RentalInformations rental);

        public List<AnnouncementInformation> AnnouncementInformations { get; }

        public void InsertAnnouncementInformation(AnnouncementInformation announcementInformation);

        public List<RateInformation> RateInformations { get; }

        public void InsertRateInformation(RateInformation announcementInformation);

        public List<LoanInformation> LoansInformations { get; }

        public void InsertLoanInformation(LoanInformation loanInformation);

        public List<RentInformation> RentInformation { get; }
        public void InsertRentInformation(RentInformation rentInformation);
        public List<RentabilityResult>  RentabilityResults  {get;}
    }
}
