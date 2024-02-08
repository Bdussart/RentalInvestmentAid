using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Announcement
{
    public class LeBonCoinWebSiteData : IAnnouncementWebSiteData
    {
        public AnnouncementInformation GetAnnouncementInformation(string url)
        {


            HtmlWeb htmlWeb = new HtmlWeb();
            string html = string.Empty;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--enable-javascript");
            options.AddArgument("--window-size=500,1080");
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(url);

                html = driver.PageSource;

                driver.Close();
            }

            // TO DO It's working with Selenium ! need to implement it :)  

            //HtmlWeb htmlWeb = new HtmlWeb();

            //htmlWeb.PreRequest += (request) =>
            //{
            //    request.Headers.Add("authority", "www.leboncoin.fr");
            //    request.Headers.Add("method", "GET");
            //    request.Headers.Add("path", $"/offre/ventes_immobilieres/{url.Split("/")[3]}");
            //    request.Headers.Add("scheme", "https");
            //    request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            //    request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            //    request.Headers.Add("Accept-Language", "fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7");
            //    request.Headers.Add("Cache-Control", "max-age=0");
            //    request.Headers.Add("Cookie", "__Secure-Install=b5694d96-1ce7-4779-97de-f74d5777fc52; __Secure-InstanceId=b5694d96-1ce7-4779-97de-f74d5777fc52; didomi_token=eyJ1c2VyX2lkIjoiMThkNGM3ZTgtMmYwMC02YWFiLTgzODctYzA3NDQ2ZDRhYTA0IiwiY3JlYXRlZCI6IjIwMjQtMDEtMjdUMTk6NTY6MTYuMTM3WiIsInVwZGF0ZWQiOiIyMDI0LTAxLTI3VDE5OjU2OjE3LjYzOVoiLCJ2ZW5kb3JzIjp7ImVuYWJsZWQiOlsiZ29vZ2xlIiwiYzpsYmNmcmFuY2UiLCJjOnJldmxpZnRlci1jUnBNbnA1eCIsImM6ZGlkb21pIiwiYzp0YWlsdGFyZ2V0IiwiYzpzcG9uZ2VjZWxsIiwiYzp6YW5veCIsImM6cHVycG9zZWxhLTN3NFpmS0tEIiwiYzppbmZlY3Rpb3VzLW1lZGlhIiwiYzp0dXJibyIsImM6dGlrdG9rLUtaQVVRTFo5IiwiYzphZGltby1QaFVWbTZGRSIsImM6Z29vZ2xlYW5hLTRUWG5KaWdSIiwiYzp1bmRlcnRvbmUtVExqcWRUcGYiLCJjOnJvY2tlcmJveC1mVE04RUo5UCIsImM6YWZmaWxpbmV0Il19LCJwdXJwb3NlcyI6eyJlbmFibGVkIjpbImV4cGVyaWVuY2V1dGlsaXNhdGV1ciIsIm1lc3VyZWF1ZGllbmNlIiwicGVyc29ubmFsaXNhdGlvbm1hcmtldGluZyIsInByaXgiLCJkZXZpY2VfY2hhcmFjdGVyaXN0aWNzIiwiZ2VvbG9jYXRpb25fZGF0YSJdfSwidmVuZG9yc19saSI6eyJlbmFibGVkIjpbImdvb2dsZSIsImM6cHVycG9zZWxhLTN3NFpmS0tEIiwiYzppbmZlY3Rpb3VzLW1lZGlhIiwiYzp0dXJibyIsImM6YWRpbW8tUGhVVm02RkUiLCJjOnVuZGVydG9uZS1UTGpxZFRwZiIsImM6cm9ja2VyYm94LWZUTThFSjlQIiwiYzphZmZpbGluZXQiXX0sInZlcnNpb24iOjIsImFjIjoiRExXQThBRVlBTElCVWdFbGdRREFpU0JLUURFUUhUZ09yQWdZQkJ1Q0tnRWM0Skp3UzFnbXRCUVlDaEVGRm9LNTRXQ2hZTUMxVUZ0NExnd1hHQXVXQmdNRENJR1dvQUFBLkRMV0E4QUVZQUxJQlVnRWxnUURBaVNCS1FERVFIVGdPckFnWUJCdUNLZ0VjNEpKd1MxZ210QlFZQ2hFRkZvSzU0V0NoWU1DMVVGdDRMZ3dYR0F1V0JnTURDSUdXb0FBQSJ9; euconsent-v2=CP5CssAP5CssAAHABBENAkEsAP_gAELgAAAAJoNB_G_dTSNi8Xp1YPtwcQ1P4VAjoiAABgaJAwwBiBLAMIwEhmAAIAGqAAACABAEIDZAAQBlCAHAAAAAYIAAAyEMAAAAARAIJgAAAEAAAmJICABJCYAgAQAQgkgAABUAgAIAABsgSFAAAAAAFAAAACAAAAAAAAAAAAAAQAAAAAAAAgAAAAAAAAAAAAAEABAAAAAAAAAAAAAAAAAEEAQAzDQqIAGwJCQikHCIAACIIAgAgBAAAAJAwQAABAgAEAYACjAAAABFAAAAAAAAEBAAAAAAgAQgAAAAIEAAAAAEAAAAAAgEAAAAACAAABAAAAAEAMAQAAAAgAAAAAIAQAAgAAgAJCAAAAAAAgAAABAAAAQAEAAAAAAAAAAAAAAAAQAxQAGAAIg1DAAMAARBqIAAYAAiDUAA.f_wACFwAAAAA; _ga=GA1.1.1187106595.1706385378; _gcl_au=1.1.130917612.1706385378; ry_ry-l3b0nco_realytics=eyJpZCI6InJ5X0FEQjAzN0JFLTVGREYtNDgxRC1CNzg4LTk0NjAyMEYyODZEQyIsImNpZCI6bnVsbCwiZXhwIjoxNzM3OTIxMzc4MTM2LCJjcyI6MX0%3D; _fbp=fb.1.1706385390857.1759660633; __gsas=ID=d3045c774d0596f3:T=1706385387:RT=1706385387:S=ALNI_MZgcHK0GWJJqxi9VqqWvkVOumjaBg; trc_cookie_storage=taboola%2520global%253Auser-id%3Dbf91d010-984b-4675-8e00-3ea4d73e903a-tuctc50ddae; lux_uid=170729376078375995; include_in_experiment=true; dblockV=2; ry_ry-l3b0nco_so_realytics=eyJpZCI6InJ5X0FEQjAzN0JFLTVGREYtNDgxRC1CNzg4LTk0NjAyMEYyODZEQyIsImNpZCI6bnVsbCwib3JpZ2luIjp0cnVlLCJyZWYiOm51bGwsImNvbnQiOm51bGwsIm5zIjpmYWxzZSwic2MiOiJvayIsInNwIjpudWxsfQ%3D%3D; adview_clickmeter=search__listing__0__a29ed0e0-d258-4632-97af-74af672fd364; atauthority=%7B%22name%22%3A%22atauthority%22%2C%22val%22%3A%7B%22authority_name%22%3A%22default%22%2C%22visitor_mode%22%3A%22optin%22%7D%2C%22options%22%3A%7B%22end%22%3A%222025-03-08T08%3A25%3A27.616Z%22%2C%22path%22%3A%22%2F%22%7D%7D; atidvisitor=%7B%22name%22%3A%22atidvisitor%22%2C%22val%22%3A%7B%22an%22%3A%220%22%2C%22ac%22%3A%220%22%2C%22vrn%22%3A%22-562498-%22%7D%2C%22options%22%3A%7B%22path%22%3A%22%2F%22%2C%22session%22%3A34128000%2C%22end%22%3A34128000%7D%7D; ivBlk=n; _ga_Z707449XJ2=GS1.1.1707293762.2.1.1707295133.0.0.0; _ga_PREJSHXDSS=GS1.1.1707293762.2.1.1707295133.0.0.0; utag_main=v_id:018d4c7e8328001aa099fd3e0dc20506f001906700bd0$_sn:2$_ss:0$_st:1707296935309$_pn:17%3Bexp-session$ses_id:1707293761936%3Bexp-session; cto_bundle=DtDIuF95YXFseWNVMnVtQ2tITjdpRlNXJTJCbWJvdHdkSmpRbUNub1VqbEQwYnAxN3dRcVA5V1JRSEElMkZ3QSUyRnhXRGxZYk4xd2hUY2RNYlM5UzJydXpicXFZN2VVbSUyQnh5OWJQSWlCJTJGQ0FqNU5qcmdybldOWFc2TnRhNWU0QzI2djFjRzR3SW9ORTRWanNObEtKRWhiU3Y0QlN5cDRTZnpKSDZRM3haYW1ZeGVLdnFseDUwJTNE; __gads=ID=4bb439d34bdb635e:T=1706385378:RT=1707295136:S=ALNI_MbaFE6g5xzq69SrsbQVcvRYgEYmlQ; __eoi=ID=f1d1f959b60cff71:T=1707293763:RT=1707295136:S=AA-AfjYyxSwPiLbDWrnKMFYvH-Vo; datadome=bJ6MyoibFghSgGOtf0J~ObeNB9RfviwehwoNc1E~6PJ4yebeknZ25ST0C68GNlrewJV3ZKbNK5G3HX11x8ifsNjPw0k3A~QtxyJPgdoX0ZGrqthspnr70JbQStAd6mhh; dblockS=39");
            //    request.Headers.Add("Sec-Ch-Device-Memory", "8");
            //    request.Headers.Add("Sec-Ch-Ua", "Not A(Brand\";v=\"99\", \"Google Chrome\";v=\"121\", \"Chromium\";v=\"121");
            //    request.Headers.Add("Sec-Ch-Ua-Arch", "x86");
            //    request.Headers.Add("Sec-Ch-Ua-Full-Version-List", "Not A(Brand\";v=\"99.0.0.0\", \"Google Chrome\";v=\"121.0.6167.140\", \"Chromium\";v=\"121.0.6167.140");
            //    request.Headers.Add("Sec-Ch-Ua-Mobile", "?0");
            //    request.Headers.Add("Sec-Ch-Ua-Model", "");
            //    request.Headers.Add("Sec-Ch-Ua-Platform", "Windows");
            //    request.Headers.Add("Sec-Fetch-Dest", "document");
            //    request.Headers.Add("Sec-Fetch-Mode", "navigate");
            //    request.Headers.Add("Sec-Fetch-Site", "same-origin");
            //    request.Headers.Add("Sec-Fetch-User", "?1");
            //    request.Headers.Add("Upgrade-Insecure-Requests", "1");
            //    request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36");
            //    return true;
            //};

            //HtmlDocument document = htmlWeb.Load(url);

            //HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("//*[@id=\"grid\"]/article/div[1]");

            AnnouncementInformation announcementInformation = new AnnouncementInformation();


            return announcementInformation;
        }
    }
}
