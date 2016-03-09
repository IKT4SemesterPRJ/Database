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

        public List<string> AutoComplete(string lookUpWord)
        {
            var productList = _unit.Products.FindProductStartingWith(lookUpWord);

            if (productList == null)
                return null;          //Produktet findes ikke i databasen

            List<string> autoCopmpleteList = new List<string>();
            for (int i = 0; i < productList.Count; i++)
            {
                autoCopmpleteList.Add(productList[i].ProductName);
                if (i == 2)
                    break;
            }

            return autoCopmpleteList;
        }
    }
}
