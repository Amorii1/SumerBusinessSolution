using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class ProdImg
    {
            public int Id { get; set; }
            [Required]
        [Display(Name = "ملف الصوره")]

        public string ImgFile { get; set; }
       
            public int ProdId { get; set; }

            [ForeignKey("ProdId")]
            public virtual ProdInfo ProdInfo { get; set; }
    }
}
