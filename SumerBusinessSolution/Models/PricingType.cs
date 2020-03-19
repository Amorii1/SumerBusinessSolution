using System;
using System.Collections.Generic;
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
        public string PriceType { get; set; }
        public string Notes { get; set; }
    }
}
