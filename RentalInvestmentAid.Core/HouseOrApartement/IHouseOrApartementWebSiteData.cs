using RentalInvestmentAid.Models.HouseOrApartement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.HouseOrApartement
{
    public interface IHouseOrApartementWebSiteData
    {
        public HouseOrApartementInformation GetHouseOrApartementInformation(string url);
    }
}
