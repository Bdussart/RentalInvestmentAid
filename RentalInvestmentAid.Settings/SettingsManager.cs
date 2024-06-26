﻿

using Microsoft.Extensions.Configuration;

namespace RentalInvestmentAid.Settings
{
    public static class SettingsManager
    {

        private static IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appSettings.json", optional: true, reloadOnChange: true);
        
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


        public static String MiscAnnouncementPromptCacheKey
        {
            get
            {
                IConfiguration configuration = builder.Build();
                return configuration.GetSection("Cache").GetSection("MiscAnnouncementPromptCacheKey").Value.ToString();
            }
        }
        public static String RentalQueueName
        {
            get
            {
                IConfiguration configuration = builder.Build();
                return configuration.GetSection("Queue").GetSection("RentalQueueName").Value.ToString();
            }
        }
        public static String AnnouncementQueueName
        {
            get
            {
                IConfiguration configuration = builder.Build();
                return configuration.GetSection("Queue").GetSection("AnnouncementQueueName").Value.ToString();
            }
        }
        public static String AnnouncementPromptKey
        {
            get
            {
                IConfiguration configuration = builder.Build();
                return configuration.GetSection("MiscKey").GetSection("AnnouncementPromptKey").Value.ToString();
            }
        }

        public static String GeminiAPIUrl
        {
            get
            {
                IConfiguration configuration = builder.Build();
                return configuration.GetSection("API").GetSection("GeminiAPIUrl").Value.ToString();
            }
        }
    }
}

