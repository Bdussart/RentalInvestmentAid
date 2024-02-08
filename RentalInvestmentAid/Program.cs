// See https://aka.ms/new-console-template for more information

using RentalInvestmentAid.Core;
using RentalInvestmentAid.Core.Bank;
using RentalInvestmentAid.Core.HouseOrApartement;
using RentalInvestmentAid.Core.Rental;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.HouseOrApartement;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models.Rental;
using System.ComponentModel;
using System.Text;

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
            Console.OutputEncoding = Encoding.UTF8;

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

            List<RentalInformations> rentalInformations = GetRentalInformations("https://www.lacoteimmo.com/prix-de-l-immo/location/rhone-alpes/haute-savoie/vulbens/740314.htm");

            Console.WriteLine("****** Annoucement Information *****");
            HouseOrApartementInformation houseOrApartementInformation = houseOrApartementWebSiteData.GetHouseOrApartementInformation("https://www.century21.fr/trouver_logement/detail/5187388611/");
           
            Console.WriteLine("****** Find the right rental information *****");
            RentalInformations? currentRentalInformation = rentalTreament.FindRentalInformationForAnAnnoucement(rentalInformations, houseOrApartementInformation);

            Console.WriteLine("****** Find the right rental information Check if not null *****");
            if (currentRentalInformation == null)
                throw new NullReferenceException("Damn the current rental is Null !");

            Console.WriteLine("****** Get rates for the loan *****");
            List<BankInformation> bankInformations = bankWebSiteData.GetRatesInformations("https://www.pap.fr/acheteur/barometre-taux-emprunt");

            Console.WriteLine("****** Let's go for some Math ! *****");

            Console.WriteLine("****** Calcul the loan for each duration *****");
            List<RealLoanCost> realRentalCosts = rentalTreament.CalculAllLoan(bankInformations, houseOrApartementInformation.Price);

            Console.WriteLine("****** Calcul all prices for the rent *****");
            rentalTreament.CalculAllRentalPrices(currentRentalInformation, ref houseOrApartementInformation);


            Console.WriteLine("****** Check viability of the rent *****");
            RentalResult result =  rentalTreament.CheckIfRentable(houseOrApartementInformation, realRentalCosts);


            Console.WriteLine("****** There is the result :  *****");

            DisplayResult(result);
            Console.WriteLine("********-- Ending process --*******");
        }


        private static void  DisplayResult(RentalResult result)
        {
            Console.WriteLine("----------------------------------------------------------------------------");
            Console.WriteLine("----------------------------------------------------------------------------");

            Console.WriteLine($"--Pour l'annonce :{result.HouseOrApartementInformation.UrlWebSite}");
            Console.WriteLine($"--Située         :{result.HouseOrApartementInformation.City} - {result.HouseOrApartementInformation.ZipCode}");
            Console.WriteLine($"--Description    :{result.HouseOrApartementInformation.Description}");
            Console.WriteLine($"--Prix           :{result.HouseOrApartementInformation.Price}");




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
