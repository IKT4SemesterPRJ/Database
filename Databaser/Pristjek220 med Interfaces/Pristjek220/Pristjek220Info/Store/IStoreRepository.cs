using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pristjek220Data
{
    public interface IStoreRepository : IRepository<Store>
    {
        Store Get(int id);
        List<Store> FindStoreStartingWith(string storeNameStart);
        List<string> FindProductsInStore(string storeName);
    }
}
