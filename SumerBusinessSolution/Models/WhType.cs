using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class WhType
    {
        [Required]
        public int Id { get; set; }
        [Required]
       [Display(Name = "نوع المخزن")]

        public string Type { get; set; }
    }
}
