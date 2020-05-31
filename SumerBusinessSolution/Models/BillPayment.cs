using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class BillPayment
    {
        public int Id { get; set; }
        public int BillHeaderId { get; set; }

        public int? CustId { get; set; }
        public double PaidAmt { get; set; }
        public string CreatedById { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime CreatedDateTime { get; set; }
        public string Note { get; set; }


        [ForeignKey("BillHeaderId")]
        public virtual BillHeader BillHeader { get; set; }

        [ForeignKey("CustId")]
        public virtual Customer Customer { get; set; }

    }
}
