

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
            
            
            
    }
}
