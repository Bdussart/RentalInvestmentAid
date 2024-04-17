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
using System.Diagnostics;
using System.Text;

namespace FetchAnnoucementWorkerService
{
    public class AnnouncementWorker : BackgroundService
    {
        private readonly ILogger<AnnouncementWorker> _logger;
        private static Dictionary<int, string> _dicoDepartements = new Dictionary<int, string>
            {
                {01, "Ain"},
                {73, "Savoie"},
                {74, "Haute-Savoie"},
            };

        private static int? _maxPrice = 200000;
        private static List<IAnnouncementWebSiteData> _announcementWebSites = null;
        private static CityTreatment _cityTreatment = null;
        private static CachingManager _cachingManager = null;
        private static IDatabaseFactory _databaseFactory = new SqlServerDatabase();
        private static AnnouncementTreatment _announcementTreatment = null;
        
        public AnnouncementWorker(ILogger<AnnouncementWorker> logger)
        {
            _logger = logger;

            _logger.LogError($"ça marche ?? : {Directory.GetCurrentDirectory()}");
            _logger.LogError($"ça marche ?? : {System.AppDomain.CurrentDomain.BaseDirectory}");

            _cachingManager = new CachingManager(_databaseFactory);
            _cityTreatment = new CityTreatment(_cachingManager, _databaseFactory);
            _announcementTreatment = new AnnouncementTreatment(_cachingManager, _databaseFactory);

            _announcementWebSites = new List<IAnnouncementWebSiteData>
                {
                    {new Century21WebSiteData(_announcementTreatment) },
                    //{new LeBonCoinWebSiteData(_announcementTreatment) },
                    {new IADWebSite(_announcementTreatment) }
                };

            HandleNewAnnouncementQueue();
        }

        private static void HandleNewAnnouncementQueue()
        {
            EventingBasicConsumer consumer = AnnouncementQueue.Consumer;

            consumer.Received += ReceivedAnnouncementInformation;
            AnnouncementQueue.SetConsumer(consumer);
        }

        private static void ReceivedAnnouncementInformation(object? model, BasicDeliverEventArgs ea)
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
