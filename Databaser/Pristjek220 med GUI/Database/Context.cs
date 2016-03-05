using System.Data.Entity;
using System.Linq;


namespace Database
{
    public class Context : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StoreProduct> StoreProducts { get; set; }

        public int AddProductToDatabase(string productName)
        {
            if (FindProduct(productName) != null)
            {
                return -1;
            }
            var product = new Product() { ProductName = productName };

            Products.Add(product);
            SaveChanges();

            return 0;
        }

        public int AddStoreToDatabase(string storeName)
        {
            if (FindStore(storeName) != null)
            {
                return -1;
            }
            var store = new Store() { StoreName = storeName };

            Stores.Add(store);
            SaveChanges();
            return 0;
        }

        public void AddStoreProductRelationToDatabase(string productName, string storeName, double price)
        {
            var product = FindProduct(productName);
            var store = FindStore(storeName);

            var storeProduct = new StoreProduct()
            {
                Price = price,
                Product = product,
                ProductId = product.ProductId,
                Store = store,
                StoreId = store.StoreId,
            };

            product.StoreProducts.Add(storeProduct);
            store.StoreProducts.Add(storeProduct);

            StoreProducts.Add(storeProduct);
            SaveChanges();
        }

        public void RemoveProductFromDatabase(string productName)
        {
            var product = FindProduct(productName);

            for (int i = (product.StoreProducts.Count - 1); i >= 0; i--)
            {
                StoreProducts.Remove(product.StoreProducts[i]); //.StoreProductId
            }

            Products.Remove(product);
            SaveChanges();
        }

        public void RemoveStoreFromDatabse(string storeName)
        {
            var store = FindStore(storeName);

            for (int i = (store.StoreProducts.Count - 1); i >= 0; i--)
            {
                StoreProducts.Remove(store.StoreProducts[i]);  // .StoreProductId
            }

            Stores.Remove(store);
            SaveChanges();
        }

        public void RemoveStoreProductFromDatabse(string storeName, string productName)
        {
            var storeProduct = FindStoreProduct(storeName, productName);

            StoreProducts.Remove(storeProduct);
            SaveChanges();
        }

        public void ChangePriceOfProductInAStore(string productName, string storeName, double price)
        {
            var sp = FindStoreProduct(storeName, productName);

            sp.Price = price;

            SaveChanges();
        }

        public Product FindProduct(string productName)
        {
            var product = (from t in Products where t.ProductName == productName select t).FirstOrDefault();

            return product;
        }

        public Store FindStore(string storeName)
        {
            var store = (from t in Stores where t.StoreName == storeName select t).FirstOrDefault();

            return store;
        }

        public StoreProduct FindStoreProduct(string storeName, string productName)
        {
            var product = (from t in Products where t.ProductName == productName select t).FirstOrDefault();
            if (product == null)
                return null;

            var store = (from t in Stores where t.StoreName == storeName select t).FirstOrDefault();
            if (store == null)
                return null;


            var storeproduct = (from t in StoreProducts where t.ProductId == product.ProductId && t.StoreId == store.StoreId select t).FirstOrDefault();

            return storeproduct;
        }
    }
}
