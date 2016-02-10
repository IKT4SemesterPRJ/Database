using System;
using System.Data.Entity;

namespace Prodruct200
{
    public class Product220Context : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; } 
        public DbSet<StoreProduct> StoreProducts { get; set; }

        public void AddProduct()
        {
            Console.Write("Enter the name of the Product: ");
            var pname = Console.ReadLine();
            Console.Write("Enter the name of the Store: ");
            var sname = Console.ReadLine();
            Console.Write("Enter the price of the product: ");
            var price = double.Parse(Console.ReadLine());

            var product = new Product {ProductName = pname};
            var store = new Store {StoreName = sname};
            var storeProduct = new StoreProduct { Price = price, StoreName = sname, ProductName = pname, Store = store,
                                                    Product = product, StoreProductId = (sname + pname)};


            product.StoreProductProducts.Add(storeProduct);
            store.StoreProductStores.Add(storeProduct);

            Products.Add(product);
            Stores.Add(store);
            StoreProducts.Add(storeProduct);

            SaveChanges();
        }

        public void RemoveProductdatabase(string productName)
        {
            var prod = Products.Find(productName);

            for (int i = (prod.StoreProductProducts.Count - 1); i >= 0; i--)
            {
                //prod.StoreProductProducts.Remove(prod.StoreProductProducts[i]);
                StoreProducts.Remove(StoreProducts.Find(prod.StoreProductProducts[i].StoreProductId));
            }

            Products.Remove(prod);
            SaveChanges();
        }

        public void ChangePriceOfProduct(string productName, string storeName, double price)
        {
            var sp = StoreProducts.Find((storeName + productName));

            sp.Price = price;

            SaveChanges();
        }

        public void AddStoreToProduct(string productName, string storeName, double price)
        {
            var store = Stores.Find(storeName);
            var product = Products.Find(productName);

            var storeProduct = new StoreProduct
            {
                Price = price,
                StoreName = storeName,
                ProductName = productName,
                Store = store,
                Product = product,
                StoreProductId = (storeName + productName)
            };


            store.StoreProductStores.Add(storeProduct);
            product.StoreProductProducts.Add(storeProduct);
            StoreProducts.Add(storeProduct);

            SaveChanges();
        }

        public double FindPrice(string productName, string storeName)
        {           
            var prod = StoreProducts.Find((storeName + productName));

            return prod.Price;
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
