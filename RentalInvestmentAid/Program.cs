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
using RentalInvestmentAid.Core.Announcement.Helper;
using RentalInvestmentAid.Models.City;
using System.Runtime;

namespace RentalInvestmentAid
{
    public class Program
    {

        private static bool _loop = true;

        private static CachingManager _cachingManager = null;

        private static IDatabaseFactory _databaseFactory = new SqlServerDatabase();
        private static CityTreatment _cityTreatment = null;
        private static AnnouncementTreatment _announcementTreatment = null;

        private static Dictionary<int, string> _dicoDepartements = new Dictionary<int, string>
            {
                {74, "haute-savoie" },
                {01, "ain" },
                {73, "savoie" }
            };

        private static int? _maxPrice = 200000;

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
            _cachingManager = new CachingManager(_databaseFactory);
            _cityTreatment = new CityTreatment(_cachingManager, _databaseFactory);
            _announcementTreatment = new AnnouncementTreatment(_cachingManager, _databaseFactory);
            Console.OutputEncoding = Encoding.UTF8;

            //IAnnouncementWebSiteData announcementWebSite = new EspritImmoWebSite(_cachingManager);
            //IAnnouncementWebSiteData IADWebSite = new IADWebSite(_cachingManager); 
            //List<IAnnouncementWebSiteData> workers = new List<IAnnouncementWebSiteData>
            //    {
            //        { IADWebSite },
            //        {announcementWebSite }
            //    };


            //List<string> urls = IADWebSite.GetAnnoucementUrl(_dicoDepartements.Values.ToList(), _maxPrice);
            //urls.AddRange(announcementWebSite.GetAnnoucementUrl(maxPrice : _maxPrice));



            //urls.ForEach(url =>
            //{
            //    Thread.Sleep(TimeSpan.FromSeconds(2));
            //    AnnouncementInformation? announcementInformation = HeirsHelper.FindTheRightHeir(url, workers).GetAnnouncementInformation(url);

            //    if (announcementInformation != null)
            //    {
            //        _databaseFactory.InsertAnnouncementInformation(announcementInformation);
            //        _cachingManager.ForceCacheUpdateAnnouncementInformation();
            //        CheckDataRentabilityForAnnouncement(announcementInformation);
            //    }
            //});
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
            foreach (RentalInformations info in GetRentalInformations(message))
            {
                CityInformations city = _cityTreatment.GetAndInsertIfNotExisiting(info.CityInfo.CityName, info.CityInfo.ZipCode.Substring(0, 2), info.CityInfo.ZipCode);
                info.CityInfo.Id = city.Id;
                _databaseFactory.InsertRentalInformation(info);
            };
            _cachingManager.ForceCacheUpdateRentalInformations();
        }

        private static void DoLoadDataJob()
        {
            IBankWebSiteData bankWebSiteData = new PAPWebSiteData();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            LoopGetRentalData();

            IRentalWebSiteData webSiteData = new LaCoteImmoWebSiteData(_cachingManager);
            Task.Factory.StartNew(() => {
                Parallel.ForEach(_dicoDepartements,
                            new ParallelOptions { MaxDegreeOfParallelism = 3 },
                            departement =>
                            {
                                Console.WriteLine($"******{Task.CurrentId} Start work for getting price per departement* ****");
                                webSiteData.EnQueueUrls("rhone-alpes", departement.Value, departement.Key);
                                Console.WriteLine($"******{Task.CurrentId} End work for getting price per departement* ****");
                            });
            });

            List<String> departements = _dicoDepartements.Values.ToList();

            Task.Factory.StartNew(() =>
            {
                LoopForCheckRentability();
            });

            List<RateInformation> bankInformations = bankWebSiteData.GetRatesInformations("https://www.pap.fr/acheteur/barometre-taux-emprunt");

            foreach (RateInformation rate in bankInformations)
            {
                _databaseFactory.InsertRateInformation(rate);
            }
            _cachingManager.ForceCacheUpdateRatesInformation();


            IAnnouncementWebSiteData announcementWebSiteData = new Century21WebSiteData(_announcementTreatment);
            //IAnnouncementWebSiteData announcementWebSite = new EspritImmoWebSite(_cachingManager);
            IAnnouncementWebSiteData IADWebSite = new IADWebSite(_announcementTreatment);
            List<IAnnouncementWebSiteData> workers = new List<IAnnouncementWebSiteData>
                {
                    {announcementWebSiteData },
                    { IADWebSite },
                    
                    //{announcementWebSite },

                };

            workers.ForEach(worker =>
            {
                try
                {
                    Console.WriteLine($"******{Task.CurrentId} Start work for worker {worker.GetKeyword()}* ****");
                    List<String> urls = worker.GetAnnoucementUrl(_dicoDepartements.Values.ToList(), _maxPrice);
                    Console.WriteLine($"******{Task.CurrentId} Urls count : {urls.Count} * ****");
                    Parallel.ForEach(urls,
                            new ParallelOptions { MaxDegreeOfParallelism = 3 },
                            url =>
                            {
                                Console.WriteLine($"****** [{Task.CurrentId}] [{worker.GetKeyword()}]  Url to get data : {url}*****");
                                Thread.Sleep(TimeSpan.FromSeconds(1));
                                AnnouncementInformation? announcementInformation = worker.GetAnnouncementInformation(url);
                                if (announcementInformation != null)                                {
                                    CityInformations city = _cityTreatment.GetAndInsertIfNotExisiting(announcementInformation.CityInformations.CityName, announcementInformation.CityInformations.Departement, announcementInformation.CityInformations.ZipCode);
                                    announcementInformation.CityInformations.Id = city.Id;
                                    _announcementTreatment.InsertAnnouncementInformation(announcementInformation);
                                    Console.WriteLine($"******[{Task.CurrentId}] [{worker.GetKeyword()} - {url}]  Announcement : {announcementInformation?.ToString()} - Check rentability *****");
                                    CheckDataRentabilityForAnnouncement(announcementInformation);
                                    Console.WriteLine($"****** [{Task.CurrentId}] [{worker.GetKeyword()} - {url}]  Announcement : {announcementInformation?.ToString()} - DONE Check rentability *****");
                                }
                            });
                    Console.WriteLine($"{Task.CurrentId}****** END work for worker {worker.GetKeyword()}* ****");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{Task.CurrentId}Damn an Exception ! {ex}");
                }
            });


            Console.ReadKey();
            _loop = false;
        }


        private static void LoopForCheckRentability()
        {
            while (_loop)
            {
                CheckAllDataRentability();
                Thread.Sleep(TimeSpan.FromMinutes(5));
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
                    _databaseFactory.InsertLoanInformation(loan);
                }

                List<RentInformation> realRentalCosts = rentalTreament.CalculAllRentalPrices(currentsRentalInformation, announcement);
                foreach (RentInformation rent in realRentalCosts)
                {
                    rent.AnnouncementInformation = announcement;
                    _databaseFactory.InsertRentInformation(rent);
                }

                _databaseFactory.UpdateRentabilityInformation(announcement.Id);
                //RentalResult result = rentalTreament.CheckIfRentable(announcement.Price, realRentalCosts, loansInformation);
            }
        }

        private static void CheckAllDataRentability()
        {
            foreach (AnnouncementInformation announcement in _announcementTreatment.GetAnnouncementInformationWithRentabilityNotCalculated())
            {
                CheckDataRentabilityForAnnouncement(announcement);
                Thread.Sleep(TimeSpan.FromMilliseconds(2));
            }
        }
    }
}