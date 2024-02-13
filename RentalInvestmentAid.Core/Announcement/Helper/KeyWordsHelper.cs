using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Announcement.Helper
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
        private static List<string> LandKeyWord = new List<string>
        {
            "Terrain"
        };
        private static List<string> ParkingKeyWord = new List<string>
        {
            "Parking"
        };
        public static RentalTypeOfTheRent GetRentalType(string keyWord)
        {
            RentalTypeOfTheRent rentalType = RentalTypeOfTheRent.Other;
            if (HouseKeyWord.Any(s => keyWord.Contains(s, StringComparison.CurrentCultureIgnoreCase)))
                rentalType = RentalTypeOfTheRent.House;
            else if (ApartementKeyWord.Any(s => keyWord.Contains(s, StringComparison.CurrentCultureIgnoreCase)))
                rentalType = RentalTypeOfTheRent.Apartment;
            else if (LandKeyWord.Any(s => keyWord.Contains(s, StringComparison.CurrentCultureIgnoreCase)))
                rentalType = RentalTypeOfTheRent.Land;
            else if (ParkingKeyWord.Any(s => keyWord.Contains(s, StringComparison.CurrentCultureIgnoreCase)))
                rentalType = RentalTypeOfTheRent.Parking;

            return rentalType;
        }
    }
}
