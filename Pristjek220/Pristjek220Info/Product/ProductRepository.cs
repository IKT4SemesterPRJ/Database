using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Pristjek220Data
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(DataContext context) : base(context)
        {
            
        }

        public Product Get(int id)
        {
            return Context.Set<Product>().Find(id);
        }

        public Product FindProduct(string productName)
        {
            return (from t in DataContext.Products where t.ProductName == productName select t).FirstOrDefault();
        }

        public List<Product> FindProductStartingWith(string productNameStart)
        {
            var productList = (from t in DataContext.Products where t.ProductName.StartsWith(productNameStart) select t).ToList();

            return productList;
        }

        public List<StoreAndPrice> FindStoresThatSellsProduct(string productName)
        {
            var query = (from product in DataContext.Products
                         where product.ProductName == productName
                         join hasA in DataContext.HasARelation on product.ProductId equals hasA.ProductId
                         join store in DataContext.Stores on hasA.StoreId equals store.StoreId
                         select new StoreAndPrice { Price = hasA.Price, Name = store.StoreName}
                        );

            return query.ToList();
        }

        public List<StoreAndPrice> FindCheapestStoreForAllProductsWithSum(List<ProductInfo> products)
        {
            List<StoreAndPrice> storeAndPrice = new List<StoreAndPrice>();

            if (products.Count == 0)
                return null;

            foreach (var store in DataContext.Stores)
            {
                foreach (var product in products)
                {
                    var quantity = double.Parse(product.Quantity);
                    var query =
                        from stor in DataContext.Stores
                        where stor.StoreName == store.StoreName
                        join hasA in DataContext.HasARelation on stor.StoreId equals hasA.StoreId
                        
                        where hasA.Product.ProductName == product.Name
                        select new StoreAndPrice() { Name = store.StoreName, Price = (hasA.Price * quantity) };

                    if (query.Count() != 0)
                        storeAndPrice.Add(query.First());
                    else
                    {
                        storeAndPrice.RemoveAll(x => x.Name == store.StoreName);
                        break;
                    }
                }
            }
            return storeAndPrice;
        } 


        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }

        public bool ConnectToDb()
        {
            try
            {
                (from t in DataContext.Products where t.ProductName == "connect" select t).FirstOrDefault();
                return true;
            }
            catch (EntityException odbcEx)
            {
                return false;
            }
            catch (InvalidOperationException exception)
            {
                return false;
            }

        }
    }
}
