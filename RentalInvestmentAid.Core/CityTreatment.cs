﻿using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Models.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core
{
    public class CityTreatment : MustInitializeCache
    {

        private IDatabaseFactory _databaseFactory;
        public CityTreatment(CachingManager cachingManager, IDatabaseFactory databaseFactory) : base(cachingManager)
        {
            _cachingManager = cachingManager;
            _databaseFactory = databaseFactory;
        }

        public CityInformations? GetCity(string cityName, string departement)
        {

            CityInformations? result = _cachingManager.GetCities().FirstOrDefault(city =>
                              (city.CityName.Equals(cityName, StringComparison.OrdinalIgnoreCase))
                              && city.Departement.Equals(departement));

            return result;
        }

        public CityInformations InsertCity(CityInformations city)
        {
            city = _databaseFactory.InsertCity(city);
            _cachingManager.ForceCacheUpdateCities();
            return city;
            
        }

        public CityInformations GetAndInsertIfNotExisiting(string cityName, string departement, string zipCode) {
            
            CityInformations? city = GetCity(cityName, departement);

            if(city is null)
            {
                city = InsertCity(new CityInformations
                {
                    CityName = cityName,
                    Departement = departement,
                    ZipCode = zipCode,
                });
            }

            return city;        
        }
    }
}
