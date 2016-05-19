using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pristjek220Data
{
    /// <summary>
    /// Configures the columns in HasA table in the database
    /// </summary>
    public class HasA
    {
        /// <summary>
        /// First primary key and foreign key to the store
        /// </summary>
        [Key, Column(Order = 1)]
        public int StoreId { get; set; }

        /// <summary>
        /// Reference to the store, is virtual for lazy loading
        /// </summary>
        public virtual Store Store { get; set; }

        /// <summary>
        /// Second primary key and foreign key to the product
        /// </summary>
        [Key, Column(Order = 2)]
        public int ProductId { get; set; }

        /// <summary>
        /// Reference to the product, is virtual for lazy loading
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// Price for the product in store
        /// </summary>
        public double Price { get; set; }
    }
}
