using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Prodruct200
{
    public class Product
    {
        [Key]
        public string ProductName { get; set; }

        public virtual List<StoreProduct> StoreProductProducts { get; set; }

        public Product()
        {
            StoreProductProducts = new List<StoreProduct>();
        }
    }
}
