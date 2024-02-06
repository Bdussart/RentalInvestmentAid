using HtmlAgilityPack;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Rental
{
    public class SeLogerWebSiteData : IRentalWebSiteData
    {
        public RentalInformations GetApartmentRentalInformation(string url)
        {
            throw new NotImplementedException();
        }

        public RentalInformations GetHouseRentalInformation(string url)
        {
            throw new NotImplementedException();
        }
    }
}
