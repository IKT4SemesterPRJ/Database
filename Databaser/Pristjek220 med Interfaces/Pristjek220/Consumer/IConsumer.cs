using System.Collections.Generic;
using Pristjek220Data;

namespace Consumer
{
    public interface IConsumer
    {
        Store FindCheapestStore(string productName);
        bool DoesProductExist(string productName);
        List<ProductAndPrice> FindStoresAssortment(string storeName);
        List<StoreAndPrice> FindStoresThatSellsProduct(string productName);

        List<StoreProductAndPrice> CreateShoppingList(List<ProductInfo> productNames);
    }
}
