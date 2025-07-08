using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Entities.Supermarket
{
    public class ProductUnit
    {
        public int UnitID { get; set; }
        public int? ProductID { get; set; }
        public string UnitName { get; set; }
        public decimal ConversionRate { get; set; }
        public Decimal? UnitPrice { get; set; }
        public bool IsBaseUnit { get; set; }
        public Product Product { get; set; }
    }
}
