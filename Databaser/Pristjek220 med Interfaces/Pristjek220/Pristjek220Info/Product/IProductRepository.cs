using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pristjek220Data
{
    public interface IProductRepository : IRepository<Product>
    {
        Product Get(int id);
        Product FindProduct(string productName);
        List<Product> FindProductStartingWith(string productNameStart);
    }
}
