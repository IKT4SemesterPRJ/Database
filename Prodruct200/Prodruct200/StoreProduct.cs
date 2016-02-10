using System.ComponentModel.DataAnnotations;

namespace Prodruct200
{
    public class StoreProduct
    {
        [Key]
        public string StoreProductId { get; set; }
        public double Price { get; set; }


        public string StoreName { get; set; }
        public virtual Store Store { get; set; }

        public string ProductName { get; set; }
        public virtual Product Product { get; set; }
    }
}
