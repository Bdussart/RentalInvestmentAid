
namespace RentalInvestmentAid.Models.City
{
    public class CityInformations
    {
        public int Id { get; set; }
        public string CityName { get; set; } = String.Empty;
        public string ZipCode { get; set; } = String.Empty;
        public string Departement { get; set; } = String.Empty;

        public DateTime CreatedDate { get; set; } 
    }
}
