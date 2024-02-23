// See https://aka.ms/new-console-template for more information

using RentalInvestmentAid.Core;
using RentalInvestmentAid.Core.Bank;
using RentalInvestmentAid.Core.Announcement;
using RentalInvestmentAid.Core.Rental;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models.Rental;
using System.ComponentModel;
using System.Text;
using RentalInvestmentAid.Database;
using System.Diagnostics;
using Microsoft.IdentityModel.Logging;

namespace RentalInvestmentAid
{
    public class Program {

        private static List<RentalInformations> GetRentalInformations(string url)
        {
            IRentalWebSiteData webSiteData = new LaCoteImmoWebSiteData();

            List<RentalInformations> rentalInformations = new List<RentalInformations>();

            Thread.Sleep(TimeSpan.FromSeconds(2)); //Take easy for the external server :)
            rentalInformations.AddRange(webSiteData.GetApartmentRentalInformation(url));

            Thread.Sleep(TimeSpan.FromSeconds(2)); //Take easy for the external server :)
            rentalInformations.AddRange(webSiteData.GetHouseRentalInformation(url));
            return (rentalInformations);
        }
        
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            DoLoadDataJob();


            //IDatabaseFactory databaseFactory = new SqlServerDatabase();

            ////IRentalWebSiteData webSiteData = new LaCoteImmoWebSiteData();
            ////webSiteData.GetUrlForRentalInformation("rhone-alpes", "haute-savoie", 74);
            ////List<RentalInformations> rentalInformations = new List<RentalInformations>();
            //List<string> listOfWebSiteForRentalInformation = new List<string>
            //{
            //    "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/seyssel/740269.htm",
            //    "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/ain/champagne-en-valromey/10079.htm",
            //    //"https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/ain/hauteville-lompnes/10185.htm",
            //    //"https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/savoie/chanaz/730073.htm",
            //    //"https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/clarafond-arcine/740077.htm",
            //    //"https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/ain/seyssel/10407.htm",
            //    //"https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/ain/corbonod/10118.htm",
            //    //"https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/savoie/aix-les-bains/730008.htm",
            //    //"https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/savoie/le-bourget-du-lac/730051.htm",
            //    //"https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/desingy/740100.htm",
            //    //"https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/vulbens/740314.htm"
            //};

            //List<string> listOfAnnoncmentWebSite = new List<string>
            //{
            //    "https://www.leboncoin.fr/offre/ventes_immobilieres/2460065778",
            //    "https://www.century21.fr/trouver_logement/detail/5187388611/",
            //};


            //listOfAnnoncmentWebSite.ForEach(url =>
            //{
            //    HeirsHelper.FindTheRightHeir(url, announcementWebSiteDatas).GetAnnouncementInformation(url);
            //});
            // DoTheJob();
            //listOfWebSiteForRentalInformation.ForEach(url =>
            //{
            //    foreach (var info in GetRentalInformations(url))
            //    {
            //        databaseFactory.InsertRentalInformation(info);
            //    }
            //});
            //var plop2 = databaseFactory.RentalInformations;


            //List<IAnnouncementWebSiteData> announcementWebSiteDatas = new List<IAnnouncementWebSiteData>
            //{
            //    { new Century21WebSiteData() },
            //};


            //IBankWebSiteData bankWebSiteData = new PAPWebSiteData();
            //List<RateInformation> bankInformations = bankWebSiteData.GetRatesInformations("https://www.pap.fr/acheteur/barometre-taux-emprunt");

            //foreach (RateInformation rate in bankInformations)
            //{
            //    databaseFactory.InsertRateInformation(rate);
            //}

            //var plop = databaseFactory.RateInformations;
            ////Console.ReadLine();
            //DoTheCentury21Job();
        }


        private static void DoLoadDataJob()
        {
            IDatabaseFactory databaseFactory = new SqlServerDatabase();
            IBankWebSiteData bankWebSiteData = new PAPWebSiteData();

            Task[] tasks = new Task[3];

            Dictionary<int, string> dicoDepartements = new Dictionary<int, string>
            {
                //{ 74, "haute-savoie" },
                //{01, "ain" },
                {73, "savoie" }
            };

            List<String> departements = dicoDepartements.Values.ToList();

            tasks[0] = Task.Factory.StartNew(() =>
            {
                IRentalWebSiteData webSiteData = new LaCoteImmoWebSiteData();
                Parallel.ForEach(dicoDepartements, departement =>
                {
                    {
                        List<String> urls = new List<string>();

                        urls = webSiteData.GetUrlForRentalInformation("rhone-alpes", departement.Value, departement.Key);

                        urls.ForEach(url =>
                        {
                            foreach (var info in GetRentalInformations(url))
                            {
                                databaseFactory.InsertRentalInformation(info);
                            }
                        });
                    }
                });
            });
            tasks[1] = Task.Factory.StartNew(() =>
            {
                List<RateInformation> bankInformations = bankWebSiteData.GetRatesInformations("https://www.pap.fr/acheteur/barometre-taux-emprunt");

                foreach (RateInformation rate in bankInformations)
                {
                    databaseFactory.InsertRateInformation(rate);
                }
            });
            tasks[2] = Task.Factory.StartNew(() =>
            {
                IAnnouncementWebSiteData announcementWebSiteData = new Century21WebSiteData();

                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                List<String> urls = announcementWebSiteData.GetAnnoucementUrl(departements, 200000);
                urls.ForEach(url =>
                {
                    Thread.Sleep(TimeSpan.FromSeconds(2));
                    AnnouncementInformation announcementInformation = announcementWebSiteData.GetAnnouncementInformation(url);
                    databaseFactory.InsertAnnouncementInformation(announcementInformation);
                });
            });


            Task.Factory.ContinueWhenAll(tasks, task =>
            {
                Console.Write("DONE");
                CheckAllDataRentability();
            });

            CheckAllDataRentability();


            Console.ReadKey();

        }

        private static void CheckAllDataRentability()
        {
            IDatabaseFactory databaseFactory = new SqlServerDatabase();

            RentalTreament rentalTreament = new RentalTreament();

            foreach (AnnouncementInformation announcement in databaseFactory.AnnouncementInformations)
            {               
                List<RentalInformations> currentsRentalInformation = rentalTreament.FindRentalInformationForAnAnnoucement(databaseFactory.RentalInformations, announcement);

                Console.WriteLine("****** Find the right rental information Check if not null *****");
                if (currentsRentalInformation.Count == 0)
                    Logger.LogHelper.LogInfo($"{announcement.ToString()} - Don't find rental information -----");
                else
                {
                    List<LoanInformation> loansInformation = rentalTreament.CalculAllLoan(databaseFactory.RateInformations, announcement.Price);

                    foreach (LoanInformation loan in loansInformation) {
                        loan.AnnouncementInformation = announcement;
                        databaseFactory.InsertLoanInformation(loan);
                    }

                    List<RentInformation> realRentalCosts = rentalTreament.CalculAllRentalPrices(currentsRentalInformation,  announcement);
                    foreach (RentInformation rent in realRentalCosts)
                    {
                        rent.AnnouncementInformation = announcement;
                        databaseFactory.InsertRentInformation(rent);
                    }
                    //RentalResult result = rentalTreament.CheckIfRentable(announcement.Price, realRentalCosts, loansInformation);
                }
            }
        }
        private static void DoTheCentury21Job()
        {

            List<string> departements = new List<string>
            {
                "haute-savoie","ain", "savoie"
            };

            IAnnouncementWebSiteData announcementWebSiteData = new Century21WebSiteData();
            IDatabaseFactory databaseFactory = new SqlServerDatabase();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Console.WriteLine("********-- Starting process --*******");
            Console.WriteLine("********-- DoTheCentury21Job --*******");

            Console.WriteLine($"********-- Start Searching announcement for : {String.Join(",", departements)} --*******");
            List<String> urls = announcementWebSiteData.GetAnnoucementUrl(departements, 200000);

            Console.WriteLine($"********-- End Searching announcement for : {String.Join(",", departements)} --*******");
            Console.WriteLine($"********-- urls : {urls.Count}  --*******");
            Console.WriteLine($"********-- elapsed : {stopwatch.Elapsed.TotalSeconds}  --*******");

            Console.WriteLine($"********-- Start  Announcement information --*******");
            urls.ForEach(url =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(2));
                Console.WriteLine($"********-- url : {url} --*******");
                AnnouncementInformation announcementInformation = announcementWebSiteData.GetAnnouncementInformation(url);
                Console.WriteLine($"********-- END url : {url}  --*******");
                Console.WriteLine($"********-- elapsed : {stopwatch.Elapsed.TotalSeconds}  --*******");


                Console.WriteLine($"********-- Insert in database : {url} --*******");
                databaseFactory.InsertAnnouncementInformation(announcementInformation);
                Console.WriteLine($"********-- elapsed : {stopwatch.Elapsed.TotalSeconds}  --*******");
            });

            Console.WriteLine($"********-- STOP  Announcement information --*******");
            Console.WriteLine($"********-- elapsed : {stopwatch.Elapsed.TotalSeconds}  --*******");

            var plop = databaseFactory.AnnouncementInformations;
            Console.ReadLine();
        }
      
    }
}
