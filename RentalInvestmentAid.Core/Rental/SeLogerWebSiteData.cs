﻿using HtmlAgilityPack;
using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Rental
{
    public class SeLogerWebSiteData : MustInitializeCache, IRentalWebSiteData
    {
        public SeLogerWebSiteData(CachingManager cachingManager) : base(cachingManager)
        {
            base._cachingManager = cachingManager;
        }

        public List<RentalInformations> GetApartmentRentalInformation(string url)
        {
            throw new NotImplementedException();
        }

        public List<RentalInformations> GetHouseRentalInformation(string url)
        {
            throw new NotImplementedException();
        }

        public void EnQueueUrls(string area, string department, int departmentNumber)
        {
            throw new NotImplementedException();
        }

        public void SearchByCityNameAndDepartementAndEnqueueUrl(string cityName, int departement)
        {
            throw new NotImplementedException();
        }
    }
}
