﻿using RentalInvestmentAid.Models.Rental;
using Microsoft.Data.SqlClient;
using System.Data;
using RentalInvestmentAid.Settings;
using RentalInvestmentAid.Database.Converter;
using System.Collections.Generic;
using RentalInvestmentAid.Models.Announcement;
using System.Diagnostics;
using RentalInvestmentAid.Models.Bank;

namespace RentalInvestmentAid.Database
{
    public class SqlServerDatabase : IDatabaseFactory
    {
        public List<RentalInformations> RentalInformations
        {
            get
            {
                if (_rentalInformation == null)
                    SetRentalsInformations();
                return _rentalInformation;
            }
        }
        public List<AnnouncementInformation> AnnouncementInformations
        {
            get
            {
                if (_announcementInformation == null)
                    SetAnnoucementInformation();
                return _announcementInformation;
            }
        }
        public List<RateInformation> RateInformations
        {
            get
            {
                if (_ratelInformation == null)
                    SetRateInformation();
                return _ratelInformation;
            }
        }

        private List<RentalInformations> _rentalInformation = null;
        private List<AnnouncementInformation> _announcementInformation = null;
        private List<RateInformation> _ratelInformation = null;
        private void SetRentalsInformations()
        {
            _rentalInformation = new List<RentalInformations>();
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetRentalInformations", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _rentalInformation.Add(new RentalInformations
                            {
                                Id = reader.GetInt32(0),
                                City = reader.GetString(1),
                                ZipCode = reader.GetString(2),
                                LowerPrice = reader.GetDecimal(3).ToString(),
                                MediumPrice = reader.GetDecimal(4).ToString(),
                                HigherPrice = reader.GetDecimal(5).ToString(),
                                RentalType = (RentalTypeOfTheRent)reader.GetInt32(6),
                                CreatedDate = reader.GetDateTime(7),
                                UpdatedDate = reader.GetDateTime(8)
                            });
                        }
                    }
                }
            }
        }
        public void InsertRentalInformation(RentalInformations rental)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspInsertRentalInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@city", rental.City);
                    sqlCommand.Parameters.AddWithValue("@zipcode", rental.ZipCode);
                    sqlCommand.Parameters.AddWithValue("@lowerPrice", rental.LowerPrice);
                    sqlCommand.Parameters.AddWithValue("@mediumPrice", rental.MediumPrice);
                    sqlCommand.Parameters.AddWithValue("@higherPrice", rental.HigherPrice);
                    sqlCommand.Parameters.AddWithValue("@idPropertyType", rental.RentalType);
                    sqlCommand.Parameters.AddWithValue("@url", rental.Url);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        private void SetAnnoucementInformation()
        {
            _announcementInformation = new List<AnnouncementInformation>();
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetAnnoncementInformations", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _announcementInformation.Add(new AnnouncementInformation
                            {
                                Id = reader.GetInt32(0),
                                City = reader.GetString(1),
                                ZipCode = reader.GetString(2),
                                Price = reader.GetDecimal(3).ToString(),
                                Metrage = reader.GetDecimal(4).ToString(),
                                Description = reader.GetString(5).ToString(),
                                RentalType = (RentalTypeOfTheRent)reader.GetInt32(6),
                                UrlWebSite = reader.GetString(7).ToString(),
                                CreatedDate = reader.GetDateTime(8),
                                UpdatedDate = reader.GetDateTime(9)
                            });
                        }
                    }
                }
            }
        }
        public void InsertAnnouncementInformation(AnnouncementInformation announcementInformation)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspInsertAnnoncementInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@city", announcementInformation.City);
                    sqlCommand.Parameters.AddWithValue("@zipcode", announcementInformation.ZipCode);
                    sqlCommand.Parameters.AddWithValue("@price", announcementInformation.Price);
                    sqlCommand.Parameters.AddWithValue("@metrage", announcementInformation.Metrage);
                    sqlCommand.Parameters.AddWithValue("@description", announcementInformation.Description);
                    sqlCommand.Parameters.AddWithValue("@idProptertyType", announcementInformation.RentalType);
                    sqlCommand.Parameters.AddWithValue("@url", announcementInformation.UrlWebSite);

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        private void SetRateInformation()
        {
            _ratelInformation = new List<RateInformation>();
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetRateInformations", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _ratelInformation.Add(new RateInformation
                            {
                                Id = reader.GetInt32(0),
                                DurationInYear = reader.GetInt32(1),
                                MaxRate = reader.GetDecimal(2).ToString(),
                                MarketRate = reader.GetDecimal(3).ToString(),
                                LowerRate = reader.GetDecimal(4).ToString(),
                                CreatedDate = reader.GetDateTime(5),
                                UpdatedDate = reader.GetDateTime(6)
                            });
                        }
                    }
                }
            }
        }
        public void InsertRateInformation(RateInformation rateInformation)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspInsertRateInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@durationInYear", rateInformation.DurationInYear);
                    sqlCommand.Parameters.AddWithValue("@maxRate", Convert.ToDecimal(rateInformation.MaxRate));
                    sqlCommand.Parameters.AddWithValue("@marketRate", Convert.ToDecimal(rateInformation.MarketRate));
                    sqlCommand.Parameters.AddWithValue("@lowerRate", Convert.ToDecimal(rateInformation.LowerRate));

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
