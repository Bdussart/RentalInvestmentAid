using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.HouseOrApartement
{
    public class HouseOrApartementInformation
    {
        public RentalTypeOfTheRent RentalType { get; set; }
        public string City { get; set; } = String.Empty;
        public string ZipCode { get; set; } = String.Empty;
        public string Price { get; set; } = String.Empty;
        public string Metrage { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

        public string UrlWebSite { get; set; } = String.Empty;
        public List<RealRentalCost> RentalCost { get; private set; } = new List<RealRentalCost>();

        public void SetRealRentalCost(List<RealRentalCost> realRentalCosts)
        {
            RentalCost = realRentalCosts;
        }
    }
}
