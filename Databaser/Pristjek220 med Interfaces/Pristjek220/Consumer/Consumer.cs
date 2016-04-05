using System;
using System.Collections.Generic;
using System.Linq;
using Pristjek220Data;

namespace Consumer
{
    /// <summary>
    /// The namespace <c>Consumer</c> contains the classes <see cref="Consumer"/> 
    /// and <see cref="ProductInfo"/> and is placed in the Business Logic Layer.
    /// </summary>
    [System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    class NamespaceDoc
    { }

    /// <summary>
    /// This class is used to handle all of the consumers functionality when 
    /// interacting with the program.
    /// </summary>
    public class Consumer : IConsumer
    {
        private readonly IUnitOfWork _unit;

        public Consumer(IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }

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

        public List<StoreProductAndPrice> CreateShoppingList(List<ProductInfo> productNames)
        {
            var shoppingList = new List<StoreProductAndPrice>();

            foreach (var product in productNames)
            {
                var cheapestStore = FindCheapestStore(product.Name);

                var productInStore = cheapestStore.HasARelation.Find(x => x.Product.ProductName.Contains(product.Name));

                shoppingList.Add(new StoreProductAndPrice() {StoreName = cheapestStore.StoreName, ProductName = product.Name, Price = productInStore.Price, Quantity = product.Quantity, Sum = (productInStore.Price * Double.Parse(product.Quantity))});
            }

            return shoppingList;
        }
    }

    /// <summary>
    /// This class is used to store the name and quantity of a product.
    /// </summary>
    public class ProductInfo
    {
        public string Name { set; get; }
        public string Quantity { set; get; }

        public ProductInfo(string name, string quantity = "1")
        {
            Name = char.ToUpper(name[0]) + name.Substring(1).ToLower();
            Quantity = quantity;
        }
    }
}
