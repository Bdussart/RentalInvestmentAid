using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Debugging;
using System.Runtime.CompilerServices;

namespace RentalInvestmentAid.Logger
{
    public static class LogHelper
    {
        private static Serilog.ILogger _logger;

        static LogHelper()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            _logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            _logger.Information("InitDone");
        }

        public static void LogInfo(string  message, [CallerMemberName] string callerName = "")
        {
            _logger.Information($"[{callerName}]{message}");
        }

        public static void LogException(Exception exception, [CallerMemberName] string callerName = "")
        {
            _logger.Warning(exception, $"[{callerName}]{exception.Message}");
        }
        public static void LogDebug(string message, [CallerMemberName] string callerName = "")
        {
            _logger.Debug($"[{callerName}]{message}");
        }

    }
}
