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
        public string  CompanyName { get; set; } 

        [Required]
        public string ContactName { get; set; }

        public string Address { get; set; }
        public string PhoneNo { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string CreatedById { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        [ForeignKey("CreatedById")]
        public virtual ApplicationUser ApplicationUser { get; set; }



    }
}
