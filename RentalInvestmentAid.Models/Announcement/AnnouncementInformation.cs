﻿using RentalInvestmentAid.Models.City;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models.Announcement
{
    public class AnnouncementInformation
    {
        public int Id { get; set; }
        public AnnouncementProvider AnnouncementProvider { get; set; }
        public RentalTypeOfTheRent RentalType { get; set; }

        public CityInformations CityInformations { get; set; } = null;
        public string IdFromProvider { get; set; } = String.Empty;
        public string Price { get; set; } = String.Empty;
        public string Metrage { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string UrlWebSite { get; set; } = String.Empty;
        public bool RentabilityCalculated { get; set; }
        public string InformationProvidedByGemini { get; set; } = String.Empty;
        public bool Readed { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<RentInformation> RentalCost { get; private set; } = new List<RentInformation>();

        public bool? IsRentable { get; set; }

        public void SetRealRentalCost(List<RentInformation> realRentalCosts)
        {
            RentalCost = realRentalCosts;
        }


        public override string ToString()
        {
            return $"-AnnouncementInformation-[{Id} - {IdFromProvider}] {CityInformations?.CityName} - {CityInformations?.ZipCode} - {Price} - {Metrage} : {UrlWebSite}";
        }
    }
}
