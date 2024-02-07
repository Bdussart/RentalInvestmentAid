// See https://aka.ms/new-console-template for more information

using RentalInvestmentAid.Core;
using RentalInvestmentAid.Core.Bank;
using RentalInvestmentAid.Core.HouseOrApartement;
using RentalInvestmentAid.Core.Rental;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.HouseOrApartement;
using RentalInvestmentAid.Models.Rental;

namespace RentalInvestmentAid
{

    public class Program {

        private static List<RentalInformations>  GetRentalInformations(string url)
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
            List<RentalInformations> rentalInformations = new List<RentalInformations>();
            List<string> listOfWebSite = new List<string>
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
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/desingy/740100.htm"
            };

            //foreach (string url in listOfWebSite)
            //{
            //    rentalInformations.AddRange(GetRentalInformations(url));
            //}


            //IBankWebSiteData bankWebSiteData = new PAPWebSiteData();

            //bankWebSiteData.GetRatesInformations("https://www.pap.fr/acheteur/barometre-taux-emprunt");

            //            FinancialCalcul.LoanInformation(4.46, 25, 1700, 0.30);

            //IHouseOrApartementWebSiteData houseOrApartementWebSiteData = new Century21WebSiteData();
            //houseOrApartementWebSiteData.GetHouseOrApartementInformation("https://www.century21.fr/trouver_logement/detail/6627930707/");

            DoTheJob();
            Console.ReadLine();
        }

        private static void DoTheJob()
        {
            Console.WriteLine("********-- Starting process --*******");
            IHouseOrApartementWebSiteData houseOrApartementWebSiteData = new Century21WebSiteData();
            IBankWebSiteData bankWebSiteData = new PAPWebSiteData();
            RentalTreament rentalTreament = new RentalTreament();

            /* Simple Process Minimum Viable Process => Test annonce sur Desingy */

            Console.WriteLine("****** Getting Data *****");
            Console.WriteLine("****** Rental Information *****");

            List<RentalInformations> rentalInformations = GetRentalInformations("https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/desingy/740100.htm");

            Console.WriteLine("****** Annoucement Information *****");
            HouseOrApartementInformation houseOrApartementInformation = houseOrApartementWebSiteData.GetHouseOrApartementInformation("https://www.century21.fr/trouver_logement/detail/6301369140/");
           
            Console.WriteLine("****** Find the right rental information *****");
            RentalInformations? currentRentalInformation = rentalTreament.FindRentalInformationForAnAnnoucement(rentalInformations, houseOrApartementInformation);

            Console.WriteLine("****** Find the right rental information Check if not null *****");
            if (currentRentalInformation == null)
                throw new NullReferenceException("Damn the current rental is Null !");

            Console.WriteLine("****** Get rates for the loan *****");
            List<BankInformation> bankInformations = bankWebSiteData.GetRatesInformations("https://www.pap.fr/acheteur/barometre-taux-emprunt");

            Console.WriteLine("****** Let's go for some Math ! *****");



            Console.WriteLine("********-- Ending process --*******");

        }
    }
}
