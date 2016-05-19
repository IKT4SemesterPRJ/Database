using System.Collections.Generic;

namespace Pristjek220Data
{
    /// <summary>
    /// Configures the columns in Store table in the database
    /// </summary>
    public class Store
    {
        /// <summary>
        /// Primary key
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Name of the store
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// List of the entity HasA witch is the relations to the products.
        /// This list contains all the products the store sells.
        /// Is virtual for lazy loading.
        /// </summary>
        public virtual List<HasA> HasARelation { get; } = new List<HasA>();
    }
}
