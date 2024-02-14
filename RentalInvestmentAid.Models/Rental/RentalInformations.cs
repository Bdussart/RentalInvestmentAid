namespace RentalInvestmentAid.Models.Rental
{
    public class RentalInformations
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public RentalPriceType RentalPriceType { get; set; }
        public RentalTypeOfTheRent RentalTypeOfTheRent { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
