using RentalInvestmentAid.Models.Rental;
using Microsoft.Data.SqlClient;
using System.Data;
using RentalInvestmentAid.Settings;
using RentalInvestmentAid.Database.Converter;
using System.Collections.Generic;

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
        private List<RentalInformations> _rentalInformation = null;

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
    }
}
