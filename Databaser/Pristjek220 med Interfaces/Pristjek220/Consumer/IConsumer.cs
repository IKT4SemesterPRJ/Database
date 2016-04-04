using System.Collections.Generic;
using System.Collections.ObjectModel;
using Pristjek220Data;

namespace Consumer
{
    public interface IConsumer
    {
        Store FindCheapestStore(string productName);
        bool DoesProductExist(string productName);
        List<ProductAndPrice> FindStoresAssortment(string storeName);
        List<StoreAndPrice> FindStoresThatSellsProduct(string productName);
        ObservableCollection<StoreProductAndPrice> GeneratedShoppingListData { get;}
        ObservableCollection<ProductInfo> ShoppingListData { get;}
        void CreateShoppingList();
    }
}
