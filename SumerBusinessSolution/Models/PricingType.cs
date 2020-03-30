using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SumerBusinessSolution.Models
{
    public class PricingType
    {
        /// <summary>
        /// either whole sales or retrail price جملة او مفرد
        /// </summary>
        public int Id { get; set; }
        [Display(Name = "التسعير")]

        public string PriceType { get; set; }
        [Display(Name = "الملاحظات")]

        public string Notes { get; set; }
    }
}
