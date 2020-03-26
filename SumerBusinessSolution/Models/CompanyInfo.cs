using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class CompanyInfo
    {
        public int Id { get; set; }
        public string CompanyNameEn { get; set; }

        public string CompanyNameAr { get; set; }
        public string AddressEn { get; set; }

        public string AddressAr { get; set; }

        public string PhoneNo { get; set; }
        public string PhoneNo02 { get; set; }

        public string Email { get; set; }

        public string Note { get; set; }
    }
}
