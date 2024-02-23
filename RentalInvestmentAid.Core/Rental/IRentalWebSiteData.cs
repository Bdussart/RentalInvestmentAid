using RentalInvestmentAid.Models.Rental;

namespace RentalInvestmentAid.Core.Rental
{
    public interface IRentalWebSiteData
    {
        public List<RentalInformations> GetApartmentRentalInformation(string url);
        public List<RentalInformations> GetHouseRentalInformation(string url);
        public void EnQueueUrls(string area, string department, int departmentNumber);
    }
}
