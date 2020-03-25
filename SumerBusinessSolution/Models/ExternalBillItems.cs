using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    // External bills issued when a customers purchases items which are supplied by another seller. 
    // when external bills issued, there will have no effect on inventory and will not issue any inventory transaction
    public class ExternalBillItems
    {
        public int Id { get; set; }

        public int HeaderId { get; set; }

        public string ProdName { get; set; }

        [Required]
        [Display(Name = "الكمية")]
        public double Qty { get; set; }
        [Display(Name = "سعر المنتج")]
        public double UnitPrice { get; set; }

        [Required]
        [Display(Name = "المبلغ الكلي")]
        public double TotalAmt { get; set; }
        [Display(Name = "الملاحظات")]
        public string Note { get; set; }

        [ForeignKey("HeaderId")]
        public virtual ExternalBillHeader ExternalBillHeader { get; set; }
    }
}
