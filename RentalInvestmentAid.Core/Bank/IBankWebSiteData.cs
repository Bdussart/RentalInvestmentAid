
using RentalInvestmentAid.Models.Bank;

namespace RentalInvestmentAid.Core.Bank
{
    public interface IBankWebSiteData
    {
        public List<RateInformation> GetRatesInformations(string url);
    }
}
