using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalInvestmentAid.Models
{
    public class DepartmentToSearchData
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }

        public string DepartmentNumber { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
