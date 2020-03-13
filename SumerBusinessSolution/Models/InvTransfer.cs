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

        public int HeaderId { get; set; }

        public int? ProdId { get; set; }
  
       [Required]
       [Display(Name = "الكمية")]
        public double Qty { get; set; }

       [Display(Name = "وحدة القياس")]
        public string UOM { get; set; }

        
        public string Note { get; set; }


        [ForeignKey("ProdId")]
        public virtual ProdInfo ProdInfo { set; get; }

        [ForeignKey("HeaderId")]
        public virtual InvTransferHeader InvTransferHeader { set; get; }




    }
}
