using RentalInvestmentAid.Models.Rental;

namespace RentalInvestmentAid.Core.Rental
{
    public interface IRentalWebSiteData
    {
        public RentalInformations GetApartmentRentalInformation(string url);
        public RentalInformations GetHouseRentalInformation(string url);
        public List<string> GetUrlForRentalInformation(string area, string department, int departmentNumber);
    }
}
