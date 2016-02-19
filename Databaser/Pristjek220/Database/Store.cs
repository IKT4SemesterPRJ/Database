using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
