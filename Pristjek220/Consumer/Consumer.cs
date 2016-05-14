using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Pristjek220Data;

namespace Consumer
{
    /// <summary>
    ///     The namespace <c>Consumer</c> contains the classes <see cref="Consumer" />
    ///     and <see cref="ProductInfo" /> along with the interface <see cref="IConsumer" />
    ///     and is placed in the Business Logic Layer.
    /// </summary>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }

    /// <summary>
    ///     This class is used to handle all of the consumers functionality when
    ///     interacting with the program.
    /// </summary>
    public class Consumer : IConsumer
    {
        private readonly string _path =
            Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + @"\Pristjek220";

        private readonly IUnitOfWork _unit;

        private ObservableCollection<ProductInfo> _shoppingListData;

        private List<string> _storeNames = new List<string>();

        /// <summary>
        ///     Consumer constructor takes a UnitOfWork to access the database, creates new ObservableCollections and calls
        ///     FillOptionsStores
        /// </summary>
        /// <param name="unitOfWork"></param>
        public Consumer(IUnitOfWork unitOfWork)
        {
            OptionsStores = new ObservableCollection<StoresInPristjek>();
            ShoppingListData = new ObservableCollection<ProductInfo>();
            _unit = unitOfWork;
            GeneratedShoppingListData = new ObservableCollection<StoreProductAndPrice>();
            NotInAStore = new ObservableCollection<ProductInfo>();

            FillOptionsStores();
        }

        /// <summary>
        ///     This string shows how much money has been saved
        /// </summary>
        public string MoneySaved { get; set; }

        /// <summary>
        ///     ObservableCollection that contains the items that is found in the database with the info: Store, name, price, quantity
        ///     and sum
        /// </summary>
        public ObservableCollection<StoreProductAndPrice> GeneratedShoppingListData { get; set; }

        /// <summary>
        ///     ObservableCollection that contains all the available stores
        /// </summary>
        public ObservableCollection<StoresInPristjek> OptionsStores { get; set; }

        /// <summary>
        ///     String that is printed to a label that tell how much it will cost to buy all the wanted products in one store
        /// </summary>
        public string BuyInOneStore { get; set; }

        /// <summary>
        ///     ObservableCollection that contains the items it the shoopinglist that is not in the database
        /// </summary>
        public ObservableCollection<ProductInfo> NotInAStore { get; set; }

        /// <summary>
        ///     String that is printed to a label that tell how much the total of all the products cost
        /// </summary>
        public string TotalSum { get; set; }

        /// <summary>
        ///     ObservableCollection that contains the items it the shoopinglist, and when the list is requested it writes the List to
        ///     a local Json file on the computer
        /// </summary>
        public ObservableCollection<ProductInfo> ShoppingListData
        {
            set { _shoppingListData = value; }
            get
            {
                WriteToJsonFile();
                return _shoppingListData;
            }
        }

        /// <summary>
        ///     Gets all the storenames in the database, except for the admin
        /// </summary>
        public List<string> StoreNames
        {
            get
            {
                if (_storeNames.Count == 0)
                {
                    var allStores = _unit.Stores.GetAllStores();
                    foreach (
                        var storesInPristjek in
                            from store in allStores
                            where store.StoreName != "Admin"
                            select new StoresInPristjek(store.StoreName))
                    {
                        _storeNames.Add(storesInPristjek.Store);
                    }
                    return _storeNames;
                }
                return _storeNames;
            }
            set { _storeNames = value; }
        }


        /// <summary>
        ///     Change the product to another store if the product exist in that store
        /// </summary>
        /// <param name="storeName"></param>
        /// <param name="product"></param>
        /// <returns>1 if the product has been changed to another store and -1 if the other store does not have the product</returns>
        public int ChangeProductToAnotherStore(string storeName, StoreProductAndPrice product)
        {
            var productIndex = GeneratedShoppingListData.IndexOf(product);
            ProductAndPrice productAndPrice;
            if ((productAndPrice = _unit.Stores.FindProductInStore(storeName, product.ProductName)) == null) return -1;
            var newSum = productAndPrice.Price*double.Parse(GeneratedShoppingListData[productIndex].Quantity);
            GeneratedShoppingListData[productIndex] = new StoreProductAndPrice
            {
                Price = productAndPrice.Price,
                ProductName = GeneratedShoppingListData[productIndex].ProductName,
                Quantity = GeneratedShoppingListData[productIndex].Quantity,
                StoreName = storeName,
                Sum = newSum
            };
            var sum = CalculateSumForGeneratedList();
            StoreAndPrice diff;
            if ((diff = FindDifferenceforProducts()) != null)
                MoneySaved = (diff.Price - sum).ToString(CultureInfo.CurrentCulture) + " kr";

            return 1;
        }


        /// <summary>
        ///     Checks if product exist
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>true if product exist</returns>
        public bool DoesProductExist(string productName)
        {
            return _unit.Products.FindProduct(productName) != null;
        }

        /// <summary>
        ///     Finds the cheapest store for a product
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>The store that got the product cheapest</returns>
        public Store FindCheapestStore(string productName)
        {
            var product = _unit.Products.FindProduct(productName);
            if (product == null)
            {
                return null;
            }

            HasA cheapest = null;

            foreach (
                var hasA in
                    product.HasARelation.Where(
                        hasA => OptionsStores.Any(x => (x.Store == hasA.Store.StoreName) && x.IsChecked)))
            {
                if (cheapest == null)
                {
                    cheapest = new HasA {Price = double.MaxValue};
                }

                if (hasA.Price < cheapest.Price)
                    cheapest = hasA;
            }

            return cheapest?.Store;
        }

        /// <summary>
        ///     Find the cheapest Store with sum that sells all the products in a list
        /// </summary>
        /// <param name="products"></param>
        /// <returns>The store and the sum of all the products in the list</returns>
        public StoreAndPrice FindCheapestStoreWithSumForListOfProducts(List<ProductInfo> products)
        {
            var cheapestStore = new StoreAndPrice {Price = double.MaxValue};

            var list = _unit.Products.FindCheapestStoreForAllProductsWithSum(products);
            if (list == null || list.Count == 0)
                return null;

            var name = list[0].Name;
            double sum = 0;

            foreach (var item in list)
            {
                if (item.Name == name)
                {
                    sum += item.Price;
                }
                else
                {
                    if (cheapestStore.Price > sum)
                        cheapestStore = new StoreAndPrice {Name = name, Price = sum};
                    name = item.Name;
                    sum = item.Price;
                }
            }

            if (name == list[list.Count - 1].Name && cheapestStore.Price > sum)
                cheapestStore = new StoreAndPrice {Name = name, Price = sum};

            return cheapestStore;
        }

        /// <summary>
        ///     Find all stores that sells a requested product
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>A list with all the stores that sells the requested product </returns>
        public List<StoreAndPrice> FindStoresThatSellsProduct(string productName)
        {
            return _unit.Products.FindStoresThatSellsProduct(productName);
        }

        /// <summary>
        ///     Creates two generated shoppinglists, from the shoppinglist one with products thats in the database and one with
        ///     products thats not. Calculate what is the sum if you buy all the products in the database in the cheapest shops and
        ///     calculate if you buy all products in the cheapest store,.
        /// </summary>
        public void CreateShoppingList()
        {
            foreach (var product in ShoppingListData)
            {
                var cheapestStore = FindCheapestStore(product.Name);
                if (cheapestStore == null)
                {
                    NotInAStore.Add(product);
                }
                else
                {
                    var productInStore =
                        cheapestStore.HasARelation.Find(x => x.Product.ProductName.Contains(product.Name));

                    GeneratedShoppingListData.Add(new StoreProductAndPrice
                    {
                        StoreName = cheapestStore.StoreName,
                        ProductName = product.Name,
                        Price = productInStore.Price,
                        Quantity = product.Quantity,
                        Sum = productInStore.Price*double.Parse(product.Quantity)
                    });
                }
            }

            FillBuyInOneStore();
            CalculateSumForGeneratedList();
        }

        /// <summary>
        ///     Writes the shopping list data to a file
        /// </summary>
        public void WriteToJsonFile()
        {
            Directory.CreateDirectory(_path);

            using (var file = File.CreateText(_path + @"\Shoppinglist.json"))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, _shoppingListData);
            }
        }

        /// <summary>
        ///     Reads the shopping list data from a file
        /// </summary>
        public void ReadFromJsonFile()
        {
            try
            {
                using (var file = File.OpenText(_path + @"\Shoppinglist.json"))
                {
                    var serializer = new JsonSerializer();
                    ShoppingListData =
                        (ObservableCollection<ProductInfo>)
                            serializer.Deserialize(file, typeof (ObservableCollection<ProductInfo>));
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        /// <summary>
        ///     Clear the shopping list data
        /// </summary>
        public void ClearGeneratedShoppingListData()
        {
            GeneratedShoppingListData.Clear();
        }

        /// <summary>
        ///     Clear the not in store list
        /// </summary>
        public void ClearNotInAStore()
        {
            NotInAStore.Clear();
        }

        private StoreAndPrice FindDifferenceforProducts()
        {
            var listOfProducts =
                GeneratedShoppingListData.Select(item => new ProductInfo(item.ProductName, item.Quantity)).ToList();
            return FindCheapestStoreWithSumForListOfProducts(listOfProducts);
        }

        private void FillBuyInOneStore()
        {
            BuyInOneStore = string.Empty;
            MoneySaved = string.Empty;
            var buyInOneStoreNameAndPrice = FindDifferenceforProducts();
            if (buyInOneStoreNameAndPrice == null)
            {
                BuyInOneStore = "Der er ingen forretninger der sælger alle vare";
                MoneySaved = "-";
                return;
            }
            BuyInOneStore =
                $"I forhold til køb af alle varer i {buyInOneStoreNameAndPrice.Name} hvor det koster {buyInOneStoreNameAndPrice.Price} kr.";
            var test = buyInOneStoreNameAndPrice.Price - CalculateSumForGeneratedList();
            MoneySaved = test.ToString(CultureInfo.CurrentCulture) + " kr";
        }

        private void FillOptionsStores()
        {
            var allStores = _unit.Stores.GetAllStores();
            foreach (
                var storesInPristjek in
                    from store in allStores
                    where store.StoreName != "Admin"
                    select new StoresInPristjek(store.StoreName))
            {
                OptionsStores.Add(storesInPristjek);
            }
        }

        private double CalculateSumForGeneratedList()
        {
            var sum = GeneratedShoppingListData.Sum(item => item.Sum);
            TotalSum = sum.ToString(CultureInfo.CurrentCulture) + " kr";
            return sum;
        }
    }

    /// <summary>
    ///     Holds a store in a string and whether or not it is choosen
    /// </summary>
    public class StoresInPristjek
    {
        /// <summary>
        ///     Constructor for StoresInPristjek, that takes a store, and ses IsChecked to true
        /// </summary>
        /// <param name="store"></param>
        public StoresInPristjek(string store)
        {
            Store = store;
            IsChecked = true;
        }

        /// <summary>
        ///     Holds a storename in a string
        /// </summary>
        public string Store { get; set; }

        /// <summary>
        ///     A bool that tells if a store is checked or not
        /// </summary>
        public bool IsChecked { get; set; }
    }
}