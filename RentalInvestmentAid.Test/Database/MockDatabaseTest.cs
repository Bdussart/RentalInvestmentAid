using RentalInvestmentAid.Database;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.City;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models.Rate;
using RentalInvestmentAid.Models.Rental;
using RentalInvestmentAid.Test.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Test.Database
{
    [TestClass]
    public class MockDatabaseTest
    {

        #region DataGenerator

        private int NumberGenerator(int length)
        {
            Random rnd = new Random();

            return rnd.Next(length);
        }

        private string FakeDataGenerator(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
            char[] stringChars = new char[length];
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }


        private AnnouncementInformation CreateAnnouncement(CityInformations cityInformations)
        {
            return (new AnnouncementInformation
            {
                IdFromProvider = $"{NumberGenerator(30)}-{FakeDataGenerator(15)}",
                AnnouncementProvider = AnnouncementProvider.century21,
                Description = FakeDataGenerator(100),
                RentabilityCalculated = false,
                Metrage = NumberGenerator(300).ToString(),
                Price = NumberGenerator(10000000).ToString(),
                RentalType = RentalTypeOfTheRent.House,
                UrlWebSite = $"www.{FakeDataGenerator(25)}.fr",
                CityInformations = cityInformations
            });
        }
        private CityInformations CreateCity()
        {
            string zipCode = NumberGenerator(10000).ToString();
            return (new CityInformations
            {
                ZipCode = zipCode,
                Departement = zipCode.Substring(0, 2),
                CityName = FakeDataGenerator(15)
            });
        }

        private RateInformation CreateRate(RateType rateType = RateType.LowRate)
        {
            return new RateInformation
            {
                Rate = NumberGenerator(5).ToString(),
                DurationInYear = NumberGenerator(30),
                RateType = rateType,
                Title = FakeDataGenerator(50)
            };
        }


        private RentalInformations CreateRental(CityInformations cityInformations,
                                                RentalPriceType rentalPriceType = RentalPriceType.LowerPrice,
                                                RentalTypeOfTheRent rentalTypeOfTheRent = RentalTypeOfTheRent.House)
        {
            return new RentalInformations
            {
                CityInfo = cityInformations,
                IdFromProvider = $"{NumberGenerator(30)}-{FakeDataGenerator(15)}",
                RentalPriceType = rentalPriceType,
                RentalTypeOfTheRent = rentalTypeOfTheRent,
                Price = NumberGenerator(100).ToString(),
                Url = $"www.{FakeDataGenerator(25)}.fr"

            };
        }

        private RentInformation CreateRents(AnnouncementInformation announcementInformation,
                                            RentalInformations rentalInformations)
        {
            int price = NumberGenerator(500);
            return new RentInformation
            {
                AnnouncementInformation = announcementInformation,
                RentPrice = Convert.ToDouble(price),
                Rental70Pourcent = Convert.ToDouble(price) * 0.70,
                RentalInformations = rentalInformations
            };
        }

        private LoanInformation CreateLoan(AnnouncementInformation announcementInformation,
                                            RateInformation rateInformation)
        {
            return new LoanInformation
            {
                AnnouncementInformation = announcementInformation,
                RateInformation = rateInformation,
                MonthlyCost = NumberGenerator(500),
                MonthlyCostWithInsurrance = NumberGenerator(900),
                TotalCost = NumberGenerator(5000000),
                TotalCostWithInsurrance = NumberGenerator(500000000),
                InsurranceRate = Convert.ToDouble(NumberGenerator(500))
            };
        }

        #endregion

        [TestMethod]
        public void empty_database()
        {
            IDatabaseFactory databaseFactory = new MockDatabase();

            Assert.AreEqual(databaseFactory.GetAnnouncementsInformations().Count, 0);
            Assert.AreEqual(databaseFactory.GetCities().Count, 0);
            Assert.AreEqual(databaseFactory.GetLoansInformations().Count, 0);
            Assert.AreEqual(databaseFactory.GetRatesInformations().Count, 0);
            Assert.AreEqual(databaseFactory.GetRentalsInformations().Count, 0);
            Assert.AreEqual(databaseFactory.GetRentabilitiesResults().Count, 0);
            Assert.AreEqual(databaseFactory.GetRentsInformations().Count, 0);
        }

        [TestMethod]
        public void insert_3_city_and_check_id()
        {
            IDatabaseFactory databaseFactory = new MockDatabase();
            databaseFactory.InsertCity(CreateCity());
            databaseFactory.InsertCity(CreateCity());
            databaseFactory.InsertCity(CreateCity());

            Assert.AreEqual(databaseFactory.GetCities().Count, 3);
            Assert.AreEqual(databaseFactory.GetCities()[0].Id, 1);
            Assert.AreEqual(databaseFactory.GetCities()[1].Id, 2);
            Assert.AreEqual(databaseFactory.GetCities()[2].Id, 3);
        }


        [TestMethod]
        public void insert_3_announcement_and_check_id()
        {
            IDatabaseFactory databaseFactory = new MockDatabase();
            CityInformations cityInformations = CreateCity();
            databaseFactory.InsertAnnouncementInformation(CreateAnnouncement(cityInformations));
            databaseFactory.InsertAnnouncementInformation(CreateAnnouncement(cityInformations));
            databaseFactory.InsertAnnouncementInformation(CreateAnnouncement(cityInformations));

            Assert.AreEqual(databaseFactory.GetAnnouncementsInformations().Count, 3);
            Assert.AreEqual(databaseFactory.GetAnnouncementsInformations()[0].Id, 1);
            Assert.AreEqual(databaseFactory.GetAnnouncementsInformations()[1].Id, 2);
            Assert.AreEqual(databaseFactory.GetAnnouncementsInformations()[2].Id, 3);
        }

        [TestMethod]
        public void insert_6_announcment_and_update_the_second_for_rentability()
        {
            IDatabaseFactory databaseFactory = new MockDatabase();
            CityInformations cityInformations = CreateCity();
            databaseFactory.InsertAnnouncementInformation(CreateAnnouncement(cityInformations));
            databaseFactory.InsertAnnouncementInformation(CreateAnnouncement(cityInformations));
            databaseFactory.InsertAnnouncementInformation(CreateAnnouncement(cityInformations));
            databaseFactory.InsertAnnouncementInformation(CreateAnnouncement(cityInformations));
            databaseFactory.InsertAnnouncementInformation(CreateAnnouncement(cityInformations));
            databaseFactory.InsertAnnouncementInformation(CreateAnnouncement(cityInformations));

            Assert.AreEqual(databaseFactory.GetAnnouncementsInformations().Count, 6);


            AnnouncementInformation announcementInformation = databaseFactory.GetAnnouncementsInformations()[1];
            Assert.AreEqual(databaseFactory.GetAnnouncementsInformations().First(ann => ann.Id == announcementInformation.Id).RentabilityCalculated, false);

            databaseFactory.UpdateRentabilityInformation(announcementInformation.Id);
            Assert.AreEqual(databaseFactory.GetAnnouncementsInformations().First(ann => ann.Id == announcementInformation.Id).RentabilityCalculated, true);
        }
    }
}