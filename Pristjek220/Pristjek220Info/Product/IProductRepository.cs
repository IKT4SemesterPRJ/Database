using System.Collections.Generic;

namespace Pristjek220Data
{
    /// <summary>
    /// Interface to DAL ProductRepository, inherit from IRepository for the functions Add and Remove.
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        /// <summary>
        /// Find a product in database from primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the found product if one, else returns null</returns>
        Product Get(int id);

        /// <summary>
        /// Find a product in database from product name
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Returns the found product if any, else null</returns>
        Product FindProduct(string productName);

        /// <summary>
        /// Finds the product in database which name starts with the given input
        /// </summary>
        /// <param name="productNameStart"></param>
        /// <returns>Returns a list containing the found products</returns>
        List<Product> FindProductStartingWith(string productNameStart);

        /// <summary>
        /// Find the stores that sells a product from product name
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Returns a list that contains the stores and the price for the products</returns>
        List<StoreAndPrice> FindStoresThatSellsProduct(string productName);

        /// <summary>
        /// Finds the cheapest store from a list of products
        /// </summary>
        /// <param name="products"></param>
        /// <returns>Returns a list of stores and the price for each product</returns>
        List<StoreAndPrice> FindCheapestStoreForAllProductsWithSum(List<ProductInfo> products);

        /// <summary>
        /// Makes a call to the database
        /// </summary>
        /// <returns></returns>
        bool ConnectToDb();
    }
}
