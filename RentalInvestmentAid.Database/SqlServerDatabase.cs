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
using RentalInvestmentAid.Models;

namespace RentalInvestmentAid.Database
{
    public class SqlServerDatabase : IDatabaseFactory
    {
        public List<RentalInformations> GetRentalsInformations()
        {
            List<RentalInformations> rentalInformations = new List<RentalInformations>();
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
                            rentalInformations.Add(new RentalInformations
                            {
                                Id = reader.GetInt32(0),
                                IdFromProvider = reader.GetString(1),
                                City = reader.GetString(2),
                                ZipCode = reader.GetString(3),
                                Price = reader.GetDecimal(4).ToString(),
                                RentalPriceType = (RentalPriceType)reader.GetInt32(5),
                                RentalTypeOfTheRent = (RentalTypeOfTheRent)reader.GetInt32(6),
                                CreatedDate = reader.GetDateTime(7),
                                UpdatedDate = reader.GetDateTime(8)
                            });
                        }
                    }
                }
            }
            return rentalInformations;
        }
        public List<AnnouncementInformation> GetAnnouncementsInformations()
        {
            List<AnnouncementInformation> announcementInformations = new List<AnnouncementInformation>();
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
                            announcementInformations.Add(new AnnouncementInformation
                            {
                                Id = reader.GetInt32(0),
                                AnnouncementProvider = (AnnouncementProvider)reader.GetInt32(1),
                                IdFromProvider = reader.GetString(2),
                                City = reader.GetString(3),
                                ZipCode = reader.GetString(4),
                                Price = reader.GetDecimal(5).ToString(),
                                Metrage = reader.GetDecimal(6).ToString(),
                                Description = reader.GetString(7).ToString(),
                                RentalType = (RentalTypeOfTheRent)reader.GetInt32(8),
                                UrlWebSite = reader.GetString(9).ToString(),
                                rentabilityCalculated = reader.GetBoolean(10),
                                CreatedDate = reader.GetDateTime(11),
                                UpdatedDate = reader.GetDateTime(12)
                            });
                        }
                    }
                }
            }
            return announcementInformations;
        }
        public List<RateInformation> GetRatesInformations()
        {

            List<RateInformation> rateInformations = new List<RateInformation>();
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
                            rateInformations.Add(new RateInformation
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

            return rateInformations;
        }
        public List<LoanInformation> GetLoansInformations()
        {
            List<LoanInformation> loansInformations = new List<LoanInformation>();
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetLoanInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            loansInformations.Add(new LoanInformation
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
                                TotalCost = Convert.ToDouble(reader.GetDecimal(3)),
                                MonthlyCost = Convert.ToDouble(reader.GetDecimal(4)),
                                InsurranceRate = Convert.ToDouble(reader.GetDecimal(5)),
                                TotalCostWithInsurrance = Convert.ToDouble(reader.GetDecimal(6)),
                                MonthlyCostWithInsurrance = Convert.ToDouble(reader.GetDecimal(7)),
                                CreatedDate = reader.GetDateTime(8),
                                UpdatedDate = reader.GetDateTime(9)
                            });
                        }
                    }
                }
            }
            return loansInformations;
        }
        public List<RentInformation> GetRentsInformations()
        {
            List<RentInformation> rentInformations = new List<RentInformation>();
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
                            rentInformations.Add(new RentInformation
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
                                RentPrice = Convert.ToDouble(reader.GetDecimal(3)),
                                Rental70Pourcent = Convert.ToDouble(reader.GetDecimal(4)),
                                CreatedDate = reader.GetDateTime(5),
                                UpdatedDate = reader.GetDateTime(6)
                            });
                        }
                    }
                }
            }
            return rentInformations;
        }
        public List<RentabilityResult> GetRentabilitiesResults()
        {
            List<RentabilityResult> rentabilityResults = new List<RentabilityResult>();
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetAnnoncementWithRentAndLoanInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        int lastAnnouncementId = -1;
                        RentabilityResult current = new RentabilityResult();
                        while (reader.Read())
                        {

                            int currentId = reader.GetInt32(0);
                            int currentLoanId = reader.GetInt32(1);
                            int currentRentId = reader.GetInt32(2);

                            if (lastAnnouncementId != currentId && lastAnnouncementId != -1)
                            {
                                rentabilityResults.Add(current);
                                current = new RentabilityResult();
                            }

                            current.AnnouncementId = currentId;
                            if (!current.LoanIds.Contains(currentLoanId))
                                current.LoanIds.Add(currentLoanId);
                            if (!current.RentsIds.Contains(currentRentId))
                                current.RentsIds.Add(currentRentId);
                            lastAnnouncementId = currentId;
                        }

                        rentabilityResults.Add(current);
                    }
                }
            }

            return rentabilityResults;
        }
        public RentalInformations InsertRentalInformation(RentalInformations rental)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspInsertRentalInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@idFromProvider", rental.IdFromProvider);
                    sqlCommand.Parameters.AddWithValue("@city", rental.City);
                    sqlCommand.Parameters.AddWithValue("@zipcode", rental.ZipCode);
                    sqlCommand.Parameters.AddWithValue("@price", rental.Price);
                    sqlCommand.Parameters.AddWithValue("@idPriceType", rental.RentalPriceType);
                    sqlCommand.Parameters.AddWithValue("@idPropertyType", rental.RentalTypeOfTheRent);
                    sqlCommand.Parameters.AddWithValue("@url", rental.Url);

                    SqlParameter retval = sqlCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                    retval.Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    rental.Id =(int) sqlCommand.Parameters["@RETURN_VALUE"].Value;
                }
            }
            return rental;
        }
        public AnnouncementInformation InsertAnnouncementInformation(AnnouncementInformation announcementInformation)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspInsertAnnoncementInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@city", announcementInformation.City);
                    sqlCommand.Parameters.AddWithValue("@idAnnouncementProvider", announcementInformation.AnnouncementProvider);
                    
                    sqlCommand.Parameters.AddWithValue("@zipcode", announcementInformation.ZipCode);
                    sqlCommand.Parameters.AddWithValue("@idFromProvider", announcementInformation.IdFromProvider);
                    sqlCommand.Parameters.AddWithValue("@price", announcementInformation.Price);
                    sqlCommand.Parameters.AddWithValue("@metrage", announcementInformation.Metrage);
                    sqlCommand.Parameters.AddWithValue("@description", announcementInformation.Description);
                    sqlCommand.Parameters.AddWithValue("@idProptertyType", announcementInformation.RentalType);
                    sqlCommand.Parameters.AddWithValue("@url", announcementInformation.UrlWebSite);

                    SqlParameter retval = sqlCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                    retval.Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    announcementInformation.Id = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                }
            }

            return announcementInformation;
        }
        public RateInformation InsertRateInformation(RateInformation rateInformation)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspInsertRateInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@durationInYear", rateInformation.DurationInYear);
                    sqlCommand.Parameters.AddWithValue("@rate", Convert.ToDecimal(rateInformation.Rate));
                    sqlCommand.Parameters.AddWithValue("@rateType", Convert.ToDecimal(rateInformation.RateType));

                    SqlParameter retval = sqlCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                    retval.Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();

                    rateInformation.Id = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                }
            }

            return rateInformation;
        }
        public LoanInformation InsertLoanInformation(LoanInformation loanInformation)
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

                    SqlParameter retval = sqlCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                    retval.Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    loanInformation.Id = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                }
            }

            return loanInformation;
        }
        public RentInformation InsertRentInformation(RentInformation rentInformation)
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

                    SqlParameter retval = sqlCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                    retval.Direction = ParameterDirection.ReturnValue;

                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                    rentInformation.Id = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                }
            }

            return rentInformation;
        }

        public void UpdateRentabilityInformation(int announcementId)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspUpdateRentabilityInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@announcementId", announcementId);
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

    }
}
