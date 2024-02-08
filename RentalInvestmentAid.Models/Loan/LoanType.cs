using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.Loan
{
    public enum LoanType
    {
        [Description("Taux Haut")]
        HigherRate = 0,

        [Description("Taux Moyen")]
        MarketRate = 1,

        [Description("Taux Bas")]
        LowerRate = 2
    }
}
