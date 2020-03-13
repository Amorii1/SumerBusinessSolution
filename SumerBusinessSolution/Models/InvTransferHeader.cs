using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class InvTransferHeader
    {
        public int Id { get; set; }

        [Display(Name = "من مخزن")]
        public int? FromWhId { get; set; }

        [Display(Name = "الى مخزن")]
        public int? ToWhId { get; set; }

        [Required]
        [Display(Name = "حالة الحركة")]
        public string TransferStatus { get; set; }

        [Required]
        [Display(Name = "تاريخ الحركة ")]

        public DateTime CreatedDateTime { get; set; }

        [Required]
        [Display(Name = "معرف المنشئ")]
        public string CreatedById { get; set; }

        public string ApprovedById { get; set; }

        [Display(Name = "ملاحظات")]
        public string Note { get; set; }


        [ForeignKey("CreatedById")]
        public virtual ApplicationUser ApplicationUser { set; get; }

        [ForeignKey("ApprovedById")]
        public virtual ApplicationUser ApprovedApplicationUser { set; get; }

        [ForeignKey("FromWhId")]
        [Display(Name = "الى المخزن")]
        public virtual Warehouse ToWarehouse { set; get; }

        [ForeignKey("ToWhId")]
        [Display(Name = "من المخزن")]
        public virtual Warehouse FromWarehouse { set; get; }




    }

}
