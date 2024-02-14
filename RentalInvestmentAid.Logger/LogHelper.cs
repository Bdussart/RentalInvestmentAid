using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;

namespace RentalInvestmentAid.Logger
{
    public static class LogHelper
    {
        private static bool _isInit = false;
        private static Serilog.ILogger _logger;
        private static void Init()
        {
            if (!_isInit)
            {
                _isInit = true;
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                _logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();

                _logger.Information("InitDone");
            }
        }

        public static void LogInfo(string  message)
        {
            Init();
            _logger.Information("Hello, world!");
        }

    }
}
