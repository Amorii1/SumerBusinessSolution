using Sumer.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class InvTransfer
    {
        public int Id { get; set; }

        //[Required]
        public int? ProdId { get; set; }

        //[Required]
        public int? FromWhId { get; set; }
     
        //[Required]
        public int? ToWhId { get; set; }

        [Required]
        [Display(Name = "الكميه")]

        public double Qty { get; set; }
        [Display(Name = "وحده القياس")]

        public string UOM { get; set; }
        [Required]
        [Display(Name = "حاله الحركه")]

        public string TransferStatus { get; set; }

        [Required]
        [Display(Name = "تاريخ الاضافه ")]

        public DateTime CreatedDateTime { get; set; }

        [Required] 
        public string CreatedById { get; set; }

        public string ApprovedById { get; set; }
        [Display(Name = " الملاحظات ")]

        public string Note { get; set; }



        [ForeignKey("CreatedById")]
        public virtual ApplicationUser ApplicationUser { set; get; }

        [ForeignKey("ApprovedById")]
        public virtual ApplicationUser ApprovedApplicationUser { set; get; }

        [ForeignKey("ProdId")]
        public virtual ProdInfo ProdInfo { set; get; }

        [ForeignKey("ToWhId")]
        [Display(Name ="  من المخزن")]
        
        public virtual Warehouse FromWarehouse { set; get; }

        [ForeignKey("FromWhId")]
        [Display(Name = "الى المخزن")]

        public virtual Warehouse ToWarehouse { set; get; }



    }
}
