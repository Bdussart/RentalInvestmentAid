//using RentalInvestmentAid.Core;
//using RentalInvestmentAid.Core.Announcement;
//using RentalInvestmentAid.Core.Announcement.Helper;
//using RentalInvestmentAid.Models.Loan;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace RentalInvestmentAid.Test
//{
//    [TestClass]
//    public class HeirsUnitTests
//    {
//        private List<IAnnouncementWebSiteData> _AnnouncementWebSiteDatas = new List<IAnnouncementWebSiteData>
//            {
//                { new Century21WebSiteData() },
//                { new LeBonCoinWebSiteData() },
//            };


//        [TestMethod]
//        public void FindTheRightHeirs_Century21()
//        {
//            string url = "sadfsaodfjasdgfhasfozuiasgfhasouifhaspofhasdpfoicEnTurY21asdgfsagasdgasg";

//            IAnnouncementWebSiteData? announcementWebSiteData =  HeirsHelper.FindTheRightHeir(url, _AnnouncementWebSiteDatas);

//            Assert.AreEqual(new Century21WebSiteData().GetKeyword(), announcementWebSiteData.GetKeyword());
//        }

//        [TestMethod]
//        public void FindTheRightHeirs_LeBonCoin()
//        {
//            string url = "www.Leboncoin.com/dfsgdsgsdgfsdgf/sdgsdgsdgsdgsdg/sdgsd/fgsd/gA/SDA/F";

//            IAnnouncementWebSiteData? announcementWebSiteData = HeirsHelper.FindTheRightHeir(url, _AnnouncementWebSiteDatas);

//            Assert.AreEqual(new LeBonCoinWebSiteData().GetKeyword(), announcementWebSiteData.GetKeyword());
//        }


//        [TestMethod]
//        public void FindTheRightHeirs_null()
//        {
//            string url = "www.Lceebnotnucroyi2n1.com/dfsgdsgsdgfsdgf/sdgsdgsdgsdgsdg/sdgsd/fgsd/gA/SDA/F";

//            IAnnouncementWebSiteData? announcementWebSiteData = HeirsHelper.FindTheRightHeir(url, _AnnouncementWebSiteDatas);

//            Assert.AreEqual(null, announcementWebSiteData);
//        }
//    }
//}