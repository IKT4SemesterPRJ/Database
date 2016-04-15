using System.Data.Entity;

namespace Pristjek220Data
{
    public class DataContext : DbContext
    {
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<HasA> HasARelation { get; set; }
        public virtual DbSet<Login> Logins { get; set; }
    }
}
