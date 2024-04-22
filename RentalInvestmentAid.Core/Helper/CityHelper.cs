using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Helper
{
    public static class CityHelper
    {
        private static Dictionary<string, string> _DictionnaryOfKeyWordToClean = new Dictionary<string, string>() 
        {
            { "st", "saint"}
        };
        public static string CleanCityName(string cityName)
        {
            cityName = cityName.Replace("-", " ").Trim().ToLower();
            foreach(KeyValuePair<string, string> kvp in _DictionnaryOfKeyWordToClean)
                cityName = cityName.Replace(kvp.Key, kvp.Value);
            
            return cityName.ToLower();
        }
    }
}
