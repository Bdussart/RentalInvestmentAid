using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Core.Bank;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Logger;
using RentalInvestmentAid.Models.Bank;

namespace BankWorkerService
{
    public class BankInformationWorkerService : BackgroundService
    {
        private CachingManager _cachingManager = null;

        private IDatabaseFactory _databaseFactory = new SqlServerDatabase();
        private BankTreatment _bankTreatment = null;


        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        public BankInformationWorkerService(ILogger<BankInformationWorkerService> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            LogHelper.LogInfo($"Start : {nameof(BankInformationWorkerService)}");
            _cachingManager = new CachingManager(_databaseFactory);
            _hostApplicationLifetime = hostApplicationLifetime;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bankTreatment = new BankTreatment(_cachingManager, _databaseFactory);
            IBankWebSiteData bankWebSiteData = new PAPWebSiteData(_bankTreatment);
            List<RateInformation> bankInformations = bankWebSiteData.GetRatesInformations("https://www.pap.fr/acheteur/barometre-taux-emprunt");

            LogHelper.LogInfo($"BankInformation  : {String.Join(';', bankInformations)}");
            foreach (RateInformation rate in bankInformations)
            {
                _bankTreatment.InsertRate(rate);
            }
            _hostApplicationLifetime.StopApplication();
            //while (!stoppingToken.IsCancellationRequested)
            //{
            //    LogHelper.LogInformation($"BankInformation  : {String.Join(';', bankInformations)}");
            //    await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            //}
        }

    }
}
