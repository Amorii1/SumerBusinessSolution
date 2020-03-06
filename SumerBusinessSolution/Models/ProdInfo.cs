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
        [Display(Name = "رمز المنتج")]

        public string ProdCode { get; set; }
        [Required]
      [Display(Name = "اسم المنتج")]

        public string ProdName { set; get; }
      [Display(Name = "وصف المنتج")]

        public string ProdDescription { get; set; }
        [Display(Name="صورة المنتج")]
        public string ImgFile { get; set; }
        [Display(Name = "فئة المنتج")]

        public string ProdCategory { get; set; }
        [Required]
       [Display(Name = "سعر المفرد")]

        public double RetailPrice { get; set; }
        [Required]
       [Display(Name = "سعر الجملة")]

        public double WholePrice  { get; set; }

        [Required]
        [Display(Name = "معرف المنشئ")]
        public string CreatedById { get; set; }
  
        [Required]
        [Display(Name="تاريخ الاضافة")]
        [DataType(DataType.Date)]
        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
