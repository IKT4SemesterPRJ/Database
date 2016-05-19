using System.Collections.Generic;

namespace Pristjek220Data
{
    /// <summary>
    /// Interface to DAL StoreRepository, inherit from IRepository for the functions Add and Remove.
    /// </summary>
    public interface IStoreRepository : IRepository<Store>
    {
        /// <summary>
        /// Find a Store in database from primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the found store if any, else return null</returns>
        Store Get(int id);

        /// <summary>
        /// Find store from store name
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>If a store has the given name the store is returned else null</returns>
        Store FindStore(string storeName);

        /// <summary>
        /// Finds all stores which names start with the input given
        /// </summary>
        /// <param name="storeNameStart"></param>
        /// <returns>Returns a list containing the stores found</returns>
        List<Store> FindStoreStartingWith(string storeNameStart);

        /// <summary>
        /// Finds the products sold by the store and the price for the products
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>Returns a list containing the stores products and the price for them</returns>
        List<ProductAndPrice> FindProductsInStore(string storeName);

        /// <summary>
        /// Finds the products from one store which names starts with the input
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="productNameStart"></param>
        /// <returns>Returns a list containing the products found and the price for them</returns>
        List<ProductAndPrice> FindProductsInStoreStartingWith(string storeName, string productNameStart);

        /// <summary>
        /// Get all stores in the database
        /// </summary>
        /// <returns>Returns a list containing all stores in the database</returns>
        List<Store> GetAllStores();

        /// <summary>
        /// Find a product from one store from product name
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="productName"></param>
        /// <returns>Returns the product and the price for it</returns>
        ProductAndPrice FindProductInStore(string storeName, string productName);
    }
}
