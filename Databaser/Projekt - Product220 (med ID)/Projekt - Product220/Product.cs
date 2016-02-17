using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt___Product220
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public virtual List<StoreProduct> StoreProducts { get; set; } //initiering?

        public Product()
        {
            StoreProducts = new List<StoreProduct>();
        }
    }
}
