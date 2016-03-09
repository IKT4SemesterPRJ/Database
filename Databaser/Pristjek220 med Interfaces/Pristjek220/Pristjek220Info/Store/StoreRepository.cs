using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pristjek220Data
{
    public class StoreRepository : Repository<Store>, IStoreRepository
    {
        public StoreRepository(DataContext context) : base(context)
        {
            
        }

        public List<Store> FindStoreStartingWith(string storeNameStart)
        {
            var storeList = (from t in DataContext.Stores where t.StoreName.StartsWith(storeNameStart) select t).ToList();

            return storeList;
        }

        public DataContext DataContext
        {
            get { return Context as DataContext; }
        }
    }
}
