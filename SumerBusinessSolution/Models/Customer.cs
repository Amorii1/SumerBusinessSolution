using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string  CompanyName { get; set; }
        public string ContactName { get; set; }

        public string Address { get; set; }
        public string PhoneNo { get; set; }

        public string Status { get; set; }


        public string CreatedBy { get; set; }
        public DateTime CreatedDateTime { get; set; }



    }
}
