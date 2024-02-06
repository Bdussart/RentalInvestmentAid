using RentalInvestmentAid.Models.Rental;

namespace RentalInvestmentAid.Core.Rental
{
    public interface IRentalWebSiteData
    {
        public RentalInformations GetApartmentRentalInformation(string url);
        public RentalInformations GetHouseRentalInformation(string url);
    }
}
