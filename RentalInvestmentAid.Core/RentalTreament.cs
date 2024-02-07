using RentalInvestmentAid.Models.HouseOrApartement;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core
{
    public class RentalTreament
    {
        public RentalInformations? FindRentalInformationForAnAnnoucement(List<RentalInformations> rentalInformations, HouseOrApartementInformation houseOrApartementInformation)
        {
            //SameCity name AND zipcode different not handled yet
            return rentalInformations.Find(rent => rent.City.Equals(houseOrApartementInformation.City, StringComparison.CurrentCultureIgnoreCase));
        } 
    }
}
