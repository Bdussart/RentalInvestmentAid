using RentalInvestmentAid.Models.Rental;

namespace RentalInvestmentAid.Database
{
    public interface IDatabaseFactory
    {
        public List<RentalInformations> RentalInformations { get; }

        public void InsertRentalInformation(RentalInformations rental);
    }
}
