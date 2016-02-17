using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Prodruct200
{
    public class Store
    {
        [Key]
        public string StoreName { get; set; }

        public virtual List<StoreProduct> StoreProductStores { get; set; }

        public Store()
        {
            StoreProductStores = new List<StoreProduct>();
        }
    }
}
