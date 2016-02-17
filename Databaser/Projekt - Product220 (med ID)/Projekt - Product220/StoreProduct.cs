using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt___Product220
{
    public class StoreProduct
    {
        public int StoreProductId { get; set; }
        public double Price { get; set; }

        public int StoreId { get; set; }
        public virtual Store Store { get; set; } 

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
