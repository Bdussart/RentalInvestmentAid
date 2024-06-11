using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Core;
using RentalInvestmentAid.Core.Announcement;
using RentalInvestmentAid.Core.Announcement.Helper;
using RentalInvestmentAid.Core.Bank;
using RentalInvestmentAid.Core.Rental;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Logger;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.City;
using RentalInvestmentAid.Queue;
using RentalInvestmentAid.Settings;
using System.Text;

namespace TreatAnnouncementWorkerService
{
    public class TreatAnnouncementWoker : BackgroundService
    {
        private readonly ILogger<TreatAnnouncementWoker> _logger;
        private CachingManager _cachingManager = null;

        private IDatabaseFactory _databaseFactory = new SqlServerDatabase();
        private IBroker _announcementRabbitMQBroker = null;
        private List<IAnnouncementWebSiteData> _announcementWebSites = null;
        private CityTreatment _cityTreatment = null;
        private AnnouncementTreatment _announcementTreatment = null;
        private RentalTreament _rentalTreament = null;

        public TreatAnnouncementWoker(ILogger<TreatAnnouncementWoker> logger)
        {
            string rabbitMqHost = String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("RABBITMQ_HOST")) ? "localhost" : Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            _announcementRabbitMQBroker = new RabbitMQBroker(SettingsManager.AnnouncementQueueName, rabbitMqHost);
            _cachingManager = new CachingManager(_databaseFactory);
            _cityTreatment = new CityTreatment(_cachingManager, _databaseFactory);
            _announcementTreatment = new AnnouncementTreatment(_cachingManager, _databaseFactory);
            _rentalTreament = new RentalTreament(_cachingManager, _databaseFactory);

            _announcementWebSites = new List<IAnnouncementWebSiteData>
                {
                    {new Century21WebSiteData(_announcementTreatment, _announcementRabbitMQBroker) },
                    {new IADWebSite(_announcementTreatment, _announcementRabbitMQBroker) }
                };
            _logger = logger;
            HandleNewAnnouncementQueue();
        }

        private void HandleNewAnnouncementQueue()
        {
            EventingBasicConsumer consumer = _announcementRabbitMQBroker.GetConsumer();

            consumer.Received += ReceivedAnnouncementInformation;
            _announcementRabbitMQBroker.SetConsumer(consumer);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
        private void ReceivedAnnouncementInformation(object? model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var data = Encoding.UTF8.GetString(body);

            string message = JsonConvert.DeserializeObject<string>(data);

            LogHelper.LogInfo($"Received {message}");
            IAnnouncementWebSiteData worker = HeirsHelper.FindTheRightHeir(message, _announcementWebSites);
            if (worker != null)
            {
                LogHelper.LogInfo($"****** [{Task.CurrentId}] [{worker.GetKeyword()}]  Url to get data : {message}*****");
                Thread.Sleep(TimeSpan.FromSeconds(1));
                AnnouncementInformation? announcementInformation = worker.GetAnnouncementInformation(message);
                if (announcementInformation != null)
                {
                    CityInformations city = _cityTreatment.GetAndInsertIfNotExisiting(announcementInformation.CityInformations.CityName, announcementInformation.CityInformations.Departement, announcementInformation.CityInformations.ZipCode);
                    announcementInformation.CityInformations.Id = city.Id;
                    _announcementTreatment.InsertAnnouncementInformation(announcementInformation);
                    LogHelper.LogInfo($"******[{Task.CurrentId}] [{worker.GetKeyword()} - {message}]  Announcement : {announcementInformation?.ToString()} - Check rentability *****");
                    CheckDataRentabilityForAnnouncement(announcementInformation);
                    LogHelper.LogInfo($"****** [{Task.CurrentId}] [{worker.GetKeyword()} - {message}]  Announcement : {announcementInformation?.ToString()} - DONE Check rentability *****");
                }
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
