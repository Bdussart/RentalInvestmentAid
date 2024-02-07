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
        public RentalType RentalType { get; set; }
        public string City { get; set; } = String.Empty;
        public string ZipCode { get; set; } = String.Empty;
        public string Price { get; set; } = String.Empty;
        public string Metrage { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
    }
}
