using SumerBusinessSolution.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class InvTransaction
    {
        public int Id { get; set; }

        //[Required]
        public int? ProdId { get; set; }

        // [Required]
        public int? WhId { get; set; }

        [Required]
        [Display(Name = "الكمية")]

        public double Qty { get; set; }

        [Required]
        [Display(Name = "نوع العملية")]

        public string TransType { get; set; }

        [Display(Name = "رقم العملية")]

        public int RefTransId { get; set; }

        [Required]
        [Display(Name = "معرف المنشئ")]
        public string CreatedById { get; set; }


        [Required]
        [Display(Name = "تاريخ العملية")]

        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("ProdId")]
        public virtual ProdInfo ProdInfo { set; get; }
        [ForeignKey("WhId")]
        public virtual Warehouse Warehouse { set; get; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser ApplicationUser { set; get; }
    }
}
