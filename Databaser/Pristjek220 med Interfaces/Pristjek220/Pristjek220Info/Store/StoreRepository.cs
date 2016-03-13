using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pristjek220Data
{
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        public StoreRepository(DataContext context) : base(context)
        {
            
        }

        public Store Get(int id)
        {
            return Context.Set<Store>().Find(id);
        }

        public List<Store> FindStoreStartingWith(string storeNameStart)
        {
            var storeList = (from t in DataContext.Stores where t.StoreName.StartsWith(storeNameStart) select t).ToList();

            return storeList;
        }

        public List<string> FindProductsInStore(string storeName)
        {
            List<string> productAndPrice = new List<string>();

            var query = (from store in DataContext.Stores
                where store.StoreName == storeName
                join hasA in DataContext.HasARelation on store.StoreId equals hasA.StoreId
                join product in DataContext.Products on hasA.ProductId equals product.ProductId
                         select new {Price = hasA.Price, ProductName = product.ProductName}
            );

            foreach (var thing in query)
            {
                productAndPrice.Add($"{thing.ProductName} {thing.Price:F2}");
            }

            return productAndPrice;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }  
    }
}
