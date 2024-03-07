

using Microsoft.Extensions.Configuration;

namespace RentalInvestmentAid.Settings
{
    public static class SettingsManager
    {

        private static IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
        public static String ConnectionString
        {
            get
            {          
                IConfiguration configuration= builder.Build(); 
                return configuration.GetConnectionString("RentalInvestmentAidDatabase");
            }
        }

        public static String RentalCacheKey
        {
            get
            {
                IConfiguration configuration = builder.Build();
                return configuration.GetSection("Cache").GetSection("RentalCacheKey").Value.ToString() ;
            }
        }
        public static String AnnouncementCacheKey
        {
            get
            {
                IConfiguration configuration = builder.Build();
                return configuration.GetSection("Cache").GetSection("AnnouncementCacheKey").Value.ToString();
            }
        }
        public static String RateCacheKey
        {
            get
            {
                IConfiguration configuration = builder.Build();
                return configuration.GetSection("Cache").GetSection("RateCacheKey").Value.ToString();
            }
        }
        public static String CityCacheKey
        {
            get
            {
                IConfiguration configuration = builder.Build();
                return configuration.GetSection("Cache").GetSection("CityCacheKey").Value.ToString();
            }
        }

        public static String LoanCacheKey
        {
            get
            {
                IConfiguration configuration = builder.Build();
                return configuration.GetSection("Cache").GetSection("LoanCacheKey").Value.ToString();
            }
        }

        public static String RentCacheKey
        {
            get
            {
                IConfiguration configuration = builder.Build();
                return configuration.GetSection("Cache").GetSection("RentCacheKey").Value.ToString();
            }
        }
    }
}

