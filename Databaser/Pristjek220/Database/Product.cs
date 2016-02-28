using System.Collections.Generic;

namespace Database
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public virtual List<StoreProduct> StoreProducts { get; set; } = new List<StoreProduct>();

    }
}
