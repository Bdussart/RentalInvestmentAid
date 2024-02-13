
namespace RentalInvestmentAid.Models.Bank
{
    public class RateInformation
    {
        public int DurationInYear { get; set; }
        public string MaxRate { get; set; } = String.Empty;
        public string MarketRate { get; set; } = String.Empty;
        public string LowerRate { get; set; } = String.Empty;
    }
}
