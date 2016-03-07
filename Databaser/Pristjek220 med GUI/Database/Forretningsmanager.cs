using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Storemanager
    {
        public readonly Store Store;


        public int AddProduct(string productName, double price)
        {
            using (var db = new Context())
            {
                if (db.FindStoreProduct(Store.StoreName, productName) != null)
                    return -1;

                Product product;

                if ((product = db.FindProduct(productName)) == null)
                {
                    product = new Product() { ProductName = productName };
                    db.Products.Add(product);
                }

                var store = db.Stores.Find(Store.StoreId);


                var storprod = new StoreProduct()
                {
                    Price = price,
                    Product = product,
                    ProductId = product.ProductId,
                    Store = store,
                    StoreId = store.StoreId
                };

                db.StoreProducts.Add(storprod);
                
                db.SaveChanges();
                return 0;
            }
        }

        public Storemanager(string storeName)
        {
            using (var db = new Context())
            {
                Store = new Store() { StoreName = storeName };

                db.Stores.Add(Store);
                db.SaveChanges();
            }
        }
    }
}
