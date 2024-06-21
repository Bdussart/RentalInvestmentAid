using OpenQA.Selenium.DevTools.V119.Inspector;
using RentalInvestmentAid.Caching;
using RentalInvestmentAid.Database;
using RentalInvestmentAid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Core.Department
{
    public class DepartmentTreatment : MustInitializeCache
    {
        private IDatabaseFactory _databaseFactory;
        public DepartmentTreatment(CachingManager cachingManager, IDatabaseFactory databaseFactory) : base(cachingManager)
        {
            _databaseFactory = databaseFactory;
            base._cachingManager = cachingManager;
        }

        public List<DepartmentToSearchData> GetDepartmentToSearchDatas()
        {            
            return _databaseFactory.GetDepartmentToSearchDatas();            
        }

        public DepartmentToSearchData InsertDepartment(string departmentName, string departmentNumber)
        {
            DepartmentToSearchData departmentToSearchData = new DepartmentToSearchData()
            {
                DepartmentName = departmentName,
                DepartmentNumber = departmentNumber
            };
            departmentToSearchData = _databaseFactory.InsertDepartment(departmentToSearchData);

            return departmentToSearchData;
        }

        public void DeleteDepartment(int departmentId)
        {
            _databaseFactory.DeleteDepartment(departmentId);
        }

    }
}
