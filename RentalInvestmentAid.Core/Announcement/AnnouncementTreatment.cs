using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Models.Announcement;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        public async IAsyncEnumerable<AnnouncementInformation> GetAnnouncementInformationWithRentabilityCalculatedAsync()
        {
            foreach (AnnouncementInformation annoucement in _cachingManager.GetAnnouncementInformationWithCityInformation().Where(ann => ann.RentabilityCalculated))
            {
                yield return annoucement;
                await Task.Delay(TimeSpan.FromMilliseconds(2)); //Get things slowly
            }
        }
        public async IAsyncEnumerable<AnnouncementInformation> GetAnnouncementInformationWithRentabilityCalculatedAndRentableAsync()
        {
            foreach (AnnouncementInformation annoucement in _cachingManager.GetAnnouncementInformationWithCityInformation().Where(ann => ann.RentabilityCalculated && (ann.IsRentable.HasValue && ann.IsRentable.Value)).OrderByDescending(a => a.CreatedDate))
            {
                yield return annoucement;
                await Task.Delay(TimeSpan.FromMilliseconds(2)); //Get things slowly
            }
        }

        public async IAsyncEnumerable<AnnouncementInformation> GetAnnouncementInformationWithNoRentCalculatedAsync()
        {
            foreach (AnnouncementInformation annoucement in _cachingManager.GetAnnouncementInformationWithCityInformation().Where(ann => !ann.RentabilityCalculated))
            {
                yield return annoucement;
                await Task.Delay(TimeSpan.FromMilliseconds(2)); //Get things slowly
            }
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

        public bool ExistAnnouncementByProviderAndProviderId(string providerAnnouncmentId, AnnouncementProvider announcementProvider)
        {
            AnnouncementInformation? val = _databaseFactory.GetAnnouncementsInformationsByProviderId((int)announcementProvider, providerAnnouncmentId);

            return val != null;
        }

        public void UpdateRentabilityInformation(int announcementId, bool isRentable)
        {
            _databaseFactory.UpdateRentabilityInformation(announcementId, isRentable);
            _cachingManager.ForceCacheUpdateAnnouncementInformation();
        }

        public async Task DeleteAnnoucementInformation(int announcementId)
        {
            await _databaseFactory.DeleteAnnouncementInformation(announcementId);
            _cachingManager.ForceCacheUpdateAnnouncementInformation();
        }
    }
}
