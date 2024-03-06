using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Models.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Bank
{
    public class BankTreatment : MustInitializeCache
    {
        private IDatabaseFactory _databaseFactory;
        public BankTreatment(CachingManager cachingManager, IDatabaseFactory databaseFactory) : base(cachingManager)
        {
            _databaseFactory = databaseFactory;
            base._cachingManager = cachingManager;
        }

        public List<RateInformation> GetRates()
        {
            return _cachingManager.GetRatesInformation();
        }

        public List<RateInformation> GetRatesByTitle(string title)
        {
            return _cachingManager.GetRatesInformation().Where(rate => rate.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase)).ToList();
        }
        public bool AnyRatesByTitle(RateInformation rate)
        {
            return _cachingManager.GetRatesInformation().Any(rateInfo =>
            rateInfo.Title.Equals(rate.Title, StringComparison.CurrentCultureIgnoreCase) &&
            rateInfo.DurationInYear.Equals(rate.DurationInYear) &&
            rateInfo.Rate.Equals(rate.Rate, StringComparison.CurrentCultureIgnoreCase));
        }

        public RateInformation InsertRate(RateInformation rateInformation)
        {
           if (!AnyRatesByTitle(rateInformation))
            {
                rateInformation = _databaseFactory.InsertRateInformation(rateInformation);
                _cachingManager.ForceCacheUpdateRatesInformation();
            }
            return rateInformation;
        }       

    }
}
