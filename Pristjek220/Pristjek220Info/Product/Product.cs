using System.Collections.Generic;

namespace Pristjek220Data
{
    /// <summary>
    /// Configures the columns in Product table in the database
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// List of the entity HasA witch is the relations to the stores.
        /// This list contains all the stores that sells the product.
        /// Is virtual for lazy loading.
        /// </summary>
        public virtual List<HasA> HasARelation { get; } = new List<HasA>();
    }
}
