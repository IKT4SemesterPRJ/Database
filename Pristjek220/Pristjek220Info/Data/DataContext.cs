using System.Data.Entity;

namespace Pristjek220Data
{
    /// <summary>
    /// Datacontext containing the db sets for the database
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// The interaction with stores table in database
        /// </summary>
        public virtual DbSet<Store> Stores { get; set; }

        /// <summary>
        /// The interaction with products table in database
        /// </summary>
        public virtual DbSet<Product> Products { get; set; }

        /// <summary>
        /// The interaction with hasAs table in database
        /// </summary>
        public virtual DbSet<HasA> HasARelation { get; set; }

        /// <summary>
        /// The interaction with logins table in database
        /// </summary>
        public virtual DbSet<Login> Logins { get; set; }
    }
}
