using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;


namespace Database
{
    public class StoreProduct
    {   
        [Key, Column(Order = 1)]
        public int StoreId { get; set; }
        public virtual Store Store { get; set; }

        [Key, Column(Order = 2)]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public double Price { get; set; }
    }
}
