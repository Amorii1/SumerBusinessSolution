using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class TempProdImg
    {
        public int Id { get; set; }
      //  [Display(Name = "ملف الصوره")]

        public string ImgFile { get; set; }
    }
}
