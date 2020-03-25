using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class ExternalBillPayment
    {

        public int Id { get; set; }
        public int ExternalBillHeaderId { get; set; }

        public int? CustId { get; set; }
        public double PaidAmt { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Note { get; set; }


        [ForeignKey("ExternalBillHeaderId")]
        public virtual ExternalBillHeader ExternalBillHeader { get; set; }

        [ForeignKey("CustId")]
        public virtual Customer Customer { get; set; }
    }
}
