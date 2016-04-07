using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Database
{
    public class Context : DbContext
    {
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<StoreProduct> StoreProducts { get; set; }


        public Product FindProduct(string productName)
        {
            var product = (from t in Products where t.ProductName == productName select t).FirstOrDefault();

            return product;
        }

        public List<Product> FindProductStartingWith(string productNameStart)
        {
            var productList = (from t in Products where t.ProductName.StartsWith(productNameStart) select t).ToList();

            return productList;
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
