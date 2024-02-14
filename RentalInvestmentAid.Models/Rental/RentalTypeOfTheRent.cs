using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.Rental
{
    public enum RentalTypeOfTheRent
    {
        Apartment = 1,
        House = 2,
        Land = 3,
        Parking = 4,
        Other = 4
    }

    public enum RentalPriceType
    {
        [Description("Location Basse")]
        LowerPrice = 1,
        [Description("Location Moyenne")]
        MediumPrice = 2,
        [Description("Location Haute")]
        HigherPrice = 3
    }
}
