using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Core;
using RentalInvestmentAid.Core.Rental;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Logger;
using RentalInvestmentAid.Models.City;
using RentalInvestmentAid.Models.Rental;
using RentalInvestmentAid.Queue;
using RentalInvestmentAid.Settings;
using System.Text;

namespace TreatCityWorkerService
{
    public class TreatCityInformationWorkerService : BackgroundService
    {
        private readonly ILogger<TreatCityInformationWorkerService> _logger;
        private IDatabaseFactory _databaseFactory = new SqlServerDatabase();
        private IBroker _rentalRabbitMQBroker = null;
        private CityTreatment _cityTreatment = null;
        private CachingManager _cachingManager = null;

        public TreatCityInformationWorkerService(ILogger<TreatCityInformationWorkerService> logger)
        {
            string rabbitMqHost = String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("RABBITMQ_HOST")) ? "localhost" : Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            LogHelper.LogInfo($"Queue : {SettingsManager.RentalQueueName}");
            _rentalRabbitMQBroker = new RabbitMQBroker(SettingsManager.RentalQueueName, rabbitMqHost);
            _cachingManager = new CachingManager(_databaseFactory);
            _cityTreatment = new CityTreatment(_cachingManager, _databaseFactory);
            _logger = logger;
            EventingBasicConsumer consumer = _rentalRabbitMQBroker.GetConsumer();
            consumer.Received += ReceivedRentalInformation;
            _rentalRabbitMQBroker.SetConsumer(consumer);
        }

        private  void ReceivedRentalInformation(object? model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var data = Encoding.UTF8.GetString(body);

            string message = JsonConvert.DeserializeObject<string>(data);
            LogHelper.LogInfo($"Received {message}");
            foreach (RentalInformations info in GetRentalInformations(message))
            {
                CityInformations city = _cityTreatment.GetAndInsertIfNotExisiting(info.CityInfo.CityName, info.CityInfo.ZipCode.Substring(0, 2), info.CityInfo.ZipCode);
                info.CityInfo.Id = city.Id;
                _databaseFactory.InsertRentalInformation(info);
            }
            _cachingManager.ForceCacheUpdateRentalInformations();
        }

        private  List<RentalInformations> GetRentalInformations(string url)
        {
            IRentalWebSiteData webSiteData = new LeFigaroWebSiteData(_cachingManager, _rentalRabbitMQBroker);
            List<RentalInformations> rentalInformations = new List<RentalInformations>();

            Thread.Sleep(TimeSpan.FromSeconds(2)); //Take easy for the external server :)
            rentalInformations.AddRange(webSiteData.GetApartmentRentalInformation(url));

            Thread.Sleep(TimeSpan.FromSeconds(2)); //Take easy for the external server :)
            rentalInformations.AddRange(webSiteData.GetHouseRentalInformation(url));
            return (rentalInformations);
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
    }
}
