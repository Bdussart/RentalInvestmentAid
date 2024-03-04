using MathNet.Numerics.Distributions;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Announcement.Helper
{
    public static class InteratiorHelper
    {
        public static void ImitateHumanTyping(string sentenceToImitate, IWebElement element)
        {
            foreach (char c in sentenceToImitate)
            {
                Normal normalDist = new Normal(50, 10);
                int randomGaussianValue = (int)normalDist.Sample();
                Thread.Sleep(randomGaussianValue * 15 + 50);
                string s = new StringBuilder().Append(c).ToString();
                element.SendKeys(s);
            }
        }
    }
}
