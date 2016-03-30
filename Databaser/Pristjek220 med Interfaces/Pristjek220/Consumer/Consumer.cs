using System;
using System.Collections.Generic;
using System.Linq;
using Pristjek220Data;

namespace Consumer
{
    public class Consumer : IConsumer
    {
        private readonly IUnitOfWork _unit;

        public Consumer(IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }

        public bool DoesProductExsist(string productName)
        {
            return _unit.Products.FindProduct(char.ToUpper(productName[0]) + productName.Substring(1)) != null;
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

        public List<StoreProductAndPrice> CreateShoppingList(List<ProduktInfo> productNames)
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
    public class ProduktInfo
    {
        public string Name { set; get; }
        public string Quantity { set; get; }

        public ProduktInfo(string name, string quantity = "1")
        {
            Name = name;
            Quantity = quantity;
        }
    }
}
