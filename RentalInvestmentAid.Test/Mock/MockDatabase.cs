using RentalInvestmentAid.Database;
using RentalInvestmentAid.Models;
using RentalInvestmentAid.Models.Announcement;
using RentalInvestmentAid.Models.Bank;
using RentalInvestmentAid.Models.City;
using RentalInvestmentAid.Models.Loan;
using RentalInvestmentAid.Models.Rental;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Test.Mock
{
    internal class MockDatabase : IDatabaseFactory
    {
        private List<AnnouncementInformation> _announcementInformation = new List<AnnouncementInformation>();
        private List<CityInformations> _cityInformation = new List<CityInformations>();
        private List<LoanInformation> _loanstInformation = new List<LoanInformation>();
        private List<RateInformation> _rateInformation = new List<RateInformation>();
        private List<RentabilityResult> _rentabilityInformation = new List<RentabilityResult>();
        private List<RentalInformations> _rentalInformation = new List<RentalInformations>();
        private List<RentInformation> _rentInformation = new List<RentInformation>();
        public List<AnnouncementInformation> GetAnnouncementsInformations()
        {
            return _announcementInformation.OrderBy(x=> x.Id).ToList();
        }

        public List<CityInformations> GetCities()
        {
            return _cityInformation.OrderBy(x => x.Id).ToList();
        }

        public List<CityInformations> GetCitiesWithNoRent()
        {
            throw new NotImplementedException();
        }

        public List<DepartmentToSearchData> GetDepartmentToSearchDatas()
        {
            throw new NotImplementedException();
        }

        public List<LoanInformation> GetLoansInformations()
        {
            return _loanstInformation.OrderBy(x => x.Id).ToList();
        }

        public List<RateInformation> GetRatesInformations()
        {
            return _rateInformation.OrderBy(x => x.Id).ToList();
        }

        public List<RentabilityResult> GetRentabilitiesResults()
        {
            return _rentabilityInformation;
        }

        public List<RentalInformations> GetRentalsInformations()
        {
            return _rentalInformation.OrderBy(x => x.Id).ToList();
        }

        public List<RentInformation> GetRentsInformations()
        {
            return _rentInformation.OrderBy(x => x.Id).ToList();
        }

        public AnnouncementInformation InsertAnnouncementInformation(AnnouncementInformation announcementInformation)
        {
            announcementInformation.Id = _announcementInformation.Count + 1;
            announcementInformation.CreatedDate = DateTime.Now;
            announcementInformation.UpdatedDate = DateTime.Now;
            _announcementInformation.Add(announcementInformation);
            return announcementInformation;
        }

        public CityInformations InsertCity(CityInformations city)
        {
            city.Id = _cityInformation.Count + 1;
            city.CreatedDate = DateTime.Now;
            _cityInformation.Add(city);
            return city;
        }

        public DepartmentToSearchData InsertDepartment(DepartmentToSearchData departmentToSearchData)
        {
            throw new NotImplementedException();
        }

        public LoanInformation InsertLoanInformation(LoanInformation loanInformation)
        {
            loanInformation.Id = _loanstInformation.Count + 1;
            loanInformation.CreatedDate = DateTime.Now;
            loanInformation.UpdatedDate = DateTime.Now;
            _loanstInformation.Add(loanInformation);
            return loanInformation;
        }

        public RateInformation InsertRateInformation(RateInformation rateInformation)
        {
            rateInformation.Id = _rateInformation.Count + 1;
            rateInformation.CreatedDate = DateTime.Now;
            rateInformation.UpdatedDate = DateTime.Now;
            _rateInformation.Add(rateInformation);
            return rateInformation;
        }

        public RentalInformations InsertRentalInformation(RentalInformations rental)
        {
            rental.Id = _rentalInformation.Count + 1;
            rental.CreatedDate = DateTime.Now;
            rental.UpdatedDate = DateTime.Now;
            _rentalInformation.Add(rental);
            return rental;
        }

        public RentInformation InsertRentInformation(RentInformation rentInformation)
        {
            rentInformation.Id = _rentInformation.Count + 1;
            rentInformation.CreatedDate = DateTime.Now;
            rentInformation.UpdatedDate = DateTime.Now;
            _rentInformation.Add(rentInformation);
            return rentInformation;
        }

        public void UpdateRentabilityInformation(int announcementId)
        {
            _announcementInformation.First(ann => ann.Id == announcementId).RentabilityCalculated = true;
        }

        public void UpdateRentabilityInformation(int announcementId, bool isRentable)
        {
            throw new NotImplementedException();
        }
    }
}