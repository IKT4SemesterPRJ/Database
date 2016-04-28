using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IUnitOfWork _unit;

        private ObservableCollection<ProductInfo> _shoppingListData;

        private readonly string path =
            Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents") + @"\Pristjek220";

        public Consumer(IUnitOfWork unitOfWork)
        {
            ShoppingListData = new ObservableCollection<ProductInfo>();
            _unit = unitOfWork;
            GeneratedShoppingListData = new ObservableCollection<StoreProductAndPrice>();
            NotInAStore = new ObservableCollection<ProductInfo>();
        }

        public ObservableCollection<StoreProductAndPrice> GeneratedShoppingListData { get; set; }

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

            var cheapest = product?.HasARelation.FirstOrDefault();

            if (cheapest == null)
                return null;

            foreach (var hasA in product.HasARelation)
            {
                if (hasA.Price < cheapest.Price)
                    cheapest = hasA;
            }

            return cheapest.Store;
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
    }

    /// <summary>
    ///     This class is used to store the name and quantity of a product.
    /// </summary>
    public class ProductInfo
    {
        public ProductInfo(string name, string quantity = "1")
        {
            Name = char.ToUpper(name[0]) + name.Substring(1).ToLower();
            Quantity = quantity;
        }

        public string Name { set; get; }
        public string Quantity { set; get; }
    }
}