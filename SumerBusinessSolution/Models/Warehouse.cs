using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class Warehouse
    {
        [Key]
        public int Id { get; set; }

        [Required]
       [Display(Name = "رمز المخزن")]

        public string WhCode { get; set; }

        [Required]
       [Display(Name = "اسم المخزن")]

        public string WhName { get; set; }
       [Display(Name = "عنوان المخزن")]

        public string WhLocation { get; set; }

        [Required]
        [Display(Name = "نوع المخزن")]
        public int TypeId { get; set; }
 
        [Required]
        public string CreatedById { get; set; }
 
        [Required]
        [Display(Name = "تاريخ الاضافة")]

        public DateTime CreatedDateTime { get; set; }

        [Required]
        public bool Active { get; set; }

        [ForeignKey("TypeId")]
        public virtual WhType WhType { set; get; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser ApplicationUser { set; get; }
    }
}
