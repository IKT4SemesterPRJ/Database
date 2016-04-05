using System.Collections.Generic;

namespace Pristjek220Data
{
    public interface IProductRepository : IRepository<Product>
    {
        Product Get(int id);
        Product FindProduct(string productName);
        List<Product> FindProductStartingWith(string productNameStart);
        List<StoreAndPrice> FindStoresThatSellsProduct(string productName);
        bool connectToDB();
    }
}
