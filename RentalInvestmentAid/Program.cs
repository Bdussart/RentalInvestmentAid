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
using System;
using RentalInvestmentAid.Logger;

namespace RentalInvestmentAid
{
    public class Program
    {

        private static bool _loop = true;

        private static CachingManager _cachingManager = null;

        private static IDatabaseFactory _databaseFactory = new SqlServerDatabase();
        private static CityTreatment _cityTreatment = null;
        private static AnnouncementTreatment _announcementTreatment = null;
        private static BankTreatment _bankTreatment = null;

        private static RentalTreament _rentalTreament = null;

        private static Dictionary<int, string> _dicoDepartements = new Dictionary<int, string>
            {
                   {01, "Ain"},
    // {22, "Côtes-d'Armor"},
    // {29, "Finistère"},
    // {34, "Hérault"},
    // {35, "Ille-et-Vilaine"},
    {73, "Savoie"},
    {74, "Haute-Savoie"},
            };

        private static int? _maxPrice = 200000;

        private static List<IAnnouncementWebSiteData> _workers = null;

        private static List<RentalInformations> GetRentalInformations(string url)
        {
            IRentalWebSiteData webSiteData = new leFigaroWebSiteData(_cachingManager);
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
            _bankTreatment = new BankTreatment(_cachingManager, _databaseFactory);
            _rentalTreament = new RentalTreament(_cachingManager, _databaseFactory);
            Console.OutputEncoding = Encoding.UTF8;

            IAnnouncementWebSiteData announcementWebSiteData = new Century21WebSiteData(_announcementTreatment);
            IAnnouncementWebSiteData lebonCoinWebSiteData = new LeBonCoinWebSiteData(_announcementTreatment);
            //IAnnouncementWebSiteData announcementWebSite = new EspritImmoWebSite(_cachingManager);
            IAnnouncementWebSiteData IADWebSite = new IADWebSite(_announcementTreatment);
            _workers = new List<IAnnouncementWebSiteData>
                {
                    {announcementWebSiteData },
                    {IADWebSite },
                    {lebonCoinWebSiteData }
                    //{announcementWebSite },

                };

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

        private static void LoopGetAnnouncementData()
        {
            EventingBasicConsumer consumer = AnnouncementQueue.Consumer;

            consumer.Received += ReceivedAnnouncementInformation;
            AnnouncementQueue.SetConsumer(consumer);
        }

        private static void ReceivedAnnouncementInformation(object? model, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            LogHelper.LogInfo($"Received {message}");
            IAnnouncementWebSiteData worker = HeirsHelper.FindTheRightHeir(message, _workers);
            if (worker != null)
            {
                LogHelper.LogInfo($"****** [{Task.CurrentId}] [{worker.GetKeyword()}]  Url to get data : {message}*****");
                Thread.Sleep(TimeSpan.FromSeconds(1));
                AnnouncementInformation? announcementInformation = worker.GetAnnouncementInformation(message);
                if (announcementInformation != null)
                {
                    CityInformations city = _cityTreatment.GetAndInsertIfNotExisiting(announcementInformation.CityInformations.CityName, announcementInformation.CityInformations.Departement, announcementInformation.CityInformations.ZipCode);
                    announcementInformation.CityInformations.Id = city.Id;
                    _announcementTreatment.InsertAnnouncementInformation(announcementInformation);
                    LogHelper.LogInfo($"******[{Task.CurrentId}] [{worker.GetKeyword()} - {message}]  Announcement : {announcementInformation?.ToString()} - Check rentability *****");
                    CheckDataRentabilityForAnnouncement(announcementInformation);
                    LogHelper.LogInfo($"****** [{Task.CurrentId}] [{worker.GetKeyword()} - {message}]  Announcement : {announcementInformation?.ToString()} - DONE Check rentability *****");
                }
            }
        }

        private static void DoLoadDataJob()
        {
            IBankWebSiteData bankWebSiteData = new PAPWebSiteData(_bankTreatment);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            LoopGetRentalData();
            LoopGetAnnouncementData();

            IRentalWebSiteData webSiteData = new leFigaroWebSiteData(_cachingManager);
            Task.Factory.StartNew(() =>
            {
               foreach(var departement in _dicoDepartements)
               {
                   LogHelper.LogInfo($"******{Task.CurrentId} Start work for getting price per departement* ****");
                   webSiteData.EnQueueUrls("rhone-alpes", departement.Value, departement.Key);
                   LogHelper.LogInfo($"******{Task.CurrentId} End work for getting price per departement* ****");
               }
            });

            List<String> departements = _dicoDepartements.Values.ToList();

            Task.Factory.StartNew(() =>
            {
                LoopForCheckRentability();
            });


            List<RateInformation> bankInformations = bankWebSiteData.GetRatesInformations("https://www.pap.fr/acheteur/barometre-taux-emprunt");

            foreach (RateInformation rate in bankInformations)
            {
                _bankTreatment.InsertRate(rate);
            }

            Parallel.ForEach(_workers, worker =>
            {
               try
               {
                   LogHelper.LogInfo($"******{Task.CurrentId} Start work for worker {worker.GetKeyword()}* ****");
                   worker.EnQueueAnnoucementUrl(_dicoDepartements.Values.ToList(), _maxPrice);
               }
               catch (Exception ex)
               {
                   LogHelper.LogInfo($"{Task.CurrentId}Damn an Exception ! {ex}");
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

            if (_rentalTreament.CheckDataRentabilityForAnnouncement(announcement))
                _announcementTreatment.UpdateRentabilityInformation(announcement.Id);
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