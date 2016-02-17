using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;

namespace Projekt___Product220
{
    public class Product220Context : DbContext
    {
        public DbSet<Store> Stores { get; set; } 
        public DbSet<Product> Products { get; set; } 
        public DbSet<StoreProduct> StoreProducts { get; set; }

        public void AddProductToDatabase(string productName)
        {
            var product = new Product() {ProductName = productName};

            Products.Add(product);
            SaveChanges();
        }

        public void AddStoreToDatabase(string storeName)
        {
            var store = new Store() {StoreName = storeName};

            Stores.Add(store);
            SaveChanges();
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
                StoreProducts.Remove(StoreProducts.Find(product.StoreProducts[i].StoreProductId));
            }

            Products.Remove(product);
            SaveChanges();
        }

        public void RemoveStoreFromDatabse(string storeName)
        {
            var store = FindStore(storeName);

            for (int i = (store.StoreProducts.Count - 1); i >= 0; i--)
            {
                StoreProducts.Remove(StoreProducts.Find(store.StoreProducts[i].StoreProductId));
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
            if (product == null)
            {
                //Fejl.. Vare ikke fundet.
                //return;
            }

            return product;
        }

        public Store FindStore(string storeName)
        {
            var store = (from t in Stores where t.StoreName == storeName select t).FirstOrDefault();
            if (store == null)
            {
                //Fejl.. Vare ikke fundet.
                //return;
            }

            return store;
        }

        public StoreProduct FindStoreProduct(string storeName, string productName)
        {
            var product = (from t in Products where t.ProductName == productName select t).FirstOrDefault();
            if (product == null)
            {
                //Fejl.. Vare ikke fundet.
                //return;
            }

            var store = (from t in Stores where t.StoreName == storeName select t).FirstOrDefault();
            if (store == null)
            {
                //Fejl.. Vare ikke fundet.
                //return;
            }


            var storeproduct = (from t in StoreProducts where t.ProductId == product.ProductId && t.StoreId == store.StoreId select t).FirstOrDefault();
            
            return storeproduct;
        }
    }
}
