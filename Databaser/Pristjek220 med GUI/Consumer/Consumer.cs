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
        public Store FindCheapestStore(string productName)
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

                return cheapest.Store;
            }
        }

        public List<string> AutoComplete(string lookUpWord)
        {
            using (var db = new Database.Context())
            {
                var productList = db.FindProductStartingWith(lookUpWord);

                if (productList == null)
                    return null;          //Produktet findes ikke i databasen

                List<string> AutoCopmpleteList = new List<string>();
                for (int i = 0; i < productList.Count ; i++)
                {
                    AutoCopmpleteList.Add(productList[i].ProductName);
                    if(i==2)
                        break;
                }

                return AutoCopmpleteList;
            }
        } 

    }
}
