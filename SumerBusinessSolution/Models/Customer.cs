using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "اسم الشركة")]
        public string  CompanyName { get; set; } 

        [Required]
        [Display(Name = "اسم الزبون")]
        public string ContactName { get; set; }
        [Display(Name = "عنوان الزبون")]
        public string Address { get; set; }
        [Display(Name = "رقم الهاتف")]
        public string PhoneNo { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string CreatedById { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy hh:mm tt}")]
        [Required]
        public DateTime CreatedDateTime { get; set; }

        //public int CustAccId { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser ApplicationUser { get; set; }


        //[ForeignKey("CustAccId")]
        //public virtual CustAcc CustAcc { get; set; }


    }
}
