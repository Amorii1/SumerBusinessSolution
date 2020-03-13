using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    //Customer Account
    public class CustAcc
    {
        public int Id { get; set; }
        public int CustId { get; set; }

        public double Paid { get; set; }

        public double Debt { get; set; }


    }
}
