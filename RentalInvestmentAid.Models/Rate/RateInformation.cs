﻿
using RentalInvestmentAid.Models.Rate;

namespace RentalInvestmentAid.Models.Bank
{
    public class RateInformation
    {
        public int Id { get; set; } 
        public int DurationInYear { get; set; }
        public string Rate { get; set; } = String.Empty;
        public RateType RateType { get; set; } 
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}