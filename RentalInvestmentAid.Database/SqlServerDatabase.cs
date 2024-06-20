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
using RentalInvestmentAid.Models.City;
using System.Globalization;
using RentalInvestmentAid.Database.Extension;

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
                                CityInfo = new CityInformations
                                {
                                    Id = reader.GetInt32(2)
                                },
                                Price = reader.GetDecimal(3).ToString(),
                                RentalPriceType = (RentalPriceType)reader.GetInt32(4),
                                RentalTypeOfTheRent = (RentalTypeOfTheRent)reader.GetInt32(5),
                                Url = reader.GetString(6),
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
                            announcementInformations.Add(new AnnouncementInformation().AnnouncementInformationFromSqlDataReader(reader));
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
                                Title = reader.GetString(4),
                                CreatedDate = reader.GetDateTime(5),
                                UpdatedDate = reader.GetDateTime(6)
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
                        if (current.AnnouncementId != 0)
                            rentabilityResults.Add(current);
                    }
                }
            }

            return rentabilityResults;
        }
        public RentalInformations InsertRentalInformation(RentalInformations rental)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("uspInsertRentalInformation", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@idFromProvider", rental.IdFromProvider);
                        sqlCommand.Parameters.AddWithValue("@idCity", rental.CityInfo.Id);
                        sqlCommand.Parameters.AddWithValue("@price", rental.Price);
                        sqlCommand.Parameters.AddWithValue("@idPriceType", rental.RentalPriceType);
                        sqlCommand.Parameters.AddWithValue("@idPropertyType", rental.RentalTypeOfTheRent);
                        sqlCommand.Parameters.AddWithValue("@url", rental.Url);
                        SqlParameter retval = sqlCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                        retval.Direction = ParameterDirection.ReturnValue;

                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        rental.Id = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex, objectInfo: rental);
            }
            return rental;
        }
        public AnnouncementInformation InsertAnnouncementInformation(AnnouncementInformation announcementInformation)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("uspInsertAnnoncementInformation", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@idAnnouncementProvider", announcementInformation.AnnouncementProvider);
                        sqlCommand.Parameters.AddWithValue("@idCity", announcementInformation.CityInformations.Id);
                        sqlCommand.Parameters.AddWithValue("@idFromProvider", announcementInformation.IdFromProvider);
                        sqlCommand.Parameters.AddWithValue("@price", Convert.ToDouble(announcementInformation.Price, CultureInfo.InvariantCulture));
                        sqlCommand.Parameters.AddWithValue("@metrage", Convert.ToDouble(announcementInformation.Metrage, CultureInfo.InvariantCulture));
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
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex, objectInfo: announcementInformation);
            }

            return announcementInformation;
        }
        public RateInformation InsertRateInformation(RateInformation rateInformation)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("uspInsertRateInformation", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@durationInYear", rateInformation.DurationInYear);
                        sqlCommand.Parameters.AddWithValue("@rate", Convert.ToDouble(rateInformation.Rate));
                        sqlCommand.Parameters.AddWithValue("@rateType", Convert.ToDouble(rateInformation.RateType));
                        sqlCommand.Parameters.AddWithValue("@title", rateInformation.Title);

                        SqlParameter retval = sqlCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                        retval.Direction = ParameterDirection.ReturnValue;

                        connection.Open();
                        sqlCommand.ExecuteNonQuery();

                        rateInformation.Id = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex, objectInfo: rateInformation);
            }
            return rateInformation;
        }
        public LoanInformation InsertLoanInformation(LoanInformation loanInformation)
        {
            try
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
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex, objectInfo: loanInformation);
            }

            return loanInformation;
        }
        public RentInformation InsertRentInformation(RentInformation rentInformation)
        {
            try
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
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex, objectInfo: rentInformation);
            }

            return rentInformation;
        }
        public void UpdateRentabilityInformation(int announcementId, bool isRentable)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspUpdateRentabilityInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@announcementId", announcementId);
                    sqlCommand.Parameters.AddWithValue("@isRentable", isRentable);
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }
        public List<CityInformations> GetCities()
        {
            List<CityInformations> cities = new List<CityInformations>();
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetCity", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cities.Add(new CityInformations
                            {
                                Id = reader.GetInt32(0),
                                CityName = reader.GetString(1),
                                ZipCode = reader.GetString(2),
                                Departement = reader.GetString(3),
                                CreatedDate = reader.GetDateTime(4),
                            });
                        }
                    }
                }
            }
            return cities;
        }

        public List<CityInformations> GetCitiesWithNoRent()
        {
            List<CityInformations> cities = new List<CityInformations>();
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetCityWithNoRent", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cities.Add(new CityInformations
                            {
                                Id = reader.GetInt32(0),
                                CityName = reader.GetString(1),
                                ZipCode = reader.GetString(2),
                                Departement = reader.GetString(3),
                                CreatedDate = reader.GetDateTime(4),
                            });
                        }
                    }
                }
            }
            return cities;
        }
        public CityInformations InsertCity(CityInformations city)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("uspInsertCity", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@cityName", city.CityName);
                        sqlCommand.Parameters.AddWithValue("@zipcode", city.ZipCode);
                        sqlCommand.Parameters.AddWithValue("@departement", city.Departement);

                        SqlParameter retval = sqlCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                        retval.Direction = ParameterDirection.ReturnValue;

                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        city.Id = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex, objectInfo: city);
            }
            return city;
        }

        public List<DepartmentToSearchData> GetDepartmentToSearchDatas()
        {

            List<DepartmentToSearchData> departments = new List<DepartmentToSearchData>();
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetDepartmentToSearchData", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    connection.Open();
                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departments.Add(new DepartmentToSearchData
                            {
                                Id = reader.GetInt32(0),
                                DepartmentName = reader.GetString(1),
                                DepartmentNumber = reader.GetString(2),
                                CreatedDate = reader.GetDateTime(3),
                            });
                        }
                    }
                }
            }
            return departments;
        }

        public DepartmentToSearchData InsertDepartment(DepartmentToSearchData departmentToSearchData)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("uspInsertDepartmentToSearchData", connection))
                    {
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Parameters.AddWithValue("@departmentName", departmentToSearchData.DepartmentName);
                        sqlCommand.Parameters.AddWithValue("@departmentNumber", departmentToSearchData.DepartmentNumber);

                        SqlParameter retval = sqlCommand.Parameters.Add("@RETURN_VALUE", SqlDbType.Int);
                        retval.Direction = ParameterDirection.ReturnValue;

                        connection.Open();
                        sqlCommand.ExecuteNonQuery();
                        departmentToSearchData.Id = (int)sqlCommand.Parameters["@RETURN_VALUE"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogHelper.LogException(ex, objectInfo: departmentToSearchData);
            }
            return departmentToSearchData;
        }

        public async Task DeleteAnnouncementInformation(int announcementId)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspDeleteAnnouncementInformation", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@announcementId", announcementId);
                    connection.Open();
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }
        }

        public string GetMiscPerKey(string key)
        {
            string result = string.Empty;
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetMiscellaneous", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@key", key);
                    connection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result = reader.GetString(0);
                        }
                    }
                }
            }
            return result;
        }

        public void InsertInformationProvidedByGeminiForAnAnnouncement(int announcementId, string geminiInformation)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspSetInformationProvidedByGeminiInAnnouncement", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@announcementId", announcementId);
                    sqlCommand.Parameters.AddWithValue("@informationProvidedByGemini", geminiInformation);
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDepartment(int departmentId)
        {
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspDeleteDepartmentToSearchData", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@departmentId", departmentId);
                    connection.Open();
                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        public AnnouncementInformation? GetAnnouncementsInformationsByProviderId(int providerId, string announcmentProviderId)
        {
            AnnouncementInformation? announcementInformation = null;
            using (SqlConnection connection = new SqlConnection(SettingsManager.ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("uspGetAnnoncementInformationsByProvider", connection))
                {
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@providerId", providerId);
                    sqlCommand.Parameters.AddWithValue("@idAnnouncementProvider", announcmentProviderId);
                    connection.Open();

                    using (SqlDataReader reader = sqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            announcementInformation = new AnnouncementInformation().AnnouncementInformationFromSqlDataReader(reader);
                        }
                    }
                }
            }

            return announcementInformation;
        }
    }
}
