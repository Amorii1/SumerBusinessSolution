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
        [Display(Name = "رقم الفاتورة")]
        public int Id { get; set; }
        [Display(Name = "اسم الزبون")]
        public int? CustId { get; set; }
        [Display(Name = "الحالة")]
        [Required]
        public string Status { get; set; }

        [Display(Name = "الية الدفع")]
        public string PaymentTerms { get; set; }

        [Display(Name = "المبلغ الكلي")]
        public double TotalAmt { get; set; }
        [Display(Name = "التخفيض")]
        public double Discount { get; set; }
        [Display(Name = "المبلغ الاجمالي")]
        public double TotalNetAmt { get; set; }

        [Display(Name = "المبلغ المدفوع")]
        public double PaidAmt { get; set; }
        [Display(Name = "تاريخ الفاتورة")]
        [Required]
        public DateTime CreatedDataTime { get; set; }
        [Required]
        public string CreatedById { get; set; }
        [Display(Name = "الملاحظات")]

        public string Note { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser ApplicationUser { get; set; }

        [ForeignKey("CustId")]
        public virtual Customer Customer { get; set; }

    }
}
