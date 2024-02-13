using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Rental;

namespace RentalInvestmentAid.Database
{
    public interface IDatabaseFactory
    {
        public List<RentalInformations> RentalInformations { get; }

        public void InsertRentalInformation(RentalInformations rental);

        public List<AnnouncementInformation> AnnouncementInformations { get; }

        public void InsertAnnouncementInformation(AnnouncementInformation announcementInformation);

        public List<RateInformation> RateInformations { get; }

        public void InsertRateInformation(RateInformation announcementInformation);
    }
}
