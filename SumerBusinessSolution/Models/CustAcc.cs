using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("CustId")]
        public virtual Customer Customer { get; set; }

    }
}
