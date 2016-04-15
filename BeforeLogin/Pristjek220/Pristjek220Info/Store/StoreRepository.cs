using System.Collections.Generic;
using System.Linq;

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

        public Store FindStore(string storeName)
        {
            return (from t in DataContext.Stores where t.StoreName == storeName select t).FirstOrDefault();
        }

        public List<Store> FindStoreStartingWith(string storeNameStart)
        {
            var storeList = (from t in DataContext.Stores where t.StoreName.StartsWith(storeNameStart) select t).ToList();

            return storeList;
        }

        public List<ProductAndPrice> FindProductsInStore(string storeName)
        {
            var query = (from store in DataContext.Stores
                        where store.StoreName == storeName
                        join hasA in DataContext.HasARelation on store.StoreId equals hasA.StoreId
                        join product in DataContext.Products on hasA.ProductId equals product.ProductId
                        select new  ProductAndPrice {Price = hasA.Price, Name = product.ProductName}
                        );

            return query.ToList();
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }  
    }
}
