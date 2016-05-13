using System.Collections.Generic;
using System.Collections.ObjectModel;
using Pristjek220Data;

namespace Consumer
{
    /// <summary>
    ///     Interface for Business logic layer for Consumer
    /// </summary>
    public interface IConsumer
    {
        /// <summary>
        ///     ObservableCollection that contains the items to the generated list
        /// </summary>
        ObservableCollection<StoreProductAndPrice> GeneratedShoppingListData { get; set; }

        /// <summary>
        ///     ObservableCollection that contains all the itmes in the shoppinglist
        /// </summary>
        ObservableCollection<ProductInfo> ShoppingListData { get; set; }

        /// <summary>
        ///     ObservableCollection that contains the items to the not in store list
        /// </summary>
        ObservableCollection<ProductInfo> NotInAStore { get; set; }

        /// <summary>
        ///     ObservableCollection that contains the stores and if they are marked
        /// </summary>
        ObservableCollection<StoresInPristjek> OptionsStores { get; set; }

        /// <summary>
        ///     List that contains all the stores in the program
        /// </summary>
        List<string> StoreNames { get; set; }

        /// <summary>
        ///     The total sum in a string
        /// </summary>
        string TotalSum { get; set; }

        /// <summary>
        ///     Describes what the price is if you buy all your items in one store
        /// </summary>
        string BuyInOneStore { get; set; }

        /// <summary>
        ///     Describes how much money that has been saved
        /// </summary>
        string MoneySaved { get; set; }

        /// <summary>
        ///     Finds the cheapest store that sells a product
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>The cheapest store</returns>
        Store FindCheapestStore(string productName);

        /// <summary>
        ///     Finds the cheapest store that sells all products in the list
        /// </summary>
        /// <param name="products"></param>
        /// <returns>The store and the sum of all the products</returns>
        StoreAndPrice FindCheapestStoreWithSumForListOfProducts(List<ProductInfo> products);

        /// <summary>
        ///     Checks if a product exist
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>true if it exist</returns>
        bool DoesProductExist(string productName);

        /// <summary>
        ///     Finds all stores that sells the requested product
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>a list with stores and prices</returns>
        List<StoreAndPrice> FindStoresThatSellsProduct(string productName);

        /// <summary>
        ///     Creates a shoppinglist with products with a store, and a shoppinglist for products with no store
        /// </summary>
        void CreateShoppingList();

        /// <summary>
        ///     Reads from a file
        /// </summary>
        void ReadFromJsonFile();

        /// <summary>
        ///     Writes to a file
        /// </summary>
        void WriteToJsonFile();

        /// <summary>
        ///     Clears the generated shopping list
        /// </summary>
        void ClearGeneratedShoppingListData();

        /// <summary>
        ///     Clears the not in a store list
        /// </summary>
        void ClearNotInAStore();

        /// <summary>
        ///     Changes a product from one stopre to another
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="product"></param>
        /// <returns>1 if it was a success</returns>
        int ChangeProductToAnotherStore(string storeName, StoreProductAndPrice product);
    }
}