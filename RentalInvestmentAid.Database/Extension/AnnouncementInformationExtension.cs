using Microsoft.Data.SqlClient;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.City;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Database.Extension
{
    public static class AnnouncementInformationExtension
    {
        public static AnnouncementInformation AnnouncementInformationFromSqlDataReader(this AnnouncementInformation announcementInformation, SqlDataReader reader)
        {
            announcementInformation.Id = reader.GetInt32(0);
            announcementInformation.AnnouncementProvider = (AnnouncementProvider)reader.GetInt32(1);
            announcementInformation.IdFromProvider = reader.GetString(2);
            announcementInformation.CityInformations = new CityInformations
            {
                Id = reader.GetInt32(3),
            };
            announcementInformation.Price = reader.GetDecimal(4).ToString();
            announcementInformation.Metrage = reader.GetDecimal(5).ToString();
            announcementInformation.Description = reader.GetString(6).ToString();
            announcementInformation.RentalType = (RentalTypeOfTheRent)reader.GetInt32(7);
            announcementInformation.UrlWebSite = reader.GetString(8).ToString();
            announcementInformation.RentabilityCalculated = reader.GetBoolean(9);
            announcementInformation.IsRentable = reader.IsDBNull(10) ? null : reader.GetBoolean(10);
            announcementInformation.Readed = reader.GetBoolean(11);
            announcementInformation.InformationProvidedByGemini = reader.IsDBNull(12) ? string.Empty : reader.GetString(12);
            announcementInformation.CreatedDate = reader.GetDateTime(13);
            announcementInformation.UpdatedDate = reader.GetDateTime(14);

            return announcementInformation;
        }
    }
}
