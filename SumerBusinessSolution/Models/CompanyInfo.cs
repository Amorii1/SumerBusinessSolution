using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class CompanyInfo
    {
        public int Id { get; set; }
        [Display(Name = "اسم الشركة بالانجليزي")]

        public string CompanyNameEn { get; set; }
        [Display(Name = "اسم الشركة بالعربي")]
        public string CompanyNameAr { get; set; }
        [Display(Name = "عنوان الشركة بالانجليزية")]
        public string AddressEn { get; set; }
        [Display(Name = "عنوان الشركة بالعربي")]

        public string AddressAr { get; set; }
        [Display(Name = "رقم الهاتف الاول")]

        public string PhoneNo { get; set; }
        [Display(Name = "رقم الهاتف الثاني")]

        public string PhoneNo02 { get; set; }
        [Display(Name = "الايميل الالكتروني")]

        public string Email { get; set; }
        [Display(Name = "الملاحظات")]

        public string Note { get; set; }
    }
}
