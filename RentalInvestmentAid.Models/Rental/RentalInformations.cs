namespace RentalInvestmentAid.Models.Rental
{
    public class RentalInformations
    {
        public int Id { get; set; }
        public string City { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string LowerPrice { get; set; } = string.Empty;
        public string MediumPrice { get; set; } = string.Empty;
        public string HigherPrice { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public RentalTypeOfTheRent RentalType;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
