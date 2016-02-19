using System.Collections.Generic;


namespace Database
{
    public class Store
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }

        public virtual List<StoreProduct> StoreProducts { get; set; } //initiering

        public Store()
        {
            StoreProducts = new List<StoreProduct>();
        }
    }
}
