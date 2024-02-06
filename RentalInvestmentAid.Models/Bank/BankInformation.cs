
namespace RentalInvestmentAid.Models.Bank
{
    public class BankInformation
    {
        public int Duration { get; set; }
        public string MaxRate { get; set; } = String.Empty;
        public string MarketRate { get; set; } = String.Empty;
        public string LowerRate { get; set; } = String.Empty;
    }
}
