using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class ProdInfo
    {
        public int Id { get; set; }
        [Required]
        public string ProdCode { get; set; }
        [Required]
        public string ProdName { set; get; }
        public string ProdDescription { get; set; }
        [DisplayName("Product Image")]
        public string ImgFile { get; set; }
        public string ProdCategory { get; set; }
        [Required]
        public double RetailPrice { get; set; }
        [Required]
        public double WholePrice  { get; set; }

        [Required]
        public string CreatedById { get; set; }
  
        [Required]
        [DisplayName("Date")]
        [DataType(DataType.Date)]
        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
