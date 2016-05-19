
namespace Pristjek220Data
{
    /// <summary>
    /// Interface to DAL HasARepository, inherit from IRepository for the functions Add and Remove.
    /// </summary>
    public interface IHasARepository : IRepository<HasA>
    {
        /// <summary>
        /// Find a HasA in database from primary keys
        /// </summary>
        /// <param name="id1"></param>
        /// <param name="id2"></param>
        /// <returns>Returns the found HasA if any, else return null</returns>
        HasA Get(int id1, int id2);

        /// <summary>
        /// Finds the hasA relation from storename and product name
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="productName"></param>
        /// <returns>Returns the hasA relation if any, else return null</returns>
        HasA FindHasA(string storeName, string productName);

        /// <summary>
        /// Find the cheapest hasA relation from product
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Returns the hasA relation if any from the given product, else return null</returns>
        HasA FindCheapestHasA(Product product);
    }
}
