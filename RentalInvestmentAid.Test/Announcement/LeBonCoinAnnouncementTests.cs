using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Core.Announcement;
using RentalInvestmentAid.Test.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Test.Announcement
{
    [TestClass]
    public class LeBonCoinAnnouncementTests
    {
        private LeBonCoinWebSiteData _webSiteData;
        private CachingManager _cacheManager;
        private MockDatabase _mockDatabase;
        public LeBonCoinAnnouncementTests()
        {
            _mockDatabase = new MockDatabase();
            _cacheManager = new CachingManager(_mockDatabase);
            _webSiteData = new LeBonCoinWebSiteData(_cacheManager);
        }

        [TestMethod]
        public void Get_lot_of_urls()
        {
            List<string> departements = new List<string>()
            {
                "haute-savoie"
            };
            int? maxPrice = 200000;
            _webSiteData.GetAnnoucementUrl(departements, maxPrice);
        }
    }
}
