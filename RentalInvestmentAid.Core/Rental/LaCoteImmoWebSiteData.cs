using HtmlAgilityPack;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Rental
{
    public class LaCoteImmoWebSiteData : IRentalWebSiteData
    {
        public RentalInformations GetApartmentRentalInformation(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument document = htmlWeb.Load(url);

            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/table[1]/tbody[1]/tr/td");

            HtmlNodeCollection nodeForZipCode = document.DocumentNode.SelectNodes("//*[@id=\"wrapper\"]/script[2]/text()");



            //*[@id="wrapper"]/script[2]/text()
            RentalInformations rentalInformations = new RentalInformations()
            {
                City = nodes[0].InnerText,
                LowerPrice = nodes[1].InnerText.Split(" ")[0],
                MediumPrice = nodes[2].InnerText.Split(" ")[0],
                HigherPrice = nodes[3].InnerText.Split(" ")[0],
                ZipCode = new string(nodeForZipCode[0].InnerText.Split("\n")[2].Where(char.IsDigit).ToArray()),
                RentalType = RentalType.Apartment
            };

            return rentalInformations;
        }

        public RentalInformations GetHouseRentalInformation(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument document = htmlWeb.Load(url);

            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html[1]/body[1]/div[2]/div[2]/div[2]/div[2]/div[1]/table[1]/tbody[1]/tr/td");

            HtmlNodeCollection nodeForZipCode = document.DocumentNode.SelectNodes("//*[@id=\"wrapper\"]/script[2]/text()");
            HtmlNodeCollection nodeForCity = document.DocumentNode.SelectNodes("/html[1]/body[1]/div[2]/div[2]/div[2]/div[1]/div[1]/table[1]/tbody[1]/tr/td");
            //*[@id="wrapper"]/script[2]/text()
            RentalInformations rentalInformations = new RentalInformations()
            {
                City = nodeForCity[0].InnerText,
                LowerPrice = nodes[0].InnerText.Split(" ")[0],
                MediumPrice = nodes[1].InnerText.Split(" ")[0],
                HigherPrice = nodes[2].InnerText.Split(" ")[0],
                ZipCode = new string(nodeForZipCode[0].InnerText.Split("\n")[2].Where(char.IsDigit).ToArray()),
                RentalType = RentalType.House
            };

            return rentalInformations;
        }
    }
}
