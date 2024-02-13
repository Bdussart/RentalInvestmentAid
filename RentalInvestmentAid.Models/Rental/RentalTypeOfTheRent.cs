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

    public enum RentalType
    {
        [Description("Location Basse")]
        LowerPrice = 0,
        [Description("Location Moyenne")]
        MediumPrice = 1,
        [Description("Location Haute")]
        HigherPrice = 2
    }
}
