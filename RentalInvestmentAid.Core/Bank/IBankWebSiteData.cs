
using RentalInvestmentAid.Models.Bank;

namespace RentalInvestmentAid.Core.Bank
{
    public interface IBankWebSiteData
    {
        public List<BankInformation> GetRatesInformations(string url);
    }
}
