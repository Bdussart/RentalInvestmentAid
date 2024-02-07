// See https://aka.ms/new-console-template for more information

using RentalInvestmentAid.Core;
using RentalInvestmentAid.Core.Bank;
using RentalInvestmentAid.Core.HouseOrApartement;
using RentalInvestmentAid.Core.Rental;
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
                "https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/savoie/le-bourget-du-lac/730051.htm"
            };

            //foreach (string url in listOfWebSite)
            //{
            //    rentalInformations.AddRange(GetRentalInformations(url));
            //}


            //IBankWebSiteData bankWebSiteData = new PAPWebSiteData();

            //bankWebSiteData.GetRatesInformations("https://www.pap.fr/acheteur/barometre-taux-emprunt");

            //            FinancialCalcul.LoanInformation(4.46, 25, 1700, 0.30);

            IHouseOrApartementWebSiteData houseOrApartementWebSiteData = new Century21WebSiteData();
            houseOrApartementWebSiteData.GetHouseOrApartementInformation("https://www.century21.fr/trouver_logement/detail/6627930707/");

            Console.ReadLine();
        }

    }
}
