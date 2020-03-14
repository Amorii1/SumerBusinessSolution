using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class BillHeader
    {
        public int Id { get; set; }
        

        public int CustId { get; set; }

        [Required]
        public string Status { get; set; }

        public double TotalAmt { get; set; }
        public double Discount { get; set; }
        public double TotalNetAmt { get; set; }
        public double PaidAmt { get; set; }

        [Required]
        public DateTime CraetedDataTime { get; set; }
        [Required]
        public string CreatedById { get; set; }
        public string Note { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("CustId")]
        public virtual Customer Customer { get; set; }

    }
}
