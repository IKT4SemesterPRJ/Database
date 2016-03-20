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

        public List<StoreProductAndPrice> CreateShoppingList(List<string> productNames)
        {
            var shoppingList = new List<StoreProductAndPrice>();

            foreach (var product in productNames)
            {
                var cheapestStore = FindCheapestStore(product);

                var productInStore = cheapestStore.HasARelation.Find(x => x.Product.ProductName.Contains(product));

                shoppingList.Add(new StoreProductAndPrice() {StoreName = cheapestStore.StoreName, ProductName = product, Price = productInStore.Price});
            }

            return shoppingList;
        }
    }
}
