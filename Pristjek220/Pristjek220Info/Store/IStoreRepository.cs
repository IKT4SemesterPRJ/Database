using System.Collections.Generic;

namespace Pristjek220Data
{
    public interface IStoreRepository : IRepository<Store>
    {
        Store Get(int id);
        Store FindStore(string storeName);
        List<Store> FindStoreStartingWith(string storeNameStart);
        List<ProductAndPrice> FindProductsInStore(string storeName);
        List<ProductAndPrice> FindProductsInStoreStartingWith(string storeName, string productNameStart);
    }
}
