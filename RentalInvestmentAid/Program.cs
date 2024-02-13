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
using static System.Net.WebRequestMethods;
using RentalInvestmentAid.Core.Announcement.Helper;
using RentalInvestmentAid.Database;

namespace RentalInvestmentAid
{

    public class Program {

        private static List<RentalInformations> GetRentalInformations(string url)
        {
            IRentalWebSiteData webSiteData = new LaCoteImmoWebSiteData();

            List<RentalInformations> rentalInformations = new List<RentalInformations>();

            Thread.Sleep(TimeSpan.FromSeconds(2)); //Take easy for the external server :)
            rentalInformations.Add(webSiteData.GetApartmentRentalInformation(url));

            Thread.Sleep(TimeSpan.FromSeconds(2)); //Take easy for the external server :)
            rentalInformations.Add(webSiteData.GetHouseRentalInformation(url));
            return (rentalInformations);
        }

        public static void Main(string[] args)
        {


            Console.OutputEncoding = Encoding.UTF8;

            //List<RentalInformations> rentalInformations = new List<RentalInformations>();
            List<string> listOfWebSiteForRentalInformation = new List<string>
            {
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/seyssel/740269.htm",
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/ain/champagne-en-valromey/10079.htm",
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/ain/hauteville-lompnes/10185.htm",
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/savoie/chanaz/730073.htm",
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/clarafond-arcine/740077.htm",
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/ain/seyssel/10407.htm",
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/ain/corbonod/10118.htm",
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/savoie/aix-les-bains/730008.htm",
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/savoie/le-bourget-du-lac/730051.htm",
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/desingy/740100.htm",
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/vulbens/740314.htm"
            };

            //List<string> listOfAnnoncmentWebSite = new List<string>
            //{
            //    "https://www.leboncoin.fr/offre/ventes_immobilieres/2460065778",
            //    "https://www.century21.fr/trouver_logement/detail/5187388611/",
            //};


            //listOfAnnoncmentWebSite.ForEach(url => {
            //    HeirsHelper.FindTheRightHeir(url, announcementWebSiteDatas).GetAnnouncementInformation(url);
            //});
            // DoTheJob();
            //IDatabaseFactory databaseFactory = new SqlServerDatabase();
            //listOfWebSiteForRentalInformation.ForEach(url =>
            //{
            //    foreach (var info in GetRentalInformations(url))
            //    {
            //        databaseFactory.InsertRentalInformation(info);
            //    }
            //});
            // var plop = databaseFactory.RentalInformations;


            List<IAnnouncementWebSiteData> announcementWebSiteDatas = new List<IAnnouncementWebSiteData>
            {
                { new Century21WebSiteData() },
            };

            List<string> departements = new List<string>
            {
                "haute-savoie","ain", "savoie"
            };

            new Century21WebSiteData().GetAnnoucementUrl(departements, 200000);


            Console.ReadLine();
        }

        private static void DoTheJob()
        {
            Console.WriteLine("********-- Starting process --*******");
            IAnnouncementWebSiteData announcementWebSiteData = new Century21WebSiteData();
            IBankWebSiteData bankWebSiteData = new PAPWebSiteData();
            RentalTreament rentalTreament = new RentalTreament();

            /* Simple Process Minimum Viable Process => Test annonce sur Vulbens */

            Console.WriteLine("****** Getting Data *****");
            Console.WriteLine("****** Rental Information *****");

            List<RentalInformations> rentalInformations = GetRentalInformations("https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/vulbens/740314.htm");

            Console.WriteLine("****** Annoucement Information *****");
            AnnouncementInformation announcementInformation = announcementWebSiteData.GetAnnouncementInformation("https://www.century21.fr/trouver_logement/detail/5187388611/");
           
            Console.WriteLine("****** Find the right rental information *****");
            RentalInformations? currentRentalInformation = rentalTreament.FindRentalInformationForAnAnnoucement(rentalInformations, announcementInformation);

            Console.WriteLine("****** Find the right rental information Check if not null *****");
            if (currentRentalInformation == null)
                throw new NullReferenceException("Damn the current rental is Null !");

            Console.WriteLine("****** Get rates for the loan *****");
            List<BankInformation> bankInformations = bankWebSiteData.GetRatesInformations("https://www.pap.fr/acheteur/barometre-taux-emprunt");

            Console.WriteLine("****** Let's go for some Math ! *****");

            Console.WriteLine("****** Calcul the loan for each duration *****");
            List<RealLoanCost> realRentalCosts = rentalTreament.CalculAllLoan(bankInformations, announcementInformation.Price);

            Console.WriteLine("****** Calcul all prices for the rent *****");
            rentalTreament.CalculAllRentalPrices(currentRentalInformation, ref announcementInformation);


            Console.WriteLine("****** Check viability of the rent *****");
            RentalResult result =  rentalTreament.CheckIfRentable(announcementInformation, realRentalCosts);


            Console.WriteLine("****** There is the result :  *****");

            DisplayResult(result);
            Console.WriteLine("********-- Ending process --*******");
        }


        private static void  DisplayResult(RentalResult result)
        {
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("----------------------------------------------------------------------------");

            Console.WriteLine($"--Pour l'annonce :{result.AnnouncementInformation.UrlWebSite}");
            Console.WriteLine($"--Située         :{result.AnnouncementInformation.City} - {result.AnnouncementInformation.ZipCode}");
            Console.WriteLine($"--Description    :{result.AnnouncementInformation.Description}");
            Console.WriteLine($"--Prix           :{result.AnnouncementInformation.Price}");




            foreach (var res in result.LoanInformationWithRentalInformation)
            {
                Console.WriteLine("***************************************************************************");

                var rateDescription = res.Type.GetType()
                    .GetMember(res.Type.ToString())[0]
                    .GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)[0] as DescriptionAttribute;
                Console.WriteLine($"-----Pour un prêt de         :{res.DurationInYear} ans");
                Console.WriteLine($"-----avec un taux à          :{(res.Rate).ToString("#.##")}({rateDescription.Description})");
                Console.WriteLine($"-----Le coût total est de    :{res.TotalCost.ToString("#.##")}€");
                Console.WriteLine($"-----Le coût par mois est de :{res.MonthlyCost.ToString("#.##")}€");

                Console.WriteLine("UUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU");
                foreach (RealRentalCost rental in res.RealRentalCosts)
                {

                    var rentalDescription = rental.Type.GetType()
                        .GetMember(rental.Type.ToString())[0]
                        .GetCustomAttributes(typeof(DescriptionAttribute), inherit: false)[0] as DescriptionAttribute;

                    Console.WriteLine($"******************** Pour une location au prix    :{rental.RealPrice.ToString("#.##")}€ par mois -> 70% : {rental.Rental70Pourcent.ToString("#.##")}€");
                    Console.WriteLine($"******************** Pour un prix mètre carré de  :{rental.PricePerSquareMeter.ToString("#.##")}€ - {rentalDescription.Description}");
                    if (rental.IsViable.HasValue && rental.IsViable.Value)
                        Console.WriteLine($"///////////////////////////////// Ce bien est rentable \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\");
                    else
                        Console.WriteLine($"\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\ Ce bien est pas rentable /////////////////////////////////");

                    Console.WriteLine("UUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU");
                }

            }


            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("----------------------------------------------------------------------------");
        }
    }
}
