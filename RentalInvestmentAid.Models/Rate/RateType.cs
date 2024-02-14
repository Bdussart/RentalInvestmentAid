using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.Rate
{
    public enum RateType
    {
        [Description("Taux Bas")]
        LowRate = 1,
        [Description("Taux Moyen")]
        MediumRate = 2,
        [Description("Taux Haut")]
        HighRate = 3
    }
}
