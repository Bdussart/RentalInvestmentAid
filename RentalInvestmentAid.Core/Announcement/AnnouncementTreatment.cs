using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Models.Announcement;

namespace RentalInvestmentAid.Core.Announcement
{
    public class AnnouncementTreatment : MustInitializeCache
    {
        private IDatabaseFactory _databaseFactory;

        public AnnouncementTreatment(CachingManager cachingManager, IDatabaseFactory databaseFactory) : base(cachingManager)
        {
            _databaseFactory = databaseFactory;
            base._cachingManager = cachingManager;
        }

        private void UpdateCityInformation(List<AnnouncementInformation> announcementsInformation)
        {
            announcementsInformation.ForEach(ann =>
            {
                ann.CityInformations = _cachingManager.GetCities().FirstOrDefault(city => ann.CityInformations.Id == city.Id);
            });
        }

        public List<AnnouncementInformation> GetAnnouncementsInformation()
        {
            List<AnnouncementInformation> announcementsInformation = _cachingManager.GetAnnouncementInformation();

            UpdateCityInformation(announcementsInformation);
            return announcementsInformation;
        }

        public List<AnnouncementInformation> GetAnnouncementInformationByCityId(int cityId)
        {
            List<AnnouncementInformation> announcementsInformation = _cachingManager.GetAnnouncementInformation().Where(ann => ann.CityInformations.Id == cityId).ToList();

            UpdateCityInformation(announcementsInformation);
            return announcementsInformation;
        }

        public List<AnnouncementInformation> GetAnnouncementInformationWithRentabilityCalculated()
        {
            List<AnnouncementInformation> announcementsInformation = _cachingManager.GetAnnouncementInformation().Where(ann => ann.RentabilityCalculated).ToList();

            UpdateCityInformation(announcementsInformation);
            return announcementsInformation;
        }
        public List<AnnouncementInformation> GetAnnouncementInformationWithRentabilityNotCalculated()
        {
            List<AnnouncementInformation> announcementsInformation = _cachingManager.GetAnnouncementInformation().Where(ann => !ann.RentabilityCalculated).ToList();

            UpdateCityInformation(announcementsInformation);
            return announcementsInformation;
        }

        public AnnouncementInformation InsertAnnouncementInformation(AnnouncementInformation announcementInformation)
        {
            announcementInformation = _databaseFactory.InsertAnnouncementInformation(announcementInformation);
            _cachingManager.ForceCacheUpdateAnnouncementInformation();
            return announcementInformation;
        }

        public bool ExistAnnouncementByProviderAndProviderId(string providerId, AnnouncementProvider announcementProvider)
        {
            bool result = false;

            if (_cachingManager.GetAnnouncementInformation().Any(ann => ann.IdFromProvider.Equals(providerId) && ann.AnnouncementProvider.Equals(announcementProvider)))
                result = true;

            return result;
        }

        public void UpdateRentabilityInformation(int announcementId)
        {
            _databaseFactory.UpdateRentabilityInformation(announcementId);
            _cachingManager.ForceCacheUpdateAnnouncementInformation();
        }
    }
}
