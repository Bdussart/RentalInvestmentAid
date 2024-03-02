// See https://aka.ms/new-console-template for more information

using RentalInvestmentAid.Core;
using RentalInvestmentAid.Core.Bank;
using RentalInvestmentAid.Core.Announcement;
using RentalInvestmentAid.Core.Rental;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models.Rental;
using System.Text;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Caching;
using System.Diagnostics;
using RentalInvestmentAid.Queue;
using RabbitMQ.Client.Events;

namespace RentalInvestmentAid
{
    public class Program {

        private static bool _loop = true;

        private static CachingManager _cachingManager = null;

        private static IDatabaseFactory databaseFactory = new SqlServerDatabase();
        private static List<RentalInformations> GetRentalInformations(string url)
        {
            IRentalWebSiteData webSiteData = new LaCoteImmoWebSiteData(_cachingManager);

            List<RentalInformations> rentalInformations = new List<RentalInformations>();

            Thread.Sleep(TimeSpan.FromSeconds(2)); //Take easy for the external server :)
            rentalInformations.AddRange(webSiteData.GetApartmentRentalInformation(url));

            Thread.Sleep(TimeSpan.FromSeconds(2)); //Take easy for the external server :)
            rentalInformations.AddRange(webSiteData.GetHouseRentalInformation(url));
            return (rentalInformations);
        }
        
        public static void Main(string[] args)
        {
            _cachingManager = new CachingManager(databaseFactory);
            Console.OutputEncoding = Encoding.UTF8;
            DoLoadDataJob();
        }


        private static void LoopGetRentalData()
        {
            IDatabaseFactory databaseFactory = new SqlServerDatabase();
            EventingBasicConsumer consumer = RentalQueue.Consumer;

            consumer.Received += ReceivedRentalInformation;
            RentalQueue.SetConsumer(consumer);
        }

        private static void ReceivedRentalInformation(object? model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Logger.LogHelper.LogInfo($"Received {message}");
            foreach (var info in GetRentalInformations(message))
            {
                databaseFactory.InsertRentalInformation(info);
            };
            _cachingManager.ForceCacheUpdateRentalInformations();
        }

        private static void DoLoadDataJob()
        {
            IBankWebSiteData bankWebSiteData = new PAPWebSiteData();

            Dictionary<int, string> dicoDepartements = new Dictionary<int, string>
            {
                {74, "haute-savoie" },
                {01, "ain" },
                {73, "savoie" }
            };

            LoopGetRentalData();

            List<String> departements = dicoDepartements.Values.ToList();

            Task.Factory.StartNew(() =>
            {
                LoopForCheckRentability();
            });

            List<RateInformation> bankInformations = bankWebSiteData.GetRatesInformations("https://www.pap.fr/acheteur/barometre-taux-emprunt");

            foreach (RateInformation rate in bankInformations)
            {
                databaseFactory.InsertRateInformation(rate);
            }
            _cachingManager.ForceCacheUpdateRatesInformation();


            IAnnouncementWebSiteData announcementWebSiteData = new Century21WebSiteData(_cachingManager);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<String> urls = announcementWebSiteData.GetAnnoucementUrl(departements, 200000);
            urls.ForEach(url =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                AnnouncementInformation? announcementInformation = announcementWebSiteData.GetAnnouncementInformation(url);

                if (announcementInformation != null)
                {
                    databaseFactory.InsertAnnouncementInformation(announcementInformation);
                    _cachingManager.ForceCacheUpdateAnnouncementInformation();
                    CheckDataRentabilityForAnnouncement(announcementInformation);
                }
            });

            IRentalWebSiteData webSiteData = new LaCoteImmoWebSiteData(_cachingManager);
            Parallel.ForEach(dicoDepartements, departement =>
            {
                webSiteData.EnQueueUrls("rhone-alpes", departement.Value, departement.Key);
            });

            Console.ReadKey();
            _loop = false;
        }


        private static void LoopForCheckRentability()
        {
            while (_loop) { 
                CheckAllDataRentability();
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        private static void CheckDataRentabilityForAnnouncement(AnnouncementInformation announcement)
        {
            RentalTreament rentalTreament = new RentalTreament();
            List<RentalInformations> currentsRentalInformation = rentalTreament.FindRentalInformationForAnAnnoucement(_cachingManager.GetRentalInformations(), announcement);

            Console.WriteLine("****** Find the right rental information Check if not null *****");
            if (currentsRentalInformation.Count == 0)
                Logger.LogHelper.LogInfo($"{announcement.ToString()} - Don't find rental information -----");
            else
            {
                List<LoanInformation> loansInformation = rentalTreament.CalculAllLoan(_cachingManager.GetRatesInformation(), announcement.Price);

                foreach (LoanInformation loan in loansInformation)
                {
                    loan.AnnouncementInformation = announcement;
                    databaseFactory.InsertLoanInformation(loan);
                }

                List<RentInformation> realRentalCosts = rentalTreament.CalculAllRentalPrices(currentsRentalInformation, announcement);
                foreach (RentInformation rent in realRentalCosts)
                {
                    rent.AnnouncementInformation = announcement;
                    databaseFactory.InsertRentInformation(rent);
                }

                databaseFactory.UpdateRentabilityInformation(announcement.Id);
                //RentalResult result = rentalTreament.CheckIfRentable(announcement.Price, realRentalCosts, loansInformation);
            }
        }

        private static void CheckAllDataRentability()
        {
            foreach (AnnouncementInformation announcement in databaseFactory.GetAnnouncementsInformations().Where(ann => !ann.rentabilityCalculated)) { 
                CheckDataRentabilityForAnnouncement(announcement);
                Thread.Sleep(TimeSpan.FromMilliseconds(2));
            }
        }            
    }
}
