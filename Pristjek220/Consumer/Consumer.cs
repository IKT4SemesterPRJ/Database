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
    /// The namespace <c>Consumer</c> contains the classes <see cref="Consumer"/> 
    /// and <see cref="ProductInfo"/> along with the interface <see cref="IConsumer"/> 
    /// and is placed in the Business Logic Layer.
    /// </summary>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }

    /// <summary>
    /// This class is used to handle all of the consumers functionality when
    /// interacting with the program.
    /// </summary>
    public class Consumer : IConsumer
    {
        public string MoneySaved { get; set; }
        private readonly IUnitOfWork _unit;

        private ObservableCollection<ProductInfo> _shoppingListData;

        private readonly string path =
            Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + @"\Pristjek220";

        public Consumer(IUnitOfWork unitOfWork)
        {
            OptionsStores = new ObservableCollection<StoresInPristjek>();
            ShoppingListData = new ObservableCollection<ProductInfo>();
            _unit = unitOfWork;
            GeneratedShoppingListData = new ObservableCollection<StoreProductAndPrice>();
            NotInAStore = new ObservableCollection<ProductInfo>();

            FillOptionsStores();
            
        }

        private void FillBuyInOneStore()
        {
            BuyInOneStore = string.Empty;
            MoneySaved = string.Empty;
            var BuyInOneStoreNameAndPrice = FindDifferenceforProducts();
            
                BuyInOneStore =
                    $"I forhold til køb i {BuyInOneStoreNameAndPrice.Name} hvor det koster {BuyInOneStoreNameAndPrice.Price}";
               var test = (BuyInOneStoreNameAndPrice.Price - double.Parse(TotalSum));
                MoneySaved = test.ToString(CultureInfo.CurrentCulture);
            
        }

        private void FillOptionsStores()
        {
            var allStores = _unit.Stores.GetAllStores();
            foreach (var storesInPristjek in from store in allStores where store.StoreName != "Admin" select new StoresInPristjek(store.StoreName))
            {
                OptionsStores.Add(storesInPristjek);
            }
        }

        public ObservableCollection<StoreProductAndPrice> GeneratedShoppingListData { get; set; }
        public ObservableCollection<StoresInPristjek> OptionsStores { get; set; }
        public string BuyInOneStore { get; set; }

        private int count;
        public string TotalSum { get; set; }
        public ObservableCollection<ProductInfo> ShoppingListData
        {
            set{ _shoppingListData = value; }
            get
            {
                WriteToJsonFile();
                return _shoppingListData;
            }
        }

        public ObservableCollection<ProductInfo> NotInAStore { get; set; }

        public bool DoesProductExist(string productName)
        {
            return _unit.Products.FindProduct(productName) != null;
        }

        public Store FindCheapestStore(string productName)
        {
            var product = _unit.Products.FindProduct(productName);
            if (product == null)
            {
                return null;
            }

           HasA cheapest = null;

            foreach (var hasA in product.HasARelation.Where(hasA => OptionsStores.Any(x => (x.Store == hasA.Store.StoreName) && x.IsChecked == true)))
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

        public StoreAndPrice FindCheapestStoreWithSumForListOfProducts(List<ProductInfo> products)
        {
            var cheapestStore = new StoreAndPrice() {Price = Double.PositiveInfinity};

            var list = _unit.Products.FindCheapestStoreForAllProductsWithSum(products);
            if (list == null)
                return null;
            string name = list[0].Name;
            double sum = 0;

            foreach (var item in list)
            {
                if (item.Name == name)
                {
                    
                    sum += item.Price;
                }
                else
                {
                    if(cheapestStore.Price > sum)
                        cheapestStore = new StoreAndPrice() {Name = name, Price = sum};
                    name = item.Name;
                    sum = item.Price;
                }
            }

            return cheapestStore;
        }

        public List<ProductAndPrice> FindStoresAssortment(string storeName)
        {
            return _unit.Stores.FindProductsInStore(storeName);
        }

        public List<StoreAndPrice> FindStoresThatSellsProduct(string productName)
        {
            return _unit.Products.FindStoresThatSellsProduct(productName);
        }

        public void CreateShoppingList()
        {
            StoreProductAndPrice ItemInList;
            TotalSum = "0";
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

                    GeneratedShoppingListData.Add( ItemInList = new StoreProductAndPrice
                    {
                        StoreName = cheapestStore.StoreName,
                        ProductName = product.Name,
                        Price = productInStore.Price,
                        Quantity = product.Quantity,
                        Sum = productInStore.Price*double.Parse(product.Quantity)
                        
                    });
                    TotalSum = (double.Parse(TotalSum) + ItemInList.Sum).ToString();
                }
            }

            FillBuyInOneStore();

            TotalSum += " kr";
        }

        public void WriteToJsonFile()
        {
            Directory.CreateDirectory(path);

            using (var file = File.CreateText(path + @"\Shoppinglist.json"))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(file, _shoppingListData);
            }
        }
        ///
        public void ReadFromJsonFile()
        {
            try
            {
                using (var file = File.OpenText(path + @"\Shoppinglist.json"))
                {
                    var serializer = new JsonSerializer();
                    ShoppingListData =
                        (ObservableCollection<ProductInfo>)
                            serializer.Deserialize(file, typeof(ObservableCollection<ProductInfo>));
                }
            }
            catch(Exception Fn)
            { }
            
        }

        public void ClearGeneratedShoppingListData()
        {
            GeneratedShoppingListData.Clear();
        }

        public void ClearNotInAStore()
        {
            NotInAStore.Clear();
        }

        public StoreAndPrice FindDifferenceforProducts()
        {
            List<ProductInfo> listOfProducts = GeneratedShoppingListData.Select(item => new ProductInfo(item.ProductName, item.Quantity)).ToList();
            return FindCheapestStoreWithSumForListOfProducts(listOfProducts);
        }
    }

    /// <summary>
    ///     This class is used to store the name and quantity of a product.
    /// </summary>

    public class StoresInPristjek
    {
        public StoresInPristjek(string store)
        {
            Store = store;
            IsChecked = true;
        }
        public string Store { get; set; }
        public bool IsChecked { get; set; }

    }
}