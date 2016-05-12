using Pristjek220Data;

namespace Administration
{
    /// <summary>
    ///     Interface for Business logic layer for Storemanager
    /// </summary>
    public interface IStoremanager
    {
        /// <summary>
        ///     Store get function, that is used when the Storemanager needs his Store
        /// </summary>
        Store Store { get; }

        /// <summary>
        ///     Add product to database
        /// </summary>
        /// <param name="product"></param>
        /// <returns>-1 if product is not found and return 0 if product is added</returns>
        int AddProductToDb(Product product);
        /// <summary>
        /// Add product to storemanagers store with price
        /// </summary>
        /// <param name="product"></param>
        /// <param name="price"></param>
        /// <returns>-1 if the product exist in that store and 0 if it has been added</returns>
        int AddProductToMyStore(Product product, double price);
        /// <summary>
        /// Finds a product with the requested productName
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Returns the product that got the productName</returns>
        Product FindProduct(string productName);
        /// <summary>
        ///     Finds a product and its price with the requested productName in the storemanagers store
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Returns the product and its price</returns>
        ProductAndPrice FindProductInStore(string productName);
        /// <summary>
        /// Remove the requested product from the storemanagers store, and if the product got no other relations remove the
        ///     product too
        /// </summary>
        /// <param name="product"></param>
        /// <returns>-1 if the product does not exist and 0 if it has been removed successfully</returns>
        int RemoveProductFromMyStore(Product product);
        /// <summary>
        /// Changes the price of the requested product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="price"></param>
        void ChangePriceOfProductInStore(Product product, double price);
    }
}
