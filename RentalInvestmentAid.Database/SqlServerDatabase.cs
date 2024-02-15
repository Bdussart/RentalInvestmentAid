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
                if (_rentalInformations == null)
                    SetRentalsInformations();
                return _rentalInformations;
            }
        }
        public List<AnnouncementInformation> AnnouncementInformations
        {
            get
            {
                if (_announcementInformations == null)
                    SetAnnoucementInformation();
                return _announcementInformations;
            }
        }
        public List<RateInformation> RateInformations
        {
            get
            {
                if (_rateInformations == null)
                    SetRateInformation();
                return _rateInformations;
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

        public List<RentInformation> rentInformation
        {
            get
            {
                if (_rateInformations == null)
                    SetRentInformation();
                return _rentInformations;
            }
        }

        private List<RentalInformations> _rentalInformations = null;
        private List<AnnouncementInformation> _announcementInformations = null;
        private List<RateInformation> _rateInformations = null;
        private List<LoanInformation> _loansInformations = null;
        private List<RentInformation> _rentInformations = null;
        private void SetRentalsInformations()
        {
            _rentalInformations = new List<RentalInformations>();
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
                            _rentalInformations.Add(new RentalInformations
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
            _announcementInformations = new List<AnnouncementInformation>();
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
                            _announcementInformations.Add(new AnnouncementInformation
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
            _rateInformations = new List<RateInformation>();
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
                            _rateInformations.Add(new RateInformation
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
        private void SetRentInformation()
        {
            _rentInformations = new List<RentInformation>();
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetRentInformations", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _rentInformations.Add(new RentInformation
                            {
                                Id = reader.GetInt32(0),
                                AnnouncementInformation = new AnnouncementInformation
                                {
                                    Id = reader.GetInt32(1)
                                },
                                RentalInformations = new RentalInformations
                                {
                                    Id = reader.GetInt32(2)
                                },
                                RentPrice = reader.GetDouble(3),
                                Rental70Pourcent = reader.GetDouble(4),
                                CreatedDate = reader.GetDateTime(5),
                                UpdatedDate = reader.GetDateTime(6)
                            });
                        }
                    }
                }
            }
        }
        public void InsertRentInformation(RentInformation rentInformation)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspInsertRentInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@idAnnoncementInformation", rentInformation.AnnouncementInformation.Id);
                    sqlCommand.Parameters.AddWithValue("@idRentalInformation", rentInformation.RentalInformations.Id);
                    sqlCommand.Parameters.AddWithValue("@rentPrice", rentInformation.RentPrice);
                    sqlCommand.Parameters.AddWithValue("@rent70Price", rentInformation.Rental70Pourcent);
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
