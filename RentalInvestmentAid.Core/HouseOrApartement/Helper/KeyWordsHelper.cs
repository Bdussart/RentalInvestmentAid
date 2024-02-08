using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.HouseOrApartement.Helper
{
    public static class KeyWordsHelper
    {
        private static List<string> HouseKeyWord = new List<string>
        {
            "Maison", "Villa", "proprité"
        };
        private static List<string> ApartementKeyWord = new List<string>
        {
            "appartement", "duplex", "triplex"
        };
        public static RentalTypeOfTheRent GetRentalType(string keyWord)
        {
            RentalTypeOfTheRent rentalType = RentalTypeOfTheRent.Apartment;
            if (HouseKeyWord.Any(s => keyWord.Contains(s, StringComparison.CurrentCultureIgnoreCase)))
                rentalType = RentalTypeOfTheRent.House;

            return rentalType;
        }
    }
}
