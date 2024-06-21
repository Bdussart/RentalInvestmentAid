using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Core.Announcement;
using RentalInvestmentAid.Core.Bank;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Queue;
using RentalInvestmentAid.Test.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Test.Announcement
{
    [TestClass]
    public class IADAnnouncementTests
    {
        private IADWebSite _webSiteData;
        private CachingManager _cacheManager;
        private MockDatabase _mockDatabase;
        private AnnouncementTreatment AnnouncementTreatment;
        private IBroker _broker;
        public IADAnnouncementTests()
        {
            _broker = new MockBroker();
            _mockDatabase = new MockDatabase();
            _cacheManager = new CachingManager(_mockDatabase);
            AnnouncementTreatment = new AnnouncementTreatment(_cacheManager, _mockDatabase);
            _webSiteData = new IADWebSite(AnnouncementTreatment, _broker);
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

        [TestMethod]
        public void Get_announcement_information()
        {
           AnnouncementInformation info =  _webSiteData.GetAnnouncementInformation("https://www.leboncoin.fr/offre/ventes_immobilieres/2403739123");


            Assert.AreEqual(AnnouncementProvider.LeBonCoin, info.AnnouncementProvider);
            Assert.AreEqual("2403739123", info.IdFromProvider);
            Assert.AreEqual("83", info.Metrage);
            Assert.AreEqual("250000", info.Price);
            Assert.AreEqual("Seyssel", info.CityInformations.CityName);
            Assert.AreEqual("74910", info.CityInformations.ZipCode);
            Assert.AreEqual("74", info.CityInformations.Departement);
        }
    }
}
