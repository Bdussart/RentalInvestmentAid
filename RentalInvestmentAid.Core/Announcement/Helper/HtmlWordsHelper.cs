using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Announcement.Helper
{
    public static class HtmlWordsHelper
    {
        public static string CleanHtml(string html)
        {           
            return Regex.Replace(html, @"<[^>]+>|&nbsp;", "").Trim(); ;
        }
    }
}
