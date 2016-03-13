using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Product> FindProductStartingWith(string productNameStart)
        {
            var productList = (from t in DataContext.Products where t.ProductName.StartsWith(productNameStart) select t).ToList();

            return productList;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }
    }
}
