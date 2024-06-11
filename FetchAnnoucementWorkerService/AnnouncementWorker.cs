using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Core;
using RentalInvestmentAid.Core.Announcement;
using RentalInvestmentAid.Core.Announcement.Helper;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Logger;
using RentalInvestmentAid.Models;
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


        private  int? _maxPrice = 200000;
        private  List<IAnnouncementWebSiteData> _announcementWebSites = null;
        private  CachingManager _cachingManager = null;
        private  IDatabaseFactory _databaseFactory = new SqlServerDatabase();
        private  AnnouncementTreatment _announcementTreatment = null;
        private IBroker _announcementRabbitMQBroker = null;

        private List<DepartmentToSearchData> _departments = null;

        public AnnouncementWorker(ILogger<AnnouncementWorker> logger)
        {
            _logger = logger;
            string rabbitMqHost = String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("RABBITMQ_HOST")) ? "localhost" : Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            _announcementRabbitMQBroker = new RabbitMQBroker(SettingsManager.AnnouncementQueueName, rabbitMqHost);
            _cachingManager = new CachingManager(_databaseFactory);
            _announcementTreatment = new AnnouncementTreatment(_cachingManager, _databaseFactory);

            _announcementWebSites = new List<IAnnouncementWebSiteData>
                {
                    {new Century21WebSiteData(_announcementTreatment, _announcementRabbitMQBroker) },
                    {new IADWebSite(_announcementTreatment, _announcementRabbitMQBroker) }
                };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _departments = _databaseFactory.GetDepartmentToSearchDatas();
                foreach (IAnnouncementWebSiteData announcementWebSite in _announcementWebSites)
                {
                    try
                    {
                        LogHelper.LogInfo($"******{Task.CurrentId} Start work for worker {announcementWebSite.GetKeyword()}* ****");
                        announcementWebSite.EnQueueAnnoucementUrl(_departments.Select(department => department.DepartmentName).ToList(), _maxPrice);
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
