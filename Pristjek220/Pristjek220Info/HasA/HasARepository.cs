using System.Linq;

namespace Pristjek220Data
{
    /// <summary>
    /// The HasARepository class is a part of the repository pattern,
    /// and it handles the interactions with the database HasA table
    /// </summary>
    public class HasARepository : Repository<HasA>, IHasARepository
    {
        /// <summary>
        /// Constructor which sends the Datacontext to the base class
        /// </summary>
        /// <param name="context"></param>
        public HasARepository(DataContext context) : base(context)
        {
        }

        /// <summary>
        /// Find a HasA in database from primary keys
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns>Returns the found HasA if any, else return null</returns>
        public HasA Get(int id1, int id2)
        {
            return Context.Set<HasA>().Find(id1, id2);
        }

        /// <summary>
        /// Finds the hasA relation from storename and product name
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="productName"></param>
        /// <returns>Returns the hasA relation if any, else return null</returns>
        public HasA FindHasA(string storeName, string productName)
        {
            return (from t in DataContext.HasARelation where storeName == t.Store.StoreName && productName == t.Product.ProductName select t).FirstOrDefault();
        }

        /// <summary>
        /// Find the cheapest hasA relation from product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns the hasA relation if any from the given product, else return null</returns>
        public HasA FindCheapestHasA(Product product)
        {
            return DataContext.HasARelation.OrderBy(c => c.Price).FirstOrDefault();
        }

        /// <summary>
        /// When using DataContext it uses the Context from base class
        /// </summary>
        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }
    }
}
