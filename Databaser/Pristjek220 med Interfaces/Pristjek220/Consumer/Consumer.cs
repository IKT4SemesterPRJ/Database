using System.Collections.Generic;
using System.Linq;
using Pristjek220Data;

namespace Consumer
{
    public class Consumer : IConsumer
    {
        private readonly IUnitOfWork _unit;

        public Consumer(UnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }

        public Store FindCheapestStore(string productName)
        {
            var product = _unit.Products.Find(c => c.ProductName == productName).FirstOrDefault();

            var cheapest = product?.HasARelation.FirstOrDefault();

            if (cheapest == null)
                return null;

            foreach (var hasA in product.HasARelation)
            {
                if (hasA.Price < cheapest.Price)
                    cheapest.Price = hasA.Price;
            }

            return cheapest.Store;
            
        }
    }
}
