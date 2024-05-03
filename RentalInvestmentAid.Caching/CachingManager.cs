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
using RentalInvestmentAid.Models.City;
using RentalInvestmentAid.Models.Loan;

namespace RentalInvestmentAid.Caching
{
    public class CachingManager 
    {
        private readonly IMemoryCache _memoryCache;
        private IDatabaseFactory _databaseFactory;
        private Dictionary<string, DateTime> _cacheSettedInformation = new Dictionary<string, DateTime>();
        private TimeSpan _durationCache = TimeSpan.FromMinutes(2);

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
            DateTime dt = DateTime.Now;
            if (!_memoryCache.TryGetValue(key, out list) ||(_cacheSettedInformation.ContainsKey(key) && _cacheSettedInformation[key] < DateTime.Now))
            {
                if (_cacheSettedInformation.TryGetValue(key, out dt)){
                    _cacheSettedInformation.Remove(key);
                }
                list = func();
                SetCache(key, func());
                _cacheSettedInformation.Add(key, DateTime.Now.Add(_durationCache));
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

        #region Cities
        public List<CityInformations> GetCities()
        {
            return GetFromCache(SettingsManager.CityCacheKey,
                    () => _databaseFactory.GetCities().Cast<Object>().ToList()).Cast<CityInformations>().ToList();
        }

        public void ForceCacheUpdateCities()
        {
            SetCache(SettingsManager.CityCacheKey, _databaseFactory.GetCities().Cast<Object>().ToList());
        }

        #endregion

        #region Loan
        public List<LoanInformation> GetLoans()
        {
            return GetFromCache(SettingsManager.LoanCacheKey,
                    () => _databaseFactory.GetLoansInformations().Cast<Object>().ToList()).Cast<LoanInformation>().ToList();
        }
        public List<LoanInformation> GetLoansByAnnoncementId(int announcementId)
        {
            return GetLoans().Where(loan => loan.AnnouncementInformation.Id.Equals(announcementId)).ToList();
        }

        public void ForceCacheUpdateLoans()
        {
            SetCache(SettingsManager.LoanCacheKey, _databaseFactory.GetLoansInformations().Cast<Object>().ToList());
        }

        #endregion


        #region Rent
        public List<RentInformation> GetRents()
        {
            return GetFromCache(SettingsManager.RentCacheKey,
                    () => _databaseFactory.GetRentsInformations().Cast<Object>().ToList()).Cast<RentInformation>().ToList();
        }

        public List<RentInformation> GetRentsByAnnouncementId(int announcementId)
        {
            return GetRents().Where(rent => rent.AnnouncementInformation.Id.Equals(announcementId)).ToList();
        }

        public void ForceCacheUpdateRents()
        {
            SetCache(SettingsManager.RentCacheKey, _databaseFactory.GetRentsInformations().Cast<Object>().ToList());
        }

        #endregion
    }
}
