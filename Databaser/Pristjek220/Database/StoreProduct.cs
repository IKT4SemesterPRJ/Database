using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class StoreProduct
    {
        public double Price { get; set; }

        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
