using Microsoft.Extensions.Caching.Memory;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Rental;
using RentalInvestmentAid.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalInvestmentAid.Models.Rental;
using RentalInvestmentAid.Models.Bank;

namespace RentalInvestmentAid.Caching
{
    public class CachingManager 
    {
        private readonly IMemoryCache _memoryCache;
        private IDatabaseFactory _databaseFactory;

        public CachingManager(IDatabaseFactory databaseFactory)
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _databaseFactory = databaseFactory;
        }
        #region Generic

        private void SetCache(string key, List<object> data)
        {
            _memoryCache.Set(key, data, TimeSpan.FromDays(1));
        }

        private List<object> GetFromCache(string key, Func<List<object>> func)
        {
            List<object> list = new List<object>();
            if (!_memoryCache.TryGetValue(key, out list))
            {
                list = func();
                SetCache(key, func());
            }
            return list;
        }


        #endregion

        #region RentalInformation

        public List<RentalInformations> GetRentalInformations()
        {
            return GetFromCache(SettingsManager.RentalCacheKey, 
                    () => _databaseFactory.GetRentalsInformations().Cast<Object>().ToList()).Cast<RentalInformations>().ToList();
        }

        public void ForceCacheUpdateRentalInformations()
        {
            SetCache(SettingsManager.RentalCacheKey, _databaseFactory.GetRentalsInformations().Cast<Object>().ToList());
        }


        #endregion

        #region AnnouncementInformation
        public List<AnnouncementInformation> GetAnnouncementInformation()
        {
            return GetFromCache(SettingsManager.AnnouncementCacheKey,
                    () => _databaseFactory.GetAnnouncementsInformations().Cast<Object>().ToList()).Cast<AnnouncementInformation>().ToList();
        }


        public void ForceCacheUpdateAnnouncementInformation()
        {
            SetCache(SettingsManager.AnnouncementCacheKey, _databaseFactory.GetAnnouncementsInformations().Cast<Object>().ToList());
        }

        #endregion


        #region RatesInformation
        public List<RateInformation> GetRatesInformation()
        {
            return GetFromCache(SettingsManager.RateCacheKey,
                    () => _databaseFactory.GetRatesInformations().Cast<Object>().ToList()).Cast<RateInformation>().ToList();
        }


        public void ForceCacheUpdateRatesInformation()
        {
            SetCache(SettingsManager.RateCacheKey, _databaseFactory.GetRatesInformations().Cast<Object>().ToList());
        }

        #endregion
    }
}
