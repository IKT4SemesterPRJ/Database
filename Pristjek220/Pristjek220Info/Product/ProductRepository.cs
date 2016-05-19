using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;

namespace Pristjek220Data
{
    /// <summary>
    /// The ProductRepository class is a part of the repository pattern,
    /// and it handles the interactions with the database product table
    /// </summary>
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        /// <summary>
        /// Constructor which sends the Datacontext to the base class
        /// </summary>
        /// <param name="context"></param>
        public ProductRepository(DataContext context) : base(context)
        {
            
        }


        /// <summary>
        /// Find a product in database from primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns the found product if one, else returns null</returns>
        public Product Get(int id)
        {
            return Context.Set<Product>().Find(id);
        }

        /// <summary>
        /// Find a product in database from product name
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Returns the found product if any, else null</returns>
        public Product FindProduct(string productName)
        {
            return (from t in DataContext.Products where t.ProductName == productName select t).FirstOrDefault();
        }

        /// <summary>
        /// Finds the product in database which name starts with the given input
        /// </summary>
        /// <param name="productNameStart"></param>
        /// <returns>Returns a list containing the found products</returns>
        public List<Product> FindProductStartingWith(string productNameStart)
        {
            var productList = (from t in DataContext.Products where t.ProductName.StartsWith(productNameStart) select t).ToList();

            return productList;
        }

        /// <summary>
        /// Find the stores that sells a product from product name
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Returns a list that contains the stores and the price for the products</returns>
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

        /// <summary>
        /// Finds the cheapest store from a list of products
        /// </summary>
        /// <param name="products"></param>
        /// <returns>Returns a list of stores and the price for each product</returns>
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

        /// <summary>
        /// When using DataContext it uses the Context from base class
        /// </summary>
        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }

        /// <summary>
        /// Makes a call to the database
        /// </summary>
        /// <returns></returns>
        public bool ConnectToDb()
        {
            try
            {
                (from t in DataContext.Products where t.ProductName == "connect" select t).FirstOrDefault();
                return true;
            }
            catch (EntityException)
            {
                return false;
            }
            catch (InvalidOperationException)
            {
                return false;
            }

        }
    }
}
