using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Core;
using RentalInvestmentAid.Core.Rental;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Logger;
using RentalInvestmentAid.Queue;
using RentalInvestmentAid.Settings;

namespace FetchCityWorkerService
{
    public class FetchCityInformationWorkerService : BackgroundService
    {
        private readonly ILogger<FetchCityInformationWorkerService> _logger;
        private RentalTreament _rentalTreament = null;
        private CachingManager _cachingManager = null;
        private IDatabaseFactory _databaseFactory = new SqlServerDatabase();
        private IBroker _rentalRabbitMQBroker = null;
        public FetchCityInformationWorkerService(ILogger<FetchCityInformationWorkerService> logger)
        {
            _logger = logger;
            _cachingManager = new CachingManager(_databaseFactory);
            string rabbitMqHost = String.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("RABBITMQ_HOST")) ? "localhost" : Environment.GetEnvironmentVariable("RABBITMQ_HOST");
            LogHelper.LogInfo($"Queue : {SettingsManager.RentalQueueName}");
            _rentalRabbitMQBroker = new RabbitMQBroker(SettingsManager.RentalQueueName, rabbitMqHost);
            _rentalTreament = new RentalTreament(_cachingManager, _databaseFactory, new LeFigaroWebSiteData(_cachingManager, _rentalRabbitMQBroker));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _rentalTreament.UpdateCitiesRentInformations();
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }
}
