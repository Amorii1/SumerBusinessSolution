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

        public int? ProdId { get; set; }

        [Required]
        [Display(Name = "الكمية")]
        public double Qty { get; set; }

        [Display(Name = "المخزن")]
        public int WhId { get; set; }

        [Display(Name = "سعر المنتج")]
        public double UnitPrice { get; set; }

        [Required]
        [Display(Name = "المبلغ الكلي")]
        public double TotalAmt { get; set; }
        [Display(Name = "الملاحظات")]
        public string Note { get; set; }

        [ForeignKey("ProdId")]
        public virtual ProdInfo ProdInfo { get; set; }

        [ForeignKey("HeaderId")]
        public virtual BillHeader BillHeader { get; set; }

    }
}
