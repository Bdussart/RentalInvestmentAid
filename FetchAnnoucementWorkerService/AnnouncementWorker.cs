using RabbitMQ.Client.Events;
using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Core;
using RentalInvestmentAid.Core.Announcement;
using RentalInvestmentAid.Core.Announcement.Helper;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Logger;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.City;
using RentalInvestmentAid.Queue;
using RentalInvestmentAid.Settings;
using System.Diagnostics;
using System.Text;

namespace FetchAnnoucementWorkerService
{
    public class AnnouncementWorker : BackgroundService
    {
        private readonly ILogger<AnnouncementWorker> _logger;
        private  Dictionary<int, string> _dicoDepartements = new Dictionary<int, string>
            {
                {01, "Ain"},
                {22, "Côtes-d'Armor"},
                {29, "Finistère"},
                {34, "Hérault"},
                {35, "Ille-et-Vilaine"},
                {73, "Savoie"},
                {74, "Haute-Savoie"},
            };

        private  int? _maxPrice = 200000;
        private  List<IAnnouncementWebSiteData> _announcementWebSites = null;
        private  CityTreatment _cityTreatment = null;
        private  CachingManager _cachingManager = null;
        private  IDatabaseFactory _databaseFactory = new SqlServerDatabase();
        private  AnnouncementTreatment _announcementTreatment = null;
        private IBroker _announcementRabbitMQBroker = null;

        public AnnouncementWorker(ILogger<AnnouncementWorker> logger)
        {
            _logger = logger;
            _announcementRabbitMQBroker = new RabbitMQBroker(SettingsManager.AnnouncementQueueName);
            _cachingManager = new CachingManager(_databaseFactory);
            _cityTreatment = new CityTreatment(_cachingManager, _databaseFactory);
            _announcementTreatment = new AnnouncementTreatment(_cachingManager, _databaseFactory);

            _announcementWebSites = new List<IAnnouncementWebSiteData>
                {
                    {new Century21WebSiteData(_announcementTreatment, _announcementRabbitMQBroker) },
                    //{new LeBonCoinWebSiteData(_announcementTreatment) },
                    {new IADWebSite(_announcementTreatment, _announcementRabbitMQBroker) }
                };
            

            HandleNewAnnouncementQueue();
        }

        private  void HandleNewAnnouncementQueue()
        {
            EventingBasicConsumer consumer = _announcementRabbitMQBroker.GetConsumer();
            consumer.Received += ReceivedAnnouncementInformation;
            _announcementRabbitMQBroker.SetConsumer(consumer);
        }

        private  void ReceivedAnnouncementInformation(object? model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
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
                }
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (IAnnouncementWebSiteData announcementWebSite in _announcementWebSites)
                {
                    try
                    {
                        LogHelper.LogInfo($"******{Task.CurrentId} Start work for worker {announcementWebSite.GetKeyword()}* ****");
                        announcementWebSite.EnQueueAnnoucementUrl(_dicoDepartements.Values.ToList(), _maxPrice);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.LogInfo($"{Task.CurrentId}Damn an Exception ! {ex}");
                    }
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
