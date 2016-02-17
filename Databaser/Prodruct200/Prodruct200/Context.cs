using System;
using System.Data.Entity;

namespace Prodruct200
{
    public class Product220Context : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; } 
        public DbSet<StoreProduct> StoreProducts { get; set; }

        public void AddStoreProductRelation(string productName, string storeName, double price)
        {
            var product = Products.Find(productName);
            var store = Stores.Find(storeName);
            var storeProduct = new StoreProduct
            {
                Price = price,
                Product = product,
                Store = store,
                ProductName = productName,
                StoreName = storeName,
                StoreProductId = (storeName + productName)
            };

            product.StoreProductProducts.Add(storeProduct);
            store.StoreProductStores.Add(storeProduct);

            StoreProducts.Add(storeProduct);
            SaveChanges();
        }


        public void AddProductToDb(string productName)
        {
            var product = new Product {ProductName = productName};

            Products.Add(product);
            SaveChanges();
        }

        public void AddStoreToDb(string storeName)
        {
            var store = new Store { StoreName = storeName };

            Stores.Add(store);
            SaveChanges();
        }

        public void RemoveProductFromDatabase(string productName)
        {
            var prod = Products.Find(productName);

            for (int i = (prod.StoreProductProducts.Count - 1); i >= 0; i--)
            {
                StoreProducts.Remove(StoreProducts.Find(prod.StoreProductProducts[i].StoreProductId));
            }

            Products.Remove(prod);
            SaveChanges();
        }

        public void RemoveStoreFromDatabse(string storeName)
        {
            var stor = Stores.Find(storeName);

            for (int i = (stor.StoreProductStores.Count - 1); i >= 0; i--)
            {
                StoreProducts.Remove(StoreProducts.Find(stor.StoreProductStores[i].StoreProductId));
            }

            Stores.Remove(stor);
            SaveChanges();
        }

        public void RemoveStoreProductFromDatabse(string storeName, string productName)
        {
            var storProd = StoreProducts.Find((storeName + productName));

            storProd.Store.StoreProductStores.Remove(storProd);
            storProd.Product.StoreProductProducts.Remove(storProd);

            StoreProducts.Remove(storProd);

            SaveChanges();
        }

        public void ChangePriceOfProductInAStore(string productName, string storeName, double price)
        {
            var sp = StoreProducts.Find((storeName + productName));

            sp.Price = price;

            SaveChanges();
        }

        public StoreProduct FindStoreProduct(string productName, string storeName)
        {           
            return StoreProducts.Find((storeName + productName));
        }

        public Store FindStore(string storeName)
        {
            return Stores.Find(storeName);
        }

        public Product FindProduct(string productName)
        {
            return Products.Find(productName);
        }

        public string CheapestStore(string productName)
        {
            var prod = Products.Find(productName);
            double cheapestPrice = 1000;
            string cheapestStore = null;

            foreach (var sp in prod.StoreProductProducts)
            {
                if (cheapestPrice > sp.Price)
                {
                    cheapestPrice = sp.Price;
                    cheapestStore = sp.StoreName;
                }
            }

            return cheapestStore;
        }
    }
}
