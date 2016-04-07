using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Consumer
{
    public class Consumer
    {
        public StoreProduct FindCheapestStore(string productName)
        {
            using (var db = new Database.Context())
            {
                var product = db.FindProduct(productName);

                if (product == null)
                    return null;          //Produktet findes ikke i databasen

                var cheapest = product.StoreProducts.First();

                foreach (var storprod in product.StoreProducts)
                {
                    if (storprod.Price < cheapest.Price)
                        cheapest = storprod;
                }

                return cheapest;
            }
        }
    }
}
