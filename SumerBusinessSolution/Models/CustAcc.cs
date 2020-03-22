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
        [Display(Name = "الرصيد")]

        public double Paid { get; set; }
        [Display(Name = "الديون")]

        public double Debt { get; set; }

        [ForeignKey("CustId")]
        public virtual Customer Customer { get; set; }

    }
}
