using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.HouseOrApartement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.HouseOrApartement
{
    public class LogicImmoWebSiteData : IHouseOrApartementWebSiteData
    {
        public HouseOrApartementInformation GetHouseOrApartementInformation(string url)
        {
            throw new NotImplementedException();
            //HtmlWeb htmlWeb = new HtmlWeb();

            //htmlWeb.PreRequest += (request) =>
            //{
            //    request.Headers.Add("authority", "www.logic-immo.com");
            //    request.Headers.Add("method", "GET");
            //    request.Headers.Add("path", $"/{url.Split("/")[1]}");
            //    request.Headers.Add("scheme", "https");
            //    request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8");
            //    request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            //    request.Headers.Add("Accept-Language", "fr-FR,fr;q=0.7");
            //    request.Headers.Add("Cache-Control", "max-age=0");
            //    request.Headers.Add("Cookie", "xtvrn=$490804$; xtat490804=-; xtant490804=1");
            //    request.Headers.Add("Sec-Ch-Ua", "Not A(Brand\";v=\"99\", \"Brave\";v=\"121\", \"Chromium\";v=\"121");
            //    request.Headers.Add("Sec-Ch-Ua-Mobile", "?0");
            //    request.Headers.Add("Sec-Ch-Ua-Platform", "Windows");
            //    request.Headers.Add("Sec-Fetch-Dest", "document");
            //    request.Headers.Add("Sec-Fetch-Mode", "navigate");
            //    request.Headers.Add("Sec-Fetch-Site", "none");
            //    request.Headers.Add("Sec-Fetch-User", "?1");
            //    request.Headers.Add("Sec-Gpc", "1");
            //    request.Headers.Add("Upgrade-Insecure-Requests", "1");
            //    request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36");
            //    return true;
            //};

            //HtmlDocument document = htmlWeb.Load(url);

            //HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html/body/main/div/div[1]/div[2]/section/div[2]/h1");

            //ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--enable-javascript");
            //options.AddArgument("--window-size=500,1080");
            //using (IWebDriver driver = new ChromeDriver(options))
            //{
            //    driver.Navigate().GoToUrl(url);
                
            
            //}         



            //HouseOrApartementInformation houseOrApartementInformation = new HouseOrApartementInformation();


            //return houseOrApartementInformation;
        }
    }
}
