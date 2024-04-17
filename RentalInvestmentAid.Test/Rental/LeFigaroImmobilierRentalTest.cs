using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Core.Rental;
using RentalInvestmentAid.Models.Rental;
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
        private LeFigaroWebSiteData _LeFigaroWebSiteData;
        private CachingManager _cacheManager;
        private MockDatabase _mockDatabase;
        public LeFigaroImmobilierRentalTest() {
            _mockDatabase = new MockDatabase();
            _cacheManager = new CachingManager(_mockDatabase);
            _LeFigaroWebSiteData = new LeFigaroWebSiteData(_cacheManager);
        }

        [TestMethod]
        public void GetCityUrl()
        {
            string city = "Argenteuil";
            int zipCode = 95100;

            _LeFigaroWebSiteData.SearchByCityNameAndDepartementAndEnqueueUrl(city, zipCode);
            //Need to make Update on the Ctor to inject A MockOftheQueue
        }
        [TestMethod]
        public void GetHouseRentalInformation()
        {
            string url = "https://immobilier.lefigaro.fr/prix-immobilier/poncin/ville-01303";
            List<RentalInformations> rentailHouse =  _LeFigaroWebSiteData.GetHouseRentalInformation(url);


            Assert.AreEqual(3, rentailHouse.Count);
        }


    }
}
