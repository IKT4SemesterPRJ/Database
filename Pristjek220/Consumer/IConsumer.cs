using System.Collections.Generic;
using System.Collections.ObjectModel;
using Pristjek220Data;

namespace Consumer
{
    public interface IConsumer
    {
        ObservableCollection<StoreProductAndPrice> GeneratedShoppingListData { get; set; }
        ObservableCollection<ProductInfo> ShoppingListData { get; set; }
        ObservableCollection<ProductInfo> NotInAStore { get; set; }
        ObservableCollection<StoresInPristjek> OptionsStores { get; set; }
        List<string> StoreNames { get; set; }
        string TotalSum { get; set; }
        string BuyInOneStore { get; set; }
        string MoneySaved { get; set; }
        Store FindCheapestStore(string productName);
        StoreAndPrice FindCheapestStoreWithSumForListOfProducts(List<ProductInfo> products);
        bool DoesProductExist(string productName);
        List<StoreAndPrice> FindStoresThatSellsProduct(string productName);
        void CreateShoppingList();
        void ReadFromJsonFile();
        void WriteToJsonFile();
        void ClearGeneratedShoppingListData();
        void ClearNotInAStore();
        int ChangeItemToAnotherStore(string storeName, StoreProductAndPrice product);
    }
}