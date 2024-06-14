using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Core;
using RentalInvestmentAid.Core.Announcement;
using RentalInvestmentAid.Core.Rental;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Models.Announcement;

namespace RentWorkerService
{
    public class RentCalculWorkerService : BackgroundService
    {
        private readonly ILogger<RentCalculWorkerService> _logger;
        private CachingManager _cachingManager = null;
        private AnnouncementTreatment _announcementTreatment = null;
        private RentalTreament _rentalTreament = null;

        private IDatabaseFactory _databaseFactory = new SqlServerDatabase();

        public RentCalculWorkerService(ILogger<RentCalculWorkerService> logger)
        {
            _cachingManager = new CachingManager(_databaseFactory);
            _announcementTreatment = new AnnouncementTreatment(_cachingManager, _databaseFactory);
            _rentalTreament = new RentalTreament(_cachingManager, _databaseFactory);
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                CheckAllDataRentability();
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }

        private void CheckAllDataRentability()
        {
            foreach (AnnouncementInformation announcement in _announcementTreatment.GetAnnouncementInformationWithRentabilityNotCalculated())
            {
                CheckDataRentabilityForAnnouncement(announcement);
                Thread.Sleep(TimeSpan.FromMilliseconds(2));
            }
        }

        private void CheckDataRentabilityForAnnouncement(AnnouncementInformation announcement)
        {
            if (_rentalTreament.CheckDataRentabilityForAnnouncement(announcement))
            {
                _announcementTreatment.UpdateRentabilityInformation(announcement.Id, _rentalTreament.isRentable(announcement));
            }
        }
    }
}
