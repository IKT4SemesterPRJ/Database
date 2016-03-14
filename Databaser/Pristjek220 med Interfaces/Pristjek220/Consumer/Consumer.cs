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

        public List<string> FindStoresAssortment(string storeName)
        {
            return _unit.Stores.FindProductsInStore(storeName);
        }
    }
}
