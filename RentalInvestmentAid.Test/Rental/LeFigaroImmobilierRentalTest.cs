using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Core.Rental;
using RentalInvestmentAid.Test.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Test.Rental
{
    [TestClass]
    public class LeFigaroImmobilierRentalTest
    {
        private leFigaroWebSiteData _LeFigaroWebSiteData;
        private CachingManager _cacheManager;
        private MockDatabase _mockDatabase;
        public LeFigaroImmobilierRentalTest() {
            _mockDatabase = new MockDatabase();
            _cacheManager = new CachingManager(_mockDatabase);
            _LeFigaroWebSiteData = new leFigaroWebSiteData(_cacheManager);
        }

        [TestMethod]
        public void GetCityUrl()
        {
            string city = "Argenteuil";
            int zipCode = 95100;

            _LeFigaroWebSiteData.SearchByCityNameAndDepartementAndEnqueueUrl(city, zipCode);
        }


    }
}
