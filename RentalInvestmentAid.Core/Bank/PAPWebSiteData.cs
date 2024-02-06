using HtmlAgilityPack;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Bank
{
    public class PAPWebSiteData : IBankWebSiteData
    {
        public List<BankInformation> GetRatesInformations(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            HtmlDocument document = htmlWeb.Load(url);

            HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html/body/div[1]/div[1]/table/tbody/tr");

            List<BankInformation> bankInformation = new List<BankInformation>();

            foreach (HtmlNode node in nodes)
            {
                List<HtmlNode> childs = node.ChildNodes.Where(child => child.Name.Equals("td", StringComparison.InvariantCultureIgnoreCase)).ToList();
                bankInformation.Add(new BankInformation
                {
                    Duration = int.Parse(childs[0].InnerText),
                    LowerRate = childs[4].InnerText,
                    MarketRate = childs[3].InnerText,
                    MaxRate = childs[2].InnerText
                });
            }
            return bankInformation;
        }
    }
}
