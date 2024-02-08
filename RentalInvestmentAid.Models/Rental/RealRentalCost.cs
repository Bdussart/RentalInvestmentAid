using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.Rental
{
    public class RealRentalCost
    {
        public RentalType Type { get;set; }
        public double PricePerSquareMeter { get; set; }
        public double RealPrice { get; set; }

        public double Rental70Pourcent
        {
            get
            {
                return RealPrice * 0.70;
            }
        }

        public bool? IsViable = null;
    }
}
