using RentalInvestmentAid.Models.Rental;
using Microsoft.Data.SqlClient;
using System.Data;
using RentalInvestmentAid.Settings;
using RentalInvestmentAid.Database.Converter;
using System.Collections.Generic;
using RentalInvestmentAid.Models.Announcement;
using System.Diagnostics;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Rate;
using RentalInvestmentAid.Models.Loan;

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
        public List<LoanInformation> LoansInformations
        {
            get
            {
                if (_loansInformations == null)
                    SetLoanInformation();
                return _loansInformations;
            }
        }

        private List<RentalInformations> _rentalInformation = null;
        private List<AnnouncementInformation> _announcementInformation = null;
        private List<RateInformation> _ratelInformation = null;
        private List<LoanInformation> _loansInformations = null;
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
                                Price = reader.GetDecimal(3).ToString(),
                                RentalPriceType= (RentalPriceType)reader.GetInt32(4),
                                RentalTypeOfTheRent = (RentalTypeOfTheRent)reader.GetInt32(5),
                                CreatedDate = reader.GetDateTime(6),
                                UpdatedDate = reader.GetDateTime(7)
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
                    sqlCommand.Parameters.AddWithValue("@price", rental.Price);
                    sqlCommand.Parameters.AddWithValue("@idPriceType", rental.RentalPriceType);
                    sqlCommand.Parameters.AddWithValue("@idPropertyType", rental.RentalTypeOfTheRent);
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
                                Rate = reader.GetDecimal(2).ToString(),
                                RateType = (RateType)reader.GetInt32(3),
                                CreatedDate = reader.GetDateTime(4),
                                UpdatedDate = reader.GetDateTime(5)
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
                    sqlCommand.Parameters.AddWithValue("@rate", Convert.ToDecimal(rateInformation.Rate));
                    sqlCommand.Parameters.AddWithValue("@rateType", Convert.ToDecimal(rateInformation.RateType));
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        private void SetLoanInformation()
        {
            _loansInformations = new List<LoanInformation>();
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetLoanInformations", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _loansInformations.Add(new LoanInformation
                            {
                                Id = reader.GetInt32(0),
                                AnnouncementInformation = new AnnouncementInformation
                                {
                                    Id = reader.GetInt32(1)
                                },
                                RateInformation = new RateInformation
                                {
                                    Id = reader.GetInt32(2)
                                },
                                TotalCost = reader.GetDouble(3),
                                MonthlyCost = reader.GetDouble(4),
                                InsurranceRate = reader.GetDouble(5),
                                TotalCostWithInsurrance = reader.GetDouble(6),
                                MonthlyCostWithInsurrance = reader.GetDouble(7),
                                CreatedDate = reader.GetDateTime(8),
                                UpdatedDate = reader.GetDateTime(9)
                            });
                        }
                    }
                }
            }
        }
        public void InsertLoanInformation(LoanInformation loanInformation)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspInsertLoanInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@idAnnoncementInformation", loanInformation.AnnouncementInformation.Id);
                    sqlCommand.Parameters.AddWithValue("@idRateInformation", loanInformation.RateInformation.Id);
                    sqlCommand.Parameters.AddWithValue("@totalCost", loanInformation.TotalCost);
                    sqlCommand.Parameters.AddWithValue("@monthlyCost", loanInformation.MonthlyCost);
                    sqlCommand.Parameters.AddWithValue("@insuranceRate", loanInformation.InsurranceRate);
                    sqlCommand.Parameters.AddWithValue("@totalCostWithInssurance", loanInformation.TotalCostWithInsurrance);
                    sqlCommand.Parameters.AddWithValue("@monthlyCostWithInssurance", loanInformation.MonthlyCostWithInsurrance);
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
