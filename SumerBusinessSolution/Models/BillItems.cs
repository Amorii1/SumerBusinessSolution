using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class BillItems
    {
        public int Id { get; set; }

        public int HeaderId { get; set; }

        public int ProdId { get; set; }

        [Required]
        public double Qty { get; set; }

        public double UnitPrice { get; set; }
        public double TotalAmt { get; set; }
        public string Note { get; set; }

        [ForeignKey("ProdId")]
        public virtual ProdInfo ProdInfo { get; set; }

        [ForeignKey("HeaderId")]
        public virtual BillHeader BillHeader { get; set; }

    }
}
