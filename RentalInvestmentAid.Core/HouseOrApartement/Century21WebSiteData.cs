using HtmlAgilityPack;
using RentalInvestmentAid.Core.HouseOrApartement.Helper;
using RentalInvestmentAid.Models.HouseOrApartement;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.HouseOrApartement
{

    public class Century21WebSiteData : IHouseOrApartementWebSiteData
    {
        public HouseOrApartementInformation GetHouseOrApartementInformation(string url)
        {

            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument document = htmlWeb.Load(url);

            HtmlNodeCollection nodesInformationWhitoutPrice = document.DocumentNode.SelectNodes("/html/body/main/article/div/section/div[1]/div[1]/h1");

            HtmlNodeCollection nodesInformationWithPrice = document.DocumentNode.SelectNodes("/html/body/main/article/div/section/div[1]/div[2]/div[1]/p[1]");
            HtmlNodeCollection nodesWithDescription = document.DocumentNode.SelectNodes("/html/body/main/article/div/div[2]/section[1]/div");
            List <HtmlNode> spanInfos = nodesInformationWhitoutPrice.First().ChildNodes.Where(child => child.Name.Equals("span", StringComparison.CurrentCultureIgnoreCase)).ToList();
            

            string[] locationInformations =  spanInfos[2].InnerText.Trim().Split("-");
            HouseOrApartementInformation houseOrApartementInformation = new HouseOrApartementInformation()
            {
                RentalType = KeyWordsHelper.GetRentalType(spanInfos[0].InnerText.Trim()),
                Metrage = HtmlWordsHelper.CleanHtml(spanInfos[1].InnerText.Trim().Split("-")[1].Split("m")[0].Trim()),
                Location = HtmlWordsHelper.CleanHtml(locationInformations[0].Trim()),
                ZipCode = HtmlWordsHelper.CleanHtml(locationInformations[1].Trim()),
                Price = new string(HtmlWordsHelper.CleanHtml(nodesInformationWithPrice[0].InnerText).Where(char.IsDigit).ToArray()),
                Description = HtmlWordsHelper.CleanHtml(nodesWithDescription[0].InnerText.Trim())
            };
            return houseOrApartementInformation;
        }
    }
}
