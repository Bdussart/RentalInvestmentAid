using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel.FinancialFunctions;

namespace RentalInvestmentAid.Core
{
    public static class FinancialCalcul
    {

        public static void test()
        {
           var value =  Financial.Pmt(4.46, 25 * 12, 200000, 0, 0);
        }
        
    }
}
