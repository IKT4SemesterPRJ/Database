using System.Collections.Generic;
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

        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }
    }
}
