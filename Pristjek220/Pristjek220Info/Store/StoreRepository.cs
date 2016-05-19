using System.Collections.Generic;
using System.Linq;

namespace Pristjek220Data
{
    /// <summary>
    /// The StoreRepository class is a part of the repository pattern,
    /// and it handles the interactions with the database store table
    /// </summary>
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        /// <summary>
        /// Constructor which sends the Datacontext to the base class
        /// </summary>
        /// <param name="context"></param>
        public StoreRepository(DataContext context) : base(context)
        {
            
        }

        /// <summary>
        /// Get all stores in the database
        /// </summary>
        /// <returns>Returns a list containing all stores in the database</returns>
        public List<Store> GetAllStores()
        {
            var storeList = (from store in DataContext.Stores select store).ToList();

            return storeList;

        }

        /// <summary>
        /// Find a Store in database from primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If a store has the given id the store is returned else null</returns>
        public Store Get(int id)
        {
            return DataContext.Set<Store>().Find(id);
        }

        /// <summary>
        /// Find store from store name
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>If a store has the given name the store is returned else null</returns>
        public Store FindStore(string storeName)
        {
            return (from t in DataContext.Stores where t.StoreName == storeName select t).FirstOrDefault();
        }

        /// <summary>
        /// Finds all stores which names start with the input given
        /// </summary>
        /// <param name="storeNameStart"></param>
        /// <returns>Returns a list containing the stores found</returns>
        public List<Store> FindStoreStartingWith(string storeNameStart)
        {
            var storeList = (from t in DataContext.Stores where t.StoreName.StartsWith(storeNameStart) select t).ToList();

            return storeList;
        }

        /// <summary>
        /// Finds the products sold by the store and the price for the products
        /// </summary>
        /// <param name="storeName"></param>
        /// <returns>Returns a list containing the stores products and the price for them</returns>
        public List<ProductAndPrice> FindProductsInStore(string storeName)
        {
            var query = (from store in DataContext.Stores
                        where store.StoreName == storeName
                        join hasA in DataContext.HasARelation on store.StoreId equals hasA.StoreId
                        join product in DataContext.Products on hasA.ProductId equals product.ProductId
                        select new  ProductAndPrice {Price = hasA.Price, Name = product.ProductName}
                        );

            return query.ToList();
        }

        /// <summary>
        /// Finds the products from one store which names starts with the input
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="productNameStart"></param>
        /// <returns>Returns a list containing the products found and the price for them</returns>
        public List<ProductAndPrice> FindProductsInStoreStartingWith(string storeName, string productNameStart)
        {
            var storesProductList = FindProductsInStore(storeName);
            var productList = (from t in storesProductList where t.Name.StartsWith(productNameStart, true, null) select t).ToList();

            return productList;
        }

        /// <summary>
        /// Find a product from one store from product name
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="productName"></param>
        /// <returns>Returns the product and the price for it</returns>
        public ProductAndPrice FindProductInStore(string storeName, string productName)
        {
            var storesProductList = FindProductsInStore(storeName);
            return storesProductList.Find(x => x.Name == productName);
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
